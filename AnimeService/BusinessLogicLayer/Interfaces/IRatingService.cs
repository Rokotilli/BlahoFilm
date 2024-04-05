using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IRatingService
    {
        Task<string> RateAnime(int animeId, int rate, string userid);
    }
}
