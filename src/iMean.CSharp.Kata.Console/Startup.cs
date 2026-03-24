using iMean.CSharp.Kata.Console.Helpers;
using iMean.CSharp.Kata.Core.Abstractions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Spectre.Console;

namespace iMean.CSharp.Kata.Console
{
    public class Startup : IHostedService, IHostedLifecycleService
    {
        private readonly KataExecutionContext _context;

        private readonly IServiceProvider _serviceProvider;
        private readonly PromptHelper _promptHelper;
        private readonly WidgetHelper _widgetHelper;

        private readonly IHostApplicationLifetime _applicationLifetime;
        private readonly ILogger _logger;

        public Startup(KataExecutionContext context,
            IServiceProvider serviceProvider, 
            PromptHelper promptHelper,
            WidgetHelper widgetHelper,
            IHostApplicationLifetime applicationLifetime,
            ILogger<Startup> logger)
        {
            _context = context;
            _serviceProvider = serviceProvider;
            _promptHelper = promptHelper;
            _widgetHelper = widgetHelper;
            _applicationLifetime = applicationLifetime;
            _logger = logger;

            _applicationLifetime.ApplicationStarted.Register(OnStarted);
            _applicationLifetime.ApplicationStopping.Register(OnStopping);
            _applicationLifetime.ApplicationStopped.Register(OnStopped);
        }

        public Task StartingAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("1. Starting application...");

            return Task.CompletedTask;
        }

        public async Task StartAsync(CancellationToken cancellationToken) 
        {
            _logger.LogInformation("2. Initializing application context...");

            IEnumerable<IKata> katas = _serviceProvider.GetServices<IKata>();
            SelectionPrompt<IKata> prompt = _promptHelper.CreateKataSelectionPrompt(katas);

            while (!cancellationToken.IsCancellationRequested)
            {
                IKata selectedKata = await SelectKataAsync(prompt, cancellationToken);

                await RunAsync(selectedKata);
                await ConfirmAsync("Do you want to run another kata ?", cancellationToken);
            }
        }

        public Task StartedAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("3. Application is started...");
            return Task.CompletedTask;
        }

        public Task StoppingAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("6. Stopping application...");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("7. Cleaning application context...");
            return Task.CompletedTask; 
        }

        public Task StoppedAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("8. Application is stopped...");
            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            _logger.LogInformation("4. Running application...");
        }

        private void OnStopping()
        {
            _logger.LogInformation("5. Preparing for application shutdown...");
        }

        private void OnStopped()
        {
            _logger.LogInformation("9. Finalizing application shutdown...");
        }

        private async Task RunAsync(IKata kata)
        {
            AnsiConsole.Clear();
            IKataOutput output = kata.IsAsync ? await _context.RunAsync(kata) : _context.Run(kata);

            if (output.HasValue)
            {
                Panel outputPanel = new(output.AsStringValue());
                outputPanel.Header($"{kata.Name} - Output");

                AnsiConsole.Write(outputPanel);
            }
        }

        private async Task<IKata> SelectKataAsync(SelectionPrompt<IKata> prompt,
            CancellationToken cancellationToken)
        {
            AnsiConsole.Clear();

            Panel panel = _widgetHelper.CreateApplicationNamePanel();
            AnsiConsole.Write(panel);

            return await AnsiConsole.PromptAsync(prompt);
        }

        private async Task ConfirmAsync(string message, CancellationToken cancellationToken)
        {
            AnsiConsole.WriteLine();

            bool confirmation = await AnsiConsole.ConfirmAsync(message);

            if (!confirmation)
            {
                Environment.Exit(0);
            }
        }
    }
}
