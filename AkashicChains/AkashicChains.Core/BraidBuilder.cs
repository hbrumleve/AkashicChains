using System;
using System.Collections.Generic;
using System.Text;

namespace AkashicChains.Core
{
    public class BraidBuilder
    {
        readonly BraidLinkDiscriminators _discriminators;
        readonly ChainBuilder _chainBuilder;
        readonly int _addThreshold;

        private BraidBuilder(BraidLinkDiscriminators discriminators, ChainBuilder chainBuilder, int addThreshold)
        {
            this._discriminators = discriminators;
            this._chainBuilder = chainBuilder;
            this._addThreshold = addThreshold;
        }

        public static BraidBuilder Build(BraidLinkDiscriminators discriminators, ChainBuilder chainBuilder, int addThreshold = 1)
        {
            var braidBuilder = new BraidBuilder(discriminators, chainBuilder, addThreshold);

            return braidBuilder;
        }

        public Braid BuildBraid(Trunk trunk)
        {
            var braid = Braid.Build(_discriminators, _chainBuilder, _addThreshold);

            return braid;
        }
    }
}
