using System;
using System.Collections;
using System.Collections.Generic;
using Catalog.Domain.Enums;

namespace Catalog.Domain.Models
{
    public class File
    {
        public File(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
        public string FileName { get; set; }
        public long Length { get; set; }
        public DateTime UploadedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public FileProcessingStatus Status => GetStatus();
        
        public IList<KeyValuePair<string, string>> ValidationErrors { get; set; }

        private FileProcessingStatus GetStatus()
        {
            if (ProcessedAt == null)
                return FileProcessingStatus.Uploaded;

            return ValidationErrors.Count == 0 ? FileProcessingStatus.Processed : FileProcessingStatus.InvalidFile;
        }
    }
}
