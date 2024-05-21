namespace DataAccessLayer.Entities
{
    public class Cartoon
    {
        public int Id { get; set; }
        public byte[] Poster { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? CountSeasons { get; set; }
        public int? CountParts { get; set; }
        public TimeOnly? Duration { get; set; }
        public int CategoryId { get; set; }
        public int AnimationTypeId { get; set; }
        public int Year { get; set; }
        public string Director { get; set; }
        public int Rating { get; set; }
        public string StudioName { get; set; }
        public string TrailerUri { get; set; }

        public Category Category { get; set; }
        public AnimationType AnimationType { get; set; }
        public ICollection<CartoonPart> Cartoons { get; set; }
        public ICollection<CategoriesCartoon> CategoriesCartoons { get; set; }
        public ICollection<GenresCartoon> GenresCartoons { get; set; }
    }
}
