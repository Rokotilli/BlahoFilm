using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class SeriesPartRating
    {
        public int Id { get; set; }
        public int SeriesPartId { get; set; }
        public string UserId { get; set; }
        public double Rate { get; set; }
        public User User { get; set; }
        public SeriesPart SeriesPart { get; set; }
    }
}
