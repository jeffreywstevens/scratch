namespace KeyVaultWorker;

using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using KeyVaultSecretClient;
using mykvscretclient;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ISecretClient secretClient;

    public Worker(ILogger<Worker> logger, ISecretClient secretClient)
    {
        this._logger = logger;
        this.secretClient = secretClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var secretValue = await this.secretClient.GetSecretAsync("SecretValue");
            this._logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await Task.Delay(1000, stoppingToken);
        }
    }
}
