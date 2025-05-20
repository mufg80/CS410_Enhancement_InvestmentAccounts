using CS410_Enhancement_InvestmentAccounts.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS410_Enhancement_InvestmentAccounts.Models
{
    public class UsersModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private string name;

        private string pass;
        private string pass2;
        private bool isValid;
        private UserModel selectedItem;
        private bool isSelected;

        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsSelected"));
            }
        }

        public UserModel SelectedItem
        {
            get
            {
                return selectedItem;
            }
            set
            {
                selectedItem = value;
                if(selectedItem != null)
                {
                    IsSelected = true;
                }
                else
                {
                    IsSelected = false;
                }
            }
        }
        public ObservableCollection<UserModel> Models { get; set; } = new ObservableCollection<UserModel>();

        

        public string Name
        {
            get { return name; }
            set { name = value; validate(); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name")); }
        }

       

        public string Pass
        {
            get { return pass; }
            set { pass = value; validate(); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Pass")); }
        }

        

        public string Pass2
        {
            get { return pass2; }
            set { pass2 = value; validate(); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Pass2")); }
        }


        

       

        public bool IsValid
        {
            get { return isValid; }
            set { isValid = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsValid")); }
        }

        public UsersModel()
        {
            UpdateView();
            Models.CollectionChanged += Models_CollectionChanged;
        }

        private void Models_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            FileSaver saver = new FileSaver();
            saver.WriteToDisk(Models.ToList(), new List<AccountModel>());
        }

        private void validate()
        {

            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Pass) && !string.IsNullOrEmpty(Pass2) && Pass.Equals(Pass2) && isUnique(Name) &&  Name.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            {
                IsValid = true;
            }
            else
            {
                IsValid = false;
            }
        }

        private bool isUnique(string name)
        {
            FileSaver saver = new FileSaver();
            var items = saver.ReadFromDisk();

            bool isInDatabase = items.Item2.Any(x => x.UserName.Equals(name));
            if (isInDatabase)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        internal void UpdateView()
        {
            FileSaver saver = new FileSaver();
            var items = saver.ReadFromDisk();
            Models.Clear();
            foreach (var item in items.Item2)
            {
                Models.Add(item);
            }
        }
    }
}
