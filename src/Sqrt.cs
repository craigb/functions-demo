using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ICF.Demo
{
    public static class Sqrt
    {
        [FunctionName("Sqrt")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string numStr = req.Query["num"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            numStr = numStr ?? data?.num;
            
            string responseMessage;
            if (double.TryParse(numStr, out var num))
            {
                responseMessage = $"The square root of {num} is {Math.Sqrt(num)}";
            }
            else
            {
                responseMessage = $"Could not parse a number from the supplied parameter 'num':{numStr}";
            }

            return new OkObjectResult(responseMessage);
        }
    }
}
