using Global.Abstractions.Areas.Account;
using Global.Settings;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Global.Helpers;

public static class EncryptionHelper
{
    private const int _bitLength = 256;
    private const int _saltKeySize = 64;
    private const int _iterations = 350000;

    private static string? _symmetricKey;
    private static byte[]? _symmetricKeyBytes;

    private static readonly HashAlgorithmName _hashAlgorithm = HashAlgorithmName.SHA512;

    public static string OneWayEncrypt(string? value, IIdentityUser identityUser)
    {
        return OneWayEncrypt(value, identityUser.GlobalUserId);
    }

    public static string OneWayEncrypt(string? value, string saltKey)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value ?? string.Empty;
        }
        else
        {
            byte[] salt = GetSaltKeyBytes(saltKey);

            var hash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(value),
                salt,
                _iterations,
                _hashAlgorithm,
                _saltKeySize);

            return Convert.ToBase64String(hash);
        }
    }

    public static string TwoWayEncrypt(string? value, bool alphanumericOnly = false, string? saltKey = null)
    {
        if (string.IsNullOrEmpty(value))
        {
            return value ?? string.Empty;
        }
        else
        {
            byte[] array;

            using (Aes aes = Aes.Create())
            {
                aes.Key = GetSymmetricKeyBytes(saltKey);
                aes.IV = new byte[16];
                //aes.Padding = PaddingMode.Zeros;

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(value);
                        }

                        array = memoryStream.ToArray();
                    }
                }
            }

            if (alphanumericOnly)
            {
                return Convert.ToHexString(array);
            }
            else
            {
                return Convert.ToBase64String(array);
            }
        }
    }

    public static string Decrypt(string? _hashedString, bool alphanumericOnly = false, string? saltKey = null)
    {
        if (string.IsNullOrEmpty(_hashedString))
        {
            return _hashedString ?? string.Empty;
        }
        else
        {
            try
            {
                byte[] buffer = alphanumericOnly ? Convert.FromHexString(_hashedString)
                    : Convert.FromBase64String(_hashedString);

                using (Aes aes = Aes.Create())
                {
                    aes.Key = GetSymmetricKeyBytes(saltKey);
                    aes.IV = new byte[16];
                    //aes.Padding = PaddingMode.Zeros;

                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader(cryptoStream))
                            {
                                string returnString = streamReader.ReadToEnd();
                                return returnString;
                            }
                        }
                    }
                }
            }
            catch
            {
                return _hashedString ?? string.Empty;
            }
        }
    }

    private static string GetSymmetricEncryptionKey()
    {
        if (string.IsNullOrWhiteSpace(_symmetricKey))
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .AddEnvironmentVariables()
                .Build();

            string? key = configuration["EncryptionSettings:SecretSantaSymmetricKeyEnd"];
            if (key == null)
            {
                throw new ArgumentException("No symmetric key has been created by the system administrator.");
            }

            _symmetricKey = IdentitySettings.SymmetricKeyStart + key; // the prefix means anyone who finds the secrets file still can't use it directly
        }

        return _symmetricKey;
    }

    private static byte[] GetSymmetricKeyBytes(string? saltKey = null)
    {
        if (!string.IsNullOrWhiteSpace(saltKey))
        {
            string key = GetSymmetricEncryptionKey();
            string saltedKey = OneWayEncrypt(key, saltKey);
            return StringToByteArray(saltedKey, _bitLength / 8); // 8 bits per byte, so divide by 8 for byte length
        }

        if (_symmetricKeyBytes == null)
        {
            string key = GetSymmetricEncryptionKey();
            _symmetricKeyBytes = StringToByteArray(key, _bitLength / 8); // 8 bits per byte, so divide by 8 for byte length
        }

        return _symmetricKeyBytes;
    }

    private static byte[] GetSaltKeyBytes(string saltKey)
    {
        return StringToByteArray(saltKey, _saltKeySize);
    }

    private static byte[] StringToByteArray(string text, int byteLength)
    {
        byte[] bytes = Encoding.UTF8.GetBytes(text);

        var result = new byte[byteLength];

        int start = Math.Max(result.Length - bytes.Length, 0);
        int count = Math.Min(bytes.Length, byteLength);

        Buffer.BlockCopy(bytes, 0, result, start, count);

        return result;
    }
}
