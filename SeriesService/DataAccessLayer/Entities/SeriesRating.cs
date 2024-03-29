using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class SeriesRating
    {
        public int Id { get; set; }
        public int SeriesId { get; set; }
        public string UserId { get; set; }
        public double Rate { get; set; }
        public User User { get; set; }
        public Series Series { get; set; }
    }
}
