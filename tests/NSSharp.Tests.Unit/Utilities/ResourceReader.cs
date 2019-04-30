using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NSSharp.Tests.Unit.Utilities
{
    internal static class ResourceReader
    {
        public static string Read(string resource)
        {
            if(string.IsNullOrWhiteSpace(resource))
                throw new ArgumentNullException(nameof(resource));

            var assembly = Assembly.GetExecutingAssembly();

            if(!assembly.GetManifestResourceNames().Contains(resource))
                throw new ArgumentException($"{resource} not found. {string.Join(";", assembly.GetManifestResourceNames())}", nameof(resource));

            using(var stream = assembly.GetManifestResourceStream(resource))
                using(var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
        }
    }
}