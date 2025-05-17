using CS410_Enhancement_InvestmentAccounts.Enums;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS410_Enhancement_InvestmentAccounts.Models
{
    public class AccountModel
    {
        private Option option;
        public event EventHandler? PropChanged;

        public string Name { get; set; }
        public Enums.Option Option { get { return option; } set { option = value; PropChanged?.Invoke(this, new EventArgs()); } }

        public AccountModel(string n, Enums.Option o)
        {
            this.Name = n;
            this.Option = o;
        }

        public override bool Equals(object? obj)
        {
            if (obj is AccountModel model)
            {
                return this.Name == model.Name;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }

}
