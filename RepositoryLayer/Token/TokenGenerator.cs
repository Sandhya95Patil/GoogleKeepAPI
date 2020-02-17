//-----------------------------------------------------------------------
// <copyright file="TokenGenerator.cs" company="BridgeLabz">
//     Company copyright tag.
// </copyright>
// <creater name="Sandhya Patil"/>
//-----------------------------------------------------------------------
namespace RepositoryLayer.Token
{
    using CommonLayer.Model;
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;

    /// <summary>
    /// Token Generator class
    /// </summary>
    public class TokenGenerator
    {
        /// <summary>
        /// Inject the IConfiguration
        /// </summary>
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes the memory for Token generator class
        /// </summary>
        /// <param name="configuration">configuration parameter</param>
        public TokenGenerator (IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Token Generate method
        /// </summary>
        /// <param name="forgetModel">forgetModel parameter</param>
        /// <returns>returns the token</returns>
        public string TokenGenerate(ForgetModel forgetModel)
        {
            ////Here generate encrypted key and result store in security key
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:token"]));

            //// here using securitykey and algorithm(security) the credentials is generate(SigningCredentials present in Token)
            var creadintials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] {
                        // new Claim ("Id", userData.Id.ToString()),
                         new Claim("Email", forgetModel.Email.ToString()),
                      //   new Claim("Password", userData.Password.ToString()),
                        };

            ////Security tken
            var token = new JwtSecurityToken("Security token", "https://Test.com",
                claims,
                DateTime.UtcNow,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creadintials);
            var returnToken = new JwtSecurityTokenHandler().WriteToken(token);
            return returnToken;
        }
    }
        
}
