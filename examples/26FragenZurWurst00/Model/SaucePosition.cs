using System;

namespace FragenZurWurst.Model
{
    internal enum SaucePosition
    {
        Drauf,
        Daneben
    }

    internal static class SaucePositionHandler
    {
        public static string ToDisplayText(this SaucePosition saucePosition)
        {
            return saucePosition.ToString();
        }

        public static string ToOrderSentenceText(this SaucePosition saucePosition)
        {
            return ToDisplayText(saucePosition).ToLower();
        }

        public static SaucePosition FromDisplayText(string displayText)
        {
            return Enum.Parse<SaucePosition>(displayText, true);
        }
    }
}
