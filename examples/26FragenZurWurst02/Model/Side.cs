using System;

namespace FragenZurWurst.Model
{
    public enum Side
    {
        Gurkel,
        Pfeffaroni,
        Nix
    }

    internal static class SideHandler
    {
        public static string ToDisplayText(this Side side)
        {
            return side.ToString();
        }

        public static string ToOrderSentenceText(this Side side)
        {
            return ToDisplayText(side);
        }

        public static Side FromDisplayText(string displayText)
        {
            return Enum.Parse<Side>(displayText, true);
        }
    }
}
