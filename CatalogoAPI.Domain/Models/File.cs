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
            ValidationErrors = new List<KeyValuePair<string, string>>();
        }

        public Guid Id { get; }
        public string FileName { get; set; }
        public long Length { get; set; }
        public DateTime UploadedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public FileProcessingStatus Status { get; set; }
        public IList<KeyValuePair<string, string>> ValidationErrors { get; }

        public void InformError(string cell, string errorMessage)
        {
            ValidationErrors.Add(new KeyValuePair<string, string>(cell, errorMessage));
        }
    }
}
