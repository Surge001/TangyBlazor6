using Microsoft.AspNetCore.Components.Forms;
using TangyWeb.Server.Service.IService;

namespace TangyWeb.Server.Service
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment hostEnvironment;
        public FileService(IWebHostEnvironment environment)
        {
            this.hostEnvironment = environment;
        }

        public bool DeleteFile(string filePath)
        {
            string file = this.hostEnvironment.WebRootPath + filePath;
            if (File.Exists(file))
            {
                File.Delete(file);
                return true;
            }
            return false;
        }

        public async Task<string> UploadFile(IBrowserFile file)
        {
            FileInfo info = new FileInfo(file.Name);
            string fileName = Guid.NewGuid().ToString() + info.Extension;
            string folderName = $"{this.hostEnvironment.WebRootPath}\\images\\product";
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }
            string filePath = Path.Combine(folderName, fileName);
            await using FileStream fs = new FileStream(filePath, FileMode.Create);
            await file.OpenReadStream().CopyToAsync(fs);
            string fileFullPath = $"/images/product/{fileName}";
            return fileFullPath;
        }
    }
}
