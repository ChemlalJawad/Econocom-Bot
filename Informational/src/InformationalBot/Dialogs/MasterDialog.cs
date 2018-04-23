using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Configuration;
using Newtonsoft.Json;

using InformationalBot;

namespace InformationalBot.Dialogs
{
    [Serializable]
    public class MasterDialog : IDialog<object>
    {
        
        public async Task StartAsync(IDialogContext context)
        {
           context.Wait(MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            var message = await item;
           
            if (message.Text.ToLower().Trim() == "help")
            {
               
                PromptDialog.Text(context, this.ConfirmMessage, "First, please briefly describe your problem to me.");
            }
            else {
                //context.Wait(MessageReceivedAsync);
               await context.Forward(new CustomDialogBot(),null, message, CancellationToken.None);
              }


        }

    
        public async Task ConfirmMessage(IDialogContext context, IAwaitable<string> argument)
        {
            var message = await argument;
            var text = $"Confirm if your problem is : {message}.";
           
            
            PromptDialog.Confirm(context, this.RootDialogRedirect, text);
        }

           private async Task RootDialogRedirect(IDialogContext context, IAwaitable<bool> result)
           {
               if (await result) {
              
                   await context.Forward( new RootDialog(), null, null,CancellationToken.None);


               }
               else {
                   await context.PostAsync("OK you didn't confirm your object");
                await context.Forward(new MasterDialog(), null, null, CancellationToken.None);
               }
           }
           
    }



}

