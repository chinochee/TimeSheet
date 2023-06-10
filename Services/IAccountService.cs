using Services.Dtos;

namespace Services
{
    public interface IAccountService
    {
        public Task ChangePassword(LoginEditDto userEdit);
    }
}