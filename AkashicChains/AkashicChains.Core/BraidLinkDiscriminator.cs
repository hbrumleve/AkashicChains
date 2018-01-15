using System;
using System.Collections.Generic;
using System.Text;

namespace AkashicChains.Core
{
    public class BraidLinkDiscriminator
    {
        public BraidLinkDiscriminator()
        {

        }

        private BraidLinkDiscriminator(Func<ChainLink, BraidLinkDiscriminationResult> discrimate)
        {
            _discrimate = discrimate;
        }

        public static BraidLinkDiscriminator Build(Func<ChainLink, BraidLinkDiscriminationResult> discriminatorFunc)
        {
            return new BraidLinkDiscriminator(discriminatorFunc);
        }

        private readonly Func<ChainLink, BraidLinkDiscriminationResult> _discrimate;

        public virtual BraidLinkDiscriminationResult Discriminate(ChainLink chainLink)
        {
            return _discrimate(chainLink);
        }

    }
}
