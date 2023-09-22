using System.Security.Cryptography;
using System.Text;

namespace BillMate.Helpers
{
    public static class Encryption
    {
        public static string CalculateMD5Hash(string input)
        {
            // To calculate MD5 hash from an input string
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);

            byte[] hash = md5.ComputeHash(inputBytes);

            // convert byte array to hex string
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                //to make hex string use lower case instead of uppercase add parameter "X2"
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();

        }
    }
}
