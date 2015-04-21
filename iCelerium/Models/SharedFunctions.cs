using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cryptographer;
namespace WebServer.Models
{
    
    public class SharedFunctions
    {
        readonly CryptorEngine crypto;


        public SharedFunctions()
        {
            crypto = new CryptorEngine();
        }

        public String Decr(String cypher)
        {
            try
            {
                return crypto.Decrypt(cypher,true);
            }
            catch
            {
                return null;
            }
                
        }
        public String Encr(String cypher)
        {
            return crypto.Encrypt(cypher, true);
        }
    }
}