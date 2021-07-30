using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Enums
{
    public enum FileProcessingStatus
    {
        [Description("Uploaded")]
        Uploaded = 1,

        [Description("Processed")]
        Processed = 2,

        [Description("Invalid file")]
        InvalidFile = 3
    }
}
