using EmpSecurityApi.Models;

namespace EmpSecurityApi.Services;

public interface IStaffAuthService
{
    AccountUser? VerifyCredentials(string login, string secretKey);
}
