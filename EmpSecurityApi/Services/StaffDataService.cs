using EmpSecurityApi.Models;

namespace EmpSecurityApi.Services;

public class StaffDataService : IStaffDataService
{
    private readonly List<StaffItem> _staffList = new()
    {
        new StaffItem { Id = 1, Name = "Алихан Мусин", Department = "IT", Gender = "Male", Salary = 650000m },
        new StaffItem { Id = 2, Name = "Дина Сарсенова", Department = "Бухгалтерия", Gender = "Female", Salary = 480000m },
        new StaffItem { Id = 3, Name = "Ерсултан Омаров", Department = "IT", Gender = "Male", Salary = 720000m },
        new StaffItem { Id = 4, Name = "Аружан Темирханова", Department = "Маркетинг", Gender = "Female", Salary = 430000m },
        new StaffItem { Id = 5, Name = "Бауыржан Касымов", Department = "Безопасность", Gender = "Male", Salary = 510000m },
        new StaffItem { Id = 6, Name = "Айгерим Серикова", Department = "HR", Gender = "Female", Salary = 390000m },
        new StaffItem { Id = 7, Name = "Санжар Даулетов", Department = "Аналитика", Gender = "Male", Salary = 580000m },
        new StaffItem { Id = 8, Name = "Мадина Ибрагимова", Department = "Юридический", Gender = "Female", Salary = 460000m },
        new StaffItem { Id = 9, Name = "Данияр Болатов", Department = "Логистика", Gender = "Male", Salary = 410000m },
        new StaffItem { Id = 10, Name = "Камила Нурланова", Department = "Дизайн", Gender = "Female", Salary = 450000m },
        new StaffItem { Id = 11, Name = "Берик Куанышев", Department = "IT", Gender = "Male", Salary = 600000m },
        new StaffItem { Id = 12, Name = "Зарина Ахметова", Department = "Финансы", Gender = "Female", Salary = 520000m }
    };

    public IReadOnlyList<StaffItem> FetchAll()
    {
        Console.WriteLine("получен запрос на чтение полного списка сотрудников");
        return _staffList;
    }

    public IReadOnlyList<StaffItem> FetchByGender(string gender)
    {
        Console.WriteLine($"фильтруем сотрудников по половому признаку: {gender}");
        return _staffList
            .Where(s => string.Equals(s.Gender, gender, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public StaffItem? FetchById(int id)
    {
        Console.WriteLine($"ищем сотрудника с идентификатором: {id}");
        return _staffList.FirstOrDefault(s => s.Id == id);
    }
}
