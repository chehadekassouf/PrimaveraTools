using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace P6FullHistory
{
    public class DBCredential
    {
        public DBCredential()
        {

        }
       
        
        public string GenerateDBCredential(string plainConnectionString)
        {
            return this.Write(plainConnectionString);

        }
        public string Read(string encryptedConnectionString)
        {
            string plainConnectionString = "BIBO";
            byte[] Key = { 0x02, 0x00, 0x00, 0x06, 0x02, 0x00, 0x00, 0x06, 0x02, 0x00, 0x00, 0x06, 0x02, 0x00, 0x00, 0x06 };
            byte[] IV = { 0x00, 0x02, 0x00, 0x06, 0x02, 0x00, 0x00, 0x08, 0x00, 0x02, 0x00, 0x06, 0x02, 0x00, 0x00, 0x08 };

            byte[] byteArray = Convert.FromBase64String(encryptedConnectionString);
            System.IO.MemoryStream memorystream = new System.IO.MemoryStream(byteArray);
            System.Security.Cryptography.CryptoStream cryptostream = new System.Security.Cryptography.CryptoStream(memorystream, new System.Security.Cryptography.RijndaelManaged().CreateDecryptor(Key, IV), System.Security.Cryptography.CryptoStreamMode.Read);
            System.IO.StreamReader decriptedstreamreader = new System.IO.StreamReader(cryptostream);
            plainConnectionString = decriptedstreamreader.ReadLine();

            decriptedstreamreader.Close();
            cryptostream.Close();
            memorystream.Close();
            return plainConnectionString;
        }
        private string Write(string ConnectionString)
        {

            byte[] Key = { 0x02, 0x00, 0x00, 0x06, 0x02, 0x00, 0x00, 0x06, 0x02, 0x00, 0x00, 0x06, 0x02, 0x00, 0x00, 0x06 };
            byte[] IV = { 0x00, 0x02, 0x00, 0x06, 0x02, 0x00, 0x00, 0x08, 0x00, 0x02, 0x00, 0x06, 0x02, 0x00, 0x00, 0x08 };

            System.IO.MemoryStream memorystream = new System.IO.MemoryStream();
            System.Security.Cryptography.CryptoStream cryptostream = new System.Security.Cryptography.CryptoStream(memorystream, new System.Security.Cryptography.RijndaelManaged().CreateEncryptor(Key, IV), System.Security.Cryptography.CryptoStreamMode.Write);

            System.IO.StreamWriter decriptedstreamwriter = new System.IO.StreamWriter(cryptostream);

            decriptedstreamwriter.WriteLine(ConnectionString);
            decriptedstreamwriter.WriteLine("Ana la khal bel dounia w la 3am li wla 3indi ksour tighnini w la 3imli min el bawsat bartaltic bi nitfi");

            decriptedstreamwriter.Close();
            cryptostream.Close();
            memorystream.Close();
            string returnstring = Convert.ToBase64String(memorystream.ToArray());
            return returnstring;


        }
       

    }
}
