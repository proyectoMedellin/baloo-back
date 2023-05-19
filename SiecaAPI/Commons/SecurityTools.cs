using System.Text;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace SiecaAPI.Commons
{
    public static class SecurityTools
    {
        public static string GeneratePassword()
        { 
            var rand=new Random();
            StringBuilder passwordbuilder = new("");
            passwordbuilder.Append((char)rand.Next(65, 91));
            passwordbuilder.Append((char)rand.Next(35, 47));
            for (int i=0; i<5; i++)
            {
                passwordbuilder.Append((char)rand.Next(97, 123));
            }
            passwordbuilder.Append(rand.Next(0, 9));

            string password = passwordbuilder.ToString();

            return PasswordMD5(password);
        }

        public static string PasswordMD5(string str)
        {
            StringBuilder hash = new();
            var md5provider = MD5.Create();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(str));
            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            string hashString = hash.ToString();

            return hashString;
        }

        public static string ConvertToMD5(string inputString)
        {
            using var md5 = MD5.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            string hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            return hashString;
        }

        public static byte[] EncryptAes(string plainText)
        {
            byte[] encrypted;

            byte[] Key = Encoding.UTF8.GetBytes(AppParamsTools.GetEnvironmentVariable("Security:CryptPublickey"));
            byte[] IV = Encoding.UTF8.GetBytes(AppParamsTools.GetEnvironmentVariable("Security:CryptPublickeySalt"));

            using (Aes aesAlgorithm = Aes.Create())
            {
                ICryptoTransform encryptor = aesAlgorithm.CreateEncryptor(Key, IV);

                using MemoryStream ms = new();
                using CryptoStream cs = new(ms, encryptor, CryptoStreamMode.Write);               
                using (StreamWriter sw = new(cs)) 
                    sw.Write(plainText);
                    encrypted = ms.ToArray();
            }
            return encrypted;
        }

        public static string DecryptAes(string raw)
        {
            string plainText = string.Empty;
            byte[] cipherText = Encoding.UTF8.GetBytes(raw); 
            byte[] Key = Encoding.UTF8.GetBytes(AppParamsTools.GetEnvironmentVariable("Security:CryptPublickey"));
            byte[] IV = Encoding.UTF8.GetBytes(AppParamsTools.GetEnvironmentVariable("Security:CryptPublickeySalt"));
            
            using (Aes aesAlgorithm = Aes.Create())
            {
                ICryptoTransform decryptor = aesAlgorithm.CreateDecryptor(Key, IV);    
                using MemoryStream ms = new(cipherText);
                using CryptoStream cs = new(ms, decryptor, CryptoStreamMode.Read);
                using StreamReader reader = new(cs);
                plainText = reader.ReadToEnd();
            }
            return plainText;
        }

        public static string JwtTokenGenerator(string userName, List<string> roles)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(AppParamsTools.GetEnvironmentVariable("Jwt:Key")));

            SigningCredentials signingCredentials = 
                new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512Signature);

            List<Claim> claimsList = new List<Claim>();
            claimsList.Add(new(ClaimTypes.Name, userName));
            foreach (string r in roles)
            {
                claimsList.Add(new(ClaimTypes.Role, r));
            }

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                /*issuer: AppParamsTools.GetEnvironmentVariable("Jwt:Issuer"),
                audience: AppParamsTools.GetEnvironmentVariable("Jwt:Audience"),*/
                claims: claimsList,
                expires: DateTime.UtcNow
                    .AddMinutes(
                    Convert.ToUInt32(
                        AppParamsTools.GetEnvironmentVariable("Jwt:TokenExpMinuts"))),
                signingCredentials: signingCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
    }
}
