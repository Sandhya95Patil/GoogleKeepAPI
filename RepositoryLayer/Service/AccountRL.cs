//-----------------------------------------------------------------------
// <copyright file="AccountRL.cs" company="BridgeLabz">
//     Company copyright tag.
// </copyright>
// <creater name="Sandhya Patil"/>
//-----------------------------------------------------------------------
namespace RepositoryLayer.Service
{
    using CommonLayer.ImageUpload;
    using CommonLayer.Model;
    using CommonLayer.Response;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using RepositoryLayer.EncryptedPassword;
    using RepositoryLayer.Interface;
    using RepositoryLayer.MSMQ;
    using RepositoryLayer.Token;
    using ServiceStack.Redis;
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using System.IdentityModel.Tokens.Jwt;
    using System.Linq;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Account Repository Layer class
    /// </summary>
    public class AccountRL : IAccountRL
    {
        /// <summary>
        /// Inject the IConfiguration
        /// </summary>
        IConfiguration configuration;

        /// <summary>
        /// Initializes memory for Account class
        /// </summary>
        /// <param name="configuration">configuration parameter</param>
        public AccountRL (IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// User Sign Up method
        /// </summary>
        /// <param name="registrationModel">registrationModel parameter</param>
        /// <returns>returns the register user</returns>
        public async Task<RegisterResponseModel> UserSignUp(RegisterModel registrationModel)
        {
            try
            {
                var userType = "user";
                var password = PasswordEncrypt.Encryptdata(registrationModel.Password);

                SqlConnection sqlConnection = new SqlConnection(configuration["ConnectionStrings:connectionDb"]);
                SqlCommand sqlCommand = new SqlCommand("AddUser", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@FirstName", registrationModel.FirstName);
                sqlCommand.Parameters.AddWithValue("@LastName", registrationModel.LastName);
                sqlCommand.Parameters.AddWithValue("@Email", registrationModel.Email);
                sqlCommand.Parameters.AddWithValue("@Password", password);
                sqlCommand.Parameters.AddWithValue("@MobileNumber", registrationModel.MobileNumber);
                sqlCommand.Parameters.AddWithValue("@Profile", registrationModel.Profile);
                sqlCommand.Parameters.AddWithValue("@Service", registrationModel.Service);
                sqlCommand.Parameters.AddWithValue("@UserType", userType);
                sqlConnection.Open();

                var response = await sqlCommand.ExecuteNonQueryAsync();
                if (response > 0)
                {
                    var showResponse = new RegisterResponseModel()
                    {
                       // Id = registrationModel.Id,
                        FirstName = registrationModel.FirstName,
                        LastName = registrationModel.LastName,
                        Email = registrationModel.Email,
                        MobileNumber = registrationModel.MobileNumber,
                        Profile = registrationModel.Profile,
                        Service=registrationModel.Service,
                        UserType = userType
                    };
                    return showResponse;
                }
                else
                {
                    return null;
                }
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
        public async Task<LoginResponseModel> UserLogin(LoginModel loginModel)
        {
            try
            {
                var password = PasswordEncrypt.Encryptdata(loginModel.Password);

                SqlConnection sqlConnection = new SqlConnection(configuration["ConnectionStrings:connectionDb"]);
                SqlCommand sqlCommand = new SqlCommand("LoginUser", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Email", loginModel.Email);
                sqlCommand.Parameters.AddWithValue("@Password", password);
                sqlConnection.Open();
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                var userData = new RegisterModel();
                while (sqlDataReader.Read())
                {
                    userData = new RegisterModel();
                    userData.Id = Convert.ToInt32(sqlDataReader["Id"]);
                    userData.Email = sqlDataReader["Email"].ToString();
                    userData.Password = sqlDataReader["Password"].ToString();
                    userData.FirstName = sqlDataReader["FirstName"].ToString();
                    userData.LastName = sqlDataReader["LastName"].ToString();
                    userData.MobileNumber = sqlDataReader["MobileNumber"].ToString();
                    userData.Profile = sqlDataReader["Profile"].ToString();
                    userData.Service = sqlDataReader["Service"].ToString();
                    userData.UserType = sqlDataReader["UserType"].ToString();
                }
                sqlDataReader.Close();
                if (userData != null)
                {
                    if (password.Equals(userData.Password))
                    {
                        ////Here generate encrypted key and result store in security key
                        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:token"]));

                        //// here using securitykey and algorithm(security) the credentials is generate(SigningCredentials present in Token)
                        var creadintials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                        var claims = new[] {
                         new Claim ("Id", userData.Id.ToString()),
                         new Claim("Email", userData.Email.ToString()),
                         new Claim("Password", userData.Password.ToString()),
                        };

                        var token = new JwtSecurityToken("Security token", "https://Test.com",
                            claims,
                            DateTime.UtcNow,
                            expires: DateTime.Now.AddDays(1),
                            signingCredentials: creadintials);
                        var returnToken = new JwtSecurityTokenHandler().WriteToken(token);

                        /*var loginTime = DateTime.Now.ToString();
                        RedisEndpoint redisEndpoint = new RedisEndpoint("localhost", 6379);
                        using (RedisClient client = new RedisClient(redisEndpoint))
                        {
                            if (client.Get<string>(loginModel.Email + loginModel.Password) == null)
                            {
                                client.Set<string>(loginModel.Email + loginModel.Password, loginTime);
                                loginTime = client.Get<string>(loginModel.Email + loginModel.Password);
                            }
                            else
                            {
                                client.Remove(loginModel.Email + loginModel.Password);
                                client.Set<string>(loginModel.Email + loginModel.Password, loginTime);
                                loginTime = client.Get<string>(loginModel.Email + loginModel.Password);
                            }
                        }*/

                        var responseShow = new LoginResponseModel()
                        {
                            Id=userData.Id,
                            FirstName = userData.FirstName,
                            LastName = userData.LastName,
                            Email = userData.Email,
                            MobileNumber = userData.MobileNumber,
                            Profile=userData.Profile,
                            Service=userData.Service,
                            UserType=userData.UserType,
                            Token = returnToken,
                          //  LoginTime=loginTime
                        };
                        return responseShow;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// Forget Password method
        /// </summary>
        /// <param name="forgetModel">forgetModel parameter</param>
        /// <returns>returns reset link</returns>
        public async Task<string> ForgetPassword(ForgetModel forgetModel)
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(configuration["ConnectionStrings:connectionDb"]);
                SqlCommand sqlCommand = new SqlCommand("ForgetPassword", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Email", forgetModel.Email);
                sqlConnection.Open();
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                var userData = new RegisterModel();
                while (sqlDataReader.Read())
                {
                    userData = new RegisterModel();
                    userData.Id = Convert.ToInt32(sqlDataReader["Id"]);
                    userData.Email = sqlDataReader["Email"].ToString();
                }
                sqlDataReader.Close();
                if (userData != null)
                {
                    TokenGenerator tokenGenerator = new TokenGenerator(this.configuration);
                    var token = tokenGenerator.TokenGenerate(forgetModel);
                    ////create the object of MSMQSender class
                    MSMQSender msmqSender = new MSMQSender();
                    ////call the method ForgetPasswordMessage
                    msmqSender.ForgetPasswordMessage(forgetModel.Email, token);
                    return "Token Has Been Sent";
                }
                else
                {
                    return "Enter Correct Email Address";
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        /// <summary>
        /// Reset Password Method
        /// </summary>
        /// <param name="resetModel">resetModel parameter</param>
        /// <returns>returns the reset password</returns>
        public async Task<string> ResetPassword(ResetModel resetModel)
        {
            try
            {
                string pass = PasswordEncrypt.Encryptdata(resetModel.NewPassword);

                SqlConnection sqlConnection = new SqlConnection(configuration["ConnectionStrings:connectionDb"]);
                SqlCommand sqlCommand = new SqlCommand("ResetPassword", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                ////token handler 
                var handler = new JwtSecurityTokenHandler();

                ////read the token
                var jsonToken = handler.ReadToken(resetModel.ResetToken);

                ////read token as json web token
                var tokenS = handler.ReadToken(resetModel.ResetToken) as JwtSecurityToken;

                ////claim for id
             //   var userid = tokenS.Claims.FirstOrDefault(claim => claim.Type == "Id").Value;

                ////claim for email
                var jwtEmail = tokenS.Claims.FirstOrDefault(claim => claim.Type == "Email").Value;

                sqlCommand.Parameters.AddWithValue("Email", jwtEmail);
                sqlCommand.Parameters.AddWithValue("NewPassword", pass);
                sqlConnection.Open();
                ////save the changes asynchronuosly
                var response = await sqlCommand.ExecuteNonQueryAsync();
                sqlConnection.Close();

                if (response > 0)
                {
                    return "Password Changed Successfully";
                }
                else
                {
                    return "Password Not Changed Please Try Again";
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }

        public async Task<RegisterResponseModel> Profile(IFormFile formFile, int userId)
        {
            try
            {
                UploadImage uploadImage = new UploadImage(this.configuration, formFile);
                var url = uploadImage.Upload(formFile);
                SqlConnection sqlConnection = new SqlConnection(configuration["ConnectionStrings:connectionDb"]);
                SqlCommand sqlCommand = new SqlCommand("AddProfile", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@UserId", userId);
                sqlCommand.Parameters.AddWithValue("@Profile", url);
                sqlConnection.Open();
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                var userData = new RegisterModel();
                while (sqlDataReader.Read())
                {
                    userData = new RegisterModel();
                    userData.Id = Convert.ToInt32(sqlDataReader["Id"]);
                    userData.FirstName = sqlDataReader["FirstName"].ToString();
                    userData.LastName = sqlDataReader["LastName"].ToString();
                    userData.Email = sqlDataReader["Email"].ToString();
                    userData.Profile = sqlDataReader["Profile"].ToString();
                    userData.MobileNumber = sqlDataReader["MobileNumber"].ToString();
                    userData.Service = sqlDataReader["Service"].ToString();
                    userData.UserType = sqlDataReader["UserType"].ToString();
                }
                if (userData != null)
                {
                    var showResponse = new RegisterResponseModel()
                    {
                        Id = userData.Id,
                        FirstName = userData.FirstName,
                        LastName = userData.LastName,
                        Email = userData.Email,
                        MobileNumber = userData.MobileNumber,
                        Profile = userData.Profile,
                        Service = userData.Service,
                        UserType = userData.UserType
                    };
                    return showResponse;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
        }
    }
}
