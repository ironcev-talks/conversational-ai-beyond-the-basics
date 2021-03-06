﻿using FragenZurWurst.Bot;
using FragenZurWurst.CognitiveModels;
using FragenZurWurst.Model;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Resources;
using System.Threading;
using System.Threading.Tasks;

namespace FragenZurWurst.Dialogs
{
    public class ChooseSausageKindAndSauceDialog : InterruptableDialog
    {
        private readonly ConversationState conversationState;
        private readonly IFragenZurWurstRecognizer recognizer;

        public ChooseSausageKindAndSauceDialog(ConversationState conversationState, IFragenZurWurstRecognizer recognizer)
            : base(nameof(ChooseSausageKindAndSauceDialog), conversationState, recognizer)
        {
            this.conversationState = conversationState;
            this.recognizer = recognizer;

            AddDialog(new TextPrompt(nameof(TextPrompt)));

            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), new WaterfallStep[]
            {
                AskForSausageKindAndSauceStepAsync,
                GetSausageKindAndSauceStepAsync
            }));

            InitialDialogId = nameof(WaterfallDialog);
        }

        private async Task<DialogTurnResult> AskForSausageKindAndSauceStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var order = await conversationState.GetOrder(stepContext.Context);

            if (order.IsSausageKindAndSauceDefined) return await stepContext.EndDialogAsync(order, cancellationToken);

            string question = string.Empty;
            if (!order.SausageKind.HasValue)
                question += Resource.WhichSausage;
            if (!order.Sauce.HasValue)
                question += " " + Resource.WhichSauce;
            question = question.Trim();

            var promptMessage = MessageFactory.Text(question, question, InputHints.ExpectingInput);
            return await stepContext.PromptAsync(nameof(TextPrompt), new PromptOptions { Prompt = promptMessage }, cancellationToken);
        }

        private async Task<DialogTurnResult> GetSausageKindAndSauceStepAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var result = await recognizer.RecognizeAsync(stepContext.Context, cancellationToken);

            if (!result.IsSpecifyOrderIntent)
            {
                var message = MessageFactory.Text(Resource.IDidNotUnderstandYou, Resource.IDidNotUnderstandYou, InputHints.IgnoringInput);
                await stepContext.Context.SendActivityAsync(message, cancellationToken);

                return await stepContext.ReplaceDialogAsync(nameof(ChooseSausageKindAndSauceDialog), (Order)stepContext.Options, cancellationToken);
            }

            var order = await conversationState.GetOrder(stepContext.Context);
            order.MergeWith(result.ToOrder());

            if (!order.IsSausageKindAndSauceDefined)
            {
                return await stepContext.ReplaceDialogAsync(nameof(ChooseSausageKindAndSauceDialog), order, cancellationToken);
            }

            return await stepContext.EndDialogAsync(order, cancellationToken);
        }
    }
}
