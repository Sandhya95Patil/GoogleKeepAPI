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
    using CommonLayer.ShowModel;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using RepositoryLayer.EncryptedPassword;
    using RepositoryLayer.Interface;
    using RepositoryLayer.MSMQ;
    using RepositoryLayer.Token;
    using ServiceStack.Redis;
    using System;
    using System.Collections.Generic;
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
        /// This method is for connection with database using connection string
        /// </summary>
        /// <param name="connectionName">connectionName parameter</param>
        /// <returns>return the connection</returns>
        public SqlConnection GetConnection(string connectionName)
        {
            SqlConnection connection = new SqlConnection(configuration["ConnectionStrings:connectionDb"]);
            connection.Open();
            return connection;
        }

        /// <summary>
        /// Method for get command
        /// </summary>
        /// <param name="command">command parameter</param>
        /// <returns>return command</returns>
        public SqlCommand GetCommand(string command)
        {
            string connection = "";
            SqlConnection sqlConnection = GetConnection(connection);
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            sqlCommand.CommandType = CommandType.StoredProcedure;
            return sqlCommand;
        }

        /// <summary>
        /// Stored Procedure Parameter Data class
        /// </summary>
        class StoredProcedureParameterData {
            public StoredProcedureParameterData(string name, dynamic value) 
            { 
                this.name = name; 
                this.value = value; 
            } 
            
            public string name { get; set; } 
           public dynamic value { get; set; }
        }

        /// <summary>
        /// Stored Procedure Execute Reader method
        /// </summary>
        /// <param name="spName">spName parameter</param>
        /// <param name="spParams">spParams parameter</param>
        /// <returns>return procedure name and parameters</returns>
        private async Task<DataTable> StoredProcedureExecuteReader(string spName, IList<StoredProcedureParameterData> spParams)
        {
            try
            { 
                SqlCommand command = GetCommand(spName); 
                for (int i = 0; i < spParams.Count; i++)
                {
                    command.Parameters.AddWithValue(spParams[i].name, spParams[i].value); 
                } 
                DataTable table = new DataTable(); 
                SqlDataReader dataReader = await command.ExecuteReaderAsync(); 
                table.Load(dataReader); 
                return table;
            }
            catch (Exception exception) 
            { 
                throw exception;
            } 
        }

        /// <summary>
        /// User Sign Up method
        /// </summary>
        /// <param name="registrationModel">registrationModel parameter</param>
        /// <returns>returns the register user</returns>
        public async Task<RegisterResponseModel> UserSignUp(ShowRegisterModel registrationModel)
        {
            try
            {
                var userType = "user";
                var password = PasswordEncrypt.Encryptdata(registrationModel.Password);
                List<StoredProcedureParameterData> paramList = new List<StoredProcedureParameterData> ();
                paramList.Add(new StoredProcedureParameterData("@FirstName", registrationModel.FirstName));
                paramList.Add(new StoredProcedureParameterData("@LastName", registrationModel.LastName));
                paramList.Add(new StoredProcedureParameterData("@Email", registrationModel.Email));
                paramList.Add(new StoredProcedureParameterData("@Password", password));
                paramList.Add(new StoredProcedureParameterData("@MobileNumber", registrationModel.MobileNumber));
                paramList.Add(new StoredProcedureParameterData("@Profile", registrationModel.Profile));
                paramList.Add(new StoredProcedureParameterData("@Service", registrationModel.Service));
                paramList.Add(new StoredProcedureParameterData("@UserType", userType));
                DataTable table = await StoredProcedureExecuteReader("AddUser", paramList);
                var userData = new RegisterResponseModel();

                foreach (DataRow dataRow in table.Rows)
                {
                    userData = new RegisterResponseModel();
                    userData.Id = (int)dataRow["Id"];
                    userData.FirstName = dataRow["FirstName"].ToString();
                    userData.LastName = dataRow["LastName"].ToString();
                    userData.Email = dataRow["Email"].ToString();
                    userData.MobileNumber = dataRow["MobileNumber"].ToString();
                    userData.Profile = dataRow["Profile"].ToString();
                    userData.Service = dataRow["Service"].ToString();
                    userData.UserType = dataRow["UserType"].ToString();
                }
                if (userData.Email != null)
                {
                    return userData;
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

                List<StoredProcedureParameterData> paramList = new List<StoredProcedureParameterData>();
                paramList.Add(new StoredProcedureParameterData("@Email", loginModel.Email));
                paramList.Add(new StoredProcedureParameterData("@Password", password));
                DataTable table = await StoredProcedureExecuteReader("LoginUser", paramList);
                var userData = new RegisterModel();
                foreach (DataRow dataRow in table.Rows)
                {
                    userData = new RegisterModel();
                    userData.Id = (int)dataRow["Id"];
                    userData.FirstName = dataRow["FirstName"].ToString();
                    userData.LastName = dataRow["LastName"].ToString();
                    userData.Email = dataRow["Email"].ToString();
                    userData.Password = dataRow["Password"].ToString();
                    userData.MobileNumber = dataRow["MobileNumber"].ToString();
                    userData.Profile = dataRow["Profile"].ToString();
                    userData.Service = dataRow["Service"].ToString();
                    userData.UserType = dataRow["UserType"].ToString();
                }

                if (userData.Email != null)
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

                        var logTime = DateTime.Now.ToString();
                        RedisEndpoint redisEndpoint = new RedisEndpoint("localhost", 6379);
                        using (RedisClient client = new RedisClient(redisEndpoint))
                        {
                               var set = client.Set<string>(returnToken, logTime);
                                var redisToken=client.Get<string>(returnToken);
                        }

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
                            Token = returnToken
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
            catch (Exception)
            {
                throw new Exception("Email or Passord is Not Correct");
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
                List<StoredProcedureParameterData> paramList = new List<StoredProcedureParameterData>();
                paramList.Add(new StoredProcedureParameterData("@Email", forgetModel.Email));
                DataTable table = await StoredProcedureExecuteReader("ForgetPassword", paramList);
                var userData = new RegisterModel();

                foreach (DataRow row in table.Rows)
                {
                    userData = new RegisterModel();
                    userData.Id = Convert.ToInt32(row["Id"]);
                    userData.Email = row["Email"].ToString();
                }
                if (userData.Email != null)
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
                ////token handler 
                var handler = new JwtSecurityTokenHandler();

                ////read the token
                var jsonToken = handler.ReadToken(resetModel.ResetToken);

                ////read token as json web token
                var tokenS = handler.ReadToken(resetModel.ResetToken) as JwtSecurityToken;

                ////claim for email
                var jwtEmail = tokenS.Claims.FirstOrDefault(claim => claim.Type == "Email").Value;


                List<StoredProcedureParameterData> paramList = new List<StoredProcedureParameterData>();
                paramList.Add(new StoredProcedureParameterData("@Email", jwtEmail));
                paramList.Add(new StoredProcedureParameterData("@NewPassword", pass));
                DataTable table = await StoredProcedureExecuteReader("ResetPassword", paramList);
                var res = table.NewRow();                

                if (res != null)
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

        /// <summary>
        /// Upload profile method
        /// </summary>
        /// <param name="formFile">formFile parameter</param>
        /// <param name="userId">userId parameter</param>
        /// <returns>return the uploaded profile</returns>
        public async Task<RegisterResponseModel> Profile(IFormFile file, int userId)
        {
            try
            {
                UploadImage uploadImage = new UploadImage(this.configuration, file);
                var url = uploadImage.Upload(file);
                List<StoredProcedureParameterData> paramList = new List<StoredProcedureParameterData>();
                paramList.Add(new StoredProcedureParameterData("@UserId", userId));
                paramList.Add(new StoredProcedureParameterData("@Profile", url));
                DataTable table = await StoredProcedureExecuteReader("AddProfile", paramList);
                var userData = new RegisterModel();
                foreach (DataRow row in table.Rows)
                {
                    userData = new RegisterModel();
                    userData.Id = (int)row["Id"];
                    userData.FirstName = row["FirstName"].ToString();
                    userData.LastName = row["LastName"].ToString();
                    userData.Email = row["Email"].ToString();
                    userData.MobileNumber = row["MobileNumber"].ToString();
                    userData.Profile = row["Profile"].ToString();
                    userData.Service = row["Service"].ToString();
                    userData.UserType = row["UserType"].ToString();
                }
                if (userData.Email != null)
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
