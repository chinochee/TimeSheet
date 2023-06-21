namespace Services
{
    public interface IRoleService
    {
        Task<List<string>> GetRolesNameByUserName(string userName);
    }
}