using Resources;
using System;

namespace FragenZurWurst.Model
{
    public enum Sauce
    {
        Senf,
        Ketchup,
        Beides
    }

    internal static class SauceHandler
    {
        public static string ToDisplayText(this Sauce sauce)
        {
            return sauce switch
            {
                Sauce.Senf => Resource.SauceSenf,
                Sauce.Ketchup => Resource.SauceKetchup,
                Sauce.Beides => Resource.SauceBeides,
                _ => sauce.ToString()
            };
        }

        public static string ToOrderSentenceTextDativ(this Sauce sauce)
        {
            return sauce switch
            {
                Sauce.Senf => Resource.SauceSenfOrderSentenceTextDativ,
                Sauce.Ketchup => Resource.SauceKetchupOrderSentenceTextDativ,
                Sauce.Beides => Resource.SauceBeidesOrderSentenceTextDativ,
                _ => sauce.ToString()
            };
        }

        public static string ToOrderSentenceTextNominativ(this Sauce sauce)
        {
            return sauce switch
            {
                Sauce.Senf => Resource.SauceSenfOrderSentenceTextNominativ,
                Sauce.Ketchup => Resource.SauceKetchupOrderSentenceTextNominativ,
                Sauce.Beides => Resource.SauceBeidesOrderSentenceTextNominativ,
                _ => sauce.ToString()
            };
        }

        public static Sauce FromDisplayText(string displayText)
        {
            if (displayText == Resource.SauceSenf) return Sauce.Senf;
            if (displayText == Resource.SauceKetchup) return Sauce.Ketchup;
            if (displayText == Resource.SauceBeides) return Sauce.Beides;
            return Enum.Parse<Sauce>(displayText, true);
        }
    }
}
