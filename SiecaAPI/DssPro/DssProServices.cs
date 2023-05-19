using System.Text;
using System.Text.Json;
using System.Security.Cryptography;
using SiecaAPI.Commons;
using SiecaAPI.Errors;
using System.Text.Json.Nodes;
using Microsoft.Graph;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using Microsoft.Graph.Models;
using System.Net.Http.Headers;
using SiecaAPI.Models.Services;
using SiecaAPI.Data.Factory;
using SiecaAPI.Models;
using System.Drawing.Printing;
using System.Xml.Linq;
using SiecaAPI.DTO.Data;
using System;
using SiecaAPI.DssPro.DTO;
using SiecaAPI.DssPro.Models;
using SiecaAPI.Data.SQLImpl;

namespace SiecaAPI.DssPro
{
    public static class DssProServices
    {

        private static readonly string baseUrl = AppParamsTools.GetEnvironmentVariable("DssPro:baseUrl");

        public static async Task<Dictionary<string, string>> GetValidToken()
        {
            Dictionary<string, string> fLoginResponse = await MakeFirstLogin();

            //se intenta el primer login
            if (fLoginResponse != null &&
                fLoginResponse.TryGetValue("realm", out string? realm) &&
                fLoginResponse.TryGetValue("randomKey", out string? randomKey))
            {
                //se intenta el segundo login
                return await MakeSecondLogin(realm, randomKey);
            }
            else
            {
                throw new InvalidLoginException("Fallo el primer intento de login a DssPro");
            }
        }

        private static async Task<Dictionary<string, string>> MakeFirstLogin()
        {
            using HttpClient client = new();
            var requestBody = new
            {
                userName = AppParamsTools.GetEnvironmentVariable("DssPro:userName"),
                ipAddress = AppParamsTools.GetEnvironmentVariable("DssPro:ipAdress"),
                clientType = AppParamsTools.GetEnvironmentVariable("DssPro:clientType")
            };

            var requestContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            string loginUrl = baseUrl + AppParamsTools.GetEnvironmentVariable("DssPro:loginUrl");
            var response = await client.PostAsync(loginUrl, requestContent);
            Dictionary<string, string>? rspContent = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
            
            return rspContent ?? new();
        }

        private static async Task<Dictionary<string, string>> MakeSecondLogin(string realm, string randomKey)
        {
            Dictionary<string, string> tokenData = new();

            string userName = AppParamsTools.GetEnvironmentVariable("DssPro:userName");
            string ipAddress = AppParamsTools.GetEnvironmentVariable("DssPro:ipAdress");
            string clientType = AppParamsTools.GetEnvironmentVariable("DssPro:clientType");
            string userType = AppParamsTools.GetEnvironmentVariable("DssPro:userType");

            Dictionary<string, string> signatureData = GenerateSignature(realm, randomKey);
            Dictionary<string, byte[]> rasKeys = GetRsaKeys();

            if (signatureData.TryGetValue("signature", out string? signature) &&
                signatureData.TryGetValue("md5TokenSignature", out string? md5TokenSignature) &&
                rasKeys.TryGetValue("publicKey", out byte[]? publickKey) &&
                rasKeys.TryGetValue("privateKey", out byte[]? privateKey))
            {
                using HttpClient client = new();
                var requestBody = new
                {
                    signature = signature,
                    userName = userName,
                    randomKey = randomKey,
                    publicKey = Convert.ToBase64String(publickKey).ToString(),
                    encrytType = "MD5",
                    ipAddress = ipAddress,
                    clientType = clientType,
                    userType = Int32.Parse(userType)
                };
                var requestContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

                string loginUrl = baseUrl + AppParamsTools.GetEnvironmentVariable("DssPro:loginUrl");
                var response = await client.PostAsync(loginUrl, requestContent);

                JsonObject? rspContent = await response.Content.ReadFromJsonAsync<JsonObject>();
                if (rspContent != null && rspContent.TryGetPropertyValue("token", out var tokenNode) && 
                    tokenNode != null)
                {
                    tokenData.Add("token", tokenNode.GetValue<string>());
                    tokenData.Add("md5TokenSignature", md5TokenSignature);
                }

                return tokenData;
            }
            else
            {
                throw new InvalidLoginException("Error generando el cuerpo para el segundo login");
            }
        }

        private static Dictionary<string, string> GenerateSignature(string realm, string randomKey)
        {
            Dictionary<string, string> signature = new();

            string user = AppParamsTools.GetEnvironmentVariable("DssPro:userName");
            string pass = AppParamsTools.GetEnvironmentVariable("DssPro:password");
            
            string md5Pass = SecurityTools.ConvertToMD5(pass);
            string md5User = SecurityTools.ConvertToMD5(user + md5Pass);
            string md5Seed = SecurityTools.ConvertToMD5(md5User);
            string md5Signature = SecurityTools.ConvertToMD5(user + ":" + realm + ":" + md5Seed);
            signature.Add("md5TokenSignature", md5Signature);
            string md5FinalSignature = SecurityTools.ConvertToMD5(md5Signature + ":" + randomKey);
            signature.Add("signature", md5FinalSignature);

            return signature;
        }

        private static Dictionary<string, byte[]> GetRsaKeys()
        {
            Dictionary<string, byte[]> rsaKeyPair = new();

            var rsa = new RSACryptoServiceProvider(Int32.Parse(AppParamsTools.GetEnvironmentVariable("DssPro:RsaType")));

            rsaKeyPair.Add("publicKey", rsa.ExportRSAPublicKey());
            rsaKeyPair.Add("privateKey", rsa.ExportRSAPrivateKey());

            return rsaKeyPair;
        }

        /*=========================================================================
         * servicios de actualizacion de datos
         * =======================================================================*/
        public static async Task<List<RawEmotionRecordDto>> GetFaceRecognitionData(string benDocNumber, DateTime startDate, DateTime endDate)        
        {
            List<RawEmotionRecordDto> rawData = new();
            try
            {
                Dictionary<string, string> tokenData = await GetValidToken();
                if (tokenData != null && tokenData.TryGetValue("token", out string? token) && !string.IsNullOrEmpty(token))
                {
                    using HttpClient client = new();

                    DateTimeOffset dateTimeOffset = new DateTimeOffset(startDate);
                    long startDateSec = dateTimeOffset.ToUnixTimeSeconds();
                  
                    DateTimeOffset dateTimeOffset2 = new DateTimeOffset(endDate);
                    long endDateSec = dateTimeOffset2.ToUnixTimeSeconds();

                    var requestBody = new
                    {
                        beginTime = startDateSec,
                        endTime = endDateSec,
                        personId = benDocNumber,
                        page = 1,
                        pageSize = 100,
                        currentPage = 1
                    };
                    var requestContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
                    client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en"));
                    client.DefaultRequestHeaders.Add("X-Subject-Token", token);

                    string loginUrl = baseUrl + AppParamsTools.GetEnvironmentVariable("DssPro:SearchByFeatureUrl");

                    var response = await client.PostAsync(loginUrl, requestContent);
                    JsonObject? rspContent = await response.Content.ReadFromJsonAsync<JsonObject>();
                    if (rspContent != null && 
                        rspContent.TryGetPropertyValue("data", out var dataInfo) &&
                        dataInfo != null &&
                        dataInfo.AsObject().TryGetPropertyValue("pageData", out var pageInfo) &&
                        pageInfo != null)
                    {
                        JsonArray emoData = pageInfo.AsArray();
                        foreach(var record in emoData)
                        {
                            if (record != null)
                            {
                                RawEmotionRecordDto rec = JsonSerializer.Deserialize<RawEmotionRecordDto>(record.ToJsonString());
                                if(rec != null) rawData.Add(rec);
                            }
                        }
                        
                    }

                    return rawData;
                }
                else
                {
                    throw new InvalidLoginException("Login no valido");
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public static async Task DownloadFaceRecognitionData(string? documentNumber, DateTime? from, DateTime? to)
        {
            DateTime baseDate = DateTime.UtcNow;
            DateTime startDate = new DateTime(baseDate.Year, baseDate.Month, baseDate.Day, 0, 0, 0);
            DateTime endDate = new DateTime(baseDate.Year, baseDate.Month, baseDate.Day, 23, 59, 59);
            if (from.HasValue && to.HasValue)
            {
                startDate = new DateTime(from.Value.Year, from.Value.Month, from.Value.Day, 0, 0, 0);
                endDate = new DateTime(to.Value.Year, to.Value.Month, to.Value.Day, 23, 59, 59);
            }

            List<DtoBeneficiaries> beneficiaries;
            if (string.IsNullOrEmpty(documentNumber))
            {
                beneficiaries = await BeneficiariesServices.GetAllAsync(null, null, null,null, null, null, null, null, true, null, null);
            }
            else
            {
                beneficiaries = await BeneficiariesServices.GetAllAsync(null, null, null, null, null, documentNumber, null
                    , null, true, null, null);
            }

            foreach (DtoBeneficiaries ben in beneficiaries)
            {
                List<RawEmotionRecordDto> emRawData = await GetFaceRecognitionData(ben.DocumentNumber, startDate, endDate);
                await SaveRawEmotionData(emRawData);
            }
        }

        private static async Task<bool> SaveRawEmotionData(List<RawEmotionRecordDto> emRawData)
        {
            using var sqlContext = new SqlContext();
            foreach (RawEmotionRecordDto rec in emRawData)
            {
                DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); // Unix epoch
                DateTime createdOn = unixEpoch.AddSeconds(double.Parse(rec.captureTime)).ToUniversalTime();

                EmotionRawDataEntity? eRecVal = await sqlContext.EmotionRawDataEntities.FindAsync(rec.id);
                if(eRecVal == null)
                {
                    await sqlContext.EmotionRawDataEntities.AddAsync(new EmotionRawDataEntity
                    {
                        Id = rec.id,
                        PersonId = rec.personId,
                        DahuaChannelName = rec.channelName,
                        EmotionId = Int32.Parse(rec.emotion),
                        CreatedOn = createdOn
                    });
                    await sqlContext.SaveChangesAsync();
                }
            }
            return true;
        }
    }
}

