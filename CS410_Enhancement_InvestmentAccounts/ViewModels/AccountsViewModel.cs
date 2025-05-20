using CS410_Enhancement_InvestmentAccounts.Models;
using CS410_Enhancement_InvestmentAccounts.Util;
using CS410_Enhancement_InvestmentAccounts.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CS410_Enhancement_InvestmentAccounts.ViewModels
{
    public class AccountsViewModel: INotifyPropertyChanged
    {
        private bool isAdmin = false;

        public string userLoggedIn { get; set; } = string.Empty;
        public AccountsModel Model { get; set; }
        public ICommand SubmitCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand UsersCommand { get; }
        public bool IsAdmin { get { return isAdmin; } set { isAdmin = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsAdmin")); } }
        public event EventHandler<bool> ChangePage;
        public event PropertyChangedEventHandler? PropertyChanged;

        public AccountsViewModel()
        {
            Model = new AccountsModel();

            

            Model.OptionEnum = Enums.Option.Brokerage;
            Model.IsSelected = false;


            SubmitCommand = new Commands.ButtonCommand(() =>
            {
                AccountModel mod = new AccountModel(Model.NameText, Model.OptionEnum);
                bool exists = Model.Models.Any(x => x.Equals(mod));
                if (exists)
                {
                    MessageBox.Show("Exists in database!");
                    return;
                }
                Model.Models.Add(mod);
                Model.NameText = string.Empty;
            });

            DeleteCommand = new Commands.ButtonCommand(() =>
            {
                Model.Models.Remove(Model.SelectedItem);
            });

            LogoutCommand = new Commands.ButtonCommand(() =>
            {
                Model.NameText = string.Empty;
                userLoggedIn = string.Empty;
                ChangePage?.Invoke(this, true);
            });

            UsersCommand = new Commands.ButtonCommand(() =>
            {
                ChangePage?.Invoke(this, false);
            });
        }

    }

}
