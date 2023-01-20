using PasswordsManager.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PasswordsManager.ViewModel;

namespace PasswordsManager.ViewModels
{
    class ConnexionViewModel : BaseViewModel
    {

        private MainViewModel mainViewModel;

        public ConnexionViewModel(MainViewModel viewModel)
        {
            this.mainViewModel = viewModel;
        }

        public string Pseudo
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public string Password
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
        
        public String Result_Connexion
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
        public Commands.BaseCommand CheckConnectionCommand {

            get { return new Commands.BaseCommand(CheckConnexion);  }
        }

        public Commands.BaseCommand CreateAccountFormCommand
        {
            get { return new Commands.BaseCommand(CreateAccountForm); }
        }

        private void CheckConnexion()
        {
            //Services.NavigationService.Close(this);

            // We charge the passwords datas in the User object that will be store in the next page
            // The passwords are compared with their hash in SHA256
            List<Model.UserAccount> list = DataAccess.PasswordsDbContext.Current.UserAccounts.Where(p => p.Username == this.Pseudo && p.Password == Utils.HashHelper.SHA256(this.Password)).Include(p => p.PasswordsUserAccountBelong).ThenInclude(p => p.Password).ToList();
            
            if(list.Count > 0)
            {
                Console.WriteLine("Connecté.");
                this.mainViewModel.SetUserConnected(list[0]);
                mainViewModel.CurrentPage = Services.NavigationService.GetPage<GestionnaryPasswordPage,GestionnaryPasswordViewModel>(this.mainViewModel);
                
            }
            else
            {
                Console.WriteLine("Non connecté.");
                this.Result_Connexion = "Mauvais utilisateur / Mot de passe";
                
            }
            
        }

        private void CreateAccountForm()
        {
            mainViewModel.CurrentPage = Services.NavigationService.GetPage<RegisterPage, RegisterViewModel>(this.mainViewModel);
           
        }



    }
}

