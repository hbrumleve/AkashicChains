using System;
using System.Collections.Generic;
using System.Linq;
using AkashicChains.Core;
using Jint;
using Jint.Native;

namespace AkashicChains.Usage
{
    class Program
    {

        static void Main(string[] args)
        {
            var trunk = new Trunk();

            var discriminators = new BraidLinkDiscriminators();

            var rawEvents = EventManager.DeserializeEventsFromFiles("financialaid");

            var markovEvents = new List<MarkovEvent>();

            var engine = new Engine().Execute("function parse(o){ return JSON.parse(o);}");
            var parseEvent = engine.GetValue("parse");

            for (int i = 0; i < rawEvents.Item1.Count; i++)
            {
                var rawEvent = rawEvents.Item1[i];
                var rawMetadata = rawEvents.Item2[i];

                var jsEvent = parseEvent.Invoke(rawEvent).AsObject();
                var jsMetadata = parseEvent.Invoke(rawMetadata).AsObject();

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
                var metadataProperties = new Dictionary<string, string>(jsMetadata.GetOwnProperties().Select(x => new KeyValuePair<string, string>(x.Key, (string)x.Value.Value.ToObject())));

                var occurredOn = DateTime.Parse(eventProperties["OccurredOn"].ToString());
                var eventNames = metadataProperties["CLRType"].Split(',')[0].Split('.');
                var eventName = eventNames[eventNames.Length - 1];

                var markovEvent = new MarkovEvent(eventName, eventProperties, occurredOn, rawEvent);

                markovEvents.Add(markovEvent);
            }

            discriminators.AddDiscriminator(x =>
            {
                switch (x)
                {
                    case ChainLink l when l.MarkovEvent.EventType == "Blb": return BraidLinkDiscriminationResult.HardNo;

                    default: return BraidLinkDiscriminationResult.Yes;
                }

            });

            var chainBuilder = ChainBuilder.Build(x =>
            {
                var value = string.Empty;

                //switch (x)
                //{
                //        default:
                //            value = x.Payload["EntityId"].ToString();
                //}
                value = x.Payload["EntityName"].ToString();

                return ChainIdentity.Build(value);
            }, LongitudinalEvaluators.Build());

            var braidBuilder = BraidBuilder.Build(discriminators, chainBuilder, LongitudinalEvaluators.Build());

            trunk.AddBraid(braidBuilder);
            Console.WriteLine(markovEvents.Count);
            foreach (var markovEvent in markovEvents)
            {
                trunk.Accept(markovEvent);
            }

            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
    }
}
