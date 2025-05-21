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
    /// <summary>
    /// Viewmodel to tie accounts view to its model. This handles button clicks and other binding events.
    /// Implements INotifyPropertyChanged to notify the view to update when a property changes.
    /// </summary>
    public class AccountsViewModel: INotifyPropertyChanged
    {
        /// <summary> Must know if user is admin for enabling users page button. </summary>
        private bool isAdmin = false;

        /// <summary>
        /// Gets name from mainwindow so that it can check if admin.
        /// </summary>
        public string UserLoggedIn { get; set; } = string.Empty;

        /// <summary>
        /// Model for the accounts view. This is the main model for the application, and the viewmodel uses this as its model class implementing MVVM.
        /// </summary>
        public AccountsModel Model { get; set; }

        /// <summary>
        /// These commands are tied to the view buttons and implement using the ButtonCommand class. Each command 
        /// is tied to an action in the constructor.
        /// </summary>
        public ICommand SubmitCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand UsersCommand { get; }
        public bool IsAdmin { get { return isAdmin; } set { isAdmin = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsAdmin))); } }

        /// <summary> Event is used so that mainwindow can subscribe and change views on a button click.
        public event EventHandler<bool> ChangePage;

        /// <summary>
        /// Implementation of the INotifyPropertyChanged interface. This is used to notify the view to update when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary> Constructor for the AccountsViewModel class. Initializes class and sets up button commands. </summary>
        public AccountsViewModel()
        {
            Model = new AccountsModel
            {
                OptionEnum = Enums.Option.Brokerage,
                IsSelected = false
            };

            SubmitCommand = new Commands.ButtonCommand(() =>
            {
                AccountModel mod = new(Model.NameText, Model.OptionEnum);
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
                UserLoggedIn = string.Empty;
                ChangePage?.Invoke(this, true);
            });

            UsersCommand = new Commands.ButtonCommand(() =>
            {
                ChangePage?.Invoke(this, false);
            });
        }

    }

}
