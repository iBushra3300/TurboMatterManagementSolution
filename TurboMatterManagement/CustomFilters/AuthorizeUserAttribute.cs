using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace TurboMatterManagement.CustomFilters
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        public string View { get; set; }
        public AuthorizeUserAttribute()
        {
            this.View = "AuthorizeFailed";
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
            IsUserAuthorized(filterContext);
        }

        private void IsUserAuthorized(AuthorizationContext filterContext)
        {
            // user is authorized
            if (filterContext.Result == null) return;

            // if user not autenticated, then show 'AuthorizationFailed' view.
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                var vr = new ViewResult();
                vr.ViewName = this.View;
                var vdc = new ViewDataDictionary();
                vdc.Add("Message", "You are not authorized to peform this action.");
                vr.ViewData = vdc;
                filterContext.Result = vr;
            }
        }
    }
}