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
        private Dictionary<string, string> Logins = new Dictionary<string, string>();
        private const string PATH = ".\\Models\\Logins.txt";

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
            ReadFromDisk();
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



        public bool ValidateLogin()
        {
            if (Logins.Count == 0)
            {
                Logins.Add(NameText, HashString(PassText));
                WriteToDisk();
                return true;
            }
            foreach (var item in Logins)
            {
                if (item.Key == nametext && item.Value == HashString(passtext))
                {
                    return true;
                }
            }
            return false;
        }

        private string CreateStringFromModels()
        {
            if (Logins.Count == 0)
            {
                return string.Empty;
            }
            string result = string.Empty;
            foreach (var item in Logins)
            {
                result = string.Empty;
                result += $"{item.Key}|{item.Value}\n";

            }
            return result;
        }

        private void ReadFromDisk()
        {
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
                Logins.Clear();
                foreach (string item in itemsArray)
                {
                    string[] itemArray = item.Split('|');
                    if (itemArray.Length == 2)
                    {
                        Logins.Add(itemArray[0], itemArray[1]);
                    }
                }
            }
            catch (Exception)
            {

            }

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
