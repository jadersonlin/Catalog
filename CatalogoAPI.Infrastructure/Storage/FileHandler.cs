using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Catalog.Infrastructure.Storage
{
    public class FileHandler
    {
        private readonly IFormFile file;
        private readonly string rootPath;
        private const string ValidExtension = ".xlsx";
        private const string SpreadsheetDirectory = "\\spreadsheets\\";

        public FileHandler(IFormFile file, string rootPath)
        {
            this.file = file;
            this.rootPath = rootPath;
        }

        public async Task<string> SaveFile()
        {
            if (!IsValid())
                throw new InvalidDataException("Arquivo inválido!");

            var fileGuid = Guid.NewGuid().ToString();

            var filePath = GetFilePath(fileGuid);

            using (var filestream = File.Create(filePath))
            {
                await file.CopyToAsync(filestream);

                filestream.Flush();
            }

            return fileGuid;
        }

        private string GetFilePath(string fileGuid)
        {
            var directory = rootPath + SpreadsheetDirectory;

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var fileName = file.FileName;

            var filePath = directory + fileGuid + Path.GetExtension(fileName);
            return filePath;
        }

        private bool IsValid()
        {
            return file.Length > 0 && Path.GetExtension(file.FileName) == ValidExtension;
        }
    }
}
