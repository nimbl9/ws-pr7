namespace EmpSecurityApi.Models;

public class StaffItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Gender { get; set; } = string.Empty;
    public decimal Salary { get; set; }
}
