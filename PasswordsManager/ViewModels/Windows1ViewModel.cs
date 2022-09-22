using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordsManager.ViewModels
{
    class Windows1ViewModel : BaseViewModel
    {
        public Commands.BaseCommand MyCommand
        {
            get
            {
                return new Commands.BaseCommand(executeCommand);
            }
        }

        private void executeCommand()
        {
            //Services.NavigationService.Close(this);
            List<Model.Password> list = DataAccess.PasswordsDbContext.Current.Passwords.Where(p => p.TagsBelong.Where(t => t.TagLabel=="Toto").Count()>0).ToList();
            DataAccess.PasswordsDbContext.Current.Passwords.Where(p => p.Name.Contains("azerty") || p.Login.Contains("azerty"));
            DataAccess.PasswordsDbContext.Current.Passwords.Where(p => p.Name == "kakao");
            maChaine = "toto";
        }

        public string maChaine
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
    }
}
