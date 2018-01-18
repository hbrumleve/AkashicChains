using System;
using System.Collections.Generic;
using System.Text;
using AkashicChains.Core;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace AkashicChains.Test.LongitudinalAnalysis
{
    [TestFixture]
    public class BasicFunctionalityTests
    {
        public class NameChanged
        {
            public string Name { get; set; }
            public string EntityName { get; set; }
            public DateTime OccurredOn { get; set; }

        }

        [Test]
        public void TestInCSharp()
        {
            var eventA = new NameChanged { EntityName = "1234", Name = "Bob", OccurredOn = DateTime.UtcNow };
            var eventB = new NameChanged { EntityName = "1234", Name = "Jimmy", OccurredOn = DateTime.UtcNow.AddDays(1) };

            var markovA = MarkovEvent.Build("NameChanged", eventA, eventA.OccurredOn);
            var markovB = MarkovEvent.Build("NameChanged", eventB, eventB.OccurredOn);

            var trunk = new Trunk();

            var braidDiscriminators = new BraidLinkDiscriminators();
            braidDiscriminators.AddDiscriminator(x => BraidLinkDiscriminationResult.ISaidYes);

            var chainLongitudinalEvaluators = LongitudinalEvaluators.Build();

            chainLongitudinalEvaluators.AddEvaluator(LongitudinalEvaluator.Build("Identity", () => JsonConvert.SerializeObject(0), (a, s) =>
             {
                 var state = JsonConvert.DeserializeObject<int>(s);

                 Console.WriteLine(state);

                 state++;

                 s = JsonConvert.SerializeObject(state);

                 var result = new EvaluationResult();

                 result.Value.X = JsonConvert.SerializeObject(a);
                 result.Value.Y = JsonConvert.SerializeObject(a.OccurredOn);

                 result.State = s;

                 return result;
             }, (a, b, e, s) => JsonConvert.SerializeObject(a.OccurredOn - b.OccurredOn)));

            var chainBuilder = ChainBuilder.Build(x => ChainIdentity.Build(x.Payload["EntityName"].ToString()), chainLongitudinalEvaluators);

            var braidBuilder = BraidBuilder.Build("Initial", braidDiscriminators, chainBuilder, LongitudinalEvaluators.Build());

            trunk.AddBraid(braidBuilder);

            trunk.Accept(markovA);
            trunk.Accept(markovB);


        }

        [Test]
        public void TestInJavaScript()
        {
            var eventAOccurredOn = DateTime.UtcNow;
            var eventBOccurredOn = DateTime.UtcNow.AddDays(1);

            var eventA = "{ \"EntityName\" : \"1234\", \"Name\" : \"Bob\", \"OccurredOn\" : \"" + eventAOccurredOn + "\"}";
            var eventB = "{ \"EntityName\" : \"1234\", \"Name\" : \"Jimmy\", \"OccurredOn\" : \"" + eventBOccurredOn + "\"}";

            var markovA = MarkovEvent.BuildFromJson("NameChanged", eventA, eventAOccurredOn);
            var markovB = MarkovEvent.BuildFromJson("NameChanged", eventB, eventBOccurredOn);

            var trunk = new Trunk();

            var braidDiscriminators = new BraidLinkDiscriminators();
            braidDiscriminators.AddDiscriminator(x => BraidLinkDiscriminationResult.ISaidYes);

            var chainLongitudinalEvaluators = LongitudinalEvaluators.Build();

            string jsInitialize = "function Initialize() {return 0;}";

            string jsEvaluator = " function Evaluate(n, e, s) {" +
                                 " " +
                                 " s += 1;" +
                                 " var result = {};" +
                                 " result.Value = {};" +
                                 " result.Value.X = JSON.stringify(e);" +
                                 " result.Value.Y = e.OccurredOn;" +
                                 " result.State = s;" +
                                 " return result;" +

                                 " }";

            string jsDistance = " function Distance(eventA, eventB, evaluator, state) { return eventA.OccurredOn - eventB.OccurredOn; } ";

            chainLongitudinalEvaluators.AddEvaluator(LongitudinalEvaluator.Build("Identity", jsInitialize, jsEvaluator, jsDistance));

            var chainBuilder = ChainBuilder.Build(x => ChainIdentity.Build(x.Payload["EntityName"].ToString()), chainLongitudinalEvaluators);

            var braidBuilder = BraidBuilder.Build("Initial", braidDiscriminators, chainBuilder, LongitudinalEvaluators.Build());

            trunk.AddBraid(braidBuilder);

            trunk.Accept(markovA);
            trunk.Accept(markovB);


        }
    }
}
