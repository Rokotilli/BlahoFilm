using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class CartoonRating
    {
        public int Id { get; set; }
        public int CartoonId { get; set; }
        public string UserId { get; set; }
        public double Rate { get; set; }
        public User User { get; set; }
        public Cartoon Cartoon { get; set; }
    }
}
