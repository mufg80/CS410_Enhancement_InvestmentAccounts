using CS410_Enhancement_InvestmentAccounts.Enums;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS410_Enhancement_InvestmentAccounts.Models
{
    /// <summary>This class is used to model an account for the investment app. It contains the name of the account and the type.</summary>
    /// <remarks>Contructor forces parameters upon initialization, no account is valid without both properties.
    /// The class also features an event, firing if the enum is edited. This is for serialization.</remarks>
    
    public class AccountModel
    {
        private Option option;

        /// <summary> Notifys the utility that the change must be serialized to disk.</summary>
        public event EventHandler? PropChanged;

        public string Name { get; set; }
        public Enums.Option Option { get { return option; } set { option = value; PropChanged?.Invoke(this, new EventArgs()); } }

        public AccountModel(string n, Enums.Option o)
        {
            this.Name = n;
            this.Option = o;
        }

        /// <summary>
        /// Boilerplate equals and hashcode methods. In this case they only consider the name, each name must be unique.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>Boolean</returns>
        public override bool Equals(object? obj)
        {
            if (obj is AccountModel model)
            {
                return this.Name == model.Name;
            }
            return false;
        }

        /// <summary>
        /// Boilerplate equals and hashcode methods. In this case they only consider the name, each name must be unique.
        /// </summary>
        /// <returns>integer</returns>
        public override int GetHashCode()
        {
            if(Name == null)
            {
                return 0;
            }
            return Name.GetHashCode();
        }
    }

}
