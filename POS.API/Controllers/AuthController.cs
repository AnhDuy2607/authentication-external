using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Google.Apis.PeopleService.v1;
using POS.Model.Request;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace POS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly GoogleJsonWebSignature.ValidationSettings _validationSettings;

        public AuthController()
        {
            _validationSettings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new List<string> { "117980402378555030592", "874742790685-7d96kkir7acvm2evb3rooci7d1l22a0i.apps.googleusercontent.com" }
            };
        }

        [HttpPost("google")]
        public async Task<IActionResult> GoogleSignIn([FromBody] AuthenticationModel model)
        {
            var payload = await GoogleJsonWebSignature.ValidateAsync(model.Token, _validationSettings);
            //var service = new PeopleServiceService(new BaseClientService.Initializer
            //{
            //    HttpClientInitializer = new UserCredential(new GoogleAuthorizationCodeFlow(
            //        new GoogleAuthorizationCodeFlow.Initializer
            //        {
            //            ClientSecrets = new ClientSecrets
            //            {
            //                ClientId = "874742790685-7d96kkir7acvm2evb3rooci7d1l22a0i.apps.googleusercontent.com",
            //                ClientSecret = "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"
            //            },
            //            Scopes = new[] { PeopleServiceService.Scope.UserinfoProfile }
            //        }),
            //        "user",
            //        new TokenResponse
            //        {
            //            AccessToken = accessToken,
            //            TokenType = "Bearer"
            //        })
            //});
            //var person = await service.People.Get("people/me").ExecuteAsync();
            var userId = payload.Subject;
            var userName = payload.Name;
            var email = payload.Email;

            var userClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Email, email),
            };

            var identity = new ClaimsIdentity(userClaims, "GoogleAuth");
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7),
                AllowRefresh = true
            });
            var temp = HttpContext.Response.Headers["set-cookie"];
            return Ok(temp);
        }

        [Authorize]
        [HttpGet("me")]
        public IActionResult GetUserInfo()
        {
            var result = HttpContext.User.Claims
                .Where(x => x.Type == ClaimTypes.Name || x.Type == ClaimTypes.Email)
                .ToDictionary(x => x.Type, x => x.Value);
            return Ok(result);
        }
    }
}
