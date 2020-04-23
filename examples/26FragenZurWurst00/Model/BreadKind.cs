using System;

namespace FragenZurWurst.Model
{
    internal enum BreadKind
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
                BreadKind.Semmoe => "Semmö",
                BreadKind.Soizgebaeck => "Soizgebäck",
                _ => breadKind.ToString()
            };
        }

        public static string ToOrderSentenceText(this BreadKind breadKind)
        {
            return breadKind switch
            {
                BreadKind.Semmoe => "eine Semmö",
                BreadKind.Soizgebaeck => "ein Soizgebäck",
                _ => "ein " + breadKind.ToString()
            };
        }

        public static BreadKind FromDisplayText(string displayText)
        {
            return displayText switch
            {
                "Semmö" => BreadKind.Semmoe,
                "Soizgebäck" => BreadKind.Soizgebaeck,
                _ => Enum.Parse<BreadKind>(displayText, true)
            };
        }
    }
}
