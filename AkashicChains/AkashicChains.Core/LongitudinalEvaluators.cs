using System;
using System.Collections.Generic;
using System.Text;

namespace AkashicChains.Core
{
    public class LongitudinalEvaluators
    {
        private LongitudinalEvaluations _evaluations;
        private readonly Dictionary<string, LongitudinalEvaluator> _evaluators = new Dictionary<string, LongitudinalEvaluator>();

        private LongitudinalEvaluators()
        {

        }

        public static LongitudinalEvaluators Build() => new LongitudinalEvaluators();

        public void AddEvaluator(LongitudinalEvaluator evaluator)
        {
            _evaluators.Add(evaluator.Name, evaluator);
        }

        internal void SetEvaluationDestination(LongitudinalEvaluations evaluations)
        {
            _evaluations = evaluations;
        }

        public void Evaluate(ChainLink chainLink)
        {
            foreach (var longitudinalEvaluators in _evaluators)
            {
                if (!_evaluations.Evaluations.ContainsKey(longitudinalEvaluators.Key))
                {
                    var newEvaluation = new LongitudinalEvaluation();

                    newEvaluation.State = longitudinalEvaluators.Value.StateInitializer();

                    _evaluations.Evaluations.Add(longitudinalEvaluators.Key, newEvaluation);
                }

                var evaluation = _evaluations.Evaluations[longitudinalEvaluators.Key];

                var state = evaluation.State;

                // I think this isn't right because state may not be malleable ... should it be passed back as a complex type?
                var result = longitudinalEvaluators.Value.Evaluator(chainLink.MarkovEvent, state);

                if (!result.HasNoValue)
                {
                    evaluation.Values.Add(result.Value);
                }

                if (result.State != null)
                {
                    evaluation.State = result.State;
                }
            }
        }
    }
}
