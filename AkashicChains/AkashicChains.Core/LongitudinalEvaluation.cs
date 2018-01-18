using System;
using System.Collections.Generic;
using System.Text;

namespace AkashicChains.Core
{
    public class LongitudinalEvaluation
    {
        public List<EvaluationCoordinate> Values { get; set; }
        public string State { get; set; }

        public LongitudinalEvaluation()
        {
            State = null;
            Values = new List<EvaluationCoordinate>();
        }

    }
}
