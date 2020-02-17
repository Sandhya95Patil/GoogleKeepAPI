//-----------------------------------------------------------------------
// <copyright file="RegisterResponseModel.cs" company="BridgeLabz">
//     Company copyright tag.
// </copyright>
// <creater name="Sandhya Patil"/>
//-----------------------------------------------------------------------
namespace CommonLayer.Response
{
    public class RegisterResponseModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Profile { get; set; }
        public string Service { get; set; }
        public string UserType { get; set; }
    }
}
