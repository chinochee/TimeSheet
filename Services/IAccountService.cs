using Data.Entities;
using Services.Dtos;
using System.Security.Claims;

namespace Services
{
    public interface IAccountService
    {
        public Task ChangePassword(LoginEditDto userEdit);
        public Task SignIn(Employee user, IEnumerable<Claim> customClaims, bool isPersistent = true);
    }
}