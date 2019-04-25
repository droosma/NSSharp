using System;

using NSSharp.Entities;

namespace NSSharp.Utilities
{
    internal static class LanguageExtensions
    {
        public static string AsNamedValue(this Language language)
        {
            const string dutchString = "nl";
            const string englishString = "en";

            switch(language)
            {
                case Language.Dutch:
                    return dutchString;
                case Language.English:
                    return englishString;
                default:
                    throw new ArgumentOutOfRangeException(nameof(language), language, null);
            }
        }
    }
}