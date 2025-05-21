using CS410_Enhancement_InvestmentAccounts.Models;
using CS410_Enhancement_InvestmentAccounts.Util;
using CS410_Enhancement_InvestmentAccounts.Views;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CS410_Enhancement_InvestmentAccounts
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// Mainwindow codebehind controls the views and page turning navigation.
    /// It holds the views as properties and uses callback events to turn the pages.
    /// </summary>
    public partial class MainWindow : Window
    {
        // Three views are created and held during application life.
        private readonly LoginView loginView = new();
        private readonly AccountsView accountsView = new();
        private readonly UsersView userView = new();

        /// <summary>
        /// Constructor subscribes to the events for navigation.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            myGrid.Children.Add(loginView);
            loginView.ViewModel.ILoggedIn += ViewModel_ILoggedIn;
            accountsView.ViewModel.ChangePage += ViewModel_LoggedOut;
            userView.ViewModel.ReturnPage += ViewModel_ReturnPage;
        }
        /// <summary>
        /// Return to the accounts view from the users view.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewModel_ReturnPage(object? sender, bool e)
        {
            myGrid.Children.Clear();
            myGrid.Children.Add(accountsView);
        }

        /// <summary>
        /// Either return to login or go to user view, same event for both.
        /// boolean sent decides which to activate.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewModel_LoggedOut(object? sender, bool e)
        {
            if (e)
            {
                myGrid.Children.Clear();
                myGrid.Children.Add(loginView);
            }
            else
            {
                myGrid.Children.Clear();
                myGrid.Children.Add(userView);
                userView.ViewModel.Model.UpdateView();
            }
               
        }

        /// <summary>
        /// Logging in event is sent from the login viewmodel to the main window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewModel_ILoggedIn(object? sender, bool e)
        {
            // If the user is logged in, go to the accounts view.
            // If the user is an admin, set the admin flag in the accounts view model.
            if (e)
            {
                userView.ViewModel.Model.UpdateView();
                myGrid.Children.Clear();
                myGrid.Children.Add(accountsView);

                // Check if user is admin and set appropriate properties.
                FileSaver fileSaver = new();
                var s = FileSaver.ReadFromDisk();
                var adminAccount = s.Item2.Where(x => x.IsAdmin).FirstOrDefault();
                
                if(adminAccount != null && adminAccount.UserName == loginView.ViewModel.Model.NameText)
                {
                    accountsView.ViewModel.IsAdmin = true;
                }else
                {
                    accountsView.ViewModel.IsAdmin = false;
                }
            }
        }
    }
}