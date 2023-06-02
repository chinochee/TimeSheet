using Services.Dtos;

namespace Services
{
    public interface IAccountService
    {
        public Task EditEmployee(LoginEditDto userEdit);
    }
}