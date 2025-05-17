using CS410_Enhancement_InvestmentAccounts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CS410_Enhancement_InvestmentAccounts.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginViewModel viewModel { get; set; }
        public MainWindow mainWindow { get; }
        public LoginView()
        {
            InitializeComponent();
            viewModel = new LoginViewModel();
            DataContext = viewModel;
            viewModel.ILoggedIn += ViewModel_ILoggedIn;
        }

        private void ViewModel_ILoggedIn(object? sender, bool e)
        {
            userText.Text = "";
            passText.Password = "";
        }

        private void passText_PasswordChanged(object sender, RoutedEventArgs e)
        {
            viewModel.Model.PassText = passText.Password;
        }
    }

}
