using System.Threading;
using System.Threading.Tasks;
using FragenZurWurst.Bot;
using FragenZurWurst.CognitiveModels;
using FragenZurWurst.Model;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace FragenZurWurst.Dialogs
{
    public class MainDialog : InterruptableDialog
    {
        private readonly ConversationState conversationState;
        private readonly IFragenZurWurstRecognizer recognizer;
        private readonly OrderingDialog orderingDialog;

        public MainDialog(
            ConversationState conversationState,
            IFragenZurWurstRecognizer recognizer,
            OrderingDialog orderingDialog)
            : base(nameof(MainDialog), conversationState, recognizer)
        {
            this.conversationState = conversationState;
            this.recognizer = recognizer;
            this.orderingDialog = orderingDialog;

            AddDialog(new TextPrompt(nameof(TextPrompt)));

            AddDialog(orderingDialog);

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                AskForInitialOrderStepAsync,
                ProceedWithOrderingStepAsync,
                ThankForTheOrderStepAsync
            }));

            InitialDialogId = nameof(WaterfallDialog);            
        }

        private async Task<DialogTurnResult> AskForInitialOrderStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            string question = "Wos meachst du?";

            var promptMessage = MessageFactory.Text(question, question, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> ProceedWithOrderingStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var result = await recognizer.RecognizeAsync(stepContext.Context, cancellationToken);

            var order = await conversationState.GetOrder(stepContext.Context);
            if (result.IsSpecifyOrderIntent)
            {
                order.MergeWith(result.ToOrder());
            }

            return await stepContext.BeginDialogAsync(orderingDialog.Id, order, cancellationToken);
        }

        private async Task<DialogTurnResult> ThankForTheOrderStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var order = await conversationState.GetOrder(stepContext.Context);

            string message = $"Doangschen! Dei {order.SausageKind!.Value.ToDisplayText()} Currywurscht is do. Foilst nu woas bestön mechast soagst'as hoit.";

            var promptMessage = MessageFactory.Text(message, message, InputHints.IgnoringInput);
            await stepContext.Context.SendActivityAsync(promptMessage, cancellationToken);

            await conversationState.DeleteOrder(stepContext.Context);

            return await stepContext.EndDialogAsync(null, cancellationToken);
        }
    }
}
