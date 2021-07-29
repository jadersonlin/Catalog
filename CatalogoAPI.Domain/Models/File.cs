using System;

namespace Catalog.Domain.Models
{
    public class File
    {
        public Guid Id { get; set; }

        public DateTime UploadStartedAt { get; set; }

        public DateTime UploadFinishedAt { get; set; }

        public string Username { get; set; }
    }
}
