using Intex_app.Models;
using Intex_app.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Intex_app.Infrastructure;
using System.Xml;
using System.Net;
using System.Text;
using System.IO;

namespace Intex_app.Controllers
{
    public class TokenController : Controller
    {

        private readonly JwtIssuerOptions _jwtOptions;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _loggger;
        private readonly JsonSerializerSettings _serializerSettings;
        public TokenController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILoggerFactory loggerFactory,
            IOptions<JwtIssuerOptions> jwtOptions
            )
        {
            _jwtOptions = jwtOptions.Value;
            _userManager = userManager;
            _signInManager = signInManager;
            _loggger = loggerFactory.CreateLogger<TokenController>();
            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented
            };
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("teableau")]
        [HttpGet]
        public IActionResult tableau()
        {
            return Redirect("https://10ay.online.tableau.com/#/site/intex/workbooks/820859?:origin=card_share_link");
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("token")]
        public async Task<IActionResult> Token(LoginModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
            {
                return BadRequest();
            }

            var user = _userManager.Users.FirstOrDefault(u => u.Email == model.Email);
            var valid = await _userManager.CheckPasswordAsync(user, model.Password);

            //this would make sure the user confirmed their email address, but we don't have a dummy email we can use
            if (user.EmailConfirmed)
            {

                if (valid)
                {
                    var request = (HttpWebRequest)WebRequest.Create("https://10ay.online.tableau.com/api/3.11/auth/signin");
                    request.Method = "POST";
                    request.ContentType = "text/xml";

                    byte[] bytes;
                    bytes = Encoding.ASCII.GetBytes("<tsRequest><credentials personalAccessTokenName = 'demo' personalAccessTokenSecret = 'uwl6pad4SkCd83VdVkmUAg==:t2SDxSqKBK0rcbEmpYe2TnSTpSFQLC8W'><site contentUrl = 'Intex'/></credentials></tsRequest>");

                    request.ContentLength = bytes.Length;

                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();

                    var result = (HttpWebResponse)request.GetResponse();


                    //Console.WriteLine(result);


                    using (Stream stream = result.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                        String responseString = reader.ReadToEnd();

                        XmlDocument docu = new XmlDocument();

                        docu.LoadXml(responseString);

                        var tableauTokenString = docu.GetElementsByTagName("credentials")[0].Attributes[0].Value;

                        Token tableauToken = new Token();

                        tableauToken.TokenString = tableauTokenString.ToString();

                        HttpContext.Session.SetJson("TableauToken", tableauToken);
                    }











                    var claims = new List<Claim>();
                    claims.Add(new Claim(ClaimTypes.Email, user.Email));
                    //claims.Add(new Claim(ClaimTypes.GivenName, user.name));

                    var jwt = new JwtSecurityToken(
                        issuer: _jwtOptions.Issuer,
                        audience: _jwtOptions.Audience,
                        claims: claims,
                        notBefore: _jwtOptions.NotBefore,
                        expires: _jwtOptions.Expiration,
                        signingCredentials: _jwtOptions.SigningCredentials
                        );

                    var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                    var response = new
                    {
                        access_token = encodedJwt,
                        expires_in = (int)_jwtOptions.ValidFor.TotalSeconds,
                        is_admin = user.Admin,
                    };

                    var json = JsonConvert.SerializeObject(response, _serializerSettings);
                    //return new OkObjectResult(json);
                    //use above for phone api
                    Token token = new Token();

                    token.TokenString = encodedJwt.ToString();

                    HttpContext.Session.SetJson("Token", token);

                               

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    if (user != null)
                    {
                        user.AccessFailedCount += 1;
                    }
                    return Unauthorized();
                }
            }
            else
            {
                return Ok("Make sure you confirm your email address");
            }
        }
    }
}
