using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Entities
{
    public class GenresFilm : IEntityManyToMany
    {
        public int FilmId { get; set; }
        public int GenreId { get; set; }

        int IEntityManyToMany.EntityId
        {
            get { return GenreId; }
            set { GenreId = value; }
        }

        public Film Film { get; set; }
        public Genre Genre { get; set; }
    }
}
