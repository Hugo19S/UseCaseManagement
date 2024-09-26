using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UseCaseManagement.Domain.Common
{
    public class FileRepresentation
    {
        public string Name { get; set; }

        public string ContentType { get; set; }

        public long Size { get; set; }
        public Stream Content { get; set; }
        
    }
}
