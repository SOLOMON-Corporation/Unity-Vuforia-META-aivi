using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

public static class DecryptionRSA
{
    private static string Decryption(byte[] ciphertext, RSA key)
    {
        try
        {
            var decryptedBytes = key.Decrypt(ciphertext, RSAEncryptionPadding.Pkcs1);
            return System.Text.Encoding.UTF8.GetString(decryptedBytes);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
    public static string DecrypteBarCode(string barcodeString)
    {
        try
        {
            RSA privateKey = RSA.Create();
            var privateKeyPath = @"C:\Users\kai_nguyen\Pictures\private\PrivateRSA.xml";
            string privateKeyText = System.IO.File.ReadAllText(privateKeyPath);
            privateKey.FromXmlString(privateKeyText);
            var jsonString = Decryption(Convert.FromBase64String(barcodeString), privateKey);
            return jsonString;

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return null;
        }
    }
}

public class RSAbarDecreptionObject
{
    public string server_ip { get; set; }
    public string password { get; set; }
    public string project_name { get; set; }
    public int project_id { get; set; }
    public string user_id { get; set; }
    public string vaidio_account { get; set; }
    public string vaidio_password { get; set; }
    public string fiix_account { get; set; }
    public string fiix_password { get; set; }
}

public class RSAfiixDecreptionObject
{
    public string appKey { get; set; }
    public string accessKey { get; set; }
    public string secretKey { get; set; }
}