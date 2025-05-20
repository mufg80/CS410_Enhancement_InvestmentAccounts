using CS410_Enhancement_InvestmentAccounts.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CS410_Enhancement_InvestmentAccounts.ViewModels
{
    public class UsersViewModel
    {

        public UsersModel Model { get; set; }
        public ICommand ReturnCommand { get; set; }
        public ICommand SubmitCommand { get; set; }
        public event EventHandler<bool> ReturnPage;

        public UsersViewModel()
        {
            Model = new UsersModel();

            
            ReturnCommand = new Commands.ButtonCommand(() =>
            { 
                ReturnPage?.Invoke(this, true);
            });

            SubmitCommand = new Commands.ButtonCommand(() =>
            {
              
            });
        }



    }
}
