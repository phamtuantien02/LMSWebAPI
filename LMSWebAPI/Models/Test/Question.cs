using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSWebAPI.Models
{
    public class Question
    {
        public string questionText { get; set; }

        public List<AnswerOptions> options = new List<AnswerOptions>();

        public Question(string questionText)
        {
            this.questionText = questionText;

        }

    }


}
