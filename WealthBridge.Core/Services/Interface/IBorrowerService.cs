using System.Threading.Tasks;
using WealthBridge.Core.Models;

namespace WealthBridge.Core.Services.Interface
{
    public interface IBorrowerService
    {
        void Create(SignUpRequest model);
    }
}
