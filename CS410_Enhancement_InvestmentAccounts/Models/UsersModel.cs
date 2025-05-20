using CS410_Enhancement_InvestmentAccounts.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS410_Enhancement_InvestmentAccounts.Models
{
    public class UsersModel : INotifyPropertyChanged
    {
        public UserModel SelectedItem { get; set; }
        public ObservableCollection<UserModel> Models { get; set; } = new ObservableCollection<UserModel>();

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; validate(); }
        }

        private string pass;

        public string Pass
        {
            get { return pass; }
            set { pass = value; validate(); }
        }

        private string pass2;

        public string Pass2
        {
            get { return pass2; }
            set { pass2 = value; validate(); }
        }


        private bool isValid;

        public event PropertyChangedEventHandler? PropertyChanged;

        public bool IsValid
        {
            get { return isValid; }
            set { isValid = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsValid")); }
        }

        public UsersModel()
        {
            FileSaver saver = new FileSaver();
            var items = saver.ReadFromDisk();

            foreach (var item in items.Item2)
            {
                Models.Add(item);
            }
        }


        private void validate()
        {

            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Pass) && !string.IsNullOrEmpty(Pass2) && Pass.Equals(Pass2) && Name.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            {
                IsValid = true;
            }
            else
            {
                IsValid = false;
            }
        }

    }
}
