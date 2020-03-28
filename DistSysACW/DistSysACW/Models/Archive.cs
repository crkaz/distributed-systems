using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DistSysACW.Models
{
    public class Archive
    {
        [Key] // Make primary key.
        public string ApiKey { get; set; } // Primary key.
        public virtual ICollection<Log> Logs { get; set; }

        public Archive() { }
    }
}
