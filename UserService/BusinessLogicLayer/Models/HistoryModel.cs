using DataAccessLayer.Entities;

namespace BusinessLogicLayer.Models
{
    public class HistoryModel
    {
        public MediaWithType MediaWithType { get; set; }
        public int? PartNumber { get; set; }
        public int? SeasonNumber { get; set; }
    }
}
