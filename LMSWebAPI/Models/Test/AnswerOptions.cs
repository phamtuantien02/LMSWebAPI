using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSWebAPI.Models
{
    public class AnswerOptions
    {
        public string text { get; set; }

        public bool correct { get; set; }

        public AnswerOptions(string text,bool isCorrect)
        {
            this.text = text;
            this.correct = isCorrect;
        }
    }
}
