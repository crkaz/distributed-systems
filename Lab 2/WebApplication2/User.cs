using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public class User
    {
        public int UserID { get; private set; } // Primary key.
        public string Username { get; set; }
    }
}
