using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Interfaces
{
    public interface IRatingService
    {
        Task<string> RateSeries(int seriesId, int rate, string userid);
    }
}
