using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FragenZurWurst.Dialogs;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace FragenZurWurst.Bot
{
    public class FragenZurWurstBot : DialogAndWelcomeBot<MainDialog>
    {
        public FragenZurWurstBot(ConversationState conversationState, UserState userState, MainDialog dialog) : base(conversationState, userState, dialog)
        {
        }
    }
}
