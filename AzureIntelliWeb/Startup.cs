using AzureIntelliFunc;
using AzureIntelliFunc.Data;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: WebJobsStartup(typeof(Startup))]
namespace AzureIntelliFunc
{
    internal class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            string connectionString = Environment.GetEnvironmentVariable("AzureDBConnectionString");

            builder.Services.AddDbContext<AzureDBContext>(options => options.UseSqlServer(connectionString));
            builder.Services.BuildServiceProvider();
        }
    }
}
