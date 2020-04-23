using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FragenZurWurst.Model;
using Microsoft.Bot.Builder;

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

            bool isGetMenuInformation = false;
            if (!isSpecifyOrderIntent && !isConfirmOrderIntent && !isDeclineOrderIntent) isGetMenuInformation = IsGetMenuInformation();

            bool isCancelOrder = false;
            if (!isSpecifyOrderIntent && !isConfirmOrderIntent && !isDeclineOrderIntent && !isGetMenuInformation) isCancelOrder = IsCancelOrder();

            var result = !isSpecifyOrderIntent && !isConfirmOrderIntent && !isDeclineOrderIntent && !isGetMenuInformation && !isCancelOrder
                ? new FragenZurWurstRecognizerResult()
                : new FragenZurWurstRecognizerResult(order, isConfirmOrderIntent, isDeclineOrderIntent, isGetMenuInformation, isCancelOrder);

            return Task.FromResult(result);

            bool IsConfirmOrder()
            {
                var confirm = new[] { "ja", "jo", "passt", "perfekt", "super", "gut", "guat" };
                if (confirm.Any(word => input.Contains(word)))
                    return true;

                return false;
            }

            bool IsDeclineOrder()
            {
                var decline = new[] { "nein", "ändern", "anders" };
                if (decline.Any(word => input.Contains(word)))
                    return true;

                return false;
            }

            bool IsGetMenuInformation()
            {
                var menu = new[] { "menü", "speisekarte" };
                if (menu.Any(word => input.Contains(word)))
                    return true;

                return false;
            }

            bool IsCancelOrder()
            {
                if (input.Contains("abbrechen") || (input.Contains("breche") && input.Contains(" ab")))
                    return true;

                return false;
            }

            SausageKind? ExtractSausageKind()
            {
                var woidvierdler = new[] { "woidvierdler", "woidvialda", "rauchwuascht", "rauchwiaschtl" };
                if (woidvierdler.Any(word => input.Contains(word)))
                    return SausageKind.Woidvierdler;

                var buren = new[] { "buren" };
                if (buren.Any(word => input.Contains(word)))
                    return SausageKind.Buren;

                var schoafe = new[] { "schoafe", "schafe" };
                if (schoafe.Any(word => input.Contains(word)))
                    return SausageKind.Schoafe;

                var kaesekrainer = new[] { "käsekrainer" };
                if (kaesekrainer.Any(word => input.Contains(word)))
                    return SausageKind.Kaesekrainer;

                return null;
            }

            Sauce? ExtractSauce()
            {
                if (input.Contains("senf") && input.Contains("ketchup"))
                    return Sauce.Beides;

                if (input.Contains("senf"))
                    return Sauce.Senf;

                if (input.Contains("ketchup"))
                    return Sauce.Ketchup;

                var beides = new[] { "beides", "beide", "beide sauce", "gemischt" };
                if (beides.Any(word => input.Contains(word)))
                    return Sauce.Beides;

                return null;
            }

            BreadKind? ExtractBreadKind()
            {
                var schwoazbrot = new[] { "schwoazbrot", "brot", "dunkles brot", "dunkles", "schwarzes" };
                if (schwoazbrot.Any(word => input.Contains(word)))
                    return BreadKind.Schwoazbrot;

                var scherzl = new[] { "scherzl" };
                if (scherzl.Any(word => input.Contains(word)))
                    return BreadKind.Scherzl;

                var semmoe = new[] { "semmö", "semmel" };
                if (semmoe.Any(word => input.Contains(word)))
                    return BreadKind.Semmoe;

                var soizgebaeck = new[] { "soizgebäck", "salzgebäck" };
                if (soizgebaeck.Any(word => input.Contains(word)))
                    return BreadKind.Soizgebaeck;

                return null;
            }
        }
    }
}
