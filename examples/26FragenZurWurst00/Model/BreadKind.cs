using Resources;
using System;

namespace FragenZurWurst.Model
{
    public enum BreadKind
    {
        Schwoazbrot,
        Scherzl,
        Semmoe,
        Soizgebaeck
    }

    internal static class BreadKindHandler
    {
        public static string ToDisplayText(this BreadKind breadKind)
        {
            return breadKind switch
            {
                BreadKind.Schwoazbrot => Resource.BreadKindSchwoazbrot,
                BreadKind.Scherzl => Resource.BreadKindScherzl,
                BreadKind.Semmoe => Resource.BreadKindSemmoe,
                BreadKind.Soizgebaeck => Resource.BreadKindSoizgebaeck,
                _ => breadKind.ToString()
            };
        }

        public static string ToOrderSentenceText(this BreadKind breadKind)
        {
            return breadKind switch
            {
                BreadKind.Schwoazbrot => Resource.BreadKindSchwoazbrotOrderSentenceText,
                BreadKind.Scherzl => Resource.BreadKindScherzlOrderSentenceText,
                BreadKind.Semmoe => Resource.BreadKindSemmoeOrderSentenceText,
                BreadKind.Soizgebaeck => Resource.BreadKindSoizgebaeckOrderSentenceText,
                _ => breadKind.ToString()
            };
        }

        public static BreadKind FromDisplayText(string displayText)
        {
            if (displayText == Resource.BreadKindSchwoazbrot) return BreadKind.Schwoazbrot;
            if (displayText == Resource.BreadKindScherzl) return BreadKind.Scherzl;
            if (displayText == Resource.BreadKindSemmoe) return BreadKind.Semmoe;
            if (displayText == Resource.BreadKindSoizgebaeck) return BreadKind.Soizgebaeck;
            return Enum.Parse<BreadKind>(displayText, true);
        }
    }
}
