using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jint;
using Jint.Native;
using Jint.Native.Object;
using Newtonsoft.Json;

namespace AkashicChains.Core
{
    public class MarkovEvent
    {
        private MarkovEvent(string eventType, Dictionary<string, object> payload, DateTime occurredOn, string original)
        {
            EventType = eventType;
            Payload = payload;
            OccurredOn = occurredOn;
            Original = original;
        }

        private static MarkovEvent BuildFromJintObject(string eventType, ObjectInstance jsEvent, string jsonEvent, DateTime occurredOn)
        {
            Func<JsValue, object> parseValue = value =>
            {
                switch (value.Type)
                {
                    case Jint.Runtime.Types.String:
                    default:
                        return value.ToString();
                    case Jint.Runtime.Types.Boolean:
                        return Boolean.Parse(value.ToString());
                    case Jint.Runtime.Types.Number:
                        return Decimal.Parse(value.ToString());

                    case Jint.Runtime.Types.None:
                    case Jint.Runtime.Types.Undefined:
                    case Jint.Runtime.Types.Null:
                    case Jint.Runtime.Types.Object:
                        throw new ArgumentOutOfRangeException(value.Type.ToString());
                }
            };

            var eventProperties = new Dictionary<string, object>(jsEvent.GetOwnProperties().Select(x => new KeyValuePair<string, object>(x.Key, parseValue(x.Value.Value))));

            return new MarkovEvent(eventType, eventProperties, occurredOn, jsonEvent);
        }

        public static MarkovEvent BuildFromJson(string eventType, string jsonEvent, DateTime occurredOn)
        {
            var engine = new Engine().Execute("function parse(o){ return JSON.parse(o);}");
            var parser = engine.GetValue("parse");

            var jsEvent = parser.Invoke(jsonEvent).AsObject();

            return BuildFromJintObject(eventType, jsEvent, jsonEvent, occurredOn);
        }

        public static MarkovEvent Build(string eventType, object rawEvent, DateTime occurredOn)
        {
            var jsonEvent = JsonConvert.SerializeObject(rawEvent);

            return BuildFromJson(eventType, jsonEvent, occurredOn);
        }

        public static MarkovEvent Build(string eventType, Dictionary<string, object> payload, DateTime occurredOn, string original)
        {
            return new MarkovEvent(eventType, payload, occurredOn, original);
        }

        public string EventType { get; private set; }
        public IReadOnlyDictionary<string, object> Payload { get; private set; }
        public DateTime OccurredOn { get; private set; }
        public string Original { get; private set; }
    }
}
