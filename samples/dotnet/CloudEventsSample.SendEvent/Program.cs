using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CloudEventSample.Models;
using Newtonsoft.Json;

namespace CloudEventsSample.SendEvent
{
    internal class Program
    {
        private const string TopicEndpoint = "<topic-endpoint-url>";
        private const string TopicKey = "<topic-key>";
        private const string Topic =
            "/subscriptions/<subscription-id>/resourceGroups/<resource-group-name>/providers/Microsoft.EventGrid/topics/<topic-name>";

        private static void Main(string[] args)
        {
            Console.WriteLine("Press <enter> to send");
            Console.ReadLine();
            SendEvent().Wait();
        }

        private static async Task SendEvent()
        {
            var client = new HttpClient { BaseAddress = new Uri(TopicEndpoint) };

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));            
            client.DefaultRequestHeaders.Add("aeg-sas-key", TopicKey);

            var cloudEvent = new CloudEvent<Band>
            {
                EventId = Guid.NewGuid().ToString(),
                EventType = "newBand",
                EventTypeVersion = "1.0",
                CloudEventVersion = "0.1",
                Data = GetBand(),
                Source = $"{Topic}#subjectband",
                EventTime = DateTime.UtcNow.ToString(CultureInfo.InvariantCulture),
            };

            var json = JsonConvert.SerializeObject(cloudEvent);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(string.Empty, content);
        }

        private static Band GetBand()
        {
            var band = new Band
            {
                Name = "Spinal Tap",
                Albums = new List<Album>
                {
                    new Album { AlbumName = "Intravenus de Milo", YearReleased = 1974, RecordLabel = "Megaphone"},
                    new Album { AlbumName = "Shark Sandwich", YearReleased = 1980, RecordLabel = "Polymer"}
                }
            };

            return band;
        }
    }
}
