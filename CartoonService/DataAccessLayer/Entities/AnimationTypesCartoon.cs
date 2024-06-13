namespace DataAccessLayer.Entities
{
    public class AnimationTypesCartoon
    {
        public int CartoonId { get; set; }
        public int AnimationTypeId { get; set; }

        public Cartoon Cartoon { get; set; }
        public AnimationType AnimationTypes { get; set; }
    }
}
