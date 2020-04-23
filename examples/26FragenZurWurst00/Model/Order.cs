namespace FragenZurWurst.Model
{
    internal class Order
    {
        public SausageKind SausageKind { get; set; }
        public CutKind CutKind { get; set; }
        public Sauce Sauce { get; set; }
        public SauceTaste SauceTaste { get; set; }
        public SaucePosition SaucePosition { get; set; }
        public BreadKind BreadKind { get; set; }
        public Side Side { get; set; }

        public string ToOrderSentence()
        {
            return $"Also, eine {SausageKind.ToOrderSentenceText()}, " +
                   $"{CutKind.ToOrderSentenceText()}, " +
                   $"mit {Sauce.ToOrderSentenceText()}. " +
                   $"{Sauce.ToOrderSentenceText()} {SauceTaste.ToOrderSentenceText()} und {SaucePosition.ToOrderSentenceText()}. " +
                   $"Dazu {BreadKind.ToOrderSentenceText()} " +
                   $"{(Side == Side.Nix ? "ohne ana Beiloag." : $"mit {Side.ToOrderSentenceText()}.")} " +
                   $"Passt's so?";
        }
    }
}
