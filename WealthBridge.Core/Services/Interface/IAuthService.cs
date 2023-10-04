using System.Threading.Tasks;
using WealthBridge.Core.Database;
using WealthBridge.Core.Models;

namespace WealthBridge.Core.Services.Interface
{
    public interface IAuthService
    {
        Borrower FindByEmail(string email);
        void Create(Borrower model);
        void Update(Borrower model);
        AuthenticateResponse CreateUserAuthTokenAsync(Borrower user);
    }
}
