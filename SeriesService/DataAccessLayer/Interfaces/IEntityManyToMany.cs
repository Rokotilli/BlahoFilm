namespace DataAccessLayer.Interfaces
{
    public interface IEntityManyToMany
    {
        int SeriesId { get; set; }
        int EntityId { get; set; }
    }
}
