using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace PasswordsManager.DataAccess
{
    public class PasswordsDbContext : DbContext
    {

        #region Singleton

        private static PasswordsDbContext current = null;
        public static PasswordsDbContext Current
        {
            get
            {
                if (current == null)
                    throw new Exception("Current dbContext must be initialized !");
                return current;
            }
        }
        public static async Task InitAsync()
        {
            if (current != null)
                current = null;

            current = new PasswordsDbContext();
            await current.Database.MigrateAsync();

            // Ajouter import des données
            //devoir faire un save_change


            /*Current.Add<PasswordsManager.Model.UserAccount>(new PasswordsManager.Model.UserAccount{FirstName = "thomas", Name = "soutif", Username = "toto", Password = "1234" });
            Current.Add<PasswordsManager.Model.UserAccount>(new PasswordsManager.Model.UserAccount { FirstName = "root", Name = "root", Username = "admin", Password = "admin" });
            Current.SaveChanges(); */

            /*Current.Add<PasswordsManager.Model.Password>(new PasswordsManager.Model.Password { Id = 1, Login = "test", Name = "test", Pass = "test", Description = "Une description" });
            Current.Add<PasswordsManager.Model.PasswordUserAccount>(new PasswordsManager.Model.PasswordUserAccount { Id = 1, UserId = 2, PasswordId = 1 });
            Current.SaveChanges(); */
        }

        #endregion

        internal PasswordsDbContext() : base()
        { }

        public DbSet<Model.Password> Passwords { get; set; }
        public DbSet<Model.Tag> Tags { get; set; }
        public DbSet<Model.PasswordTag> PasswordTags { get; set; }
        public DbSet<Model.Parameter> Parameters { get; set; }

        public DbSet<Model.UserAccount> UserAccounts { get; set; }
        public DbSet<Model.PasswordUserAccount> PasswordUserAccounts { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("Data Source=passwords.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Clé primaire composée
            modelBuilder.Entity<Model.PasswordTag>().HasKey(pt => new { pt.PasswordId, pt.TagLabel });
        }

    }
}
