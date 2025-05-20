using CS410_Enhancement_InvestmentAccounts.Models;
using CS410_Enhancement_InvestmentAccounts.Util;
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
        public ICommand DeleteCommand { get; set; }
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
                Model.Models.Add(new UserModel(Model.Name, FileSaver.HashString(Model.Pass), false));
                Model.Name = "";
                Model.Pass = "";
                Model.Pass2 = "";
            });

            DeleteCommand = new Commands.ButtonCommand(() =>
            {
                if (Model.SelectedItem != null)
                {
                    Model.Models.Remove(Model.SelectedItem);
                }
            });
        }

    }
}
