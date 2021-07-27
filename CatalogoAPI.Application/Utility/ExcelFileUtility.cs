using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CatalogoAPI.Application.Utility
{
    public class ExcelFileUtility
    {
        public void GetExcelFile()
        {
            using (var content = new MultipartFormDataContent())
            {
                var stream = new StreamContent(File.Open(filePath, FileMode.Open));

                stream.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                stream.Headers.Add("Content-Disposition", "form-data; name=\"import\"; filename=\"attendances.xslx\"");
                content.Add(stream, "import", "attendances.xslx");

                var response = client.PostAsync(methodePath, content).Result;
                var result = response.Content.ReadAsAsync<ResponseModel<AttendanceModel>>().Result;
                return result.IsSuccess;
            }
        }
    }
}
