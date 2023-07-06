using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Services.Configuration;

namespace Services.Attributes
{
    public class Access : Attribute, IAuthorizationFilter
    {
        private readonly string _accessType;

        public Access(string accessType)
        {
            _accessType = accessType;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userPermission = context.HttpContext.User.Claims.Where(c => c.Type == PermissionsConstant.ClaimType).Select(p => p.Value).ToList();

            if (userPermission.Contains(_accessType)) return;

            context.Result = new ContentResult
            {
                StatusCode = 403,
                ContentType = "Forbidden",
                Content = "Limited access right"
            };
        }
    }
}