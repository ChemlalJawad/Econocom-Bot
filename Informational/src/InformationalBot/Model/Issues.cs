using Microsoft.Bot.Builder.FormFlow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InformationalBot.Model
{
    public enum TypeProblems {
        Software,
        Hardware,
        Other
    }
    public enum Severity
    {
        High,
       Medium,
        Low,
        Other
    }
    [Serializable]
    public class Issues
    {   
        public TypeProblems? TypeProblems;
        
        public string Name;
        
        public Severity? Severity;

        public static IForm<Issues> BuildForm() {
            return new FormBuilder<Issues>()
                .Message("-------------------------------")
                .Build();
                }
    }
}