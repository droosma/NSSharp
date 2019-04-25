using System.Net.Http;
using System.Threading.Tasks;

namespace NSSharp.Utilities
{
    internal static class HttpContentExtensions
    {
        public static async Task<T> Read<T>(this HttpContent content, IJsonConverter jsonConverter)
        {
            var value = await content.ReadAsStringAsync();
            return jsonConverter.Deserialize<T>(value);
        }
    }
}