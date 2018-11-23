using System;
using Microsoft.AspNetCore.Identity;

namespace VakaxaIDServer.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string FullName { get; set; }
        public string CountryCode { get; set; }
        public string PhoneNational { get; set; }
        public string Avatar { get; set; }
        public string Country { get; set; }
        public string StreetAddress { get; set; }
        public string PostalCode { get; set; }
        public DateTime? Birthday { get; set; }
        public string Occupation { get; set; }
        public string About { get; set; }
        public string SocialNetwork { get; set; }
        public bool IsGoogleAuthenticator { get; set; }
        public string ShareKeySecret { get; set; }
        public string ListSecretKeySms { get; set; }
        public string Gender { get; set; }
        
    }
}
