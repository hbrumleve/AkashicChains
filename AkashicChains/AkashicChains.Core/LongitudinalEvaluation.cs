using System;
using System.Collections.Generic;
using System.Text;

namespace AkashicChains.Core
{
    public class LongitudinalEvaluation
    {
        public List<LongitudinalCoordinate> Values { get; set; }
        public string State { get; set; }

        public LongitudinalEvaluation()
        {
            Values = new List<LongitudinalCoordinate>();
        }

    }
}
