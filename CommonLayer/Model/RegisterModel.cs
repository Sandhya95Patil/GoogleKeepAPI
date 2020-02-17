//-----------------------------------------------------------------------
// <copyright file="RegisterModel.cs" company="BridgeLabz">
//     Company copyright tag.
// </copyright>
// <creater name="Sandhya Patil"/>
//-----------------------------------------------------------------------
namespace CommonLayer.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;

    public class RegisterModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is required")]
        [RegularExpression("^([a-zA-Z]{2,})$", ErrorMessage = "First Name should contain atleast 2 or more characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [RegularExpression("^([a-zA-Z]{2,})$", ErrorMessage = "Last Name should contain atleast 2 or more characters")]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Email is required")]
        public string Password { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string MobileNumber { get; set; }

        public string Profile { get; set; }

        public string Service { get; set; }
        public string UserType { get; set; }
   }
}
