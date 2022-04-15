using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSWebAPI.Models
{
    public class TestAnswerDTO
    {
        public int QuestionId { get; set; }

        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
    }
}
