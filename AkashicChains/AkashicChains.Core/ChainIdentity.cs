using System;
using System.Collections.Generic;
using System.Text;

namespace AkashicChains.Core
{
    public class ChainIdentity
    {
        public string Value { get; private set; }

        private ChainIdentity(string value)
        {
            Value = value;
        }

        public static ChainIdentity Build(string value)
        {
            return new ChainIdentity(value);
        }

        protected bool Equals(ChainIdentity other)
        {
            return string.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ChainIdentity)obj);
        }

        public override int GetHashCode()
        {
            return (Value != null ? Value.GetHashCode() : 0);
        }
    }
}
