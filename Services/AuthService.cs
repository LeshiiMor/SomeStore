using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using SomeStore.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace SomeStore.Services
{
    public interface IAuthService
    {
        Task<SignInResult> SignInUser(User user,HttpContext context);
        Task<SignInResult> SignOut(HttpContext context);
    }
    public class AuthService : IAuthService
    {

        public async Task<SignInResult> SignInUser(User user,HttpContext context)
        {

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType,user.Role.Name)
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,ClaimsIdentity.DefaultRoleClaimType);
            await AuthenticationHttpContextExtensions.SignInAsync(context, new ClaimsPrincipal(id));
            return SignInResult.Success;
        }

        public async Task<SignInResult> SignOut(HttpContext context)
        {
            await AuthenticationHttpContextExtensions.SignOutAsync(context);
            return SignInResult.Success;
        }
    }
}
