using EMSModels;

namespace EMSWebApi.AccountRepo
{
    public interface IAccountRepository
    {
        Task<LoginModel> Signup(Register model);
        Task<LoginModel> Login(Login login);
    }
}
