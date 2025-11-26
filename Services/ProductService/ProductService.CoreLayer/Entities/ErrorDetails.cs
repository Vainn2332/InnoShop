using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProductService.CoreLayer.Entities
{
    public class ErrorDetails
    {
        public string ExceptionMessage { get; set; }
        public string ExceptionInfo { get; set; }
        public int StatusCode { get; set; }
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
