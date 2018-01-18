using System;
using System.Collections.Generic;
using System.Text;

namespace AkashicChains.Core
{
    public class ChainBuilder
    {
        private Action<ChainIdentity, Chain> _addChain;
        private Predicate<ChainIdentity> _chainWithIdentityExists;
        private readonly Func<MarkovEvent, ChainIdentity> _buildChainIdentity;
        private Func<ChainIdentity, Chain> _getChainByIdentity;
        private readonly LongitudinalEvaluators _evaluators;
        private Braid _braid;


        private ChainBuilder(Func<MarkovEvent, ChainIdentity> buildChainIdentity, LongitudinalEvaluators evaluators)
        {
            _buildChainIdentity = buildChainIdentity;
            _evaluators = evaluators;
        }

        public static ChainBuilder Build(Func<MarkovEvent, ChainIdentity> buildChainIdentity, LongitudinalEvaluators evaluators)
        {
            return new ChainBuilder(buildChainIdentity, evaluators);
        }

        internal void AddBraid(Braid braid)
        {
            _braid = braid;
        }

        public void Accept(ChainLink chainLink)
        {
            var chainIdentity = _buildChainIdentity(chainLink.MarkovEvent);

            if (!_chainWithIdentityExists(chainIdentity))
            {
                var newChain = Chain.Build(chainIdentity, _braid, _evaluators);

                _addChain(chainIdentity, newChain);
            }

            var chain = _getChainByIdentity(chainIdentity);

            chain.AddChainLink(chainLink);

        }

        internal void AcceptAddChainAction(Predicate<ChainIdentity> checkChain, Action<ChainIdentity, Chain> addChain, Func<ChainIdentity, Chain> getChain)
        {
            _chainWithIdentityExists = checkChain;
            _addChain = addChain;
            _getChainByIdentity = getChain;
        }


    }
}
