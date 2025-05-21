using CS410_Enhancement_InvestmentAccounts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CS410_Enhancement_InvestmentAccounts.ViewModels
{
    /// <summary>
    /// LoginViewModel is the MVVM view model for the login page. This is used to hold data and run business logic for the login page.
    /// </summary>
    public class LoginViewModel
    {
        public string UserLoggedIn { get; set; } = string.Empty;
        public ICommand SubmitButtonCommand { get; }

        /// <summary> Model for the MVVM pattern. This is used to hold data and run business logic for the login page. </summary>
        public LoginModels Model { get; set; }
        

        /// <summary>
        /// Logged in event callback for the mainwindow to subscribe to. This is used to notify the main window when the user has logged in.
        /// </summary>
        public event EventHandler<bool>? ILoggedIn;

        /// <summary>
        /// Constructor for the LoginViewModel class. This initializes the model and the button commmands.
        /// </summary>
        public LoginViewModel()
        {
            Model = new LoginModels();

            // Submit button checks model to see if it will validate the user. Notifys upon failure.
            SubmitButtonCommand = new Commands.ButtonCommand(() =>
            {
                if (Model.ValidateLogin())
                {
                    UserLoggedIn = Model.NameText;
                    ILoggedIn?.Invoke(this, true);
                }
                else
                {
                    UserLoggedIn = string.Empty;
                    MessageBox.Show("Login Failed");
                    ILoggedIn?.Invoke(this, false);
                }
            });
        }
    }

}
