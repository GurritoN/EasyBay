using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EasyBay.BusinessLogic;
using EasyBay.DataBase;
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
        private AuctionFacade facade;

        public UserController(AuctionContext context)
        {
            facade = new AuctionFacade(context);
        }

        [HttpGet("get/{username}")]
        public User Get(string username)
        {
            return facade.GetUser(username);
        }

        [HttpPut("register")]
        public void Create(CreateUserRequest request)
        {
            facade.CreateNewUser(request.Username, request.Password, request.Email);
        }

        [Authorize]
        [HttpPatch("edit")]
        public void Edit(EditUserRequest request)
        {
            facade.EditUser(request.Username, request.Password, request.Email);
        }

        [Authorize]
        [HttpDelete("delete")]
        public void Delete()
        {
            facade.DeleteUser(User.Identity.Name);
        }

        [HttpPost("/token")]
        public string Token(string username, string password)
        {
            var identity = GetIdentity(username, password);
            if (identity == null)
            {
                return null;
            }

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
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