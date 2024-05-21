using DataAccessLayer.Interfaces;

namespace DataAccessLayer.Entities
{
    public class CategoriesCartoon : IEntityManyToMany
    {
        public int CartoonId { get; set; } 
        public int CategoryId { get; set; }
        int IEntityManyToMany.EntityId
        {
            get { return CategoryId; }
            set { CategoryId = value; }
        }
        public Cartoon Cartoon { get; set; }
        public Category Category { get; set; }
    }
}
