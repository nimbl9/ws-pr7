using System.Security.Claims;
using System.Text;
using EmpSecurityApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EmpSecurityApi.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class BasicAuthFilterAttribute : Attribute, IAuthorizationFilter, IOrderedFilter
{
    public int Order => 1;

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any())
        {
            return;
        }

        if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var authHeaderValues))
        {
            Console.WriteLine("отклонено: отсутствует заголовок authorization");
            context.Result = new UnauthorizedResult();
            return;
        }

        string rawHeader = authHeaderValues.ToString();
        if (!rawHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("отклонено: заголовок не использует схему basic");
            context.Result = new UnauthorizedResult();
            return;
        }

        string base64Payload = rawHeader.Substring("Basic ".Length).Trim();
        string decodedCredentials;
        try
        {
            byte[] rawBytes = Convert.FromBase64String(base64Payload);
            decodedCredentials = Encoding.UTF8.GetString(rawBytes);
        }
        catch
        {
            Console.WriteLine("отклонено: строка base64 повреждена или имеет неверный формат");
            context.Result = new UnauthorizedResult();
            return;
        }

        int colonIndex = decodedCredentials.IndexOf(':');
        if (colonIndex <= 0)
        {
            Console.WriteLine("отклонено: учетные данные не содержат разделитель двоеточие");
            context.Result = new UnauthorizedResult();
            return;
        }

        string username = decodedCredentials.Substring(0, colonIndex);
        string password = decodedCredentials.Substring(colonIndex + 1);

        var authService = context.HttpContext.RequestServices.GetRequiredService<IStaffAuthService>();
        var user = authService.VerifyCredentials(username, password);

        if (user == null)
        {
            Console.WriteLine($"отклонено: пользователь {username} не прошел аутентификацию");
            context.Result = new UnauthorizedResult();
            return;
        }

        var identityClaims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email)
        };

        foreach (var role in user.Roles)
        {
            identityClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        var identity = new ClaimsIdentity(identityClaims, "Basic");
        context.HttpContext.User = new ClaimsPrincipal(identity);
        Console.WriteLine($"пользователь {user.UserName} успешно аутентифицирован через basic-фильтр");
    }
}
