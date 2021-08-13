using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
namespace HBReceiveEventExample
{


   /// <summary>
   /// Example of how to use an Azure function to receive a Hirebridge send event (web hooks)  notification
   /// </summary>
   public static class ReceiveEventFunction
   {
      [FunctionName("ReceiveEvent")]
      public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
        ILogger log, ExecutionContext context)
      {
 
         ActionResult actionResponse = new OkResult();
         Dictionary<string, dynamic> returnData = new Dictionary<string, dynamic>();

         StringBuilder sbDump = new StringBuilder();

         if ("post|put".Contains(req.Method.ToLower()))
         {
            try
            {
               string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
               log.LogInformation($"requestBody: {requestBody}");

               //Rehydrate our event json to an object for easier access.
               HirebridgeEventContainer receivedEventHireData = HirebridgeEventContainer.Create(requestBody);

               #region show companyid log dump
               log.LogInformation($"CompanyID: {receivedEventHireData.companyid}");
               log.LogInformation($"Event: {receivedEventHireData.eventtype}");
               #endregion

               #region Dump Content of payload
               //Dump payload in info log
               foreach (string keyName in receivedEventHireData.payload.Keys)
               {
                  sbDump.AppendLine($"{keyName} = [{receivedEventHireData.payload[keyName]}]");
                  
               }
               log.LogInformation(sbDump.ToString());
               #endregion


               #region Transform to CSV and send to https://ptsv2.com/t/random411
               //Example you can transform data and transport elsewhere
               //We morphing to csv and doing an https post to https://ptsv2.com/t/random411/post
               RemoteService rs = new RemoteService();

               string csvData = rs.ConvertToCSV(receivedEventHireData.payload);
               log.LogInformation($"We are sending CSV Data....  {csvData}");
               await rs.SendData("https://ptsv2.com/t/random411/post", csvData);

               await rs.SendData("https://ptsv2.com/t/random411/post", sbDump.ToString());

               #endregion

               //View results by going to https://ptsv2.com/t/random411

               returnData["success"] = true;
               returnData["errormessage"] = string.Empty;
               actionResponse = (ActionResult)new OkObjectResult(returnData);   

            }
            catch (Exception e)
            {
               string errormessage = e.Message + "\n\n" + e.StackTrace;
               returnData["success"] = false;
               returnData["errormessage"] = errormessage;
               actionResponse = (ActionResult)new BadRequestObjectResult(returnData);

               log.LogInformation($"Exeception {errormessage}");
            }

         }
         else
         {
            returnData["success"] = false;
            returnData["errormessage"] = req.Method + " Method not supported";
            actionResponse = (ActionResult)new BadRequestObjectResult(returnData);

         }
         return actionResponse;
      }
   }
}
