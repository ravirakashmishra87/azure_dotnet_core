using System;
using System.IO;
using System.Linq;
using AzureIntelliFunc.Data;
using AzureIntelliFunc.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace AzureIntelliFunc
{
    public class OnBlobResizedUpdateStatusInDB
    {
        private AzureDBContext _dbContext;
        public OnBlobResizedUpdateStatusInDB(AzureDBContext azureDBContext)
        {
            _dbContext= azureDBContext;

        }
        [FunctionName("OnBlobResized")]
        public void Run([BlobTrigger("functionsalesrep-sm/{name}", Connection = "AzureWebJobsStorage")]Stream myBlob, string name, ILogger log)
        {
            string FileId = Path.GetFileNameWithoutExtension(name);

            SalesRequest salesRequest = _dbContext.SalesRequests.FirstOrDefault(a=>a.Id == FileId);
            if(salesRequest != null)
            {
                salesRequest.Status = "Image resized";
                _dbContext.SalesRequests.Update(salesRequest);
                _dbContext.SaveChanges();
            }

            log.LogInformation($"Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes - Resize status updated to DB.");
        }
    }
}
