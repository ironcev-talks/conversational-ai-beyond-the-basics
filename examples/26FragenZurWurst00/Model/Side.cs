using Resources;
using System;

namespace FragenZurWurst.Model
{
    internal enum Side
    {
        Gurkel,
        Pfeffaroni,
        Nix
    }

    internal static class SideHandler
    {
        public static string ToDisplayText(this Side side)
        {
            return side switch
            {
                Side.Gurkel => Resource.SideGurkel,
                Side.Pfeffaroni => Resource.SidePfeffaroni,
                Side.Nix => Resource.SideNix,
                _ => side.ToString()
            };
        }

        public static string ToOrderSentenceText(this Side side)
        {
            return side switch
            {
                Side.Gurkel => Resource.SideGurkelOrderSentenceText,
                Side.Pfeffaroni => Resource.SidePfeffaroniOrderSentenceText,
                Side.Nix => Resource.SideNixOrderSentenceText,
                _ => side.ToString()
            };
        }

        public static Side FromDisplayText(string displayText)
        {
            if (displayText == Resource.SideGurkel) return Side.Gurkel;
            if (displayText == Resource.SidePfeffaroni) return Side.Pfeffaroni;
            if (displayText == Resource.SideNix) return Side.Nix;
            return Enum.Parse<Side>(displayText, true);
        }
    }
}
