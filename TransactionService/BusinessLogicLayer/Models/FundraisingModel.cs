using Microsoft.AspNetCore.Http;

namespace BusinessLogicLayer.Models
{
    public class FundraisingModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }
        public string FundraisingUrl { get; set; }
    }
}
