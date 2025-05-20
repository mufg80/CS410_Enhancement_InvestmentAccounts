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
    public class LoginViewModel
    {
        public string userLoggedIn { get; set; } = string.Empty;
        public LoginModels Model { get; set; }
        public ICommand SubmitButtonCommand { get; }
        public event EventHandler<bool>? ILoggedIn;
        public LoginViewModel()
        {
            Model = new LoginModels();
            SubmitButtonCommand = new Commands.ButtonCommand(() =>
            {
                if (Model.ValidateLogin())
                {
                    userLoggedIn = Model.NameText;
                    ILoggedIn?.Invoke(this, true);
                }
                else
                {
                    userLoggedIn = string.Empty;
                    MessageBox.Show("Login Failed");
                    ILoggedIn?.Invoke(this, false);
                }
            });
        }
    }

}
