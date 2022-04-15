using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSWebAPI.Models
{
    public class StudentLoginDTO
    {
        public int StudID { get; set; }

        public string UserName { get; set; }

        public string PassWord { get; set; }


    }
}
