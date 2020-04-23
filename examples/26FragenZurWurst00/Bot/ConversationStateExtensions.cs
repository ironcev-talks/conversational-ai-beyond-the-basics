using FragenZurWurst.Model;
using Microsoft.Bot.Builder;
using System.Threading.Tasks;

namespace FragenZurWurst.Bot
{
    internal static class ConversationStateExtensions
    {
        public static async Task<Order> GetOrder(this ConversationState conversationState, ITurnContext turnContext)
        {
            var accessor = conversationState.CreateProperty<Order>(nameof(Order));
            return await accessor.GetAsync(turnContext, () => new Order());
        }
    }
}
