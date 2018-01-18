using System;
using System.Collections.Generic;
using System.Text;
using Jint;
using Jint.Parser.Ast;
using Newtonsoft.Json;

namespace AkashicChains.Core
{
    public class LongitudinalEvaluator
    {
        public string Name { get; private set; }
        public Func<string> StateInitializer { get; private set; }
        public Func<MarkovEvent, string, EvaluationResult> Evaluator { get; private set; }
        public Func<MarkovEvent, MarkovEvent, string, string> DistanceCalculation { get; private set; }

        private LongitudinalEvaluator(string name, Func<string> stateInitializer, Func<MarkovEvent, string, EvaluationResult> evaluator, Func<MarkovEvent, MarkovEvent, string, string> distanceCalculation)
        {
            Name = name;
            StateInitializer = stateInitializer;
            Evaluator = evaluator;
            DistanceCalculation = distanceCalculation;
        }

        public static LongitudinalEvaluator Build(string name, string stateInitializer, string evaluator, string distanceCalculation)
        {
            string InitializeState()
            {
                var initializer = new Engine()
                    .Execute(stateInitializer)
                    .GetValue("Initialize");

                var initialState = initializer.Invoke();

                return initialState.ToString();
            }

            EvaluationResult FuncdEvaluator(MarkovEvent a, string b)
            {
                var evaluate = new Engine()
                    .Execute(evaluator)
                    .GetValue("Evaluate");

                var messageParserEngine = new Engine().Execute("function parse(o){ return JSON.parse(o);}");
                var jsonParser = messageParserEngine.GetValue("parse");

                var messageStringifyEngine = new Engine().Execute("function stringify(o){ return JSON.stringify(o);}");
                var jsonStringify = messageStringifyEngine.GetValue("stringify");

                var s = JsonConvert.SerializeObject(a);

                var parsedEvent = jsonParser.Invoke(s);
                var parsedState = jsonParser.Invoke(b);

                var evaluation = evaluate.Invoke(parsedEvent, parsedState);

                var parsedEvaluation = jsonStringify.Invoke(evaluation);

                var longitudinalCoordinate = JsonConvert.DeserializeObject<EvaluationResult>(parsedEvaluation.ToString());

                return longitudinalCoordinate;
            }

            string FuncdCalculator(MarkovEvent a, MarkovEvent b, string state, Func<MarkovEvent, string, EvaluationResult> funcdEvaluator)
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

            return new LongitudinalEvaluator(name, InitializeState, FuncdEvaluator, CurriedFuncdCalculator);
        }

        public static LongitudinalEvaluator Build(string name, Func<string> stateInitializer, Func<MarkovEvent, string, EvaluationResult> evaluator, Func<MarkovEvent, MarkovEvent, string, Func<MarkovEvent, string, EvaluationResult>, string> distanceCalculation)
        {
            return new LongitudinalEvaluator(name, stateInitializer, evaluator, (a, b, s) => distanceCalculation(a, b, s, evaluator));
        }
    }
}
