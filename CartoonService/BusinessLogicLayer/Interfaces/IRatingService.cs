using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IRatingService
    {
        Task<string> RateCartoon(int cartoonId, int rate, string userid);
        Task<string> RateCartoonPart(int cartoonPartId, int rate, string userid);
    }
}
