using System;
using System.Collections.Generic;
using System.Text;

namespace AkashicChains.Core
{
    public class BraidBuilder
    {
        readonly BraidLinkDiscriminators _discriminators;
        readonly ChainBuilder _chainBuilder;
        private readonly LongitudinalEvaluators _evaluators;
        readonly int _addThreshold;
        public string Name { get; private set; }

        private BraidBuilder(string name, BraidLinkDiscriminators discriminators, ChainBuilder chainBuilder, LongitudinalEvaluators evaluators, int addThreshold)
        {
            Name = name;
            this._discriminators = discriminators;
            this._chainBuilder = chainBuilder;
            _evaluators = evaluators;
            this._addThreshold = addThreshold;
        }

        public static BraidBuilder Build(string name, BraidLinkDiscriminators discriminators, ChainBuilder chainBuilder, LongitudinalEvaluators evaluators, int addThreshold = 1)
        {
            var braidBuilder = new BraidBuilder(name, discriminators, chainBuilder, evaluators, addThreshold);

            return braidBuilder;
        }

        public Braid BuildBraid(Trunk trunk)
        {
            var braid = Braid.Build(_discriminators, _chainBuilder, _addThreshold);

            return braid;
        }
    }
}
