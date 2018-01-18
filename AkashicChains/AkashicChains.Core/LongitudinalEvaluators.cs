using System;
using System.Collections.Generic;
using System.Text;

namespace AkashicChains.Core
{
    public class LongitudinalEvaluators
    {
        private LongitudinalEvaluators()
        {

        }

        public static LongitudinalEvaluators Build() => new LongitudinalEvaluators();

        public void AddEvaluator(LongitudinalEvaluator evaluator)
        {

        }

        public void SetEvaluationDestination(LongitudinalEvaluations evaluations)
        {
            throw new NotImplementedException();
        }

        public void Evaluate(ChainLink chainLink)
        {
            throw new NotImplementedException();
        }
    }
}
