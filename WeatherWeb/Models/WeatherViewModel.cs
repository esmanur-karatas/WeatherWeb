using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeatherWeb.Models
{
    public class WeatherViewModel
    {
        public string Day { get; set; }
        public string Hour { get; set; }
        public string Symbol { get; set; }
        public string Temperature { get; set; }
        public string MinTemperature { get; set; }
        public string MaxTemperature { get; set; }
        public string WindSpeed { get; set; }
        public string FeelsLike { get; set; }
        public string Humidity { get; set; }
        public string Precipitation { get; set; }
        public string cityName { get; set; }

    }
}