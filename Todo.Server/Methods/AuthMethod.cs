using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Todo.Server.Methods
{
    public class AuthMethod
    {
        public static CookieAuthenticationOptions get_CookieAuthenticationOptions()
        {
            var options = new CookieAuthenticationOptions
            {
                LoginPath = "/login",
                AccessDeniedPath = "/forbidden"
            };
            return options;
        }


        public static ClaimsIdentity get_claimsIdentityAsync(string UserName, string UserId, params string[] RolesName)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, UserName),
                new Claim(ClaimTypes.NameIdentifier, UserId)
            };

            foreach (string RoleName in RolesName)
            {
                claims.Add(new Claim(ClaimTypes.Role, RoleName));
            }

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            return claimsIdentity;
        }

        public static AuthenticationProperties get_authProperties(Boolean ispersist)
        {
            return new AuthenticationProperties
            {
                AllowRefresh = true,
                // Refreshing the authentication session should be allowed.

                //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                // The time at which the authentication ticket expires. A 
                // value set here overrides the ExpireTimeSpan option of 
                // CookieAuthenticationOptions set with AddCookie.

                IsPersistent = ispersist,
                // Whether the authentication session is persisted across 
                // multiple requests. Required when setting the 
                // ExpireTimeSpan option of CookieAuthenticationOptions 
                // set with AddCookie. Also required when setting 
                // ExpiresUtc.

                //IssuedUtc = <DateTimeOffset>,
                // The time at which the authentication ticket was issued.

                //RedirectUri = <string>
                // The full path or absolute URI to be used as an http 
                // redirect response value.
            };
        }



    }
}
