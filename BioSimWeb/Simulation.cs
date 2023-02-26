//    Copyright 2023 Gregory Eakin
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.Net.Http.Json;
using System.Numerics;
using System.Reflection;
using Blazor.Extensions;
using Blazor.Extensions.Canvas.Canvas2D;
using Microsoft.AspNetCore.Components;

namespace BioSimWeb;

public interface ISimService
{
    ValueTask Step();
}

public abstract class Simulation
{
    public Display Display { get; } = new();

    public SimTime SimTime { get; } = new();

    private readonly Dictionary<Type, ISimService> _services = new();
    private bool _isInitialized;

    public T? GetService<T>() where T : class, ISimService
    {
        _services.TryGetValue(typeof(T), out var service);
        return service as T;
    }

    protected void AddService(ISimService? service)
    {
        if (service == null) throw new ArgumentNullException(nameof(service));
        _services[service.GetType()] = service;
    }

    public async ValueTask Step()
    {
        if (!_isInitialized)
        {
            _isInitialized = true;
            await Init();
            SimTime.Start();
        }

        SimTime.Step();
        foreach (var service in _services.Values)
            await service.Step();
    }

    protected abstract ValueTask Init();
}

public class BioSimulation : Simulation
{
    private readonly BECanvasComponent _canvas;

    public BioSimulation(BECanvasComponent canvas)
    {
        _canvas = canvas;
    }

    protected override async ValueTask Init()
    {
        AddService(new InputService());

        var context = await _canvas.CreateCanvas2DAsync();
        var renderService = new RenderService(this, context);
        AddService(renderService);
    }
}

public class Display
{
    private Size _size;

    public Size Size
    {
        get => _size;
        set
        {
            _size = value;
            OnSizeChanged?.Invoke();
        }
    }

    public event OnSizeChangedHandler? OnSizeChanged;

    public delegate void OnSizeChangedHandler();
}

public class InputService : ISimService
{
    private readonly IDictionary<MouseButtons, ButtonState> _buttonStates;
    private readonly IDictionary<Keys, ButtonState> _keyboardStates;

    public InputService()
    {
        _buttonStates = EnumUtils.GetAllValues<MouseButtons>().ToDictionary(v => v, _ => ButtonState.None);
        _keyboardStates = EnumUtils.GetAllValues<Keys>().ToDictionary(v => v, _ => ButtonState.None);
    }

    public void SetButtonState(MouseButtons button, ButtonState.States state)
    {
        var oldState = _buttonStates[button];
        _buttonStates[button] = new ButtonState(state, oldState.State == ButtonState.States.Down);
    }

    public ButtonState GetButtonState(MouseButtons button) => _buttonStates[button];


    public void SetKeyState(Keys key, ButtonState.States state)
    {
        var oldState = _keyboardStates[key];
        _keyboardStates[key] = new ButtonState(state, oldState.State == ButtonState.States.Down);
    }

    public ButtonState GetKeyState(Keys key) => _keyboardStates[key];

    public ValueTask Step() => ValueTask.CompletedTask;
}

public enum MouseButtons
{
    Left = 0,
    Middle = 1,
    Right = 2
}

public struct ButtonState
{
    public ButtonState(States state, bool wasPressed)
    {
        State = state;
        WasPressed = wasPressed;
    }

    public bool WasPressed { get; }
    public States State { get; }

    public enum States
    {
        Up = 0,
        Down = 1
    }

    public static readonly ButtonState None = new ButtonState(States.Up, false);
}

public enum Keys
{
    Up = 38,
    Left = 37,
    Down = 40,
    Right = 39,
    Space = 32,
    LeftCtrl = 17,
    LeftAlt = 18,
}

public class SimTime
{
    private readonly Stopwatch _stopwatch = new();

    private long _lastTick;
    private long _elapsedTicks;
    private long _elapsedMilliseconds;
    private long _lastMilliseconds;

    public static SimTime StartNew()
    {
        var simTime = new SimTime();
        simTime.Start();
        return simTime;
    }

    public void Start()
    {
        _stopwatch.Reset();
        _stopwatch.Start();

        _lastTick = 0;
        _lastMilliseconds = 0;
    }

    public void Step()
    {
        _elapsedTicks = _stopwatch.ElapsedTicks - _lastTick;
        _elapsedMilliseconds = _stopwatch.ElapsedMilliseconds - _lastMilliseconds;

        _lastTick = _stopwatch.ElapsedTicks;
        _lastMilliseconds = _stopwatch.ElapsedMilliseconds;
    }

    public long TotalTicks => _stopwatch.ElapsedTicks;

    public long TotalMilliseconds => _stopwatch.ElapsedMilliseconds;

    public long ElapsedTicks => _elapsedTicks;

    public long ElapsedMilliseconds => _elapsedMilliseconds;
}

public static class EnumUtils
{
    public static IEnumerable<TEnum> GetAllValues<TEnum>()
        where TEnum : struct, IConvertible, IComparable, IFormattable
    {
        return Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
    }
}

public class RenderService : ISimService
{
    private readonly Simulation _simulation;
    private readonly Canvas2DContext _context;

    public RenderService(Simulation simulation, Canvas2DContext context)
    {
        _simulation = simulation;
        _context = context;
    }

    public async ValueTask Step()
    {
        // var sceneGraph = _simulation.GetService<SceneGraph>();
        await _context.ClearRectAsync(0, 0, _simulation.Display.Size.Width, _simulation.Display.Size.Height);

        await _context.BeginBatchAsync();
        // await Render(sceneGraph.Root, _simulation);

        await _context.SaveAsync();
        await _context.TranslateAsync(400, 400);
        await _context.RotateAsync(_simulation.SimTime.TotalMilliseconds / 1000.0f);
        await _context.TranslateAsync(-200, -100);
        await _context.ScaleAsync(1.0, 1.0);
        await _context.SetFontAsync("48px serif");
        await _context.StrokeTextAsync($"Seconds {_simulation.SimTime.TotalMilliseconds / 1000.0}", 10.0, 100.0);
        await _context.RestoreAsync();

        await _context.EndBatchAsync();
    }

    private async ValueTask Render(SimNode simNode, Simulation simulation)
    {
        foreach (var component in simNode.Components)
            if (component is IRenderable renderable)
                await renderable.Render(simulation, _context);

        foreach (var child in simNode.Children)
            await Render(child, simulation);
    }
}

internal class ComponentsFactory
{
    private readonly ConcurrentDictionary<Type, ConstructorInfo> _cTorsByType;

    private ComponentsFactory()
    {
        _cTorsByType = new ConcurrentDictionary<Type, ConstructorInfo>();
    }

    private static readonly Lazy<ComponentsFactory> _instance = new(new ComponentsFactory());
    public static ComponentsFactory Instance => _instance.Value;

    public TC Create<TC>(SimNode owner) where TC : class, IComponent
    {
        var ctor = GetCtor<TC>();

        return ctor.Invoke(new[] { owner }) as TC;
    }

    private ConstructorInfo GetCtor<TC>() where TC : class, IComponent
    {
        var type = typeof(TC);

        if (!_cTorsByType.ContainsKey(type))
        {
            var ctor = type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null,
                new[] { typeof(SimNode) }, null);
            _cTorsByType.AddOrUpdate(type, ctor, (t, c) => ctor);
        }

        return _cTorsByType[type];
    }
}

public class ComponentsCollection : IEnumerable<IComponent>
{
    private readonly SimNode _owner;
    private readonly IDictionary<Type, IComponent> _items;

    public ComponentsCollection(SimNode owner)
    {
        _owner = owner;
        _items = new Dictionary<Type, IComponent>();
    }

    //public bool Add(IComponent component)
    //{
    //    var type = component.GetType();
    //    if (_items.ContainsKey(type))
    //        return false;

    //    _items.Add(type, component);
    //    return true;
    //}

    public class ComponentNotFoundException<TC> : Exception where TC : IComponent
    {
        public ComponentNotFoundException() : base($"{typeof(TC).Name} not found on owner")
        {
        }
    }

    public TC Add<TC>() where TC : class, IComponent
    {
        var type = typeof(TC);
        if (!_items.ContainsKey(type))
        {
            var component = ComponentsFactory.Instance.Create<TC>(_owner);
            _items.Add(type, component);
        }

        return _items[type] as TC;
    }

    public T? Get<T>() where T : class, IComponent
    {
        var type = typeof(T);
        return _items.ContainsKey(type) ? _items[type] as T : throw new ComponentNotFoundException<T>();
    }

    public bool TryGet<T>(out T? result) where T : class, IComponent
    {
        var type = typeof(T);
        var found = _items.TryGetValue(type, out var tmp);
        result = tmp as T;
        return found && result != null;
    }

    public IEnumerator<IComponent> GetEnumerator() => _items.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public class SimNode
{
    private static int _lastId;

    private readonly IList<SimNode> _children = new List<SimNode>();

    public SimNode()
    {
        Id = ++_lastId;
        Components = new ComponentsCollection(this);
    }

    public int Id { get; }

    public ComponentsCollection Components { get; }

    public IEnumerable<SimNode> Children => _children;

    public SimNode? Parent { get; private set; }

    public OnDisabledHandler? OnDisabled;

    public delegate void OnDisabledHandler(SimNode simNode);

    private bool _enabled = true;

    public bool Enabled
    {
        get => _enabled;
        set
        {
            var oldValue = _enabled;
            _enabled = value;
            if (!_enabled && oldValue)
                OnDisabled?.Invoke(this);
        }
    }

    public void AddChild(SimNode child)
    {
        if (Equals(child.Parent))
            return;

        child.Parent?._children.Remove(child);
        child.Parent = this;
        _children.Add(child);
    }

    public async ValueTask Update(Simulation simulation)
    {
        if (!Enabled)
            return;

        foreach (var component in Components)
            await component.Update(simulation);

        foreach (var child in _children)
            await child.Update(simulation);
    }

    public override int GetHashCode() => Id.GetHashCode();

    public override bool Equals(object? obj) => obj is SimNode node && Id.Equals(node.Id);

    public override string ToString() => $"GameObject {Id}";
}

public interface IComponent
{
    ValueTask Update(Simulation simulation);

    SimNode Owner { get; }
}

public abstract class BaseComponent : IComponent
{
    public SimNode Owner { get; }

    protected BaseComponent(SimNode owner)
    {
        Owner = owner ?? throw new ArgumentNullException(nameof(owner));
    }

    public virtual async ValueTask OnStart(Simulation simulation)
    {
    }

    public virtual async ValueTask Update(Simulation simulation)
    {
    }
}

public interface IRenderable
{
    ValueTask Render(Simulation simulation, Canvas2DContext context);
}

public struct Transform
{
    public Vector2 Position { get; set; }

    public Vector2 Scale { get; set; }

    public float Rotation { get; set; }

    public Transform()
    {
        Position = Vector2.Zero;
        Scale = Vector2.One;
        Rotation = 0.0f;
    }

    public void Clone(Transform source)
    {
        Position = source.Position;
        Scale = source.Scale;
        Rotation = source.Rotation;
    }

    public Vector2 GetDirection()
    {
        return new Vector2(-MathF.Sin(Rotation), MathF.Cos(Rotation));
    }
}

public class TransformComponent : BaseComponent
{
    private readonly Transform _local = new();
    private Transform _world = new();

    private TransformComponent(SimNode owner) : base(owner)
    {
    }

    public override async ValueTask Update(Simulation simulation)
    {
        _world.Clone(_local);

        if (Owner.Parent != null && Owner.Parent.Components.TryGet<TransformComponent>(out var parentTransform))
            _world.Position = _local.Position + parentTransform.World.Position;
    }

    public Transform Local => _local;
    public Transform World => _world;
}

public interface IAsset
{
    string Name { get; }
}

public interface IAssetsResolver
{
    ValueTask<TA> Load<TA>(string path) where TA : IAsset;
    TA? Get<TA>(string path) where TA : class, IAsset;
}

public class AssetsResolver : IAssetsResolver
{
    private readonly ConcurrentDictionary<string, IAsset> _assets;
    private readonly IAssetLoaderFactory _assetLoaderFactory;

    public AssetsResolver(IAssetLoaderFactory assetLoaderFactory)
    {
        _assetLoaderFactory = assetLoaderFactory;
        _assets = new ConcurrentDictionary<string, IAsset>();
    }

    public async ValueTask<TA> Load<TA>(string path) where TA : IAsset
    {
        var loader = _assetLoaderFactory.Get<TA>() ??
                     throw new TypeLoadException($"unable to load asset type '{typeof(TA)}' from path '{path}'");

        var asset = await loader.Load(path);
        if (asset == null) throw new TypeLoadException($"unable to load asset type '{typeof(TA)}' from path '{path}'");

        _assets.AddOrUpdate(path, _ => asset, (k, v) => asset);
        return asset;
    }

    public TA? Get<TA>(string path) where TA : class, IAsset => _assets[path] as TA;
}

public interface IAssetLoaderFactory
{
    IAssetLoader<TA>? Get<TA>() where TA : IAsset;
}

public interface IAssetLoader<TA> where TA : IAsset
{
    ValueTask<TA> Load(string path);
}

public class AssetLoaderFactory : IAssetLoaderFactory
{
    private readonly IDictionary<Type, object> _loaders;

    public AssetLoaderFactory()
    {
        _loaders = new Dictionary<Type, object>();
    }

    public void Register<TA>(IAssetLoader<TA> loader) where TA : IAsset
    {
        var type = typeof(TA);
        if (!_loaders.ContainsKey(type)) _loaders.Add(type, null);
        _loaders[type] = loader;
    }

    public IAssetLoader<TA>? Get<TA>() where TA : IAsset
    {
        var type = typeof(TA);
        if (!_loaders.ContainsKey(type))
            throw new ArgumentOutOfRangeException($"invalid asset type: {type.FullName}");

        return _loaders[type] as IAssetLoader<TA>;
    }
}

public class Config : IAsset
{
    public string Name { get; }
    public string ImagePath { get; }
    public Data Data { get; }
    
    public Config(string name, ElementReference elementRef, Data data, string imagePath)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        if (string.IsNullOrWhiteSpace(imagePath)) throw new ArgumentNullException(nameof(imagePath));

        Name = name;
        ImagePath = imagePath;
        ElementRef = elementRef;
        Data = data;
    }

    private ElementReference _elementRef;

    public ElementReference ElementRef
    {
        get => _elementRef;
        set => _elementRef = value;
    }
}

public record class Data(
    short sizeX,
    short sizeY,
    int population,
    uint stepsPerGeneration,
    int genomeMaxLength,
    int maxNumberNeurons,
    float populationSensorRadius,
    uint signalSensorRadius,
    uint shortProbeBarrierDistance,
    uint longProbeDistance,
    uint signalLayers);

public class ConfigAssetLoader : IAssetLoader<Config>
{
    private readonly HttpClient _httpClient;

    public ConfigAssetLoader(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async ValueTask<Config> Load(string path)
    {
        var json = await _httpClient.GetFromJsonAsync<Data>(path);
        if (json == null) throw new Exception("*** Crash!");
        var elementRef = new ElementReference(Guid.NewGuid().ToString());
        var config = new Config(path, elementRef, json, path);
        return config;
    }
}