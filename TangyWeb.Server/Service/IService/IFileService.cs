using Microsoft.AspNetCore.Components.Forms;
using System.Net;

namespace TangyWeb.Server.Service.IService
{
    public interface IFileService
    {
        /// <summary>
        /// Uploads file from browser
        /// </summary>
        /// <param name=""></param>
        Task<string> UploadFile(IBrowserFile file);

        /// <summary>
        /// Deletes file from server
        /// </summary>
        /// <param name="filePath">Path to the file which is to be deleted</param>
        /// <returns>Value indicating whether or not the file is deleted.</returns>
        bool DeleteFile(string filePath);
    }
}
