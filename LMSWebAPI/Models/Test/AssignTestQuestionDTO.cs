using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSWebAPI.Models
{
    public class AssignTestQuestionDTO
    {
        public string [] QuestionIds { get; set; }

        public int TestId { get; set; }
    }
}
