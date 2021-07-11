using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebTCI.Services;

namespace WebTCI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
            builder.Services.AddSingleton<PatientSelectService>();
            builder.Services.AddSingleton<PkModelService>();
            builder.Services.AddSingleton<ChartService>();
            builder.Services.AddSingleton<TimePointingService>();
            builder.Services.AddSingleton<WindowStateService>();
            builder.Services.AddSingleton<DosingService>();

            await builder.Build().RunAsync();
        }
    }
}
