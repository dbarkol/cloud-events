using System.IO;
using System.Linq;
using CloudEventSample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;


namespace WeWantTheFunc
{
    public static class NewBand
    {
        [FunctionName("newband")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)]HttpRequest req, 
            TraceWriter log)
        {
            log.Info("newband function triggered");
            var requestBody = new StreamReader(req.Body).ReadToEnd();

            // Check the header for the event type.            
            if (!req.Headers.TryGetValue("Aeg-Event-Type", out var headerValues))
                return new BadRequestObjectResult("Not a valid request");

            var eventTypeHeaderValue = headerValues.FirstOrDefault();
            if (eventTypeHeaderValue == "SubscriptionValidation")
            {
                // Subscription Validation event
                // Echo back the validation code
                // to confirm the subscription.
                var events = JsonConvert.DeserializeObject<EventGridEvent[]>(requestBody);
                dynamic data = events[0].Data;
                var validationCode = data["validationCode"];
                return new JsonResult(new
                {
                    validationResponse = validationCode
                });
            }
            else if (eventTypeHeaderValue == "Notification")
            {
                // Notification event
                // Deserialize the cloud event
                // and access the event data.
                log.Info(requestBody);
                var newBandEvent = JsonConvert.DeserializeObject<CloudEvent<Band>>(requestBody);                    
                log.Info($"Welcome, {newBandEvent.Data.Name}!");

                return new OkObjectResult("");
            }

            return new BadRequestObjectResult("Not a valid request");           

        }
    }
}
