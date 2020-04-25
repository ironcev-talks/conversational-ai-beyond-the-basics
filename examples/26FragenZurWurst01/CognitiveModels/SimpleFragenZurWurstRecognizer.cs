using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FragenZurWurst.Model;
using Microsoft.Bot.Builder;
using Resources;

namespace FragenZurWurst.CognitiveModels
{
    public class SimpleFragenZurWurstRecognizer : IFragenZurWurstRecognizer
    {
        public Task<FragenZurWurstRecognizerResult> RecognizeAsync(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            string input = turnContext.Activity.Text?.ToLower() ?? string.Empty;

            Order? order = new Order
            {
                SausageKind = ExtractSausageKind(),
                Sauce = ExtractSauce(),
                BreadKind = ExtractBreadKind()
            };

            if (order.IsEmpty) order = null;

            bool isSpecifyOrderIntent = order != null;

            bool isConfirmOrderIntent = false;
            if (!isSpecifyOrderIntent) isConfirmOrderIntent = IsConfirmOrder();

            bool isDeclineOrderIntent = false;
            if (!isSpecifyOrderIntent && !isConfirmOrderIntent) isDeclineOrderIntent = IsDeclineOrder();

            var result = !isSpecifyOrderIntent && !isConfirmOrderIntent && !isDeclineOrderIntent
                ? new FragenZurWurstRecognizerResult()
                : new FragenZurWurstRecognizerResult(order, isConfirmOrderIntent, isDeclineOrderIntent);

            return Task.FromResult(result);

            bool IsConfirmOrder()
            {
                var confirm = Resource.ConfirmOrder.Split(',').ToArray();
                if (confirm.Any(word => input.Contains(word, System.StringComparison.InvariantCultureIgnoreCase)))
                    return true;

                return false;
            }

            bool IsDeclineOrder()
            {
                var decline = Resource.DeclineOrder.Split(',').ToArray();
                if (decline.Any(word => input.Contains(word, System.StringComparison.InvariantCultureIgnoreCase)))
                    return true;

                return false;
            }

            SausageKind? ExtractSausageKind()
            {
                var woidvierdler = Resource.SausageKindWoidvierdlerSynonyms.Split(',').ToArray();
                if (woidvierdler.Any(word => input.Contains(word, System.StringComparison.InvariantCultureIgnoreCase)))
                    return SausageKind.Woidvierdler;

                var buren = Resource.SausageKindBurenSynonyms.Split(',').ToArray();
                if (buren.Any(word => input.Contains(word, System.StringComparison.InvariantCultureIgnoreCase)))
                    return SausageKind.Buren;

                var schoafe = Resource.SausageKindSchoafeSynonyms.Split(',').ToArray();
                if (schoafe.Any(word => input.Contains(word, System.StringComparison.InvariantCultureIgnoreCase)))
                    return SausageKind.Schoafe;

                var kaesekrainer = Resource.SausageKindKaesekrainerSynonyms.Split(',').ToArray();
                if (kaesekrainer.Any(word => input.Contains(word, System.StringComparison.InvariantCultureIgnoreCase)))
                    return SausageKind.Kaesekrainer;

                return null;
            }

            Sauce? ExtractSauce()
            {
                var senf = Resource.SauceSenfSynonyms.Split(',').ToArray();
                var ketchup = Resource.SauceKetchupSynonyms.Split(',').ToArray();
                
                if (senf.Any(word => input.Contains(word, System.StringComparison.InvariantCultureIgnoreCase)) &&
                    ketchup.Any(word => input.Contains(word, System.StringComparison.InvariantCultureIgnoreCase)))
                    return Sauce.Beides;

                if (senf.Any(word => input.Contains(word, System.StringComparison.InvariantCultureIgnoreCase)))
                    return Sauce.Senf;

                if (ketchup.Any(word => input.Contains(word, System.StringComparison.InvariantCultureIgnoreCase)))
                    return Sauce.Ketchup;

                var beides = Resource.SauceBeidesSynonyms.Split(',').ToArray();
                if (beides.Any(word => input.Contains(word, System.StringComparison.InvariantCultureIgnoreCase)))
                    return Sauce.Beides;

                return null;
            }

            BreadKind? ExtractBreadKind()
            {
                var schwoazbrot = Resource.BreadKindSchwoazbrotSynonyms.Split(',').ToArray();
                if (schwoazbrot.Any(word => input.Contains(word)))
                    return BreadKind.Schwoazbrot;

                var scherzl = Resource.BreadKindScherzlSynonyms.Split(',').ToArray();
                if (scherzl.Any(word => input.Contains(word)))
                    return BreadKind.Scherzl;

                var semmoe = Resource.BreadKindSemmoeSynonyms.Split(',').ToArray();
                if (semmoe.Any(word => input.Contains(word)))
                    return BreadKind.Semmoe;

                var soizgebaeck = Resource.BreadKindSoizgebaeckSynonyms.Split(',').ToArray();
                if (soizgebaeck.Any(word => input.Contains(word)))
                    return BreadKind.Soizgebaeck;

                return null;
            }
        }
    }
}
