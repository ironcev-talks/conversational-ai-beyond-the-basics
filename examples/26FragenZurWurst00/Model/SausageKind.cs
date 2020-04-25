using Resources;
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
                SausageKind.Woidvierdler => Resource.SausageKindWoidvierdler,
                SausageKind.Buren => Resource.SausageKindBuren,
                SausageKind.Schoafe => Resource.SausageKindSchoafe,
                SausageKind.Kaesekrainer => Resource.SausageKindKaesekrainer,
                _ => sausageKind.ToString()
            };
        }

        public static string ToOrderSentenceText(this SausageKind sausageKind)
        {
            return sausageKind switch
            {
                SausageKind.Woidvierdler => Resource.SausageKindWoidvierdlerOrderSentenceText,
                SausageKind.Buren => Resource.SausageKindBurenOrderSentenceText,
                SausageKind.Schoafe => Resource.SausageKindSchoafeOrderSentenceText,
                SausageKind.Kaesekrainer => Resource.SausageKindKaesekrainerOrderSentenceText,
                _ => sausageKind.ToString()
            };
        }

        public static SausageKind FromDisplayText(string displayText)
        {
            if (displayText == Resource.SausageKindWoidvierdler) return SausageKind.Woidvierdler;
            if (displayText == Resource.SausageKindBuren) return SausageKind.Buren;
            if (displayText == Resource.SausageKindSchoafe) return SausageKind.Schoafe;
            if (displayText == Resource.SausageKindKaesekrainer) return SausageKind.Kaesekrainer;
            return Enum.Parse<SausageKind>(displayText, true);
        }
    }
}
