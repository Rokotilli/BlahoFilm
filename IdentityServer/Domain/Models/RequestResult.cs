using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class RequestResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = "";
    }
}
