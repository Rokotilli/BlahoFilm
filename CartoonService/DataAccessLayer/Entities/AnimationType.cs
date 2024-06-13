namespace DataAccessLayer.Entities
{
    public class AnimationType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<AnimationTypesCartoon> AnimationTypesCartoons { get; set; }
    }
}
