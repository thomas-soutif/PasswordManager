using Microsoft.EntityFrameworkCore.Design;

namespace PasswordsManager.DataAccess
{
    public class PasswordsDbContextFactory : IDesignTimeDbContextFactory<PasswordsDbContext>
    {
        public PasswordsDbContext CreateDbContext(string[] args)
        {
            return new PasswordsDbContext();
        }
    }
}
