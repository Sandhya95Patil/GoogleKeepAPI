//-----------------------------------------------------------------------
// <copyright file="IAccountBL.cs" company="BridgeLabz">
//     Company copyright tag.
// </copyright>
// <creater name="Sandhya Patil"/>
//-----------------------------------------------------------------------
namespace BussinessLayer.Interface
{
    using CommonLayer.Model;
    using CommonLayer.Response;
    using CommonLayer.ShowModel;
    using Microsoft.AspNetCore.Http;
    using System.Threading.Tasks;

    /// <summary>
    /// IAccountBL interface
    /// </summary>
    public interface IAccountBL
    {
        /// <summary>
        /// User Sign Up method 
        /// </summary>
        /// <param name="registerModel">registerModel parameter</param>
        /// <returns>returns the register user</returns>
        Task<RegisterResponseModel> UserSignUp(ShowRegisterModel registerModel);

        /// <summary>
        /// User Login method
        /// </summary>
        /// <param name="loginModel">loginModel parameter</param>
        /// <returns>returns the login user</returns>
        Task<LoginResponseModel> UserLogin(LoginModel loginModel);

        /// <summary>
        /// Forget Password method
        /// </summary>
        /// <param name="forgetModel">forgetModel parameter</param>
        /// <returns>return reset link</returns>
        Task<string> ForgetPassword(ForgetModel forgetModel);

        /// <summary>
        /// Reset Password method
        /// </summary>
        /// <param name="resetModel">resetModel parameter</param>
        /// <returns>returns the reset password</returns>
        Task<string> ResetPassword(ResetModel resetModel);
        Task<RegisterResponseModel> Profile(IFormFile file, int userId);
    }
}
