using BestMatchDialog;
using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InformationalBot.Dialogs
{
    [Serializable]
    internal class CustomDialogBot : BestMatchDialog<object>
    {
        [BestMatch(new string[] { "hi", "hi there", "hello there", "hey", "hello",
            "hey there", "greetings", "good morning", "good afternoon", "good evening", "good day" },
              threshold: 0.5, ignoreCase: false, ignoreNonAlphaNumericCharacters: false)]
        public async Task HandleGreeting(IDialogContext context, string messageText)
        {
            await context.PostAsync("Well hello there. If you have any problems, you can type 'help'! I will try to help you");
            context.Wait(MessageReceived);
        }

        [BestMatch(new string[] { "how goes it", "how do", "hows it going", "how are you",
            "how do you feel", "whats up", "sup", "hows things" })]
        public async Task HandleStatusRequest(IDialogContext context, string messageText)
        {
            await context.PostAsync("I am great.");
            context.Wait(MessageReceived);
        }

        [BestMatch(new string[] { "bye", "bye bye", "got to go",
            "see you later", "laters", "adios" })]
        public async Task HandleGoodbye(IDialogContext context, string messageText)
        {
            await context.PostAsync("Bye. Looking forward to our next awesome conversation already.");
            context.Wait(MessageReceived);
        }

        [BestMatch("thank you|thanks|much appreciated|thanks very much|thanking you", listDelimiter: '|')]
        public async Task HandleThanks(IDialogContext context, string messageText)
        {
            await context.PostAsync("You're welcome.");
            context.Wait(MessageReceived);
        }

        public override async Task NoMatchHandler(IDialogContext context, string messageText)
        {
            await context.PostAsync($"I don't understand what you say '{messageText}'. You can type 'help' if you need some help");
            await context.Forward(new MasterDialog(), null, null,CancellationToken.None);
            
          
        }

    }
}