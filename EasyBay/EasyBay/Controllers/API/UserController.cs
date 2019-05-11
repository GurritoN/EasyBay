using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EasyBay.BusinessLogic;
using EasyBay.DataBase;
using EasyBay.Interfaces;
using EasyBay.Messaging;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Storage;

namespace EasyBay.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IAuctionFacade facade;

        public UserController(AuctionContext context)
        {
            facade = new AuctionFacade(context);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet("{username}")]
        public User Get(string username)
        {
            return facade.GetUser(username);
        }

        [HttpPut]
        public void Create([FromForm]string username, [FromForm]string password, [FromForm]string email)
        {
            facade.CreateNewUser(username, password, email);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPatch]
        public void Edit([FromForm]string username, [FromForm]string password, [FromForm]string email)
        {
            if (User.IsInRole(Role.Admin) || username == User.Identity.Name)
                facade.EditUser(username, password, email);
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpDelete]
        public void Delete([FromForm]string username)
        {
            if (User.IsInRole(Role.Admin) || username == User.Identity.Name)
            {
                facade.DeleteUser(User.Identity.Name);
            }
        }

        [HttpPost("token")]
        public string Token([FromForm]string username, [FromForm]string password)
        {
            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                return null;
            }

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: Auth.ISSUER,
                    audience: Auth.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromDays(1)),
                    signingCredentials: new SigningCredentials(Auth.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        private ClaimsIdentity GetIdentity(string username, string password)
        {
            if (facade.ValidateCredentials(username, password))
            {
                var user = facade.GetUser(username);
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, username),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            return null;
        }
    }
}