using DataAccessLayer.Entities;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer.Models
{
    public class HistoryModel
    {
        public MediaWithType MediaWithType { get; set; }
        public int PartNumber { get; set; }
        public int SeasonNumber { get; set; }

        [RegularExpression(@"\d+[.](0[0-9]|1[0-9]|2[0-4]):[0-5][0-9]:[0-5][0-9]",
            ErrorMessage = "TimeCode must be in the format 'd.hh:mm:ss'")]
        public string TimeCode { get; set; }
    }
}
