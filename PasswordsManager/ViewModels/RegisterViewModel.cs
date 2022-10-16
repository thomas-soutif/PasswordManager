using PasswordsManager.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace PasswordsManager.ViewModels
{
    class RegisterViewModel : BaseViewModel
    {
        private MainViewModel mainViewModel;
        public RegisterViewModel(MainViewModel mainViewModel)
        {
            this.mainViewModel = mainViewModel;
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

        public Commands.BaseCommand RegisterCommand
        {

            get { return new Commands.BaseCommand(Register); }
        }

        public Commands.BaseCommand BackToConnexionPageCommand
        {
            get { return new Commands.BaseCommand(BackToConnexionPage); }
            set { SetProperty(value); }
        }

        public void Register()
        {
            Model.UserAccount object_account = new Model.UserAccount();
            object_account.Name = Name_Inscription;
            object_account.FirstName = FirstName_Inscription;
            object_account.Username = Pseudo_Inscription;
            object_account.Password = Password_Inscription;

            if (object_account.Name == null || object_account.FirstName == null || object_account.Username == null || object_account.Password == null)
            {
                this.Result_Inscription = "Tout les champs sont obligatoires";
                return;

            }


            if (object_account.Name.Length == 0 || object_account.FirstName.Length == 0 || object_account.Username.Length == 0 || object_account.Password.Length == 0)
            {
                this.Result_Inscription = "Tout les champs sont obligatoires";
                return;

            }
            // Check if the user is already in the database
            Model.UserAccount user = DataAccess.PasswordsDbContext.Current.UserAccounts.Where(u => u.Username == object_account.Username).FirstOrDefault();
            if (user != null)
            {
                {
                    this.Result_Inscription = "Ce compte  existe déjà";
                    return;
                }
            }

            

            try
            {

                DataAccess.PasswordsDbContext.Current.Add(object_account);
                this.Result_Inscription = "Compte créé.";
                DataAccess.PasswordsDbContext.Current.SaveChanges();
                this.ClearAllField();
                mainViewModel.CurrentPage = Services.NavigationService.GetPage<ConnexionPage, ConnexionViewModel>(this.mainViewModel);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                this.Result_Inscription = "Erreur lors de la création du compte.";

            }
        }

        private void BackToConnexionPage()
        {
            mainViewModel.CurrentPage = Services.NavigationService.GetPage<ConnexionPage, ConnexionViewModel>(this.mainViewModel);
        }

        private void ClearAllField()
        {
            this.Pseudo_Inscription = "";
            this.Password_Inscription = "";
            this.Name_Inscription = "";
            this.FirstName_Inscription = "";
        }
    }
}
