//-----------------------------------------------------------------------
// <copyright file="ResetModel.cs" company="BridgeLabz">
//     Company copyright tag.
// </copyright>
// <creater name="Sandhya Patil"/>
//-----------------------------------------------------------------------
namespace CommonLayer.Model
{
    /// <summary>
    /// ResetModel class
    /// </summary>
    public class ResetModel
    {
        /// <summary>
        /// Get and set Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Get and set new password
        /// </summary>
        public string NewPassword { get; set; }

        /// <summary>
        /// Get and set reset token
        /// </summary>
        public string ResetToken { get; set; }
    }
}
