using System;
using System.Text.Json;
using Amazon;
using Amazon.Lambda.Annotations;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Shared;
using WebApi.Shared.Products;

namespace WebApi.SinglePurpose;

[LambdaStartup]
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        var credsChain = new CredentialProfileStoreChain();
        AWSCredentials creds = null;
        var profileCredentials = credsChain.TryGetAWSCredentials("dev", out creds);

        IAmazonSecretsManager secretsManagerClient = profileCredentials
            ? new AmazonSecretsManagerClient(creds, RegionEndpoint.USEast1)
            : new AmazonSecretsManagerClient();

        var dbConnectionSecret = secretsManagerClient.GetSecretValueAsync(
            new GetSecretValueRequest
            {
                SecretId = Environment.GetEnvironmentVariable("DB_CONNECTION_SECRET_NAME"),
            }).GetAwaiter().GetResult();

        var appSettings =
            JsonSerializer.Deserialize<DatabaseConnectionDetails>(dbConnectionSecret.SecretString);
            
        services.AddSingleton(appSettings);

        services.AddDbContext<DataContext>();
    }
}