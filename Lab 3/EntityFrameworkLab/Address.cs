using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EntityFrameworkLab
{
    public class Address
    {
        [Key]
        public int Address_ID { get; set; }
        public string House_Name_or_Number { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string Postcode { get; set; }
        public ICollection<Person> People { get; set; }
        public string Country { get; set; }
        public Address() { }
    }
}
