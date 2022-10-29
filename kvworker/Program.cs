using KeyVaultSecretClient;
using KeyVaultWorker;
using mykvscretclient;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        string certificateThumbprint = "f831101d72829181d8882368893a13fb3af6d9ae";
        string clientId = "f53c3e6a-5a1f-4e7b-8a6f-986fb6312b4b";
        string clientSecret = "-wn8Q~21YkxEsor_cZ~gv3UP1.riI3axXhrZ~bys";
        string keyVaultUrl = "https://orionfusion.vault.azure.net/";
        string tenantId = "1f70aa22-22a7-4792-9aca-002fe8b61892";

        var config = new VaultSecretClientConfig(tenantId, keyVaultUrl, clientId, certificateThumbprint);

        ISecretClient secretClient = new VaultSecretClient(config);
        services.AddSingleton<ISecretClient>(secretClient);
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
