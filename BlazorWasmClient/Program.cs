using BlazorWasmClient.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;

namespace BlazorWasmClient;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");



        //my
        //local ==  "https://localhost:7051/"
        //render  =  "https://eup-server.onrender.com/"
        var apiBaseUrl = "https://eup-server.onrender.com/";
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });
        builder.Services.AddScoped<ProfessorService>(sp =>
        {
            return new ProfessorService(sp.GetRequiredService<HttpClient>(), apiBaseUrl, sp.GetRequiredService<IJSRuntime>());
        });
        builder.Services.AddScoped<HealthService>();

        await builder.Build().RunAsync();
    }
}
