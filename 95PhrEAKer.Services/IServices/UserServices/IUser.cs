using _95PhrEAKer.Domain.User;
namespace _95PhrEAKer.Services.IServices.UserServices
{
    public interface IUser
    {
        List<User> GetAllUsers(string email);
    }
}
