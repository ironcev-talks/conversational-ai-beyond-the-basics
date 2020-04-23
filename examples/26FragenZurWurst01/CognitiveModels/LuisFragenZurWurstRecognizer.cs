using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Extensions.Configuration;

namespace FragenZurWurst.CognitiveModels
{
    public class LuisFragenZurWurstRecognizer : IFragenZurWurstRecognizer
    {
        private readonly LuisRecognizer recognizer;

        public LuisFragenZurWurstRecognizer(IConfiguration configuration)
        {
            var luisApplication = new LuisApplication(
                    configuration["LuisAppId"],
                    configuration["LuisAPIKey"],
                    "https://" + configuration["LuisAPIHostName"]);

            recognizer = new LuisRecognizer(luisApplication);
        }

        public virtual async Task<FragenZurWurstRecognizerResult> RecognizeAsync(ITurnContext turnContext, CancellationToken cancellationToken)
            => await recognizer.RecognizeAsync<FragenZurWurstRecognizerResult>(turnContext, cancellationToken);
    }
}
