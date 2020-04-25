using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FragenZurWurst.Bot;
using FragenZurWurst.Model;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Recognizers.Text;
using Resources;

namespace FragenZurWurst.Dialogs
{
    public class MainDialog : ComponentDialog
    {
        private readonly ConversationState conversationState;

        public MainDialog(ConversationState conversationState)
            : base(nameof(MainDialog))
        {
            this.conversationState = conversationState;

            AddDialog(CreateChoicePrompt(nameof(SausageKind)));
            AddDialog(CreateChoicePrompt(nameof(CutKind)));
            AddDialog(CreateChoicePrompt(nameof(Sauce)));
            AddDialog(CreateChoicePrompt(nameof(SauceTaste)));
            AddDialog(CreateChoicePrompt(nameof(SaucePosition)));
            AddDialog(CreateChoicePrompt(nameof(BreadKind)));
            AddDialog(CreateChoicePrompt(nameof(Side)));

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                GetSausageKindStepAsync,
                GetCutKindStepAsync,
                GetSauceStepAsync,
                GetSauceTasteStepAsync,
                GetSaucePositionStepAsync,
                GetBreadKindStepAsync,
                GetSideStepAsync,
                OrderingStepAsync,
            }));

            InitialDialogId = nameof(WaterfallDialog);            
        }

        private static Dialog CreateChoicePrompt(string dialogId)
        {
            return new ChoicePrompt(dialogId, null, Culture.German) { Style = ListStyle.SuggestedAction };
        }

        private static async Task<DialogTurnResult> GetSausageKindStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            return await stepContext.PromptAsync(nameof(SausageKind), CreateSausageKindOptions(), cancellationToken);

            static PromptOptions CreateSausageKindOptions()
            {
                var choices = new List<Choice>
                {
                    new Choice { Value = SausageKind.Woidvierdler.ToDisplayText(), Synonyms = Resource.SausageKindWoidvierdlerSynonyms.Split(',').ToList() },
                    new Choice { Value = SausageKind.Buren.ToDisplayText(), Synonyms = Resource.SausageKindBurenSynonyms.Split(',').ToList() },
                    new Choice { Value = SausageKind.Schoafe.ToDisplayText(), Synonyms = Resource.SausageKindSchoafeSynonyms.Split(',').ToList() },
                    new Choice { Value = SausageKind.Kaesekrainer.ToDisplayText(), Synonyms = Resource.SausageKindKaesekrainerSynonyms.Split(',').ToList() }
                };

                return new PromptOptions
                {
                    Prompt = MessageFactory.Text(Resource.WhichSausage),
                    Choices = choices,
                    RetryPrompt = MessageFactory.Text(Resource.IDidNotUnderstandYou)
                };
            }
        }

        private async Task<DialogTurnResult> GetCutKindStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var sausageKindChoice = SausageKindHandler.FromDisplayText(((FoundChoice)stepContext.Result).Value);
            var order = await conversationState.GetOrder(stepContext.Context);
            order.SausageKind = sausageKindChoice;

            return await stepContext.PromptAsync(nameof(CutKind), CreateCutKindOptions(), cancellationToken);

            static PromptOptions CreateCutKindOptions()
            {
                var choices = new List<Choice>
                {
                    new Choice { Value = CutKind.Aufschneiden.ToDisplayText(), Synonyms = Resource.CutKindAufschneidenSynonyms.Split(',').ToList() },
                    new Choice { Value = CutKind.ZwaHoeften.ToDisplayText(), Synonyms = Resource.CutKindZwaHoeftenSynonyms.Split(',').ToList() },
                    new Choice { Value = CutKind.ImGonzn.ToDisplayText(), Synonyms = Resource.CutKindImGonznSynonyms.Split(',').ToList() }
                };

                return new PromptOptions
                {
                    Prompt = MessageFactory.Text(Resource.WhichCutKind),
                    Choices = choices,
                    RetryPrompt = MessageFactory.Text(Resource.IDidNotUnderstandYou)
                };
            }
        }

        private async Task<DialogTurnResult> GetSauceStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var cutKindChoice = CutKindHandler.FromDisplayText(((FoundChoice)stepContext.Result).Value);
            var order = await conversationState.GetOrder(stepContext.Context);
            order.CutKind = cutKindChoice;

            return await stepContext.PromptAsync(nameof(Sauce), CreateSauceOptions(), cancellationToken);

            static PromptOptions CreateSauceOptions()
            {
                var choices = new List<Choice>
                {
                    new Choice { Value = Sauce.Senf.ToDisplayText() },
                    new Choice { Value = Sauce.Ketchup.ToDisplayText(), Synonyms = Resource.SauceKetchupSynonyms.Split(',').ToList() },
                    new Choice { Value = Sauce.Beides.ToDisplayText(), Synonyms = Resource.SauceBeidesSynonyms.Split(',').ToList() }
                };

                return new PromptOptions
                {
                    Prompt = MessageFactory.Text(Resource.WhichSauce),
                    Choices = choices,
                    RetryPrompt = MessageFactory.Text(Resource.IDidNotUnderstandYou)
                };
            }
        }

        private async Task<DialogTurnResult> GetSauceTasteStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var sauceChoice = SauceHandler.FromDisplayText(((FoundChoice)stepContext.Result).Value);
            var order = await conversationState.GetOrder(stepContext.Context);
            order.Sauce = sauceChoice;

            return await stepContext.PromptAsync(nameof(SauceTaste), CreateSauceTasteOptions(), cancellationToken);

            static PromptOptions CreateSauceTasteOptions()
            {
                var choices = new List<Choice>
                {
                    new Choice { Value = SauceTaste.Siass.ToDisplayText(), Synonyms = Resource.SauceTasteSiassSynonyms.Split(',').ToList() },
                    new Choice { Value = SauceTaste.Schoaf.ToDisplayText(), Synonyms = Resource.SauceTasteSchoafSynonyms.Split(',').ToList() }
                };

                return new PromptOptions
                {
                    Prompt = MessageFactory.Text(Resource.WhichSauceTaste),
                    Choices = choices,
                    RetryPrompt = MessageFactory.Text(Resource.IDidNotUnderstandYou)
                };
            }
        }

        private async Task<DialogTurnResult> GetSaucePositionStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var sauceTasteChoice = SauceTasteHandler.FromDisplayText(((FoundChoice)stepContext.Result).Value);
            var order = await conversationState.GetOrder(stepContext.Context);
            order.SauceTaste = sauceTasteChoice;

            return await stepContext.PromptAsync(nameof(SaucePosition), CreateSaucePositionOptions(), cancellationToken);

            static PromptOptions CreateSaucePositionOptions()
            {
                var choices = new List<Choice>
                {
                    new Choice { Value = SaucePosition.Drauf.ToDisplayText(), Synonyms = Resource.SaucePositionDraufSynonyms.Split(',').ToList() },
                    new Choice { Value = SaucePosition.Daneben.ToDisplayText(), Synonyms = Resource.SaucePositionDanebenSynonyms.Split(',').ToList() }
                };

                return new PromptOptions
                {
                    Prompt = MessageFactory.Text(Resource.WhichSaucePosition),
                    Choices = choices,
                    RetryPrompt = MessageFactory.Text(Resource.IDidNotUnderstandYou)
                };
            }
        }

        private async Task<DialogTurnResult> GetBreadKindStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var saucePositionChoice = SaucePositionHandler.FromDisplayText(((FoundChoice)stepContext.Result).Value);
            var order = await conversationState.GetOrder(stepContext.Context);
            order.SaucePosition = saucePositionChoice;

            return await stepContext.PromptAsync(nameof(BreadKind), CreateBreadKindOptions(), cancellationToken);

            static PromptOptions CreateBreadKindOptions()
            {
                var choices = new List<Choice>
                {
                    new Choice { Value = BreadKind.Schwoazbrot.ToDisplayText(), Synonyms = Resource.BreadKindSchwoazbrotSynonyms.Split(',').ToList() },
                    new Choice { Value = BreadKind.Scherzl.ToDisplayText(), Synonyms = Resource.BreadKindScherzlSynonyms.Split(',').ToList() },
                    new Choice { Value = BreadKind.Semmoe.ToDisplayText(), Synonyms = Resource.BreadKindSemmoeSynonyms.Split(',').ToList() },
                    new Choice { Value = BreadKind.Soizgebaeck.ToDisplayText(), Synonyms = Resource.BreadKindSoizgebaeckSynonyms.Split(',').ToList() }
                };

                return new PromptOptions
                {
                    Prompt = MessageFactory.Text(Resource.WhichBread),
                    Choices = choices,
                    RetryPrompt = MessageFactory.Text(Resource.IDidNotUnderstandYou)
                };
            }
        }

        private async Task<DialogTurnResult> GetSideStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var breadKindChoice = BreadKindHandler.FromDisplayText(((FoundChoice)stepContext.Result).Value);
            var order = await conversationState.GetOrder(stepContext.Context);
            order.BreadKind = breadKindChoice;

            return await stepContext.PromptAsync(nameof(Side), CreateSideOptions(), cancellationToken);

            static PromptOptions CreateSideOptions()
            {
                var choices = new List<Choice>
                {
                    new Choice { Value = Side.Gurkel.ToDisplayText(), Synonyms = Resource.SideGurkelSynonyms.Split(',').ToList() },
                    new Choice { Value = Side.Pfeffaroni.ToDisplayText(), Synonyms = Resource.SidePfeffaroniSynonyms.Split(',').ToList() },
                    new Choice { Value = Side.Nix.ToDisplayText(), Synonyms = Resource.SideNixSynonyms.Split(',').ToList() }
                };

                return new PromptOptions
                {
                    Prompt = MessageFactory.Text(Resource.WhichSide),
                    Choices = choices,
                    RetryPrompt = MessageFactory.Text(Resource.IDidNotUnderstandYou)
                };
            }
        }

        private async Task<DialogTurnResult> OrderingStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var sideChoice = SideHandler.FromDisplayText(((FoundChoice)stepContext.Result).Value);
            var order = await conversationState.GetOrder(stepContext.Context);
            order.Side = sideChoice;

            await stepContext.Context.SendActivityAsync(
                MessageFactory.Text(order.ToOrderSentence()),
                cancellationToken);

            return await stepContext.EndDialogAsync();
        }
    }
}
