using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace AkashicChains.Usage
{
    class EventManager
    {
        public static Tuple<List<string>, List<string>> DeserializeEventsFromFiles(string eventStream)
        {
            var eventsFileJson = File.ReadAllText(@".\" + eventStream + "events.json");
            var metadataFileJson = File.ReadAllText(@".\" + eventStream + "metadata.json");

            var eventsJsonList = JsonConvert.DeserializeObject<List<string>>(eventsFileJson);
            var metadataJsonList = JsonConvert.DeserializeObject<List<string>>(metadataFileJson);

            return new Tuple<List<string>, List<string>>(eventsJsonList, metadataJsonList);
        }


    }
}
