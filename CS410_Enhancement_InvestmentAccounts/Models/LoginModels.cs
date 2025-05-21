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
    /// <summary>Login Model is the MVVM model for the login page. This is used to hold data and run business logic for the login page.</summary>
    public class LoginModels
    {
        /// <summary> Properties for the textboxes on the login view. This class is held as a property by the view model. </summary>
        private string nametext;
        private string passtext;
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


        /// <summary>
        /// This method is used to validate the login. It checks if the username and password are not empty, and if they are valid.
        /// Important: Upon first login, the user is created and saved to disk as well as being made admin account for the app. No
        /// other admin is created in this application.
        /// </summary>
        /// <returns>Boolean</returns>

        public bool ValidateLogin()
        {
            if (string.IsNullOrEmpty(nametext) || string.IsNullOrEmpty(passtext))
            {
                return false;
            }

            var (_, users) = FileSaver.ReadFromDisk();

            // If first time running, create a new user and save to disk. Make user admin(can add more users).
            if (users.Count == 0 && FileSaver.ValidateStrings(NameText, PassText, PassText))
            {
                UserModel model = new(NameText, FileSaver.HashString(passtext), true);
                List<UserModel> models = [model];
                FileSaver.WriteToDisk(models, []);
                string message = $"First user created. You are now admin. Please add more users in the app. User: {NameText} Password: {PassText}";
                MessageBox.Show(message);
                return true;
            }else if(users.Count == 0)
            {
                MessageBox.Show("Please enter a valid username and password.");
                return false;
            }

            // After first run, simply check against the existing users. Compare username and password hash(SHA 256).
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
