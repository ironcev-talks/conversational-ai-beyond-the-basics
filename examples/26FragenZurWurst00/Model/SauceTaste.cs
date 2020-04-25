using Resources;
using System;

namespace FragenZurWurst.Model
{
    public enum SauceTaste
    {
        Siass,
        Schoaf
    }

    internal static class SauceTasteHandler
    {
        public static string ToDisplayText(this SauceTaste sauceTaste)
        {
            return sauceTaste switch
            {
                SauceTaste.Siass => Resource.SauceTasteSiass,
                SauceTaste.Schoaf => Resource.SauceTasteSchoaf,
                _ => sauceTaste.ToString()
            };
        }

        public static string ToOrderSentenceText(this SauceTaste sauceTaste)
        {
            return sauceTaste switch
            {
                SauceTaste.Siass => Resource.SauceTasteSiassOrderSentenceText,
                SauceTaste.Schoaf => Resource.SauceTasteSchoafOrderSentenceText,
                _ => sauceTaste.ToString()
            };
        }

        public static SauceTaste FromDisplayText(string displayText)
        {
            if (displayText == Resource.SauceTasteSiass) return SauceTaste.Siass;
            if (displayText == Resource.SauceTasteSchoaf) return SauceTaste.Schoaf;
            return Enum.Parse<SauceTaste>(displayText, true);
        }
    }
}
