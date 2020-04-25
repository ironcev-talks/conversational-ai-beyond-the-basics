using System.Threading;
using System.Threading.Tasks;
using FragenZurWurst.CognitiveModels;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Resources;

namespace FragenZurWurst.Dialogs
{
    public abstract class InterruptableDialog : ComponentDialog
    {
        private readonly ConversationState conversationState;
        private readonly IFragenZurWurstRecognizer recognizer;

        public InterruptableDialog(string id, ConversationState conversationState, IFragenZurWurstRecognizer recognizer)
            : base(id)
        {
            this.conversationState = conversationState;
            this.recognizer = recognizer;
        }

        protected override async Task<DialogTurnResult> OnContinueDialogAsync(DialogContext innerDc, CancellationToken cancellationToken = default)
        {
            var result = await InterruptAsync(innerDc, cancellationToken);
            if (result != null) return result;

            return await base.OnContinueDialogAsync(innerDc, cancellationToken);
        }

        private async Task<DialogTurnResult?> InterruptAsync(DialogContext innerDc, CancellationToken cancellationToken)
        {
            if (innerDc.Context.Activity.Type != ActivityTypes.Message) return null;

            var result = await recognizer.RecognizeAsync(innerDc.Context, cancellationToken);

            if (result.IsGetMenuInformationIntent)
            {
                var message = MessageFactory.Text(Resource.MenuInformation, Resource.MenuInformation, InputHints.ExpectingInput);
                await innerDc.Context.SendActivityAsync(message, cancellationToken);
                return new DialogTurnResult(DialogTurnStatus.Waiting);
            }

            if (result.IsCancelOrderIntent)
            {
                var cancelMessage = MessageFactory.Text(Resource.CancelOrderConfirmation, Resource.CancelOrderConfirmation, InputHints.IgnoringInput);
                await innerDc.Context.SendActivityAsync(cancelMessage, cancellationToken);
                await conversationState.ClearStateAsync(innerDc.Context, cancellationToken);
                return await innerDc.CancelAllDialogsAsync(cancellationToken);
            }

            return null;
        }
    }
}
