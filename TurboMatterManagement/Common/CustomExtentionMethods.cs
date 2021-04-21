using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Security.Principal;
using System.Security.Claims;

namespace TurboMatterManagement.Common
{
    public static class CustomExtentionMethods
    {
        public static string GetFullName(this IPrincipal usr)
        {
            var fullNameClaim = ((ClaimsIdentity)usr.Identity).FindFirst("FullName");
            return (fullNameClaim != null) ? fullNameClaim.Value : "";
        }
    }
}