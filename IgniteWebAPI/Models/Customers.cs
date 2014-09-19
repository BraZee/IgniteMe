using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IgniteWebAPI.Models
{
    public class Customers
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Hash { get; set; }
        public string Status { get; set; }
        public int StatusId { get; set; }
        public int SecurityQuestionId { get; set; }
        public string SecurityAnswer { get; set; }
        public string Telephone { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Town { get; set; }
        public DateTime Dob { get; set; }
        public bool Active { get; set; }
    }
}