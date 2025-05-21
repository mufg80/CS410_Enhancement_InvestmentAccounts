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
    /// <summary>
    /// MVVM view model class for the users view. This view allows the admin to create more users. The
    /// Model for the users view is held by this viewmodel, this class binds to the buttons for executing actions.
    /// </summary>
    public class UsersViewModel
    {
        /// <summary>
        /// Usermodel is the model for the users view. This model holds all properties needed by users view for adding and holding list of users. Also
        /// performs serialization and deserialization of the user list.
        /// </summary>
        public UsersModel Model { get; set; }

        /// <summary>
        /// Commands are bound to the buttons in the view. These commands are executed when the buttons are clicked.
        /// </summary>
        public ICommand ReturnCommand { get; set; }
        public ICommand SubmitCommand { get; set; }
        public ICommand DeleteCommand { get; set; }


        /// <summary>
        /// Event that is fired when the return button is clicked. This event is used to notify the view to return to the previous page.
        /// </summary>
        public event EventHandler<bool> ReturnPage;

        /// <summary>
        /// Constructor for the UsersViewModel class. This constructor initializes the model and the button commands.
        /// </summary>
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
                if (Model.SelectedItem != null && !Model.SelectedItem.IsAdmin)
                {
                    Model.Models.Remove(Model.SelectedItem);
                }
            });
        }

    }
}
