using System;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace infrastucture.libs.cryptography
{
    public static class CryptographicService // : ICryptographicService
    {
        public static string GenerateRandomString(int length, string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789") {
            if (length < 0) throw new ArgumentOutOfRangeException("length", "length cannot be less than zero.");
            if (string.IsNullOrEmpty(allowedChars)) throw new ArgumentException("allowedChars may not be empty.");

            const int byteSize = 0x100;
            var allowedCharSet = new HashSet<char>(allowedChars).ToArray();
            if (byteSize < allowedCharSet.Length) throw new ArgumentException(String.Format("allowedChars may contain no more than {0} characters.", byteSize));

            // Guid.NewGuid and System.Random are not particularly random. By using a
            // cryptographically-secure random number generator, the caller is always
            // protected, regardless of use.
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create()) {
                var result = new StringBuilder();
                var buf = new byte[128];
                while (result.Length < length) {
                    rng.GetBytes(buf);
                    for (var i = 0; i < buf.Length && result.Length < length; ++i) {
                        // Divide the byte into allowedCharSet-sized groups. If the
                        // random value falls into the last group and the last group is
                        // too small to choose from the entire allowedCharSet, ignore
                        // the value in order to avoid biasing the result.
                        var outOfRangeStart = byteSize - (byteSize % allowedCharSet.Length);
                        if (outOfRangeStart <= buf[i]) continue;
                        result.Append(allowedCharSet[buf[i] % allowedCharSet.Length]);
                    }
                }
                return result.ToString();
            }
        }

        public static string GenerateSaltedHash(string plainText, string salt)
        {
            byte[] _salt = Encoding.UTF8.GetBytes(salt);
            byte[] _plainText = Encoding.UTF8.GetBytes(plainText);
            HashAlgorithm algorithm = SHA256.Create();

            byte[] plainTextWithSaltBytes =
              new byte[_plainText.Length + _salt.Length];

            for (int i = 0; i < _plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = _plainText[i];
            }
            for (int i = 0; i < _salt.Length; i++)
            {
                plainTextWithSaltBytes[_plainText.Length + i] = _salt[i];
            }

            return Convert.ToBase64String(algorithm.ComputeHash(plainTextWithSaltBytes));
        }

    //    public static bool CompareHashes(string plainText, string salt, string userpassword)
    //    {
    //        byte[] _salt = Encoding.UTF8.GetBytes(salt);
    //        byte[] _plainText = Encoding.UTF8.GetBytes(plainText);
    //        HashAlgorithm algorithm = new SHA256Managed();

    //        byte[] plainTextWithSaltBytes =
    //          new byte[_plainText.Length + _salt.Length];

    //        for (int i = 0; i < _plainText.Length; i++)
    //        {
    //            plainTextWithSaltBytes[i] = _plainText[i];
    //        }
    //        for (int i = 0; i < _salt.Length; i++)
    //        {
    //            plainTextWithSaltBytes[_plainText.Length + i] = _salt[i];
    //        }

    //        return CompareByteArrays(algorithm.ComputeHash(plainTextWithSaltBytes), Convert.FromBase64String(userpassword));
    //    }

    //    public static bool CompareByteArrays(byte[] array1, byte[] array2)
    //    {
    //        if (array1.Length != array2.Length)
    //        {
    //            return false;
    //        }

    //        for (int i = 0; i < array1.Length; i++)
    //        {
    //            if (array1[i] != array2[i])
    //            {
    //                return false;
    //            }
    //        }

    //        return true;
    //    }

    }
}
