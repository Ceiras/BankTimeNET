using System;
using System.Collections.Generic;

namespace BankTimeNET.Models
{
    public class Bank
    {
        public int Id { get; set; }
        public String Place { get; set; }
        public ICollection<Service>? Services { get; set; }
    }
}
