using Services.Dtos;

namespace Services
{
    public interface IRoleService
    {
        Task<List<RoleEntryDto>> GetRoles();
        Task<List<RoleEntryDto>> GetRolesByUserId(int userName);
    }
}