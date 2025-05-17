using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CS410_Enhancement_InvestmentAccounts.Commands
{
    internal class ButtonCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;
        private Action act;
        public ButtonCommand(Action a)
        {
            act = a;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            act.Invoke();
        }
    }

}
