/* Added by CTA: Please add the correponding references..If certs are not provided for deployment communication will be on http, please remove the https section of the kestrel config in appsettings.json and also remove middleware component app.UseHttpsRedirection(); */
using System;
using System.Text.Json;
using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Extensions.Caching;
using Amazon.SecretsManager.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApi.Net6.Products;

namespace WebApi.Net6
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ConfigurationManager.Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
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
                    SecretId = Environment.GetEnvironmentVariable("DB_CONNECTION_SECRET_NAME") ??
                               Configuration["DbSecretName"],
                }).GetAwaiter().GetResult();

            var appSettings =
                JsonSerializer.Deserialize<AppSettings>(dbConnectionSecret.SecretString);
            
            services.AddSingleton(appSettings);

            services.AddDbContext<DataContext>();
            
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public class ConfigurationManager
    {
        public static IConfiguration Configuration { get; set; }
    }
}