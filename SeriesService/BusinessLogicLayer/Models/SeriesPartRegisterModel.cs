using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Models
{
    public class SeriesPartRegisterModel
    {
        public int SeriesId { get; set; }
        public int SeasonNumber { get; set; }
        public int PartNumber { get; set; }

        [RegularExpression(@"\d+[.](0[0-9]|1[0-9]|2[0-4]):[0-5][0-9]:[0-5][0-9]",
            ErrorMessage = "Duration must be in the format 'd.hh:mm:ss'")]
        public string Duration { get; set; }
    }
}
