using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class Rating
    {
        public int Id { get; set; }
        public int SeriesId { get; set; }
        public string UserId { get; set; }
        public int Rate { get; set; }

        public Series Series{ get; set; }
        public User User { get; set; }
    }
}
