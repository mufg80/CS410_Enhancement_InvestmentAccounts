﻿using CS410_Enhancement_InvestmentAccounts.Util;
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
    /// <summary> MVVM model class for the users view. This view allows the admin to create more users. The
    /// class holds all properties needed by users view for adding and holding list of users.</summary>
    public class UsersModel : INotifyPropertyChanged
    {
        /// <summary> Observable collection of user models. This is the list of users that will be displayed in the view.</summary>
        public ObservableCollection<UserModel> Models { get; set; } = [];

        /// <summary>
        /// Implementation of the InotifyPropertyChanged interface. This is used to notify the view when a property changes.
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// All properties tied to the view through the view model. These properties are used to bind the view elements such as the 
        /// textboxes to their data. Most will fire event when they change, notifying the view to update.
        /// </summary>
        private string name;
        private string pass;
        private string pass2;
        private bool isValid;
        private UserModel selectedItem;
        private bool isSelected;
        private string validationMessage;

        public string ValidationMessage
        {
            get { return validationMessage; }
            set { validationMessage = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ValidationMessage))); }
        }


        public bool IsSelected
        {
            get
            {
                return isSelected;
            }
            set
            {
                isSelected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsSelected)));
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
                if(selectedItem != null && !selectedItem.IsAdmin)
                {
                    IsSelected = true;
                }
                else
                {
                    IsSelected = false;
                }
            }
        }
        public string Name
        {
            get { return name; }
            set { name = value; Validate(); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name))); }
        }
        public string Pass
        {
            get { return pass; }
            set { pass = value; Validate(); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Pass))); }
        }
        public string Pass2
        {
            get { return pass2; }
            set { pass2 = value; Validate(); PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Pass2))); }
        }
        public bool IsValid
        {
            get { return isValid; }
            set { isValid = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsValid))); }
        }

        /// <summary> Constructor to initialize the model. Calls update view to get data from disk. Also subscribes to 
        /// observablecollection changed event. This is used to save the data to disk when the collection changes.</summary>
        public UsersModel()
        {
            UpdateView();
            Models.CollectionChanged += Models_CollectionChanged;
        }

        /// <summary>
        /// Method to save the data to disk when the collection changes. This is used to save the data to disk when the user adds or removes a user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Models_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            FileSaver.WriteToDisk([.. Models], []);
        }

        /// <summary> Validation method to check textboxes for approprate input. The name can be letters and spaces only.
        /// the password can be letters and numbers only. The password must match the confirm password field and the name must be unique.</summary>
        private void Validate()
        {
            // Perform check on user inputs.Disable or enable submit button based on the results.
            var item = FileSaver.ValidateStrings(Name, Pass, Pass2);
            if (item.IsValid)
            {
                ValidationMessage = item.Message;
                IsValid = true;
            }
            else
            {
                ValidationMessage = item.Message;
                IsValid = false;
            }
        }


        /// <summary>
        /// Helper method to update the view. This method is called when the view is initialized and when the collection changes.
        /// </summary>
        internal void UpdateView()
        {
            var items = FileSaver.ReadFromDisk();
            Models.Clear();
            foreach (var item in items.Item2)
            {
                Models.Add(item);
            }
        }
    }
}
