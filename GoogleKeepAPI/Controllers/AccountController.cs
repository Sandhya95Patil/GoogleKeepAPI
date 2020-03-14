//-----------------------------------------------------------------------
// <copyright file="AccountController.cs" company="BridgeLabz">
//     Company copyright tag.
// </copyright>
// <creater name="Sandhya Patil"/>
//-----------------------------------------------------------------------
namespace GoogleKeepAPI.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using BussinessLayer.Interface;
    using CommonLayer.Model;
    using CommonLayer.ShowModel;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// AccountController class
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        /// <summary>
        /// Inject the bussiness layer Account interface
        /// </summary>
        private readonly IAccountBL accountBL;

        /// <summary>
        /// Initializes the memory for Account controller 
        /// </summary>
        /// <param name="accountBL">accountBL parameter</param>
        public AccountController (IAccountBL accountBL)
        {
            this.accountBL = accountBL;
        }

        /// <summary>
        ///UserSignUp method
        /// </summary>
        /// <param name="registerModel">registerModel parameter</param>
        /// <returns>returns the register user</returns>
        [HttpPost]
        [Route("SignUp")]
        [AllowAnonymous]
        public async Task<IActionResult> UserSignUp(ShowRegisterModel registerModel)
        {
            try
            {
                var data = await this.accountBL.UserSignUp(registerModel);
                if (data != null)
                {
                    return this.Ok(new { status = "True", message = "Register Successfully", data });
                }
                else
                {
                    return this.BadRequest(new { status = "False", message = "Failed To Register" });
                }
            }
            catch (Exception exception)
            {
                return BadRequest(new { message = exception.Message });
            }
        }

        /// <summary>
        /// User Login method
        /// </summary>
        /// <param name="loginModel">loginModel parameter</param>
        /// <returns>returns the login user</returns>
        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> UserLogin(LoginModel loginModel)
        {
            try
            {
                var data = await this.accountBL.UserLogin(loginModel);
                if (data != null)
                {
                    return this.Ok(new { status = "True", message = "Login Successfully", data });
                }
                else
                {
                    return this.BadRequest(new { status = "False", message = "Failed To Login" });
                }
            }
            catch (Exception exception)
            {
                return BadRequest(new { message = exception.Message });
            }
        }

        /// <summary>
        /// Forget Password Method
        /// </summary>
        /// <param name="forgetModel">forgetModel parameter</param>
        /// <returns>returns reset link</returns>
        [HttpPost]
        [Route("ForgetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword(ForgetModel forgetModel)
        {
            try
            {
                var data = await this.accountBL.ForgetPassword(forgetModel);
                if (data != null)
                {
                    return this.Ok(new { status = "True", message = "Link Has Been Sent To Your Email" });
                }
                else
                {
                    return this.BadRequest(new { status = "False", message = "Your Email Is Not Correct" });
                }
            }
            catch (Exception exception)
            {
                return BadRequest(new { message = exception.Message });
            }
        }

        /// <summary>
        /// Reset Password method
        /// </summary>
        /// <param name="resetModel">resetModel parameter</param>
        /// <returns>returns the reset password</returns>
        [HttpPost]
        [Route("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetModel resetModel)
        {
            try
            {
                var data = await this.accountBL.ResetPassword(resetModel);
                if (data != null)
                {
                    return this.Ok(new { status = "true", message = "Password Reset Successfully" });
                }
                else
                {
                    return this.BadRequest(new { status = "False", message = "Failed To Reset Password" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { message = exception.Message });
            }  
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            try
            {
                var claim = Convert.ToInt32(HttpContext.User.Claims.FirstOrDefault(c => c.Type == "Id").Value);
                var data = await this.accountBL.Profile(file, claim);
                if (data != null)
                {
                    return this.Ok(new { status = "true", message = "Profile Set Successfully", data});
                }
                else
                {
                    return this.BadRequest(new { status = "False", message = "Failed To Set Profile" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { message = exception.Message });
            }
        }

        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var data = await accountBL.GetUsers();
                if (data != null)
                {
                    return this.Ok(new { status = "true", message = "Get All Users", data });
                }
                else
                {
                    return this.BadRequest(new { status = "False", message = "Failed To Get Users" });
                }
            }
            catch (Exception exception)
            {
                return this.BadRequest(new { message = exception.Message });
            }
        }

        [HttpPost]
        [Route("PushNotification")]
        [AllowAnonymous]
        public async Task Send(PushNotificationItem pushNotificationItem)
        {
            await accountBL.SendNotification(pushNotificationItem);
        }

        [HttpGet]
        [Route("signin-facebook")]
        [AllowAnonymous]
        public IActionResult Facebook()
        {
            //return Challenge(new AuthenticationProperties { RedirectUri = "/" });
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("HandleExternalLogin", "Account")
            };
          
            return Challenge(authenticationProperties, "Facebook");
        }

        [HttpGet]
        [Route("signin-google")]
        [AllowAnonymous]
        public IActionResult Google()
        {
            var authenticationProperties = new AuthenticationProperties
            {
                RedirectUri = Url.Action("HandleExternalLogin", "Account")
            };

            return Challenge(authenticationProperties, "Google");

            //return Challenge(new AuthenticationProperties { RedirectUri = "/" });
        }


        [HttpPost]
        [Route("")]
        public async Task<IActionResult> HandleExternalLogin()
        {
            var claimsPrincipal = await HttpContext.Authentication.AuthenticateAsync("ExternalCookie");

            await HttpContext.Authentication.SignInAsync("MainCookie", claimsPrincipal);
            await HttpContext.Authentication.SignOutAsync("ExternalCookie");
            return Redirect("~/");
        }
    }
}