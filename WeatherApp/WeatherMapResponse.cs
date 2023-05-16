using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp
{
    public class WeatherMapResponse
    {
        public Main main;
        public List<Weather> weather;
        // z.B.: weather[0].description
    }
}
