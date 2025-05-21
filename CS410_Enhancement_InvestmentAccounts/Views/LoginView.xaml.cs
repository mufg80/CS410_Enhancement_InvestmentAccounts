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
    /// Codebehind initializes the viewmodel and sets the datacontext to the viewmodel. This allows
    /// the datagrid and buttons to bind to the viewmodel properties and commands.
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginViewModel ViewModel { get; set; }
        public MainWindow Main { get; }
        public LoginView()
        {
            InitializeComponent();
            ViewModel = new LoginViewModel();
            DataContext = ViewModel;
            ViewModel.ILoggedIn += ViewModel_ILoggedIn;
        }
        /// <summary>
        /// Subscribes to logged in event in view model so that it can clear the text boxes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewModel_ILoggedIn(object? sender, bool e)
        {
            userText.Text = "";
            passText.Password = "";
        }
        /// <summary>
        /// Subscribes to the text changed event in the user text box so that it can update the view model. 
        /// Password boxes are not allowed to bind directly to view model, so using a click event to update the view model.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PassText_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ViewModel.Model.PassText = passText.Password;
        }
    }

}
