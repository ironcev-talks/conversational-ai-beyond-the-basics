using Resources;
using System;

namespace FragenZurWurst.Model
{
    internal enum CutKind
    {
        Aufschneiden,
        ZwaHoeften,
        ImGonzn
    }

    internal static class CutKindHandler
    {
        public static string ToDisplayText(this CutKind cutKind)
        {
            return cutKind switch
            {
                CutKind.Aufschneiden => Resource.CutKindAufschneiden,
                CutKind.ZwaHoeften => Resource.CutKindZwaHoeften,
                CutKind.ImGonzn => Resource.CutKindImGonzn,
                _ => cutKind.ToString()
            };
        }

        public static string ToOrderSentenceText(this CutKind cutKind)
        {
            return cutKind switch
            {
                CutKind.Aufschneiden => Resource.CutKindAufschneidenOrderSentenceText,
                CutKind.ZwaHoeften => Resource.CutKindZwaHoeftenOrderSentenceText,
                CutKind.ImGonzn => Resource.CutKindImGonznOrderSentenceText,
                _ => cutKind.ToString()
            };
        }

        public static CutKind FromDisplayText(string displayText)
        {
            if (displayText == Resource.CutKindAufschneiden) return CutKind.Aufschneiden;
            if (displayText == Resource.CutKindZwaHoeften) return CutKind.ZwaHoeften;
            if (displayText == Resource.CutKindImGonzn) return CutKind.ImGonzn;
            return Enum.Parse<CutKind>(displayText, true);
        }
    }
}
