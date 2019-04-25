using System;

namespace NSSharp.Entities
{
    public class UicCode
    {
        private string Value { get; }

        private UicCode(string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public static UicCode Create(string value) => new UicCode(value);

        public override string ToString() => Value;
        public override int GetHashCode() => Value.GetHashCode();

        public override bool Equals(object obj)
        {
            if(Value == null || obj == null)
                return false;

            if(obj.GetType() != GetType())
                return false;

            if(obj is string s)
                return string.Equals(Value, s, StringComparison.Ordinal);

            var otherString = $"{obj}";
            return string.Equals(Value, otherString, StringComparison.Ordinal);
        }
    }
}