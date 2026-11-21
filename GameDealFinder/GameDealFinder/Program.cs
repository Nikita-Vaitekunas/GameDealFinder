
using GameDealFinder;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Set up HttpClient for the CheapShark API
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://www.cheapshark.com/api/1.0")
});

await builder.Build().RunAsync();
