namespace Eshop.Application.Utilities
{
    public interface IAuthHelper
    {
        string CurrentAccountRole();
        AuthViewModel CurrentAccountInfo();
        long CurrentAccountId();
        //Task<EditUserDTO> GetUserInfo(long id);
    }
}
