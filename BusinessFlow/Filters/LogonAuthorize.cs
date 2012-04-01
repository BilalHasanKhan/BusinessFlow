using System.Web.Mvc;
using BusinessFlow.Controllers;

namespace BusinessFlow.Filters {
    public sealed class LogonAuthorize : AuthorizeAttribute {
        public override void OnAuthorization(AuthorizationContext filterContext) {
            bool skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true)
            || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true);
            if (!skipAuthorization) {
                base.OnAuthorization(filterContext);
            }
        }
    }
}


