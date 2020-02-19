//-----------------------------------------------------------------------
// <copyright file="LoginResponseModel.cs" company="BridgeLabz">
//     Company copyright tag.
// </copyright>
// <creater name="Sandhya Patil"/>
//-----------------------------------------------------------------------
namespace CommonLayer.Response
{
    /// <summary>
    /// LoginResponseModel class
    /// </summary>
    public class LoginResponseModel
    {
        /// <summary>
        /// Get and set Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Get and Set first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Get and Set last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Get and Set email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Get and Set mobile number
        /// </summary>
        public string MobileNumber { get; set; }

        /// <summary>
        /// Get and Set profile
        /// </summary>
        public string Profile { get; set; }

        /// <summary>
        /// Get and Set service
        /// </summary>
        public string Service { get; set; }

        /// <summary>
        /// Get and Set user type
        /// </summary>
        public string UserType { get; set; }

        /// <summary>
        /// Get and Set token
        /// </summary>
        public string Token { get; set; }
        public string LoginTime { get; set; }
    }
}
