using Models;
using System.Linq;
using System.Web.Mvc;
using Http = System.Web.HttpContext;

namespace WebApp
{
    public class RoleAttribute : AuthorizeAttribute
    {
        private readonly UserRoles[] allowedRoles;
        public RoleAttribute(params UserRoles[] roles) => allowedRoles = roles;

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if(filterContext.ActionDescriptor.IsDefined(typeof(OverrideRoleAttribute), true))
            {
                return;
            }
            else if (Http.Current.Session["Role"] is UserRoles userRole && allowedRoles.Any(x => x == userRole))
            {
                return;
            }
            else
            {
                filterContext.Result = new RedirectResult("/");
            }
        }
    }

    public class OverrideRoleAttribute : AuthorizeAttribute
    {
        private readonly UserRoles[] allowedRoles;
        public OverrideRoleAttribute(params UserRoles[] roles) => allowedRoles = roles;

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (Http.Current.Session["Role"] is UserRoles userRole && allowedRoles.Any(x => x == userRole))
            {
                return;
            }
            else
            {
                filterContext.Result = new RedirectResult("/");
            }
        }
    }
}