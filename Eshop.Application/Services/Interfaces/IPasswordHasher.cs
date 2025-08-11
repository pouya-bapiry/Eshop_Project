namespace MarketPlace.Application.Services.Interfaces
{
    public interface IPasswordHasher
    {
        string EncodePasswordMd5(string pass);
    }
}
