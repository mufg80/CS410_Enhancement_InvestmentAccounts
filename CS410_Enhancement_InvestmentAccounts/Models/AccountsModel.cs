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

        private const string PATH = ".\\Models\\Accounts.txt";
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
            WriteToDisk();
        }

        public AccountsModel()
        {
            Models = new ObservableCollection<AccountModel>();
            ReadFromDisk();
            Models.CollectionChanged += Models_CollectionChanged;

        }

        private void Models_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            WriteToDisk();
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

        private string CreateStringFromModels()
        {
            string result = string.Empty;
            foreach (var item in Models)
            {
                result += $"{item.Name}|{item.Option}\n";
            }
            return result;
        }

        private void WriteToDisk()
        {
            using (Aes aes = Aes.Create())
            {
                aes.KeySize = 256;
                aes.GenerateIV();
                aes.GenerateKey();

                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter sw = new StreamWriter(cs))
                            {
                                sw.Write(CreateStringFromModels());
                            }
                            byte[] encrypted = ms.ToArray();
                            File.WriteAllBytes(PATH, encrypted);
                            File.WriteAllBytes(PATH + ".iv", aes.IV);
                            File.WriteAllBytes(PATH + ".key", aes.Key);
                        }
                    }
                }
            }


        }

        private void ReadFromDisk()
        {
            Models.CollectionChanged -= Models_CollectionChanged;
            if (!Directory.Exists(".\\Models"))
            {
                Directory.CreateDirectory(".\\Models");
            }
            if (!File.Exists(PATH))
            {
                File.Create(PATH).Close();
                return;
            }
            try
            {
                byte[] items = File.ReadAllBytes(PATH);
                byte[] key = File.ReadAllBytes(PATH + ".key");
                byte[] iv = File.ReadAllBytes(PATH + ".iv");

                if (items.Length == 0)
                {
                    return;
                }
                string itemstring;
                using (ICryptoTransform decryptor = Aes.Create().CreateDecryptor(key, iv))
                {
                    using (MemoryStream ms = new MemoryStream(items))
                    {
                        using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader sr = new StreamReader(cs))
                            {
                                itemstring = sr.ReadToEnd();
                            }
                        }
                    }
                }



                string[] itemsArray = itemstring.Split('\n');
                Models.Clear();
                foreach (string item in itemsArray)
                {
                    string[] itemArray = item.Split('|');
                    if (itemArray.Length == 2)
                    {
                        AccountModel model = new AccountModel(itemArray[0], (Option)Enum.Parse(typeof(Option), itemArray[1]));
                        Models.Add(model);
                    }
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                Models.CollectionChanged += Models_CollectionChanged;
            }

        }

    }

}
