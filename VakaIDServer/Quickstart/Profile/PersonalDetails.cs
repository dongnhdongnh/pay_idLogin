using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.CodeAnalysis.Options;

namespace VakaxaIDServer.Quickstart.Profile
{
    public class PersonalDetails
    {
        [Required]
        [MaxLength(255)]
        public string StreetAddress { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime? Birthday { get; set; }
        [Required]
        [MaxLength(255)]
        public string Occupation { get; set; }
        [Required]
        [MaxLength(255)]
        public string About { get; set; }
        [Required]
        public string Gender { get; set; }
    }
}