namespace BusinessLogicLayer.Interfaces
{
    public interface IEncryptionHelper
    {
        string Encrypt(string plainText);
        string Decrypt(string cipherText);
    }
}
