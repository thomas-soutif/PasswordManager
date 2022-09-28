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

        public string Pseudo
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public string Pseudo_Inscription
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public String Result_Inscription
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

        public Commands.BaseCommand RegisterCommand
        {

            get { return new Commands.BaseCommand(Register); }
        }

        

        public string Password
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public string Password_Inscription
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
        public string Name_Inscription
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
        public string FirstName_Inscription
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
        private MainViewModel mainViewModel;

        public ConnexionViewModel(MainViewModel viewModel)
        {
            this.mainViewModel = viewModel;
        }

        

        private void CheckConnexion()
        {
            //Services.NavigationService.Close(this);

            // We charge the passwords datas in the User object that will be store in the next page
            List<Model.UserAccount> list = DataAccess.PasswordsDbContext.Current.UserAccounts.Where(p => p.Username == this.Pseudo && p.Password == this.Password).Include(p => p.PasswordsUserAccountBelong).ThenInclude(p => p.Password).ToList();
            
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

        private void Register()
        {
            Model.UserAccount object_account = new Model.UserAccount();
            object_account.Name = Name_Inscription;
            object_account.FirstName = FirstName_Inscription;
            object_account.Username = Pseudo_Inscription;
            object_account.Password = Password_Inscription;

            if (object_account.Name == null || object_account.FirstName == null || object_account.Username == null || object_account.Password == null)
            {
                this.Result_Inscription = "Erreur lors de l'inscription. (Tout les champs sont obligatoires)";
                return;

            }


            if (object_account.Name.Length == 0 || object_account.FirstName.Length == 0 || object_account.Username.Length == 0 || object_account.Password.Length == 0)
            {
                this.Result_Inscription = "Erreur lors de l'inscription. (Tout les champs sont obligatoires)";
                return;

            }
           
            try 
            {
               
                DataAccess.PasswordsDbContext.Current.Add(object_account);
                this.Result_Inscription = "Compte créé.";
                DataAccess.PasswordsDbContext.Current.SaveChanges();
            }
            catch (Exception e){
                Console.WriteLine(e);
                this.Result_Inscription = "Erreur lors de l'inscription. (Tout les champs sont obligatoires)";
                
            }
            
        }




    }
}

