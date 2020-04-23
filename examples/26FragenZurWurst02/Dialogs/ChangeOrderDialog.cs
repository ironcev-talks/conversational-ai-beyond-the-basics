using System.Threading;
using System.Threading.Tasks;
using FragenZurWurst.Bot;
using FragenZurWurst.CognitiveModels;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace FragenZurWurst.Dialogs
{
    public class ChangeOrderDialog : InterruptableDialog
    {
        private const string IDidNotUnderstandYou = "I hob di net goanz verstoandn.";

        private readonly ConversationState conversationState;
        private readonly IFragenZurWurstRecognizer recognizer;
        private readonly OrderingDialog orderingDialog;

        public ChangeOrderDialog(
            ConversationState conversationState,
            IFragenZurWurstRecognizer recognizer,
            OrderingDialog orderingDialog)
            : base(nameof(ChangeOrderDialog), conversationState, recognizer)
        {
            this.conversationState = conversationState;
            this.recognizer = recognizer;
            this.orderingDialog = orderingDialog;

            AddDialog(new TextPrompt(nameof(TextPrompt)));

            AddDialog(orderingDialog);

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                AskForOrderChangeStepAsync,
                GetOrderChangeStepAsync
            }));

            InitialDialogId = nameof(WaterfallDialog);            
        }

        private async Task<DialogTurnResult> AskForOrderChangeStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            string question = "Woas meachat'ns oanders hob'n?";

            var promptMessage = MessageFactory.Text(question, question, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> GetOrderChangeStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var result = await recognizer.RecognizeAsync(stepContext.Context, cancellationToken);

            var order = await conversationState.GetOrder(stepContext.Context);
            if (result.IsSpecifyOrderIntent)
            {
                order.MergeWith(result.ToOrder());
                return await stepContext.ReplaceDialogAsync(orderingDialog.Id, order, cancellationToken);
            }
            else
            {
                var message = MessageFactory.Text(IDidNotUnderstandYou, IDidNotUnderstandYou, InputHints.IgnoringInput);
                await stepContext.Context.SendActivityAsync(message, cancellationToken);

                return await stepContext.ReplaceDialogAsync(nameof(ChangeOrderDialog), order, cancellationToken);
            }
        }
    }
}
