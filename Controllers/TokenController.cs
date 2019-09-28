
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Backend.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly string keyJwt;
        public TokenController(IOptions<AppSettings> appSettings){
            keyJwt=appSettings.Value.keyJwt;
        }
        [HttpGet]
        public async Task<object[]> Get () {
            try
            {
                var header = Request.Headers["Authorization"];
                if (header.ToString ().StartsWith ("Basic")){
                    string valueheader = header.ToString ().Substring ("Basic_".Length).Trim ();
                    string[] valueuser = Encoding.UTF8.GetString (Convert.FromBase64String (valueheader)).Split (":");

                    List<Claim> claims = new List<Claim> ();
                    claims.Add (new Claim (ClaimTypes.Name, valueuser[0]));
                    //add roles
                    claims.Add (new Claim (ClaimTypes.Role, "sa"));

                    Claim[] claimdata = claims.ToArray ();

                    //Private Password
                    SymmetricSecurityKey key = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (keyJwt));

                        SigningCredentials signingCredentials = new SigningCredentials (key, SecurityAlgorithms.HmacSha256Signature);
                        JwtSecurityToken token = new JwtSecurityToken (
                            issuer: "localhost",
                            audience: "localhost",
                            expires: DateTime.Now.AddDays (90),
                            claims: claimdata,
                            signingCredentials: signingCredentials
                        );
                        string sender = new JwtSecurityTokenHandler ().WriteToken (token);
                    return await Task.Factory.StartNew(()=>{

                        return new object[]{valueuser[0],sender};
                    });
                }else{
                    throw new System.Exception("Header Authorization Basic does not exist!!");
                }
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }

}