using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FragenZurWurst.Bot;
using FragenZurWurst.Model;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using Microsoft.Recognizers.Text;

namespace FragenZurWurst.Dialogs
{
    public class MainDialog : ComponentDialog
    {
        private const string IDidNotUnderstandYou = "I hob di net goanz verstoandn.";

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
                    new Choice { Value = SausageKind.Woidvierdler.ToDisplayText(), Synonyms = new List<string> { "woidvierdler", "woidvialda", "rauchwuascht", "rauchwiaschtl" } },
                    new Choice { Value = SausageKind.Buren.ToDisplayText(), Synonyms = new List<string> { "burenheidl" } },
                    new Choice { Value = SausageKind.Schoafe.ToDisplayText(), Synonyms = new List<string> { "schafe" } },
                    new Choice { Value = SausageKind.Kaesekrainer.ToDisplayText(), Synonyms = new List<string> { "käsekrainer" } }
                };

                return new PromptOptions
                {
                    Prompt = MessageFactory.Text("Wöcha Wurscht?"),
                    Choices = choices,
                    RetryPrompt = MessageFactory.Text(IDidNotUnderstandYou)
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
                    new Choice { Value = CutKind.Aufschneiden.ToDisplayText(), Synonyms = new List<string> { "aufgeschnitten", "geschnitten", "gschnitten" } },
                    new Choice { Value = CutKind.ZwaHoeften.ToDisplayText(), Synonyms = new List<string> { "höften", "helften", "helfte", "zwei helften", "zwa helften", "zwo helften", "zwei helfte", "zwa helfte", "zwo helfte", "zwei höften", "zwo höften" } },
                    new Choice { Value = CutKind.ImGonzn.ToDisplayText(), Synonyms = new List<string> { "in gonzn", "gonz", "im ganzen", "in ganzen", "ganz" } }
                };

                return new PromptOptions
                {
                    Prompt = MessageFactory.Text("Wie?"),
                    Choices = choices,
                    RetryPrompt = MessageFactory.Text(IDidNotUnderstandYou)
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
                    new Choice { Value = Sauce.Ketchup.ToDisplayText() },
                    new Choice { Value = Sauce.Beides.ToDisplayText(), Synonyms = new List<string> { "beide", "beide sauce" } }
                };

                return new PromptOptions
                {
                    Prompt = MessageFactory.Text("Wöcha Sauce?"),
                    Choices = choices,
                    RetryPrompt = MessageFactory.Text(IDidNotUnderstandYou)
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
                    new Choice { Value = SauceTaste.Siass.ToDisplayText(), Synonyms = new List<string> { "süß", "süss", "siass" } },
                    new Choice { Value = SauceTaste.Schoaf.ToDisplayText(), Synonyms = new List<string> { "scharf" } }
                };

                return new PromptOptions
                {
                    Prompt = MessageFactory.Text("Wöchan?"),
                    Choices = choices,
                    RetryPrompt = MessageFactory.Text(IDidNotUnderstandYou)
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
                    new Choice { Value = SaucePosition.Drauf.ToDisplayText(), Synonyms = new List<string> { "darauf" } },
                    new Choice { Value = SaucePosition.Daneben.ToDisplayText(), Synonyms = new List<string> { "neben" } }
                };

                return new PromptOptions
                {
                    Prompt = MessageFactory.Text("Drauf oder daneben?"),
                    Choices = choices,
                    RetryPrompt = MessageFactory.Text(IDidNotUnderstandYou)
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
                    new Choice { Value = BreadKind.Schwoazbrot.ToDisplayText(), Synonyms = new List<string> { "brot", "dunkles brot", "dunkles", "schwarzes" } },
                    new Choice { Value = BreadKind.Scherzl.ToDisplayText(), Synonyms = new List<string> { "scherzl" } },
                    new Choice { Value = BreadKind.Semmoe.ToDisplayText(), Synonyms = new List<string> { "semmel" } },
                    new Choice { Value = BreadKind.Soizgebaeck.ToDisplayText(), Synonyms = new List<string> { "salzgebäck" } }
                };

                return new PromptOptions
                {
                    Prompt = MessageFactory.Text("Wöches Brot?"),
                    Choices = choices,
                    RetryPrompt = MessageFactory.Text(IDidNotUnderstandYou)
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
                    new Choice { Value = Side.Gurkel.ToDisplayText(), Synonyms = new List<string> { "gurken" } },
                    new Choice { Value = Side.Pfeffaroni.ToDisplayText(), Synonyms = new List<string> { "pfeffaroni" } },
                    new Choice { Value = Side.Nix.ToDisplayText(), Synonyms = new List<string> { "nichts", "keine", "nein" } }
                };

                return new PromptOptions
                {
                    Prompt = MessageFactory.Text("Wöcha Beiloag?"),
                    Choices = choices,
                    RetryPrompt = MessageFactory.Text(IDidNotUnderstandYou)
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
