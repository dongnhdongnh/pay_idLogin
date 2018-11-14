using System.Collections.Generic;
using VakaxaIDServer.Models;

namespace VakaxaIDServer.Quickstart.Profile
{
    public class ProfileModel
    {
        public PersonalDetails PersonalDetails { get; set; }
        public ApplicationUser ProfileUser { get; set; }
        public SocialNetwork SocialNetwork { get; set; }
        public bool Tab { get; set; } = true;
    }
}