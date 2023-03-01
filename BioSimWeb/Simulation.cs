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
using System.Reflection;
using BioSimLib;
using BioSimLib.Actions;
using BioSimLib.BarrierFactory;
using BioSimLib.Challenges;
using BioSimLib.Field;
using BioSimLib.Genes;
using BioSimLib.Sensors;
using Blazor.Extensions;
using Blazor.Extensions.Canvas;
using Blazor.Extensions.Canvas.Canvas2D;

namespace BioSimWeb;

public interface ISimService
{
    ValueTask Init();
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
            
            foreach (var service in _services.Values)
                await service.Init();

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

    public Config _config { get; } = new()
    {
        sizeX = 128,
        sizeY = 128,
        population = 1000,
        stepsPerGeneration = 300,
        genomeMaxLength = 24,
        maxNumberNeurons = 12,
        populationSensorRadius = 10,
        signalSensorRadius = 10,
        shortProbeBarrierDistance = 4,
        longProbeDistance = 10,
        signalLayers = 1,
        challenge = Challenge.CornerWeighted,
    };

    public Board _board { get; }
    public GeneBank _bank { get; }
    private readonly BarrierFactory _barrierFactory;
    private readonly ChallengeFactory _challengeFactory;
    public SensorFactory _sensorFactory { get; }

    public ActionFactory _actionFactory { get; }
    // private readonly Rectangle _box1;

    // private readonly Rectangle _box2;

    // private readonly Cell[] _cells;
    public float[] _actionLevels { get; } = new float[Enum.GetNames<BioSimLib.Actions.Action>().Length];

    public float[] _neuronAccumulators { get; }
    // private readonly Canvas[] _icons;
    // private readonly TextBlock[] _items;

    private double _scaleFactor = 3.4;
    public uint _generation { get; set; }
    public uint _simStep { get; set; }
    public int _census { get; set; }
    private int _skipUpdate = 10;

    public BioSimulation(BECanvasComponent canvas)
    {
        _canvas = canvas;

        _bank = new GeneBank(_config);
        _board = new Board(_config);
        _barrierFactory = new BarrierFactory(_board.Grid);
        _challengeFactory = new ChallengeFactory(_config, _board.Grid);
        _sensorFactory = new SensorFactory(_config, _board);
        _actionFactory = new ActionFactory(_config, _board);
        // _cells = new Cell[_config.population];
        _neuronAccumulators = new float[_config.maxNumberNeurons];
    }

    protected override async ValueTask Init()
    {
        //await base.Init();

        AddService(new InputService());

        var context = await _canvas.CreateCanvas2DAsync();
        var renderService = new RenderService(this, context);
        AddService(renderService);

        var sceneGraph = new SceneGraph(this);
        AddService(sceneGraph);

        var root = new SimNode();
        sceneGraph.Root.AddChild(root);

        root.Components.Add<SimStats>();
        root.Components.Add<Areana>();
    }
}

public record Display
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

    public ValueTask Init()
    {
        return ValueTask.CompletedTask;
    }

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

public record SimTime
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

    public ValueTask Init()
    {
        return ValueTask.CompletedTask;
    }

    public async ValueTask Step()
    {
        var bio = (BioSimulation)_simulation;
        var step = bio._simStep;
        if (step % 10 != 0)
             return;

        var sceneGraph = _simulation.GetService<SceneGraph>();
        if (sceneGraph == null) return;

        await _context.ClearRectAsync(0, 0, _simulation.Display.Size.Width, _simulation.Display.Size.Height);
        await _context.BeginBatchAsync();
        await Render(sceneGraph.Root, _simulation);
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
    private static readonly Lazy<ComponentsFactory> _instance = new(new ComponentsFactory());
    public static ComponentsFactory Instance => _instance.Value;

    private readonly ConcurrentDictionary<Type, ConstructorInfo> _ctorsByType;

    private ComponentsFactory()
    {
        _ctorsByType = new ConcurrentDictionary<Type, ConstructorInfo>();
    }

    public TC? Create<TC>(SimNode owner) where TC : class, IComponent
    {
        var ctor = GetCtor<TC>();
        return ctor.Invoke(new[] { owner }) as TC;
    }

    private ConstructorInfo GetCtor<TC>() where TC : class, IComponent
    {
        var type = typeof(TC);

        if (_ctorsByType.TryGetValue(type, out var ctor1)) return ctor1;

        var ctor = type.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public, null,
            new[] { typeof(SimNode) }, null);
        if (ctor != null)
            _ctorsByType.AddOrUpdate(type, ctor, (t, c) => ctor);

        return _ctorsByType[type];
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

    public class ComponentNotFoundException<TC> : Exception where TC : IComponent
    {
        public ComponentNotFoundException() : base($"{typeof(TC).Name} not found on owner")
        {
        }
    }

    public TC? Add<TC>() where TC : class, IComponent
    {
        var type = typeof(TC);
        if (_items.TryGetValue(type, out var item)) return item as TC;

        var component = ComponentsFactory.Instance.Create<TC>(_owner) ?? throw new Exception("Component is null.");
        _items.Add(type, component);
        return component;
    }

    public T? Get<T>() where T : class, IComponent
    {
        var type = typeof(T);
        var found = _items.TryGetValue(type, out var item);
        if (!found) throw new ComponentNotFoundException<T>();
        return item as T ?? throw new ComponentNotFoundException<T>();
    }

    public bool TryGet<T>(out T? result) where T : class, IComponent
    {
        var type = typeof(T);
        var found = _items.TryGetValue(type, out var tmp);
        result = tmp as T;
        return found && result != null;
    }

    public IEnumerator<IComponent> GetEnumerator() => _items.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
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

    public async ValueTask Init(Simulation simulation)
    {
        foreach (var component in Components)
            await component.Init(simulation);

        foreach (var child in _children)
            await child.Init(simulation);
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

    public override string ToString() => $"SimNode {Id}";
}

public class SceneGraph : ISimService
{
    private readonly Simulation _simulation;

    public SimNode Root { get; }

    public SceneGraph(Simulation simulation)
    {
        _simulation = simulation;
        Root = new SimNode();
    }

    public async ValueTask Init()
    {
        await Root.Init(_simulation);
    }

    public async ValueTask Step()
    {
        // if (Root == null)
        //     return;

        await Root.Update(_simulation);
    }
}

public interface IComponent
{
    SimNode Owner { get; }
    ValueTask Init(Simulation bioSimulation);
    ValueTask Update(Simulation bioSimulation);
}

public interface IRenderable
{
    ValueTask Render(Simulation bioSimulation, Canvas2DContext context);
}

public class SimStats : IComponent, IRenderable
{
    public SimStats(SimNode owner)
    {
        Owner = owner ?? throw new ArgumentNullException(nameof(owner));
    }

    public ValueTask Init(Simulation bioSimulation)
    {
        return ValueTask.CompletedTask;
    }

    public ValueTask Update(Simulation simulation)
    {
        return ValueTask.CompletedTask;
    }

    public SimNode Owner { get; }

    public async ValueTask Render(Simulation simulation, Canvas2DContext context)
    {
        var bioSimulation = (BioSimulation)simulation;

        await context.SaveAsync();
        await context.TranslateAsync(800.0, -50.0);
        // await context.RotateAsync(bioSimulation.SimTime.TotalMilliseconds / 1000.0f);
        // await context.TranslateAsync(-200, -100);
        await context.ScaleAsync(1.0, 1.0);
        await context.SetFontAsync("48px serif");
        await context.StrokeTextAsync($"Seconds {bioSimulation.SimTime.TotalMilliseconds / 1000.0}", 10.0, 100.0);
        await context.StrokeTextAsync($"World Size {bioSimulation._config.sizeX}x{bioSimulation._config.sizeY}", 10.0, 150.0);
        await context.StrokeTextAsync($"Population {bioSimulation._config.population}", 10.0, 200.0);
        await context.StrokeTextAsync($"Steps/Gen {bioSimulation._config.stepsPerGeneration}", 10.0, 250.0);
        await context.StrokeTextAsync($"Genome Length {bioSimulation._config.genomeMaxLength}", 10.0, 300.0);
        await context.StrokeTextAsync($"Neuron Length {bioSimulation._config.maxNumberNeurons}", 10.0, 350.0);

        await context.StrokeTextAsync($"Generation {bioSimulation._generation}", 10.0, 450.0);
        await context.StrokeTextAsync($"SimStep {bioSimulation._simStep}", 10.0, 500.0);

        await context.RestoreAsync();
    }
}

public class Areana : IComponent, IRenderable
{
    private readonly Cell?[] _cells = new Cell[1000];

    public Areana(SimNode owner)
    {
        Owner = owner ?? throw new ArgumentNullException(nameof(owner));
    }

    public ValueTask Init(Simulation simulation)
    {
        var bioSimulation = (BioSimulation)simulation;

        var i = 0;
        foreach (var genome in bioSimulation._bank.Startup())
        {
            var critter = bioSimulation._board.NewCritter(genome);
            _cells[i] = new Cell(critter);
            // MyCanvas.Children.Add(_cells[i].Element);
            i++;
        }

        return ValueTask.CompletedTask;
    }

    public ValueTask Update(Simulation simulation)
    {
        var bioSimulation = (BioSimulation)simulation;

        // if (bioSimulation._simStep == 1u && bioSimulation._generation % 5 == 0)
        // {
        //     var census = bioSimulation._board.Critters.Census();
        //     bioSimulation._census = census.Count;
        // }

        if (bioSimulation._simStep >= bioSimulation._config.stepsPerGeneration)
        {
            bioSimulation._generation++;
            bioSimulation._simStep = 0u;
            var i = 0;
            var survivors = bioSimulation._board.NewGeneration();
            foreach (var genome in bioSimulation._bank.NewGeneration(survivors))
            {
                var critter = bioSimulation._board.NewCritter(genome);
                _cells[i] = new Cell(critter);
                i++;
            }

            var j = 0;
            foreach (var ((red, green, blue), population) in bioSimulation._bank.Survivors
                         .OrderByDescending(o => o.population).Take(5))
            {
                // var color = Color.FromRgb(red, green, blue);
                // var brush = new SolidColorBrush(color);
                // var path = new Path
                // {
                //     Fill = brush,
                //     Stroke = brush,
                //     Data = new EllipseGeometry
                //     {
                //         RadiusX = 7.0,
                //         RadiusY = 7.0
                //     },
                //     StrokeThickness = 0.1,
                // };

                // _icons[j].Children.Add(path);
                // _items[j].Text = population.ToString();
                j++;
            }
        }

        foreach (var critter in _cells)
            critter?.Update(bioSimulation._board, bioSimulation._sensorFactory, bioSimulation._actionFactory,
                bioSimulation._actionLevels, bioSimulation._neuronAccumulators, bioSimulation._simStep);

        bioSimulation._board.Critters.DrainDeathQueue(bioSimulation._board.Grid);
        bioSimulation._board.Critters.DrainMoveQueue(bioSimulation._board.Grid);

        bioSimulation._simStep++;
        return ValueTask.CompletedTask;
    }

    public SimNode Owner { get; }

    public async ValueTask Render(Simulation simulation, Canvas2DContext context)
    {
        foreach (var critter in _cells)
        {
            if (critter == null) continue;
            await critter.Draw(context, 1.0);
        }
    }
}

public sealed class Cell
{
    public Critter Critter { get; }

    public Cell(Critter critter)
    {
        Critter = critter;
    }

    private static bool IsEnabled(IAction action)
    {
        return (int)action.Type < (int)BioSimLib.Actions.Action.KILL_FORWARD;
    }

    public void Update(Board board, SensorFactory sensorFactory, ActionFactory actionFactory, float[] actionLevels,
        float[] neuronAccumulator, uint simStep)
    {
        if (!Critter.Alive)
            return;

        Array.Clear(actionLevels);
        Array.Clear(neuronAccumulator);

        Critter.FeedForward(sensorFactory, actionLevels, neuronAccumulator, simStep);
        Critter.ExecuteActions(actionFactory, IsEnabled, actionLevels, simStep);
        var newLoc = Critter.ExecuteMoves(actionFactory, IsEnabled, actionLevels);
        if (board.Grid.IsInBounds(newLoc))
            board.Critters.QueueForMove(Critter, newLoc);
    }

    public async ValueTask Draw(Canvas2DContext context, double scaleFactor)
    {
        if (!Critter.Alive)
            return;

        await context.SaveAsync();
        await context.TranslateAsync(2.0, 10.0);
        await context.ScaleAsync(6.0, 6.0);
        await context.TranslateAsync(Critter.LocX, Critter.LocY);
        await context.SetFontAsync("2px serif");
        await context.StrokeTextAsync(".", 0.0, 0.0);
        await context.RestoreAsync();
    }
}