namespace DataAccessLayer.Entities
{
    public class GenresSeries
    {
        public int SeriesId { get; set; }
        public int GenreId { get; set; }

        public Series Series { get; set; }
        public Genre Genre { get; set; }
    }
}
