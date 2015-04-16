﻿using System.Runtime.Versioning;

namespace LostAndFound.Models
{
    public class User
    {
        public User(string fname, string phone)
        {
            FirstName = fname;
            PhoneNumber = phone;
        }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string TNumber { get; set; }
    }
}
