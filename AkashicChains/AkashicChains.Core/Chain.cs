using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AkashicChains.Core
{
    public class Chain
    {
        public Braid Braid { get; private set; }
        public ChainIdentity ChainIdentity { get; private set; }
        private readonly List<ChainLink> _chainLinks = new List<ChainLink>();
        public IReadOnlyList<ChainLink> ChainLinks => _chainLinks;
        private readonly LongitudinalEvaluations _evaluations;
        private readonly LongitudinalEvaluators _longitudinalEvaluators;

        private Chain(ChainIdentity chainIdentity, Braid braid, LongitudinalEvaluators longitudinalEvaluators, LongitudinalEvaluations evaluations)
        {
            _longitudinalEvaluators = longitudinalEvaluators;
            _evaluations = evaluations;
            ChainIdentity = chainIdentity;
            Braid = braid;
        }

        public static Chain Build(ChainIdentity chainIdentity, Braid braid, LongitudinalEvaluators longitudinalEvaluators)
        {
            var evaluations = new LongitudinalEvaluations();

            longitudinalEvaluators.SetEvaluationDestination(evaluations);

            return new Chain(chainIdentity, braid, longitudinalEvaluators, evaluations);
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

            _longitudinalEvaluators.Evaluate(chainLink);
        }
    }
}
