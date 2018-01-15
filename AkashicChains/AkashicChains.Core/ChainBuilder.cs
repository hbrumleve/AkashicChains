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
        private readonly LongitudinalEvaluators _evaluators = new LongitudinalEvaluators();


        public ChainBuilder(Func<MarkovEvent, ChainIdentity> buildChainIdentity)
        {
            _buildChainIdentity = buildChainIdentity;
        }

        public void Accept(ChainLink chainLink)
        {
            var chainIdentity = _buildChainIdentity(chainLink.MarkovEvent);

            if (!_chainWithIdentityExists(chainIdentity))
            {
                var newChain = Chain.Build(chainIdentity);

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
