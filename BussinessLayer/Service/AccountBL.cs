//-----------------------------------------------------------------------
// <copyright file="AccountBL.cs" company="BridgeLabz">
//     Company copyright tag.
// </copyright>
// <creater name="Sandhya Patil"/>
//-----------------------------------------------------------------------
namespace BussinessLayer.Service
{
    using BussinessLayer.Interface;
    using CommonLayer.Model;
    using CommonLayer.Response;
    using RepositoryLayer.Interface;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// AccountBL class
    /// </summary>
    public class AccountBL:IAccountBL
    {
        /// <summary>
        /// Inject the repository layer interface of the Account class
        /// </summary>
        private readonly IAccountRL accountRL;

        /// <summary>
        /// Initializes the memory for Account class 
        /// </summary>
        /// <param name="accountRL">accountRL parameter</param>
        public AccountBL (IAccountRL accountRL)
        {
            this.accountRL = accountRL;
        }

        /// <summary>
        /// User Sign Up method
        /// </summary>
        /// <param name="registerModel">registerModel parameter</param>
        /// <returns>returns the register user</returns>
        public Task<RegisterResponseModel> UserSignUp(RegisterModel registerModel)
        {
            try
            {
                var response = this.accountRL.UserSignUp(registerModel);
                return response;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// User Login method
        /// </summary>
        /// <param name="loginModel">loginModel parameter</param>
        /// <returns>returns the login user</returns>
        public Task<LoginResponseModel> UserLogin(LoginModel loginModel)
        {
            try
            {
                var response = this.accountRL.UserLogin(loginModel);
                return response;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// Forget Password Method
        /// </summary>
        /// <param name="forgetModel">forgetModel parameter</param>
        /// <returns>returns reset password link</returns>
        public Task<string> ForgetPassword(ForgetModel forgetModel)
        {
            try
            {
                var response = this.accountRL.ForgetPassword(forgetModel);
                return response;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public Task<string> ResetPassword(ResetModel resetModel)
        {
            try
            {
                var response = this.accountRL.ResetPassword(resetModel);
                return response;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
    }
}
