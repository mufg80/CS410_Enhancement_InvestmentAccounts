using CS410_Enhancement_InvestmentAccounts.Enums;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CS410_Enhancement_InvestmentAccounts.Models
{
    public class AccountsModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public ObservableCollection<AccountModel> Models { get; set; }

        
        private string nametext;
        private Option optionenum;
        private bool isvalid;


        public string NameText
        {
            get
            {
                return nametext;
            }
            set
            {
                nametext = value;
                validate();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NameText"));
            }
        }
        public Option OptionEnum
        {
            get
            {
                return optionenum;
            }
            set
            {
                optionenum = value;
                validate();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OptionEnum"));
            }
        }
        public bool IsValid
        {
            get
            {
                return isvalid;

            }
            set
            {
                isvalid = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("isValid"));
            }
        }

        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsSelected")); }
        }

        private AccountModel selectedItem;

        public AccountModel SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (SelectedItem != null)
                {
                    SelectedItem.PropChanged -= SelectedItem_PropChanged;
                }

                selectedItem = value;
                if (selectedItem != null)
                {
                    IsSelected = true;
                }
                else
                {
                    IsSelected = false;
                }

                if (SelectedItem != null)
                {
                    SelectedItem.PropChanged += SelectedItem_PropChanged;
                }
            }
        }

        private void SelectedItem_PropChanged(object? sender, EventArgs e)
        {
            var FileSaver = new Util.FileSaver();
            FileSaver.WriteToDisk(new List<UserModel>(), Models.ToList());
        }

        public AccountsModel()
        {
            Models = new ObservableCollection<AccountModel>();

            Models.CollectionChanged -= Models_CollectionChanged;
            var FileSaver = new Util.FileSaver();
            var items = FileSaver.ReadFromDisk();
            
            Models = new ObservableCollection<AccountModel>(items.Item1);
            Models.CollectionChanged += Models_CollectionChanged;

        }

        private void Models_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            var FileSaver = new Util.FileSaver();
            FileSaver.WriteToDisk(new List<UserModel>(), Models.ToList());
        }

        private void validate()
        {

            if (!string.IsNullOrEmpty(NameText) && NameText.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
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
