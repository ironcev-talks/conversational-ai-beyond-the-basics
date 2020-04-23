using System;

namespace FragenZurWurst.Model
{
    internal enum Sauce
    {
        Senf,
        Ketchup,
        Beides
    }

    internal static class SauceHandler
    {
        public static string ToDisplayText(this Sauce sauce)
        {
            return sauce.ToString();
        }

        public static string ToOrderSentenceText(this Sauce sauce)
        {
            return sauce == Sauce.Beides
                ? "Senf und Ketchup"
                : sauce.ToString();
        }

        public static Sauce FromDisplayText(string displayText)
        {
            return Enum.Parse<Sauce>(displayText, true);
        }
    }
}
