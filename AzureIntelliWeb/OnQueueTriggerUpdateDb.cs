using System;
using System.Collections.Generic;
using AzureIntelliFunc.Data;
using AzureIntelliFunc.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureIntelliFunc
{
    public class OnQueueTriggerUpdateDb
    {
        private AzureDBContext _dbContext;
        public OnQueueTriggerUpdateDb(AzureDBContext dbcontext)
        {
            _dbContext = dbcontext;
        }
        [FunctionName("OnQueueTriggerUpdateDb")]
        public void Run([QueueTrigger("salesinboundrequest", Connection = "AzureWebJobsStorage")]SalesRequest myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");
            myQueueItem.Status = "Submitted";
            _dbContext.SalesRequests.Add(myQueueItem);
            _dbContext.SaveChanges();
        }        
    }
}
