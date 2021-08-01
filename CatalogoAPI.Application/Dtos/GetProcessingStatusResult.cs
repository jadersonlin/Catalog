using System;
using System.Collections.Generic;

namespace Catalog.Application.Dtos
{
    public class GetProcessingStatusResult : ResultBase
    {
        public string Id { get; set; }
        public string FileName { get; set; }
        public long Length { get; set; }
        public DateTime UploadedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public string Status { get; set; }
        public IList<KeyValuePair<string, string>> ValidationErrors { get; set; }

    }
}
