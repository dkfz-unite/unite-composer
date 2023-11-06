using Unite.Composer.Web.Handlers;

namespace Unite.Composer.Web.HostedServices;

public class AnalysisProcessingHostedService : BackgroundService
{
    private readonly AnalysisProcessingHandler _handler;
    private readonly ILogger _logger;


    public AnalysisProcessingHostedService(
        AnalysisProcessingHandler handler,
        ILogger<AnalysisProcessingHostedService> logger)
    {
        _handler = handler;
        _logger = logger;
    }


    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Analysis processing service started");

        cancellationToken.Register(() => _logger.LogInformation("Analysis processing service stopped"));

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
