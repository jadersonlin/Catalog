using System;

namespace Catalog.Application.Dtos
{
    public class FileData
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public long Length { get; set; }
        public DateTime UploadedAt { get; set; }
        public string Hash { get; set; }
    }
}
