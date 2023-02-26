using BioSimWeb;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddSingleton<IAssetsResolver, AssetsResolver>();
builder.Services.AddSingleton<IAssetLoaderFactory>(ctx =>
{
    var factory = new AssetLoaderFactory();

    // factory.Register(ctx.GetRequiredService<IAssetLoader<Sprite>>());
    // factory.Register(ctx.GetRequiredService<IAssetLoader<SpriteSheet>>());

    return factory;
});

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

await builder.Build().RunAsync();
