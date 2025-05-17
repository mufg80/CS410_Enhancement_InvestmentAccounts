using CS410_Enhancement_InvestmentAccounts.Views;
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

        public MainWindow()
        {
            InitializeComponent();
            myGrid.Children.Add(loginView);
            loginView.viewModel.ILoggedIn += ViewModel_ILoggedIn;
            accountsView.viewModel.Logout += ViewModel_LoggedOut;
        }

        private void ViewModel_LoggedOut(object? sender, bool e)
        {
            myGrid.Children.Clear();
            myGrid.Children.Add(loginView);

        }

        private void ViewModel_ILoggedIn(object? sender, bool e)
        {
            if (e)
            {
                myGrid.Children.Clear();
                myGrid.Children.Add(accountsView);
            }

        }
    }

}