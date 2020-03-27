using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
    }
}