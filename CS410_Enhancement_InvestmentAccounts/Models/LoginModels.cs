using CS410_Enhancement_InvestmentAccounts.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CS410_Enhancement_InvestmentAccounts.Models
{
    public class LoginModels
    {
        private string nametext;
        public string NameText
        {
            get
            {
                return nametext;
            }
            set
            {
                nametext = value;
            }
        }

        private string passtext;
        public string PassText
        {
            get
            {
                return passtext;
            }
            set
            {
                passtext = value;
            }
        }

        public LoginModels()
        {
        }




        public bool ValidateLogin()
        {
            if (string.IsNullOrEmpty(nametext) || string.IsNullOrEmpty(passtext))
            {
                return false;
            }

            FileSaver fileSaver = new FileSaver();
            var (accounts, users) = fileSaver.ReadFromDisk();
            if (users.Count == 0)
            {
                UserModel model = new UserModel(NameText, FileSaver.HashString(passtext), true);
                List<UserModel> models = new List<UserModel>();
                models.Add(model);
                fileSaver.WriteToDisk(models, new List<AccountModel>());
                return true;
            }
            foreach (var item in users)
            {
                string hash = FileSaver.HashString(PassText);
                if (item.UserName == nametext && item.UserHash == hash)
                {
                    return true;
                }
            }
            return false;
        }



    }

}
