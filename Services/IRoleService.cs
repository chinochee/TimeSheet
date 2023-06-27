using Services.Dtos;

namespace Services
{
    public interface IRoleService
    {
        Task<List<string>> GetRolesNameByUserName(string userName);
        Task<List<RoleEntryDto>> GetRoles();
    }
}