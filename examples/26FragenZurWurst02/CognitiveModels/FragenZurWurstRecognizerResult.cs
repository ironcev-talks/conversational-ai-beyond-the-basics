// <auto-generated>
// Code generated by LUISGen 26FragenZurWurst01.json -cs Luis.FragenZurWurstRecognizerResult -o 
// Tool github: https://github.com/microsoft/botbuilder-tools
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.AI.Luis;
namespace FragenZurWurst.CognitiveModels
{
    public partial class FragenZurWurstRecognizerResult: IRecognizerConvert
    {
        [JsonProperty("text")]
        public string Text;

        [JsonProperty("alteredText")]
        public string AlteredText;

        public enum Intent {
            CancelOrder, 
            ConfirmOrder, 
            DeclineOrder, 
            GetMenuInformation, 
            None, 
            SpecifyOrder
        };
        [JsonProperty("intents")]
        public Dictionary<Intent, IntentScore> Intents;

        public class _Entities
        {

            // Lists
            public string[][] BreadKind;

            public string[][] CutKind;

            public string[][] Sauce;

            public string[][] SaucePosition;

            public string[][] SauceTaste;

            public string[][] SausageKind;

            public string[][] Side;

            // Instance
            public class _Instance
            {
                public InstanceData[] BreadKind;
                public InstanceData[] CutKind;
                public InstanceData[] Sauce;
                public InstanceData[] SaucePosition;
                public InstanceData[] SauceTaste;
                public InstanceData[] SausageKind;
                public InstanceData[] Side;
            }
            [JsonProperty("$instance")]
            public _Instance _instance;
        }
        [JsonProperty("entities")]
        public _Entities Entities;

        [JsonExtensionData(ReadData = true, WriteData = true)]
        public IDictionary<string, object> Properties {get; set; }

        public void Convert(dynamic result)
        {
            var app = JsonConvert.DeserializeObject<FragenZurWurstRecognizerResult>(JsonConvert.SerializeObject(result, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            Text = app.Text;
            AlteredText = app.AlteredText;
            Intents = app.Intents;
            Entities = app.Entities;
            Properties = app.Properties;
        }

        public (Intent intent, double score) TopIntent()
        {
            Intent maxIntent = Intent.None;
            var max = 0.0;
            foreach (var entry in Intents)
            {
                if (entry.Value.Score > max)
                {
                    maxIntent = entry.Key;
                    max = entry.Value.Score.Value;
                }
            }
            return (maxIntent, max);
        }
    }
}
