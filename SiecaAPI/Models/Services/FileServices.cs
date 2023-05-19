using SiecaAPI.Commons;
using SiecaAPI.Errors;
using System.Drawing;
using System.Net;
using System.Net.Http.Headers;

namespace SiecaAPI.Models.Services
{
    public static class FileServices
    {
        public static async Task AddImageToNetworkFolder(IFormFile file, string fileName)
        {
            var folderPath = AppParamsTools.GetEnvironmentVariable("NetworkDrive:imagesNetworkPath");
            var username = AppParamsTools.GetEnvironmentVariable("NetworkDrive:networkUserName");
            var password = AppParamsTools.GetEnvironmentVariable("NetworkDrive:networkUserPassword");

            var credentials = new NetworkCredential(username, password);
            using (new NetworkConnection(folderPath, credentials))
            {
                string fullPath = Path.Combine(folderPath, fileName);

                using var stream = new FileStream(fullPath, FileMode.Create);
                await file.CopyToAsync(stream);
            }
        }

        public static async Task<byte[]> GetFileDataByName(string fileName)
        {
            var folderPath = AppParamsTools.GetEnvironmentVariable("NetworkDrive:imagesNetworkPath");
            var username = AppParamsTools.GetEnvironmentVariable("NetworkDrive:networkUserName");
            var password = AppParamsTools.GetEnvironmentVariable("NetworkDrive:networkUserPassword");

            var credentials = new NetworkCredential(username, password);
            using (new NetworkConnection(folderPath, credentials))
            {
                string fullPath = Path.Combine(folderPath, fileName);

                if (File.Exists(fullPath))
                {
                    return await File.ReadAllBytesAsync(fullPath);
                }
            }

            throw new NoDataFoundException("Archivo de imagen no encontrado");
        }

        public static async Task<byte[]> GetFileDataByNameLocal(string fileName)
        {
            var folderName = Path.Combine("Resources", "Images");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var fullPath = Path.Combine(pathToSave, fileName);

            if (File.Exists(fullPath))
            {
                return await File.ReadAllBytesAsync(fullPath);
            }



            /*var folderPath = AppParamsTools.GetEnvironmentVariable("NetworkDrive:imagesNetworkPath");
            var username = AppParamsTools.GetEnvironmentVariable("NetworkDrive:networkUserName");
            var password = AppParamsTools.GetEnvironmentVariable("NetworkDrive:networkUserPassword");

            var credentials = new NetworkCredential(username, password);
            using (new NetworkConnection(folderPath, credentials))
            {
                string fullPath = Path.Combine(folderPath, fileName);

                if (File.Exists(fullPath))
                {
                    return await File.ReadAllBytesAsync(fullPath);
                }
            }*/

            throw new NoDataFoundException("Archivo de imagen no encontrado");
        }
    }
}
