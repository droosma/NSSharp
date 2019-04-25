using System;

namespace NSSharp.Entities
{
    public sealed class StationCode
    {
        private string Value { get; }

        private StationCode(string value)
        {
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public static StationCode Create(string value) => new StationCode(value);

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