using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AzureIntelliFunc.Data;
using AzureIntelliFunc.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace AzureIntelliFunc
{
    public class GroceryAPI
    {
        private AzureDBContext _dbContext;
        public GroceryAPI(AzureDBContext azureDBContext)
        {
            _dbContext = azureDBContext;
        }

        [FunctionName("GroceryAPI")]
        public async Task<IActionResult> CreateGrocery(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "GroceryList")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Create new Grocery - function.");



            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            GroceryItem_Upsert data = JsonConvert.DeserializeObject<GroceryItem_Upsert>(requestBody);
            GroceryItem groceryItem = new GroceryItem
            {
                Name = data.Name,
            };
            _dbContext.groceryItems.Add(groceryItem);
            _dbContext.SaveChanges();


            return new OkObjectResult(groceryItem);
        }

        [FunctionName("GetGrocery")]
        public async Task<IActionResult> GetAllGrocery(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GroceryList")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Get all groceries  - function.");






            return new OkObjectResult(await _dbContext.groceryItems.ToListAsync());
        }

        [FunctionName("GetGroceryById")]
        public async Task<IActionResult> GetGroceryById(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "GroceryList/{id}")] HttpRequest req,
            ILogger log, string id)
        {
            log.LogInformation("Get Grocery by id  - function.");
            var item = await _dbContext.groceryItems.FirstOrDefaultAsync(c => c.Id.Equals(id));

            if (item == null)
            {
                return new OkObjectResult(null);
            }

            return new OkObjectResult(item);
        }
    }
}
