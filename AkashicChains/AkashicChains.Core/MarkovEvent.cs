using System;
using System.Collections.Generic;
using System.Text;

namespace AkashicChains.Core
{
    public class MarkovEvent
    {
        public MarkovEvent(string eventType, Dictionary<string, object> payload, DateTime occurredOn, string original)
        {
            EventType = eventType;
            Payload = payload;
            OccurredOn = occurredOn;
            Original = original;
        }

        public string EventType { get; private set; }
        public IReadOnlyDictionary<string, object> Payload { get; private set; }
        public DateTime OccurredOn { get; private set; }
        public string Original { get; private set; }
    }
}
