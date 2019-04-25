using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace NSSharp.Utilities
{
    internal static class Timeing
    {
        public static async Task<(T Result, TimeSpan Elapsed)> Time<T>(Func<Task<T>> func)
        {
            var stopwatch = Stopwatch.StartNew();
            var result = await func();
            stopwatch.Stop();

            return (result, stopwatch.Elapsed);
        }
    }
}