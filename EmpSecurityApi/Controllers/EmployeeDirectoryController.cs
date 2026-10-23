using EmpSecurityApi.Attributes;
using EmpSecurityApi.Models;
using EmpSecurityApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EmpSecurityApi.Controllers;

[ApiController]
[Route("api/employees")]
[BasicAuthFilter]
public class EmployeeController : ControllerBase
{
    private readonly IStaffDataService _staffService;

    public EmployeeController(IStaffDataService staffService)
    {
        _staffService = staffService;
    }

    [HttpGet]
    [RoleAccess("Admin", "Superadmin")]
    public ActionResult<IEnumerable<StaffItem>> GetAll()
    {
        Console.WriteLine("обработка запроса на получение всех сотрудников");
        var list = _staffService.FetchAll();
        return Ok(list);
    }

    [HttpGet("male")]
    [RoleAccess("Admin")]
    public ActionResult<IEnumerable<StaffItem>> GetMaleStaff()
    {
        Console.WriteLine("обработка запроса на получение сотрудников мужского пола (только admin)");
        var list = _staffService.FetchByGender("Male");
        return Ok(list);
    }

    [HttpGet("female")]
    [RoleAccess("Superadmin")]
    public ActionResult<IEnumerable<StaffItem>> GetFemaleStaff()
    {
        Console.WriteLine("обработка запроса на получение сотрудников женского пола (только superadmin)");
        var list = _staffService.FetchByGender("Female");
        return Ok(list);
    }

    [HttpGet("{id:int}")]
    [RoleAccess("Admin", "Superadmin")]
    public ActionResult<StaffItem> GetById(int id)
    {
        Console.WriteLine($"поиск сотрудника с id = {id}");
        var item = _staffService.FetchById(id);
        if (item == null)
        {
            Console.WriteLine($"сотрудник с id = {id} не найден");
            return NotFound(new { message = "сотрудник не найден" });
        }

        return Ok(item);
    }

    [HttpGet("public")]
    [AllowAnonymous]
    public IActionResult GetPublicInfo()
    {
        Console.WriteLine("выдача публичных сведений о компании без авторизации");
        return Ok(new
        {
            company = "Alatau Tech Innovations",
            country = "Казахстан",
            city = "Алматы",
            business = "Разработка облачных веб-сервисов и корпоративного ПО",
            departments = new[] { "IT", "HR", "Бухгалтерия", "Маркетинг", "Безопасность", "Аналитика" },
            supportContact = "support@alatautech.kz"
        });
    }
}
