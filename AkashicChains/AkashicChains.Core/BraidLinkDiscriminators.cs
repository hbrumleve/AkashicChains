using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AkashicChains.Core
{
    public class BraidLinkDiscriminators
    {
        List<BraidLinkDiscriminator> _discriminators = new List<BraidLinkDiscriminator>();
        List<BraidLinkDiscriminationResult> _results = new List<BraidLinkDiscriminationResult>();


        public int Discriminate(ChainLink chainLink)
        {
            _results.Clear();

            foreach (var braidLinkDiscriminator in _discriminators)
            {
                _results.Add(braidLinkDiscriminator.Discriminate(chainLink));
            }

            int result = _results.Sum(x => (int)x);

            return result;
        }

        public void AddDiscriminator(Func<ChainLink, BraidLinkDiscriminationResult> discriminatorFunc)
        {
            var discriminator = BraidLinkDiscriminator.Build(discriminatorFunc);

            AddDiscriminator(discriminator);
        }

        public void AddDiscriminator(BraidLinkDiscriminator discriminator)
        {
            _discriminators.Add(discriminator);
        }
    }
}
