using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System;

namespace TowerSoft.SteamAchievs.Website.Infrastructure {
    public class RoleAttribute : Attribute, IAuthorizationFilter {
        private readonly string[] roles;

        public RoleAttribute(params string[] types) {
            roles = types;
        }

        public void OnAuthorization(AuthorizationFilterContext context) {
            if (context.HttpContext.User.Identity.IsAuthenticated) {
                // Automatically give admins access
                if (context.HttpContext.User.IsInRole(RoleType.Admin))
                    return;

                // Check if the user has any of the roles provided
                foreach (string type in roles) {
                    if (context.HttpContext.User.IsInRole(type)) {
                        return;
                    }
                }

                context.Result = new RedirectToRouteResult(new RouteValueDictionary {
                    { "action", "NoAccess" },
                    { "controller", "Error" },
                    { "area", "" }
                });
            } else {
                context.Result = new RedirectToRouteResult(new RouteValueDictionary {
                    { "action", "Login" },
                    { "controller", "Account" },
                    { "area", "" }
                });
            }
        }
    }
}
