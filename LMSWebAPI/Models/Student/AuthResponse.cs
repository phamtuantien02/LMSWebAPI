using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LMSWebAPI.Models
{
    public class AuthResponse
    {
        public int StudID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string token { get; set; }

    }
}
