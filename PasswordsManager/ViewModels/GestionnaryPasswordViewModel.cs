using PasswordsManager.Model;
using PasswordsManager.Utils;
using PasswordsManager.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
namespace PasswordsManager.ViewModel
{

    class GestionnaryPasswordViewModel : BaseViewModel
    {

        private MainViewModel mainViewModel;
        private readonly CollectionView _filterEntries;
        private ListFilter _filterPasswordsEntry;
        // TODO: To be changed, when import passphrase has been implemented
        private const string passPhraseTemp = "licked48ca11shore01Gasp67spoilage09protons83Spelling75Lawyers36Quotas00Daimler";
        // Constructeur, dans lequel on a accès à l'instance du view modèle d'où l'on vient
        public GestionnaryPasswordViewModel(MainViewModel viewModel)
        {
            this.mainViewModel = viewModel;

            // On set les valeurs que l'on veut dans notre liste pour filter
            IList<ListFilter> list = new List<ListFilter>();
            list.Add(new ListFilter("Nom",new SortBy("Name")));
            list.Add(new ListFilter("Description",new SortBy("Description")));
            list.Add(new ListFilter("Login",new SortBy("Login")));
            _filterEntries = new CollectionView(list);
            
        }

        public CollectionView ListFilterEntries
        {
            get { return _filterEntries; }
        }

        public ListFilter ListFilterEntry
        {
            get { return _filterPasswordsEntry; }
            set
            {
                if (value == null) return;

                IEnumerable<ListFilter> iEnumListFilter = ListFilterEntries.Cast<ListFilter>().Where(x => x.Name == value.Name);
                if (iEnumListFilter.Count() > 0)
                    _filterPasswordsEntry = value;

                OnPropertyChanged("ListFilterEntry");
                OnListFilterChanged();
            }
        }

        private void OnListFilterChanged()
        {
            
            // Lorsque le filtre change, on remet à jour la liste des mots de passes
            OnPropertyChanged("PasswordsList");

        }

        //Héritage du modèle Password pour pouvoir intégrer un nouvel attribut tout en conservant le modèle
        public class CustomPassword : Model.Password
        {
            public String PassHide { get; set; }

            public CustomPassword(Model.Password password)
            {
                this.Name = password.Name;
                this.Id = password.Id;
                this.Login = password.Login;
                this.Pass = password.Pass;
                this.Description = password.Description;
                for (int i = 0; i < new Utils.Crypto(passPhraseTemp).Decrypt(password.Pass).Count(); i++)
                {
                    this.PassHide += "*";
                }
               
                
            }
        }

        public int Id
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }
        public string Name
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public string Description
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }


        public string Login
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }


        public string Pass
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public string PassShow
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }


        public Model.UserAccount UserConnected
        {
            get { return mainViewModel.GetUserConnected(); }

            set => mainViewModel.SetUserConnected(UserConnected);
        }

        
        public string InsertError
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public Commands.BaseCommand InsertPasswordCommand
        {
            get { return new Commands.BaseCommand(InsertPassword); }
        }
        public Commands.BaseCommand DeletePasswordCommand
        {
            get { return new Commands.BaseCommand(DeletePassword); }
        }
        public Commands.BaseCommand ShowPasswordCommand
        {
            get { return new Commands.BaseCommand(ShowPassword); }
        }
        public Commands.BaseCommand UpdatePasswordCommand
        {
            get { return new Commands.BaseCommand(UpdatePassword); }
        }

        public Commands.BaseCommand ModifyAllFieldCommand
        {
            get { return new Commands.BaseCommand(ModifyAllField); }
        }

        public Commands.BaseCommand CopyPasswordCommand
        {
            get { return new Commands.BaseCommand(CopyPassword); }
        }
        public Commands.BaseCommand CopyLoginCommand
        {
            get { return new Commands.BaseCommand(CopyLogin); }
        }

        public Commands.BaseCommand WritePasswordsIntoAFileCommand
        {
            get { return new Commands.BaseCommand(WritePasswordsIntoAFile); }
        }
        public List<CustomPassword> PasswordsList
        {
            get
            {
                List<CustomPassword> list_password = new List<CustomPassword> ();
                this.UserConnected.GetPasswords().ForEach(p => list_password.Add(new CustomPassword(p)));
                if (ListFilterEntry != null)
                    list_password = ListFilterEntry.Sort(list_password);
                return list_password;
            }
            set {; }
            
        }

        public CustomPassword PasswordSelected
        {
            get
            {
                
                return GetProperty<CustomPassword>();
            }
            set 
            { 
                SetProperty(value); 
            }
        }
        private void InsertPassword()
        {
            this.InsertError = null;
            if(this.Name == null || this.Pass == null){
                this.InsertError = "Vous devez au moins entrer un nom et un mot de passe.";
                return;
            }

            // We encrypt the password. Will change quickly, the passphrase will be generate for each user and import by them.
            // TODO: To be changed when passphrase importing done.

            string encryptPassword = new Utils.Crypto(passPhraseTemp).Encrypt(this.Pass);

            // On commence par ajouter le mot de passe dans la table Password
            Model.Password password_object = new Model.Password();
            password_object.Name = this.Name;
            password_object.Description = this.Description;
            password_object.Login = this.Login;
            password_object.Pass = encryptPassword;
            DataAccess.PasswordsDbContext.Current.Add<Model.Password>(password_object);
            DataAccess.PasswordsDbContext.Current.SaveChanges();
            // On ajoute ensuite le lien entre le mot de passe et l'utilisateur dans la table PasswordUserAccount

            DataAccess.PasswordsDbContext.Current.Add<Model.PasswordUserAccount>(new Model.PasswordUserAccount { UserId = this.UserConnected.Id, PasswordId = password_object.Id });
            DataAccess.PasswordsDbContext.Current.SaveChanges();

            // Il faut ensuite remettre à jour la liste des mots de passes

            OnPropertyChanged(nameof(PasswordsList));

           
            this.ResetInput();




        }

        private void ResetInput()
        {
            // Permet de remettre à 0 la valeur des champs du formulaire
            
            this.Name = null;
            this.Description = null;
            this.Login = null;
            this.Pass = null;
        }

        private void DeletePassword()
        {
            if (PasswordSelected == null)
            {
                return;
            }
       
            try
            {
                Model.PasswordUserAccount object_find_passwordUserAccount = DataAccess.PasswordsDbContext.Current.PasswordUserAccounts.Single((p => p.PasswordId == PasswordSelected.Id && p.UserId == this.UserConnected.Id));
                DataAccess.PasswordsDbContext.Current.Remove(object_find_passwordUserAccount);
                Model.Password object_find_password = DataAccess.PasswordsDbContext.Current.Passwords.Single((p => p.Id == PasswordSelected.Id));
                DataAccess.PasswordsDbContext.Current.Remove(object_find_password);

                // Si tout s'est bien passé , on peut save les changements dans la base de données
                DataAccess.PasswordsDbContext.Current.SaveChanges();
                OnPropertyChanged(nameof(PasswordsList));
            }
            catch
            {

            }
            
            
        }

        private void ModifyAllField()
        {
            if (PasswordSelected == null)
            {
                return;
            }

            this.Name = PasswordSelected.Name;
            this.Description = PasswordSelected.Description;
            this.Login = this.PasswordSelected.Login;
            this.Id = this.PasswordSelected.Id;
            this.Pass = new Utils.Crypto(passPhraseTemp).Decrypt(this.PasswordSelected.Pass);


        }

        private void UpdatePassword()
        {
            try
            {
                Model.Password object_find_password = DataAccess.PasswordsDbContext.Current.Passwords.Find(this.Id);
                if (object_find_password == null)
                {
                    return;
                }
                object_find_password.Name = this.Name;
                object_find_password.Description = this.Description;
                object_find_password.Login = this.Login;
                object_find_password.Pass = new Utils.Crypto(passPhraseTemp).Encrypt(this.Pass);

                DataAccess.PasswordsDbContext.Current.SaveChanges();
                this.ResetInput();
                OnPropertyChanged(nameof(PasswordsList));

            }
            catch
            {

            }
            

            

        }
        private void ShowPassword()
        {
            if(this.PasswordSelected == null)
            {
                return;
            }
            Model.Password object_find_password = DataAccess.PasswordsDbContext.Current.Passwords.Single((p => p.Id == PasswordSelected.Id));
            this.PassShow = new Utils.Crypto(passPhraseTemp).Decrypt(object_find_password.Pass);
        }

        private void CopyPassword()
        {
            if(this.PassShow != null)
            {
                Clipboard.SetText(this.PassShow);
            }
            
        }
        private void CopyLogin()
        {
            if(this.PasswordSelected != null)
            {
                if(this.PasswordSelected.Login != null)
                {
                    Clipboard.SetText(this.PasswordSelected.Login);
                }
                
            }
            
        }

        private void WritePasswordsIntoAFile()
        {
            _ = WriteIntoTheFile(this.PasswordsList);

        }
        public static async Task WriteIntoTheFile(List<CustomPassword> customs)
        {
            Console.WriteLine("écriture...");
            try
            {
                StreamWriter file = new StreamWriter("SavePasswords.txt");
                foreach (CustomPassword custom in customs)
                {
                    await file.WriteLineAsync("["+ custom.Name + "]");
                    await file.WriteLineAsync(custom.Login + " => " + custom.Pass);
                    await file.WriteLineAsync("");
                }

                file.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
            
            
        }
    }
}
