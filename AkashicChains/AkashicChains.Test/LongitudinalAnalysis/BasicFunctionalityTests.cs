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
        public void Test1()
        {
            var eventA = new NameChanged { EntityName = "1234", Name = "Bob", OccurredOn = DateTime.UtcNow };
            var eventB = new NameChanged { EntityName = "1234", Name = "Jimmy", OccurredOn = DateTime.UtcNow.AddDays(1) };

            var markovA = MarkovEvent.Build("NameChanged", eventA, eventA.OccurredOn);
            var markovB = MarkovEvent.Build("NameChanged", eventB, eventB.OccurredOn);

            var trunk = new Trunk();

            var braidDiscriminators = new BraidLinkDiscriminators();
            braidDiscriminators.AddDiscriminator(x => BraidLinkDiscriminationResult.ISaidYes);

            var chainLongitudinalEvaluators = LongitudinalEvaluators.Build();

            chainLongitudinalEvaluators.AddEvaluator(LongitudinalEvaluator.Build("Identity", (a, s) => JsonConvert.SerializeObject(a), (a, b, e, s) => JsonConvert.SerializeObject(a.OccurredOn - b.OccurredOn)));

            var chainBuilder = ChainBuilder.Build(x => ChainIdentity.Build(x.Payload["EntityName"].ToString()), chainLongitudinalEvaluators);

            // Braid needs a name!
            var braidBuilder = BraidBuilder.Build("Initial", braidDiscriminators, chainBuilder, LongitudinalEvaluators.Build());

            trunk.AddBraid(braidBuilder);

            trunk.Accept(markovA);
            trunk.Accept(markovB);


        }
    }
}
