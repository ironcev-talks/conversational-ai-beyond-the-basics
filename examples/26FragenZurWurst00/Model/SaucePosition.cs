using Resources;
using System;

namespace FragenZurWurst.Model
{
    public enum SaucePosition
    {
        Drauf,
        Daneben
    }

    internal static class SaucePositionHandler
    {
        public static string ToDisplayText(this SaucePosition saucePosition)
        {
            return saucePosition switch
            {
                SaucePosition.Drauf => Resource.SaucePositionDrauf,
                SaucePosition.Daneben => Resource.SaucePositionDaneben,
                _ => saucePosition.ToString()
            };
        }

        public static string ToOrderSentenceText(this SaucePosition saucePosition)
        {
            return saucePosition switch
            {
                SaucePosition.Drauf => Resource.SaucePositionDraufOrderSentenceText,
                SaucePosition.Daneben => Resource.SaucePositionDanebenOrderSentenceText,
                _ => saucePosition.ToString()
            };
        }

        public static SaucePosition FromDisplayText(string displayText)
        {
            if (displayText == Resource.SaucePositionDrauf) return SaucePosition.Drauf;
            if (displayText == Resource.SaucePositionDaneben) return SaucePosition.Daneben;
            return Enum.Parse<SaucePosition>(displayText, true);
        }
    }
}
