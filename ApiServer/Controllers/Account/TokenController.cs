using ApiModel.Entities;
using ApiServer.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiServer.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("/token")]
    public class TokenController : Controller
    {
        Services.AuthMan authman;
        public TokenController(Data.ApiDbContext context)
        {
            authman = new Services.AuthMan(this, context);
        }

        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> RequestToken([FromBody] TokenRequestModel request)
        {
            if (ModelState.IsValid == false)
                return BadRequest(ModelState);

            var result = await authman.LoginRequest(request.Account, request.Password);

            if (result.loginResult == Services.AuthMan.LoginResult.Ok)
                return MakeToken(result.acc.Id);
            else
                return LoginFailed(result.loginResult);
        }

        class LoginSuccessModel
        {
            public string Token { get; set; }
        }
        class LoginFailedModel
        {
            public string Error { get; set; }
        }

        IActionResult LoginFailed(Services.AuthMan.LoginResult result)
        {
            string err = "";
            switch (result)
            {
                case Services.AuthMan.LoginResult.AccOrPasswordWrong: err = "account or password wrong"; break;
                case Services.AuthMan.LoginResult.Expired: err = "account expired"; break;
                case Services.AuthMan.LoginResult.Frozen: err = "account is forzen by admin"; break;
                case Services.AuthMan.LoginResult.NotActivation: err = "account not activate yet"; break;
            }
            return BadRequest(new ErrorRespondModel() { Message = err });
        }

        IActionResult MakeToken(string accid)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Services.SiteConfig.Instance.Json.TokenKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[] { new Claim(ClaimTypes.Name, accid) };

            var expires = DateTime.Now.AddDays(Services.SiteConfig.Instance.Json.TokenValidDays);
            //var expires = DateTime.Now.AddMinutes(1);
            var token = new JwtSecurityToken(
                issuer: "damaozhu.com",
                audience: "damaozhu.com",
                claims: claims,
                notBefore: DateTime.Now,
                expires: expires,
                signingCredentials: creds);

            return Ok(new { Token = new JwtSecurityTokenHandler().WriteToken(token), Expires = expires.ToString("yyyy-MM-dd HH:mm:ss") });
        }

    }

}
