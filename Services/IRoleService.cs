namespace Services
{
    public interface IRoleService
    {
        Task<List<string>> GetRolesNameByUserId(int userName);
    }
}