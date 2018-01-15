using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;

namespace AkashicChains.Core
{
    public class ChainLink
    {
        readonly ISubject<ChainPosition> _addToChainSubject = new Subject<ChainPosition>();

        readonly List<ChainPosition> _chainPositions = new List<ChainPosition>();

        public MarkovEvent MarkovEvent { get; private set; }

        private ChainLink(MarkovEvent markovEvent)
        {
            MarkovEvent = markovEvent;

            _addToChainSubject.Subscribe(_chainPositions.Add);
        }

        public static ChainLink Build(MarkovEvent markovEvent)
        {
            return new ChainLink(markovEvent);
        }

        internal void AddToChain(ChainPosition chainPosition)
        {
            _addToChainSubject.OnNext(chainPosition);
        }
    }
}
