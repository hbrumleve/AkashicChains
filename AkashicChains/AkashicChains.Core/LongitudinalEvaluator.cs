using System;
using System.Collections.Generic;
using System.Text;
using Jint;
using Jint.Parser.Ast;

namespace AkashicChains.Core
{
    public class LongitudinalEvaluator
    {
        public string Name { get; private set; }
        public Func<MarkovEvent, dynamic, string> Evaluator { get; private set; }
        public Func<MarkovEvent, MarkovEvent, string, string> DistanceCalculation { get; private set; }

        private LongitudinalEvaluator(string name, Func<MarkovEvent, dynamic, string> evaluator, Func<MarkovEvent, MarkovEvent, string, string> distanceCalculation)
        {
            Name = name;
            Evaluator = evaluator;
            DistanceCalculation = distanceCalculation;
        }

        public static LongitudinalEvaluator Build(string name, string evaluator, string distanceCalculation)
        {
            string FuncdEvaluator(MarkovEvent a, dynamic b)
            {
                var evaluate = new Engine()
                    .Execute(evaluator)
                    .GetValue("Evaluate");

                var messageParserEngine = new Engine().Execute("function parse(o){ return JSON.parse(o);}");
                var jsonParser = messageParserEngine.GetValue("parse");

                var parsedEvent = jsonParser.Invoke(a.Original);
                var parsedState = jsonParser.Invoke(b.ToString());

                var evaluation = evaluate.Invoke(parsedEvent, parsedState);

                return evaluation.ToString();
            }

            string FuncdCalculator(MarkovEvent a, MarkovEvent b, string state, Func<MarkovEvent, dynamic, string> funcdEvaluator)
            {
                var distanceCalculator = new Engine()
                    .Execute(distanceCalculation)
                    .SetValue("Evaluate", funcdEvaluator)
                    .GetValue("Distance");

                var messageParserEngine = new Engine().Execute("function parse(o){ return JSON.parse(o);}");
                var jsonParser = messageParserEngine.GetValue("parse");

                var eventA = jsonParser.Invoke(a.Original);
                var eventB = jsonParser.Invoke(b.Original);
                var parsedState = jsonParser.Invoke(state);

                var distance = distanceCalculator.Invoke(eventA, eventB, parsedState);

                return distance.ToString();
            }

            string CurriedFuncdCalculator(MarkovEvent a, MarkovEvent b, string state)
            {
                return FuncdCalculator(a, b, state, FuncdEvaluator);
            }

            return new LongitudinalEvaluator(name, FuncdEvaluator, CurriedFuncdCalculator);
        }

        public static LongitudinalEvaluator Build(string name, Func<MarkovEvent, dynamic, string> evaluator, Func<MarkovEvent, MarkovEvent, string, Func<MarkovEvent, dynamic, string>, string> distanceCalculation)
        {
            return new LongitudinalEvaluator(name, evaluator, (a, b, s) => distanceCalculation(a, b, s, evaluator));
        }
    }
}
