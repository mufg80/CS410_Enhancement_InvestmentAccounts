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
    /// </summary>
    public partial class MainWindow : Window
    {
        LoginView loginView = new LoginView();
        AccountsView accountsView = new AccountsView();
        UsersView userView = new UsersView();

        public MainWindow()
        {
            InitializeComponent();
            myGrid.Children.Add(loginView);
            loginView.viewModel.ILoggedIn += ViewModel_ILoggedIn;
            accountsView.viewModel.ChangePage += ViewModel_LoggedOut;
            userView.ViewModel.ReturnPage += ViewModel_ReturnPage;
        }

        private void ViewModel_ReturnPage(object? sender, bool e)
        {
            myGrid.Children.Clear();
            myGrid.Children.Add(accountsView);
        }

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
            }
               
        }

        private void ViewModel_ILoggedIn(object? sender, bool e)
        {
            if (e)
            {
                myGrid.Children.Clear();
                myGrid.Children.Add(accountsView);

                FileSaver fileSaver = new FileSaver();
                var s = fileSaver.ReadFromDisk();
                if(s.Item2.FirstOrDefault().UserName == loginView.viewModel.Model.NameText)
                {
                    accountsView.viewModel.IsAdmin = true;
                }else
                {
                    accountsView.viewModel.IsAdmin = false;
                }


            }

        }
    }

}