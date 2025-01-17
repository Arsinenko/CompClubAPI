using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CompClubAPI  
{
    internal static class HashHelper
    {
        public static byte[] GenerateHash(string str)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(str);

            using (SHA256 mySHA256 = SHA256.Create())
            {
                byte[] hashValue = mySHA256.ComputeHash(bytes);

                return hashValue;
            }


        }
    }
}


