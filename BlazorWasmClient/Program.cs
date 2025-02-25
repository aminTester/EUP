using BlazorWasmClient.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorWasmClient;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");



        //builder.Services.AddOidcAuthentication(options =>
        //{
        //    // Configure your authentication provider options here.
        //    // For more information, see https://aka.ms/blazor-standalone-auth
        //    builder.Configuration.Bind("Local", options.ProviderOptions);
        //});


        //my
        //builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://eup-server.onrender.com/") });
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(sp.GetRequiredService<IConfiguration>()["ApiBaseUrl"]) });
        builder.Services.AddScoped<ProfessorService>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var baseUrl = config["ApiBaseUrl"];
            return new ProfessorService(sp.GetRequiredService<HttpClient>(), baseUrl);
        });

        await builder.Build().RunAsync();
    }
}
