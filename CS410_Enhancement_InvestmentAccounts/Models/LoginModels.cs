using CS410_Enhancement_InvestmentAccounts.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
            FileSaver fileSaver = new FileSaver();
            var (accounts, users) = fileSaver.ReadFromDisk();
            if (users.Count == 0)
            {
                UserModel model = new UserModel(NameText, HashString(passtext));
                List<UserModel> models = new List<UserModel>();
                models.Add(model);
                fileSaver.WriteToDisk(models, new List<AccountModel>());
                return true;
            }
            foreach (var item in users)
            {
                string hash = HashString(PassText);
                if (item.UserName == nametext && item.UserHash == hash)
                {
                    return true;
                }
            }
            return false;
        }


        public string HashString(string input, bool useBase64 = false)
        {
            if (string.IsNullOrEmpty(input))
                throw new ArgumentNullException(nameof(input));

            // Convert string to bytes
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);

            // Create SHA-256 instance
            using (SHA256 sha256 = SHA256.Create())
            {
                // Compute hash
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                // Convert to string (hex or base64)
                if (useBase64)
                {
                    return Convert.ToBase64String(hashBytes);
                }
                else
                {
                    // Hexadecimal format
                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in hashBytes)
                    {
                        sb.Append(b.ToString("x2")); // Lowercase hex
                    }
                    return sb.ToString();
                }
            }
        }

    }

}
