using Resources;

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
            return $"{Resource.OrderSentenceBegin} {SausageKind.ToOrderSentenceText()}, " +
                   $"{CutKind.ToOrderSentenceText()}, " +
                   $"{Sauce.ToOrderSentenceTextDativ()}. " +
                   $"{Sauce.ToOrderSentenceTextNominativ()} {SauceTaste.ToOrderSentenceText()} {Resource.OrderSentenceAnd} {SaucePosition.ToOrderSentenceText()}. " +
                   $"{Resource.OrderSentenceWith} {BreadKind.ToOrderSentenceText()} " +
                   $"{Side.ToOrderSentenceText()}. " +
                   $"{Resource.OrderSentenceIsItFineLikeThat}";
        }
    }
}
