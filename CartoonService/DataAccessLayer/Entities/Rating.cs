using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class Rating
    {
        public int CartoonId { get; set; }
        public string UserId { get; set; }
        public int Rate { get; set; }
        public Cartoon Cartoon { get; set; }
        public User User { get; set; }
    }
}
