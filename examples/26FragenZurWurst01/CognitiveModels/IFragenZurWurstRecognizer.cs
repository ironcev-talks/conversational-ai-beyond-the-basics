using Microsoft.Bot.Builder;
using System.Threading;
using System.Threading.Tasks;

namespace FragenZurWurst.CognitiveModels
{
    public interface IFragenZurWurstRecognizer
    {
        Task<FragenZurWurstRecognizerResult> RecognizeAsync(ITurnContext turnContext, CancellationToken cancellationToken);
    }
}
