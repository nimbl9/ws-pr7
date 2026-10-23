using EmpSecurityApi.Models;

namespace EmpSecurityApi.Services;

public class StaffAuthService : IStaffAuthService
{
    private readonly List<AccountUser> _registeredUsers = new()
    {
        new AccountUser
        {
            Id = 1,
            UserName = "AdminUser",
            Password = "Admin123",
            Email = "admin.user@corp.kz",
            Roles = new List<string> { "Admin" }
        },
        new AccountUser
        {
            Id = 2,
            UserName = "SuperadminUser",
            Password = "Super123",
            Email = "superadmin.user@corp.kz",
            Roles = new List<string> { "Superadmin" }
        },
        new AccountUser
        {
            Id = 3,
            UserName = "BothUser",
            Password = "Both123",
            Email = "both.user@corp.kz",
            Roles = new List<string> { "Admin", "Superadmin" }
        }
    };

    public AccountUser? VerifyCredentials(string login, string secretKey)
    {
        Console.WriteLine($"проверяем учетные данные для аккаунта {login}");

        var match = _registeredUsers.FirstOrDefault(u =>
            string.Equals(u.UserName, login, StringComparison.OrdinalIgnoreCase) &&
            u.Password == secretKey);

        if (match == null)
        {
            Console.WriteLine($"не удалось авторизовать: неверный логин или пароль для {login}");
            return null;
        }

        Console.WriteLine($"пользователь {login} успешно опознан в системе");
        return match;
    }
}
