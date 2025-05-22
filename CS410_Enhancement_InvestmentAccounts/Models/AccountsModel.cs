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
    /// <summary>
    /// This class is used to model the accounts for the investment app. It contains a collection of account models and a selected item.
    /// This is the main model for the application, and the viewmodel uses this as its model class implementing MVVM. This class
    /// also controls serialization of the account models to disk. It implements INotifyPropertyChanged to notify the view to update.
    /// </summary>
    public class AccountsModel : INotifyPropertyChanged
    {
        /// <summary>Implmentation of the INotifyPropertyChanged interface. This is used to notify the view to update when a property changes.</summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>Collection of account models. This is the main model for the application.</summary>
        public ObservableCollection<AccountModel> Models { get; set; }

        /// <summary>
        /// All properties needed for account view tied through the viewmodel.
        /// Many fire the property changed interface to alert view of changes.
        /// </summary>

        private string nametext;
        private Option optionenum;
        private bool isvalid;
        private bool isSelected;
        private AccountModel selectedItem;

        private string validationMessage;

        public string ValidationMessage
        {
            get { return validationMessage; }
            set { validationMessage = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ValidationMessage))); }
        }


        public string NameText
        {
            get
            {
                return nametext;
            }
            set
            {
                nametext = value;
                Validate();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NameText)));
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
                Validate();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OptionEnum)));
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
        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected))); }
        }

        /// <summary>
        /// Selected item is the datagrids active selection. This setter unsubscribes 
        /// the old item and subscribes the new item to the property changed event. That
        /// way only one item is subscribed at a time. This is important for serialization.
        /// </summary>
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
        /// <summary>
        /// This method is used to notify the utility that the change must be serialized to disk.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectedItem_PropChanged(object? sender, EventArgs e)
        {
            Util.FileSaver.WriteToDisk([], [.. Models]);
        }

        /// <summary>
        /// This is the constructor for the AccountsModel class. It initializes the collection of account models and reads from disk.
        /// This also signes up for observable collection property changed events for serialization.
        /// </summary>
        public AccountsModel()
        {
            Models = [];

            Models.CollectionChanged -= Models_CollectionChanged;
            var items = Util.FileSaver.ReadFromDisk();
            
            Models = [.. items.Item1];
            Models.CollectionChanged += Models_CollectionChanged;

        }

        /// <summary>
        /// This method is used to notify the utility that the change must be serialized to disk. It is called when the collection changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Models_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Util.FileSaver.WriteToDisk([], [.. Models]);
        }

        /// <summary>
        /// This method is used to validate the name of the account. It checks if the name is not empty and if it only contains letters and spaces.
        /// This is called from the setters of the properties in question. IsValid is used to enable the submit button on the View.
        /// It also checks if the name is between 3 and 20 characters long.
        /// </summary>
        private void Validate()
        {
            if (nametext == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(NameText) )
            {
                IsValid = false;
                ValidationMessage = "";
            }else if (NameText.Length <= 3)
            {
                IsValid = false;
                ValidationMessage = "Name must be at least 4 characters long";
            }else if (NameText.Length >= 20)
            {
                IsValid = false;
                ValidationMessage = "Name must be less than 20 characters long";
            } else if (!NameText.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            {
                IsValid = false;
                ValidationMessage = "Name must be letters and spaces only";
            }
            else
            {
                IsValid = true;
                ValidationMessage = "Validation successful";
            }


        }



    }

}
