using System;
using System.Collections.Generic;
using System.Text;

namespace AkashicChains.Core
{
    class ChainPosition
    {
        public ChainPosition(Chain chain, int position, LongitudinalDistance distance)
        {
            Chain = chain;
            Position = position;

        }

        public Chain Chain { get; private set; }
        public int Position { get; private set; }
    }
}
