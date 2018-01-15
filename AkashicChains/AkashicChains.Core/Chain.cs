﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AkashicChains.Core
{
    class Chain
    {
        public ChainIdentity ChainIdentity { get; private set; }

        private readonly List<ChainLink> _chainLinks = new List<ChainLink>();
        public IReadOnlyList<ChainLink> ChainLinks => _chainLinks;

        private Chain(ChainIdentity chainIdentity)
        {
            ChainIdentity = chainIdentity;
        }

        public static Chain Build(ChainIdentity chainIdentity)
        {
            return new Chain(chainIdentity);
        }

        public void AddChainLink(ChainLink chainLink)
        {
            var longitudinalDistance = LongitudinalDistance.Initial;

            if (_chainLinks.Any())
            {
                var previousLink = _chainLinks[_chainLinks.Count - 1];

                var distance = chainLink.MarkovEvent.OccurredOn - previousLink.MarkovEvent.OccurredOn;

                longitudinalDistance = LongitudinalDistance.Build(distance);
            }

            var position = new ChainPosition(this, _chainLinks.Count, longitudinalDistance);

            chainLink.AddToChain(position);

            _chainLinks.Add(chainLink);
        }
    }
}