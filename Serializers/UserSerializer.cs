using System;
using System.ComponentModel.DataAnnotations;
namespace TourizmTest.Serializers
{
    public class UserSerializer
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get;set; }
        [Required]
        public string UserName { get;set; }
        [Required]
        public string RoleName { get;set; }
        [Required]
        public int Age { get;set; }
        [Required]
        public string Password { get;set; }
        [Required]
        [Compare("Password", ErrorMessage = "Not same passwords!")]
        public string PasswordConfirmation { get;set; }


    }
}