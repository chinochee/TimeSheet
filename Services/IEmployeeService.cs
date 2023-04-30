using Data.Entities;

namespace Services
{
    public interface IEmployeeService
    {
        Task<Employee[]> GetEntries();
    }
}