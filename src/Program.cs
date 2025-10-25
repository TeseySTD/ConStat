using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RazorConsole.Core;
using ConStat.Abstractions;
using ConStat.Components;
using ConStat.Services;

Console.OutputEncoding = System.Text.Encoding.UTF8;

IHost host = Host.CreateDefaultBuilder(args)
    .UseRazorConsole<App>(configure: config =>
        {
            config.ConfigureServices(services =>
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    services.AddSingleton<ICpuService, CpuServiceWindows>();
                }

                services.AddSingleton<IGpuService, GpuService>();
                services.AddSingleton<IDiskService, DiskService>();
                services.AddSingleton<IRamService, RamService>();
            });
        }
    )
    .Build();

await host.RunAsync();