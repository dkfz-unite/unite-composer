using Unite.Composer.Web.Handlers;

namespace Unite.Composer.Web.HostedServices;

public class AnalysisPreparingHostedService : BackgroundService
{
    private readonly AnalysisPreparingHandler _handler;
    private readonly ILogger _logger;


    public AnalysisPreparingHostedService(
        AnalysisPreparingHandler handler,
        ILogger<AnalysisPreparingHostedService> logger)
    {
        _handler = handler;
        _logger = logger;
    }


    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Analysis preparing service started");

        cancellationToken.Register(() => _logger.LogInformation("Analysis preparing service stopped"));

        // Delay 5 seconds to let the web api start working
        await Task.Delay(5000, cancellationToken);

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                _handler.Handle();
            }
            catch (Exception exception)
            {
                LogError(exception);
            }
            finally
            {
                await Task.Delay(1000, cancellationToken);
            }
        }
    }

    private void LogError(Exception exception)
    {
        _logger.LogError(exception.Message);

        if (exception.InnerException != null)
        {
            _logger.LogError(exception.InnerException.Message);
        }
    }
}
