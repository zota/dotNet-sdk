namespace Zotapay.Static
{
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Hasher is a helper static clas for generating signatures
    /// </summary>
    public static class Hasher
    {
        /// <summary>
        /// Returns hexadecimal SHA256 hash
        /// </summary>
        /// <param name="toSign">String already arranged in order ready to be hashed</param>
        /// <returns></returns>
        public static string ToSHA256(string toSign)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // Get the hash in a byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(toSign));

                // Convert to hex lowercase string 
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString().ToLower();
            }
        }
    }
}
