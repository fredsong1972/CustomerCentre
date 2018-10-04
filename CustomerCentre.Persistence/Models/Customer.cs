using System;
using System.Collections.Generic;

namespace CustomerCentre.Persistence.Models
{
    public partial class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public DateTimeOffset Modified { get; set; }
    }
}
