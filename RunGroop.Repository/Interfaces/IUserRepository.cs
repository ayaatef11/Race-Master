using RunGroop.Data.Models.Identity;

namespace RunGroop.Data.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<AppUser> GetUserById(string id);
    }
}
