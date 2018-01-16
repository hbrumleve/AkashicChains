using System;
using System.Collections.Generic;
using System.Text;

namespace AkashicChains.Core
{
    public class LongitudinalEvaluator
    {
        public string Name { get; private set; }

        public string Evaluator { get; private set; }

        private LongitudinalEvaluator(string name, string evaluator)
        {
            Name = name;
            Evaluator = evaluator;
        }

        public static LongitudinalEvaluator Build(string name, string evaluator)
        {
            // parse the evaluator with JINT
            return new LongitudinalEvaluator(name, evaluator);
        }
    }
}
