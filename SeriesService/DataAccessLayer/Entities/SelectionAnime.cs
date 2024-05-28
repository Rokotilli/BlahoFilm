using DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class SelectionAnime : IEntityManyToMany
    {
        public int AnimeId { get; set; }
        public int SelectionId { get; set; }

        int IEntityManyToMany.EntityId
        {
            get { return SelectionId; }
            set { SelectionId = value; } 
        }

        public Anime Anime{ get; set; }
        public Selection Selection { get; set; }
    }
}
