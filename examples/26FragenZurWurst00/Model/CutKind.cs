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
                CutKind.Aufschneiden => "Aufschneiden",
                CutKind.ZwaHoeften => "Zwa Höften",
                CutKind.ImGonzn => "Im Gonzn",
                _ => cutKind.ToString()
            };
        }

        public static string ToOrderSentenceText(this CutKind cutKind)
        {
            return cutKind switch
            {
                CutKind.Aufschneiden => "aufgeschnitten",
                CutKind.ZwaHoeften => "in zwa Höften",
                CutKind.ImGonzn => "im Gonzn",
                _ => cutKind.ToString()
            };
        }

        public static CutKind FromDisplayText(string displayText)
        {
            return displayText switch
            {
                "Aufschneiden" => CutKind.Aufschneiden,
                "Zwa Höften" => CutKind.ZwaHoeften,
                "Im Gonzn" => CutKind.ImGonzn,
                _ => Enum.Parse<CutKind>(displayText, true)
            };
        }
    }
}
