using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AzureIntelliFunc.Models;

namespace AzureIntelliWeb
{
    public static class OnUploadWriteToQueue
    {
        [FunctionName("OnUploadWriteToQueue")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            [Queue("SalesInboundRequest",Connection ="AzureWebJobsStorage")]
            IAsyncCollector<SalesRequest> SalesRequestQueue,
            ILogger log)
        {
            log.LogInformation("Sales request received - OnUploadWriteToQueue .");

            

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            SalesRequest data = JsonConvert.DeserializeObject<SalesRequest>(requestBody);

            await SalesRequestQueue.AddAsync(data);
            string responseMessage = "Sales request has been received for -"+data.Name;
            
            return new OkObjectResult(responseMessage);
        }
    }
}
