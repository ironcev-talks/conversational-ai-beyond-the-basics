using System;

namespace FragenZurWurst.Model
{
    internal enum SausageKind
    {
        Woidvierdler,
        Buren,
        Schoafe,
        Kaesekrainer
    }

    internal static class SausageKindHandler
    {
        public static string ToDisplayText(this SausageKind sausageKind)
        {
            return sausageKind switch
            {
                SausageKind.Kaesekrainer => "Käsekrainer",
                _ => sausageKind.ToString()
            };
        }

        public static string ToOrderSentenceText(this SausageKind sausageKind)
        {
            return ToDisplayText(sausageKind);
        }

        public static SausageKind FromDisplayText(string displayText)
        {
            return displayText switch
            {
                "Käsekrainer" => SausageKind.Kaesekrainer,
                _ => Enum.Parse<SausageKind>(displayText, true)
            };
        }
    }
}
