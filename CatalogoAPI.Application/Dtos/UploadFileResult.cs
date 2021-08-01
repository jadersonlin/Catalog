using System;

namespace Catalog.Application.Dtos
{
    public class UploadFileResult : ResultBase
    {
        /// <summary>
        /// Identificador do arquivo.
        /// </summary>
        public string Id { get; set; }
        public string FileName { get; set; }
        public long Length { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
