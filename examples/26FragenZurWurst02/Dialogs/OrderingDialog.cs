
using FragenZurWurst.Bot;
using FragenZurWurst.CognitiveModels;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using System.Threading;
using System.Threading.Tasks;

namespace FragenZurWurst.Dialogs
{
    public class OrderingDialog : InterruptableDialog
    {
        private const string IDidNotUnderstandYou = "I hob di net goanz verstoandn.";

        private readonly ConversationState conversationState;
        private readonly IFragenZurWurstRecognizer recognizer;
        private readonly ChooseSausageKindAndSauceDialog chooseSausageKindAndSauceDialog;
        private readonly ChangeOrderDialog changeOrderDialog;

        public OrderingDialog(
            ConversationState conversationState,
            IFragenZurWurstRecognizer recognizer,
            ChooseSausageKindAndSauceDialog chooseSausageKindAndSauceDialog)
            : base(nameof(OrderingDialog), conversationState, recognizer)
        {
            this.conversationState = conversationState;
            this.recognizer = recognizer;
            this.chooseSausageKindAndSauceDialog = chooseSausageKindAndSauceDialog;

            changeOrderDialog = new ChangeOrderDialog(conversationState, recognizer, this);

            AddDialog(new TextPrompt(nameof(TextPrompt)));

            AddDialog(chooseSausageKindAndSauceDialog);
            AddDialog(changeOrderDialog);

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                ChooseSausageKindAndSauceStepAsync,
                AskForConfirmationStepAsync,
                ConfirmDeclineOrChangeOrderStepAsync,
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> ChooseSausageKindAndSauceStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var order = await conversationState.GetOrder(stepContext.Context);
            return await stepContext.BeginDialogAsync(chooseSausageKindAndSauceDialog.Id, order /*stepContext.Options*/, cancellationToken);
        }

        private async Task<DialogTurnResult> AskForConfirmationStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var order = await conversationState.GetOrder(stepContext.Context);

            string question = order.ToOrderSentence();

            var promptMessage = MessageFactory.Text(question, question, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> ConfirmDeclineOrChangeOrderStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var order = await conversationState.GetOrder(stepContext.Context);

            var result = await recognizer.RecognizeAsync(stepContext.Context, cancellationToken);

            if (result.IsConfirmOrderIntent)
                return await stepContext.EndDialogAsync(order, cancellationToken);

            if (result.IsSpecifyOrderIntent)
            {
                order.MergeWith(result.ToOrder());
                return await stepContext.ReplaceDialogAsync(nameof(OrderingDialog), order, cancellationToken);
            }

            if (result.IsDeclineOrderIntent)
            {
                return await stepContext.ReplaceDialogAsync(changeOrderDialog.Id, order, cancellationToken);
            }

            var message = MessageFactory.Text(IDidNotUnderstandYou, IDidNotUnderstandYou, InputHints.IgnoringInput);
            await stepContext.Context.SendActivityAsync(message, cancellationToken);

            return await stepContext.ReplaceDialogAsync(nameof(OrderingDialog), order, cancellationToken);
        }
    }
}
