using EmpSecurityApi.Models;

namespace EmpSecurityApi.Services;

public interface IStaffDataService
{
    IReadOnlyList<StaffItem> FetchAll();
    IReadOnlyList<StaffItem> FetchByGender(string gender);
    StaffItem? FetchById(int id);
}
