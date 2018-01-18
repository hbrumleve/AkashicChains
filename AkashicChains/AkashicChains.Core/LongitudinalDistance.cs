using System;
using System.Collections.Generic;
using System.Text;

namespace AkashicChains.Core
{
    public class LongitudinalDistance
    {
        public static LongitudinalDistance Initial => new LongitudinalDistance { IsInitial = true };

        public bool IsInitial { get; private set; }

        public TimeSpan Distance { get; private set; }

        private LongitudinalDistance()
        {

        }

        private LongitudinalDistance(bool isInitial, TimeSpan distance)
        {
            IsInitial = isInitial;
            Distance = distance;
        }

        public static LongitudinalDistance Build(TimeSpan distance)
        {
            return new LongitudinalDistance(false, distance);
        }

    }
}
