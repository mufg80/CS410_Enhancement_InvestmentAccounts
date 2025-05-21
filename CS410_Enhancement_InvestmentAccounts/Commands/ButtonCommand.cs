using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CS410_Enhancement_InvestmentAccounts.Commands
{
    /// <summary> Command class to handle button clicks. </summary>
    /// <remarks> This class implements the ICommand interface and provides a way to execute an action when a button is clicked. </remarks>
    /// <author>Shannon Musgrave</author>
    internal class ButtonCommand : ICommand
    {
        /// <summary> Event is unused in this application. </summary>
        public event EventHandler? CanExecuteChanged;
        private readonly Action action;

        /// <summary> Constructor for the ButtonCommand class. </summary>
        /// <param name="a">The action to be executed when the button is clicked.</param>
        /// <remarks> This constructor initializes the ButtonCommand with the specified action. </remarks>
        public ButtonCommand(Action a)
        {
            action = a;
        }

        /// <summary>
        /// Checks if the command can be executed. Unused, button enable is handled by binding in XAML.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>Always returns true</returns>
        public bool CanExecute(object? parameter)
        {
            return true;
        }

        /// <summary>
        /// Executes button click by calling a delegate action. This action is assigned by the viewmodel.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>returns void</returns>
        public void Execute(object? parameter)
        {
            action.Invoke();
        }
    }

}
