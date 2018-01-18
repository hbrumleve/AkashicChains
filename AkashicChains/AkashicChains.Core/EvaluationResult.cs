using System;
using System.Collections.Generic;
using System.Text;

namespace AkashicChains.Core
{
    public class EvaluationResult
    {
        public EvaluationResult()
        {
            Value = new EvaluationCoordinate();
            HasNoValue = false;
            State = null;
        }

        public EvaluationCoordinate Value { get; set; }

        public bool HasNoValue { get; set; }

        public string State { get; set; }
    }
}
