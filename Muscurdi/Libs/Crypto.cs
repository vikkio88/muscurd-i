namespace Muscurdi.Libs;

using System;
using System.Security.Cryptography;
using Muscurdi.Models;
using System.Text;
using System.IO;
public static class Crypto
{
    const int KEY_LENGTH = 32;
    public static string Encrypt(string plainText, MasterPassword password)
    {
        var key = getKey(password);
        byte[] iv = new byte[16];
        byte[] array;
        using (Aes aes = Aes.Create())
        {
            aes.GenerateKey();
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;
            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }
                    array = memoryStream.ToArray();
                }
            }
        }
        return Convert.ToBase64String(array);
    }
    public static string Decrypt(string encryptedText, MasterPassword password)
    {
        //@TODO if you try to decrypt with a wrong master password this throws
        var key = getKey(password);
        byte[] iv = new byte[16];
        byte[] buffer = Convert.FromBase64String(encryptedText);
        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using (MemoryStream memoryStream = new MemoryStream(buffer))
            {
                using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }
    }

    private static string getKey(MasterPassword password)
    {
        var key = password.ToKey();
        if (key.Length != 32) throw new InvalidOperationException($"key should be 32 chars got {key.Length}");

        return key;
    }
}
