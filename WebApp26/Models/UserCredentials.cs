using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;


namespace WebApp26.Models
{
    public class UserCredentials
    {
        [Key]
        public string Username { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Address { get; set; }
    }
}