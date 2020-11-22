using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using EncoderDecoder.Utilities;
using Blazored.Localisation;
using Microsoft.JSInterop;
using System.Globalization;
using Blazored.Toast;

namespace EncoderDecoder
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);



            builder.RootComponents.Add<App>("app");

            builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddSingleton(sp => new EncoderDecoder.Utilities.Pages());
            builder.Services.AddSingleton(sp => new EncoderDecoder.Utilities.AppState());
            builder.Services.AddBlazoredLocalisation();
            builder.Services.AddBlazoredToast();


            var host = builder.Build();

            var jsRuntime = host.Services.GetService(typeof(IJSRuntime));
            var browserLocale = ((IJSInProcessRuntime)jsRuntime).Invoke<string>("blazoredLocalisation.getBrowserLocale");
            var culture = new CultureInfo(browserLocale);

            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;


            await host.RunAsync();
        }
    }
}
