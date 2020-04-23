using System;

namespace FragenZurWurst.Model
{
    internal enum SauceTaste
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
                SauceTaste.Siass => "Siaß",
                _ => sauceTaste.ToString()
            };
        }

        public static string ToOrderSentenceText(this SauceTaste sauceTaste)
        {
            return ToDisplayText(sauceTaste).ToLower();
        }

        public static SauceTaste FromDisplayText(string displayText)
        {
            return displayText switch
            {
                "Siaß" => SauceTaste.Siass,
                _ => Enum.Parse<SauceTaste>(displayText, true)
            };
        }
    }
}
