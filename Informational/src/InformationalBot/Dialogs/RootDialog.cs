using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Builder.CognitiveServices.QnAMaker;
using System.Linq;
using System.Threading;
using Microsoft.Bot.Builder.FormFlow;
using InformationalBot.Model;
using System.Text.RegularExpressions;

namespace InformationalBot.Dialogs
{
    [Serializable]
    [QnAMaker("6ed590ee43614bbebb9145a2ffd523c7", "6670c80b-d44f-489e-b820-a2c926fcd28e", "Still looking ...", 0.01, 3)]
    public class RootDialog : QnAMakerDialog
    {
       
        protected override async Task RespondFromQnAMakerResultAsync(IDialogContext context, IMessageActivity message, QnAMakerResults results)
        {
            if (results.Answers.Count > 0)
            {
              
                var response = "Here is a match from our App:  \r\n Answer : '" +
                    results.Answers.First().Answer +"'";
               
               
                await context.PostAsync(response);
                
            }
        }

       protected override async Task DefaultWaitNextMessageAsync(IDialogContext context, IMessageActivity message, QnAMakerResults results)
        {
            if (results.Answers.Count > 0) {
                

                PromptDialog.Confirm(context, this.ConfirmationQuestion, "Are you satisfied with the answer?"); 
            }
            else 
            {
                await context.PostAsync($"I don't really understand '{message}'. Try again with another query.");
                
            }
           
        }

        private async Task ConfirmationQuestion(IDialogContext context, IAwaitable<bool> result)
        {
            if (result != null) {
                if (await result)
                {
                    
                    await context.PostAsync("We will create a Ticket for our Insight");
                    var createUrlDialog = FormDialog.FromForm(Issues.BuildForm, FormOptions.PromptInStart);
                    context.Call(createUrlDialog, AfterFormDialog);
                    

                }
                else {
                     PromptDialog.Confirm(context, this.CreateTicket, "Do you want to create a ticket?");
                    
                }
            }

        }

        private async Task CreateTicket(IDialogContext context, IAwaitable<bool> result)
        {
          
            if (result != null) {
                if (await result)
                {
                    await context.PostAsync("Ok lets create this ticket now ! ");
                    var createUrlDialog = FormDialog.FromForm(Issues.BuildForm, FormOptions.PromptInStart);
                    context.Call(createUrlDialog, AfterFormDialog);
                    }
                else {

                    await context.Forward(new MasterDialog(), null, null, CancellationToken.None);
                }
            } 
        }

        private async Task AfterFormDialog(IDialogContext context, IAwaitable<Issues> result)
        {
            //Here we can save our Ticket
            Issues issue = await result;
            await context.PostAsync("Your ticket is create! SUCCESSFUL");

            //
            PromptDialog.Confirm(context, this.ConfirmationExit, "Do you want to exit?");
           
            //CODE FOR RESET THE BOT
            //context.Activity.GetStateClient().BotState.DeleteStateForUser(context.Activity.ChannelId,context.Activity.From.Id);
        }
        

        
        private async Task ConfirmationExit(IDialogContext context, IAwaitable<bool> result)
        {
            // result is yes if the user choose yes, otherwive Result is false
            if (result != null)
            {
                if (await result)
                {
                   //Bot will responses with this message
                    await context.PostAsync("Good Bye");
                   // Close conversation
                    context.EndConversation("end of conversation");

                }
                else
                {
                    try {
                        //
                        await context.PostAsync("Ok");
                        context.Done<string>(null);
                        // Bot will redirect the user to our Master Dialog 
                        await context.Forward(new MasterDialog(), null, null, CancellationToken.None);
                    } catch(Exception e) { }
                }
            }

        }
    }
}