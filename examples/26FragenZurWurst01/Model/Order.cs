﻿using Resources;

namespace FragenZurWurst.Model
{
    public class Order
    {
        public SausageKind? SausageKind { get; set; }
        public CutKind? CutKind { get; set; }
        public Sauce? Sauce { get; set; }
        public SauceTaste? SauceTaste { get; set; }
        public SaucePosition? SaucePosition { get; set; }
        public BreadKind? BreadKind { get; set; }
        public Side? Side { get; set; }

        public bool IsSausageKindAndSauceDefined => SausageKind != null && Sauce != null;

        public string ToOrderSentence()
        {
            return $"{Resource.OrderSentenceBegin} {(SausageKind ?? Model.SausageKind.Kaesekrainer).ToOrderSentenceText()}, " +
                   $"{(CutKind ?? Model.CutKind.Aufschneiden).ToOrderSentenceText()}, " +
                   $"{(Sauce ?? Model.Sauce.Senf).ToOrderSentenceTextDativ()}. " +
                   $"{(Sauce ?? Model.Sauce.Senf).ToOrderSentenceTextNominativ()} {(SauceTaste ?? Model.SauceTaste.Schoaf).ToOrderSentenceText()} {Resource.OrderSentenceAnd} {(SaucePosition ?? Model.SaucePosition.Daneben).ToOrderSentenceText()}. " +
                   $"{Resource.OrderSentenceWith} {(BreadKind ?? Model.BreadKind.Semmoe).ToOrderSentenceText()} " +
                   $"{(Side ?? Model.Side.Gurkel).ToOrderSentenceText()}. " +
                   $"{Resource.OrderSentenceIsItFineLikeThat}";
        }

        public void MergeWith(Order otherOrder)
        {
            if (otherOrder.SausageKind.HasValue)
                SausageKind = otherOrder.SausageKind;
            if (otherOrder.CutKind.HasValue)
                CutKind = otherOrder.CutKind;
            if (otherOrder.Sauce.HasValue)
                Sauce = otherOrder.Sauce;
            if (otherOrder.SauceTaste.HasValue)
                SauceTaste = otherOrder.SauceTaste;
            if (otherOrder.SaucePosition.HasValue)
                SaucePosition = otherOrder.SaucePosition;
            if (otherOrder.BreadKind.HasValue)
                BreadKind = otherOrder.BreadKind;
            if (otherOrder.Side.HasValue)
                Side = otherOrder.Side;
        }

        public bool IsEmpty =>
            !SausageKind.HasValue &&
            !CutKind.HasValue &&
            !Sauce.HasValue &&
            !SauceTaste.HasValue &&
            !SaucePosition.HasValue &&
            !BreadKind.HasValue &&
            !Side.HasValue;
    }
}
