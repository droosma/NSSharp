using Newtonsoft.Json;

namespace NSSharp.Tests.Unit.Utilities
{
    internal class NewtonsoftJsonConverter : IJsonConverter
    {
        public T Deserialize<T>(string value) => JsonConvert.DeserializeObject<T>(value);
    }
}