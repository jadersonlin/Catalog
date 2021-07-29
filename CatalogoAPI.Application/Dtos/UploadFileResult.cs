namespace Catalog.Application.Dtos
{
    public class UploadFileResult : ResultBase
    {
        /// <summary>
        /// Identificador do arquivo.
        /// </summary>
        public string Id { get; set; }

        public string FileName { get; set; }
    }
}
