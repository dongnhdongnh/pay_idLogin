using System;

namespace VakaxaIDServer.Models
{
    public class UserModelClaim
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public string CountryCode { get; set; }
        public string Avatar { get; set; }
        public string Country { get; set; }
        public string StreetAddress { get; set; }
        public string PostalCode { get; set; }
    }
}