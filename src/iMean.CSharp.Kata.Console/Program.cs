using iMean.CSharp.Kata.Console;
using iMean.CSharp.Kata.Console.Helpers;
using iMean.CSharp.Kata.Core.Abstractions;
using iMean.CSharp.Kata.Core.Javanais;
using iMean.CSharp.Kata.Core.WordValues;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder();

builder.Services.AddTransient<IKata, JavanaisKata>();
builder.Services.AddTransient<IKata, WordValuesKata>();

builder.Services.AddSingleton<KataExecutionContext>();

builder.Services.AddSingleton<WidgetHelper>();
builder.Services.AddTransient<PromptHelper>();

builder.Services.AddHostedService<Startup>();

using IHost host = builder.Build();

await host.RunAsync();
