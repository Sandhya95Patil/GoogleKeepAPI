//-----------------------------------------------------------------------
// <copyright file="AccountTestCases.cs" company="BridgeLabz">
//     Company copyright tag.
// </copyright>
// <creater name="Sandhya Patil"/>
//-----------------------------------------------------------------------
namespace GoogleKeepTestCases.TestCases
{
    using BussinessLayer.Interface;
    using BussinessLayer.Service;
    using CommonLayer.Model;
    using CommonLayer.ShowModel;
    using GoogleKeepAPI.Controllers;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using RepositoryLayer.Interface;
    using Xunit;

    /// <summary>
    /// Account Test Cases class
    /// </summary>
    public class AccountTestCases
    {
        /// <summary>
        /// Inject the Account Controller
        /// </summary>
        AccountController accountController;

        /// <summary>
        /// Inject the IAccountBL
        /// </summary>
        IAccountBL accountBL;

        /// <summary>
        /// Initializes the memory for Account Test Cases class
        /// </summary>
        public AccountTestCases()
        {
            var repository = new Mock<IAccountRL>();
            this.accountBL = new AccountBL(repository.Object);
            accountController = new AccountController(this.accountBL);
        }

        /// <summary>
        /// Valid Value Account Register this method check the valid value
        /// </summary>
        [Fact]
        public void NotOkResult_AccountRegister()
        {
            var model = new ShowRegisterModel()
            {
                FirstName = "abc",
                LastName = "xyz",
                MobileNumber = "7845124578",
                Email = "ads@gmail.com",
                Password = "asdjnk",
                Profile = "Rose",
                Service = "basic"
            };
            //Act: where the method we are testing is executed
            var response = accountController.UserSignUp(model);

            //Assert: Compaire what we expect
            Assert.IsNotType<OkObjectResult>(response);
        }

        /// <summary>
        /// Not Valid Account Register method
        /// </summary>
        [Fact]
        public void NotBadResult_AccountRegister()
        {
            var model = new ShowRegisterModel()
            {
                FirstName = "abc",
                LastName = "xyz",
                MobileNumber = "7845124578",
                Email = "ads@gmail.com",
                Password = "asdjnk",
                Profile = "Rose",
                Service = "basic"
            };
            //Act: where the method we are testing is executed
            var response = accountController.UserSignUp(model);

            //Assert: Compaire what we expect
            Assert.IsNotType<BadRequestResult>(response);
        }

        /// <summary>
        /// Ckeheck Not Null Account Register method
        /// </summary>
        [Fact]
        public void CkeheckNotNull_AccountRegister()
        {
            var model = new RegisterModel()
            {
                FirstName = "abc",
                LastName = "xyz",
                MobileNumber = "7845124578",
                Email = "ads@gmail.com",
                Password = "asdjnk",
                Profile = "Rose",
                Service = "basic",
                UserType = "user"
            };
          
            //Assert: Compaire what we expect
            Assert.NotNull(model);
        }

        /// <summary>
        /// Not Valid Account Login method
        /// </summary>
        [Fact]
        public void NotOkResult_AccountLogin()
        {
            var model = new LoginModel()
            {
                Email = "abc@gmail.com",
                Password = "abc"
            };
            //Act: where the method we are testing is executed
            var response = accountController.UserLogin(model);

            //Assert: Compaire what we expect
            Assert.IsNotType<OkObjectResult>(response);
        }

        /// <summary>
        /// Not Valid Account Login method
        /// </summary>
        [Fact]
        public void NtBadResult_AccountLogin()
        {
            var model = new LoginModel()
            {
                Email = "",
                Password = ""
            };
            //Act: where the method we are testing is executed
            var response = accountController.UserLogin(model);

            //Assert: Compaire what we expect
            Assert.IsNotType<BadRequestResult>(response);
        }

        /// <summary>
        /// Valid Value for Forget Password method
        /// </summary>
        [Fact]
        public void NotOkResult_ForgetPassword()
        {
            var model = new ForgetModel()
            {
                Email = "abc@gmail.com"
            };

            //Act: where the method we are testing is executed
            var response = accountController.ForgetPassword(model);

            //Assert: Compaire what we expect
            Assert.IsNotType<OkObjectResult>(response);
        }

        [Fact]
        public void NotBadResult_ForgetPassword()
        {
            var model = new ForgetModel()
            {
                Email = ""
            };

            //Act: where the method we are testing is executed
            var response = accountController.ForgetPassword(model);

            //Assert: Compaire what we expect
            Assert.IsNotType<BadRequestResult>(response);
        }

        /// <summary>
        /// Valid Value Reset Password method
        /// </summary>
        [Fact]
        public void NotOkResult_ResetPassword()
        {
            var model = new ResetModel()
            {
                Email = "abc@gmail.com",
                NewPassword = "abc",
                ResetToken = "asdfghjkl123"
            };

            //Act: where the method we are testing is executed
            var response = accountController.ResetPassword(model);

            //Assert: Compaire what we expect
            Assert.IsNotType<OkObjectResult>(response);
        }

        /// <summary>
        /// Not Valid Reset Password method
        /// </summary>
        [Fact]
        public void NotBadResult_ResetPassword()
        {
            var model = new ResetModel()
            {
                Email = "",
                NewPassword = "",
                ResetToken = ""
            };

            //Act: where the method we are testing is executed
            var response = accountController.ResetPassword(model);

            //Assert: Compaire what we expect
            Assert.IsNotType<BadRequestResult>(response);
        }
    }

}
