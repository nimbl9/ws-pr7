using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EmpSecurityApi.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RoleAccessAttribute : Attribute, IAuthorizationFilter, IOrderedFilter
{
    private readonly string[] _allowedRoles;

    public int Order => 2;

    public RoleAccessAttribute(params string[] allowedRoles)
    {
        _allowedRoles = allowedRoles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any())
        {
            return;
        }

        if (context.Result != null)
        {
            return;
        }

        var principal = context.HttpContext.User;
        if (principal?.Identity == null || !principal.Identity.IsAuthenticated)
        {
            Console.WriteLine("доступ запрещен: запрос пришел от неавторизованного пользователя");
            context.Result = new UnauthorizedResult();
            return;
        }

        if (_allowedRoles.Length > 0)
        {
            bool hasMatchingRole = false;
            foreach (var role in _allowedRoles)
            {
                if (principal.IsInRole(role))
                {
                    hasMatchingRole = true;
                    break;
                }
            }

            if (!hasMatchingRole)
            {
                Console.WriteLine($"доступ заблокирован: у пользователя {principal.Identity.Name} нет прав (роль не подходит)");
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                return;
            }
        }

        Console.WriteLine($"ролевой доступ разрешен для пользователя {principal.Identity.Name}");
    }
}
