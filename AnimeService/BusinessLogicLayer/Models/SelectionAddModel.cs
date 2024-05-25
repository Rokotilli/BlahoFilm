using Microsoft.AspNetCore.Http;

namespace BusinessLogicLayer.Models
{
    public class SelectionAddModel
    {
        public string Name { get; set; }
        public IFormFile Image { get; set; }
    }
}
