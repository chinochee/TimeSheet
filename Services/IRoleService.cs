namespace Services
{
    public interface IRoleService
    {
        Task<string> GetRoleNameByUserName(string userName);
        Task<string> GetRoleNameById(int id);
    }
}