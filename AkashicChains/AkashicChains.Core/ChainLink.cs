using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using System.Text;

namespace AkashicChains.Core
{
    public class ChainLink
    {
        readonly List<ChainPosition> _chainPositions = new List<ChainPosition>();

        public IReadOnlyList<ChainPosition> ChainPositions => _chainPositions;

        public MarkovEvent MarkovEvent { get; private set; }

        private ChainLink(MarkovEvent markovEvent)
        {
            MarkovEvent = markovEvent;
        }

        public static ChainLink Build(MarkovEvent markovEvent)
        {
            return new ChainLink(markovEvent);
        }

        internal void AddToChain(ChainPosition chainPosition)
        {
            _chainPositions.Add(chainPosition);
        }
    }
}
