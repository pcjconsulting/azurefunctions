using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace azurefunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder) 
        {
            // from local.settings.json
            var keyValutUrl = new Uri(Environment.GetEnvironmentVariable("KeyValutUrl"));

            // get cnx str from vault
            var secretClient = new SecretClient(keyValutUrl, new DefaultAzureCredential());
            var cs = secretClient.GetSecret("sql").Value.Value;

            // ioc container
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(cs));
        }
    }
}
