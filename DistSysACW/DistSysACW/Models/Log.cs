using System;
using System.ComponentModel.DataAnnotations;

namespace DistSysACW.Models
{
    public class Log
    {
        // DB fields.
        [Key] // Make primary key.
        public string LogId { get; set; } // Primary key.
        public string LogString { get; set; }
        public DateTime LogDateTime { get; set; }

        public Log() { }

        public Log(string endpoint)
        {
            string logString = "User requested " + endpoint;
            LogDateTime = DateTime.Now;
            LogString = logString;
        }
    }
}