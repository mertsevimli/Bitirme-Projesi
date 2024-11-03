using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace Sarideniz.WebUI.Utils
{
    public class FileHelper
    {
        public static async Task<string> FileLoaderAsync(IFormFile formFile, string filePath = "/Img/")
        {
            string fileName = "";
            if (formFile != null && formFile.Length > 0)
            {
                fileName = formFile.FileName.ToLower();
                
                // Dosya yolundaki boşluklar kaldırıldı.
                string directory = Directory.GetCurrentDirectory() + "/wwwroot" + filePath + fileName;
                
                using var stream = new FileStream(directory, FileMode.Create);
                await formFile.CopyToAsync(stream);
            }
            
            return fileName;
        }

        public static bool FileRemover(string fileName, string filePath = "/Img/")
        {
            // Dosya yolundaki boşluklar kaldırıldı.
            string directory = Directory.GetCurrentDirectory() + "/wwwroot" + filePath + fileName;
            
            if (File.Exists(directory))
            {
                File.Delete(directory);
                return true;
            }
            return false;
        }
    }
}