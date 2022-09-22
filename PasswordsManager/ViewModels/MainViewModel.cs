using PasswordsManager.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PasswordsManager.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public Page CurrentPage
        {
            get => GetProperty<Page>();
            set => SetProperty(value);
        }

        private Model.UserAccount UserConnected;

        public MainViewModel()
        {
            this.CurrentPage = Services.NavigationService.GetPage<ConnexionPage, ConnexionViewModel>(this);
            this.SetUserConnected(null);
        }

        public void SetUserConnected(Model.UserAccount user)
        {
            this.UserConnected = user;
        }

        public Model.UserAccount GetUserConnected()
        {
            return this.UserConnected;
        }
        
    }
}
