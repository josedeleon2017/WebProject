using System.Security.Cryptography;
using System.Text;
namespace StoreMVC.Utilities
{
    public class Cipher
    {
        public static string EncryptPassword(string password)
        {
            StringBuilder sb = new StringBuilder();
            using (SHA256 sha256 = SHA256.Create())
            {
                Encoding encoding = Encoding.UTF8;
                byte[] result = sha256.ComputeHash(encoding.GetBytes(password));
                foreach (byte b in result)
                {
                    sb.Append(b.ToString("x2"));
                }
            }
            return sb.ToString();
        }
    }
}
