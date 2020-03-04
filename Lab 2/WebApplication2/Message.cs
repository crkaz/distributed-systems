using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Server
{
    public class Message
    {
        public int MessageID { get; private set; } // Primary key.
        public User User { get; set; }
        public string Content { get; set; }
        public DateTime TimeStamp { get; private set; }

        public Message()
        {
            TimeStamp = new DateTime().Date;
        }
    }
}
