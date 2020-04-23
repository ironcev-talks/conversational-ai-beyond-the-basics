using FragenZurWurst.Model;
using System.Collections.Generic;

namespace FragenZurWurst.CognitiveModels
{
    public partial class FragenZurWurstRecognizerResult
    {
        private readonly Order? order;
        private readonly bool confirmOrder;
        private readonly bool declineOrder;

        public FragenZurWurstRecognizerResult()
        {
            Intents = new Dictionary<Intent, Microsoft.Bot.Builder.IntentScore>();
        }

        public FragenZurWurstRecognizerResult(Order? order, bool confirmOrder, bool declineOrder)
            : this()
        {
            this.order = order;
            this.confirmOrder = confirmOrder;
            this.declineOrder = declineOrder;
        }

        public Order ToOrder()
        {
            if (order != null) return order;
            return new Order();
        }

        public bool IsSpecifyOrderIntent
        {
            get
            {
                if (order != null) return true;

                var topIntent = TopIntent();
                return topIntent.intent == Intent.SpecifyOrder && topIntent.score > 0.8;
            }
        }

        public bool IsConfirmOrderIntent
        {
            get
            {
                if (confirmOrder) return true;

                var topIntent = TopIntent();
                return topIntent.intent == Intent.ConfirmOrder && topIntent.score > 0.8;
            }
        }

        public bool IsDeclineOrderIntent
        {
            get
            {
                if (declineOrder) return true;

                var topIntent = TopIntent();
                return topIntent.intent == Intent.DeclineOrder && topIntent.score > 0.8;
            }
        }
    }
}
