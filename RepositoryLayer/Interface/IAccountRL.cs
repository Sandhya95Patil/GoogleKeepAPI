//-----------------------------------------------------------------------
// <copyright file="IAccountRL.cs" company="BridgeLabz">
//     Company copyright tag.
// </copyright>
// <creater name="Sandhya Patil"/>
//-----------------------------------------------------------------------
namespace RepositoryLayer.Interface
{
    using CommonLayer.Model;
    using CommonLayer.Response;
    using System.Threading.Tasks;

    /// <summary>
    /// IAccountRL interface
    /// </summary>
    public interface IAccountRL
    {
        /// <summary>
        /// User Sign Up method
        /// </summary>
        /// <param name="registrationModel">registrationModel parameter</param>
        /// <returns>returns the register user</returns>
        Task<RegisterResponseModel> UserSignUp(RegisterModel registrationModel);

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
        /// <returns>returns the reset link</returns>
        Task<string> ForgetPassword(ForgetModel forgetModel);

        /// <summary>
        /// Reset Password method
        /// </summary>
        /// <param name="resetModel">resetModel parameter</param>
        /// <returns>returns the reset password</returns>
        Task<string> ResetPassword(ResetModel resetModel);
    }
}
