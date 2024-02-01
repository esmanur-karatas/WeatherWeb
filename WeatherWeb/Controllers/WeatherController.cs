using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using WeatherWeb.Models;

namespace WeatherWeb.Controllers
{
    public class WeatherController : Controller
    {
        // GET
        public ActionResult Index(string cityName, string cityName1)
        {
            return View();
        }

        public async Task<ActionResult> Search(string cityName)
        {
           
            try
            {
                string api = "36ba5daf32b4a9197a54aca8eea1e881";
                string connection = $"https://api.openweathermap.org/data/2.5/forecast?q={cityName}&mode=xml&lang=tr&units=metric&appid="+api;

                XDocument document = await LoadXmlFromUrl(connection);

                int numberOfDays = 5; // Örneğin, 5 gün gösterilsin

                var model = new WeatherViewModel[numberOfDays];

                for (int i = 0; i < numberOfDays; i++)
                {
                    var weatherData = new WeatherViewModel();

                    

                    weatherData.Day = DateTime.TryParseExact(document.Descendants("time").ElementAt(i * 8)?.Attribute("to")?.Value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime)
                        ? dateTime.ToString("ddd", new CultureInfo("tr-TR"))
                        : "Geçerli bir tarih bulunamadı.";

                    weatherData.Hour = DateTime.TryParseExact(document.Descendants("time").ElementAt(i * 8)?.Attribute("to")?.Value, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTimeHour)
                        ? dateTimeHour.ToString("HH:mm")
                        : "Geçerli bir saat bulunamadı.";

                    weatherData.Symbol = document.Descendants("symbol").ElementAt(i * 8).Attribute("name").Value;
                    weatherData.Temperature = TryGetTemperatureValue(document.Descendants("temperature").ElementAt(i * 8).Attribute("value").Value);
                    weatherData.MinTemperature = TryGetTemperatureValue(document.Descendants("temperature").ElementAt(i * 8).Attribute("min").Value);
                    weatherData.MaxTemperature = TryGetTemperatureValue(document.Descendants("temperature").ElementAt(i * 8).Attribute("max").Value);
                    weatherData.WindSpeed = document.Descendants("windSpeed").ElementAt(i * 8).Attribute("mps").Value;
                    weatherData.FeelsLike = document.Descendants("feels_like").ElementAt(i * 8).Attribute("value").Value;
                    weatherData.Humidity = document.Descendants("humidity").ElementAt(i * 8).Attribute("value").Value;
                    weatherData.Precipitation = document.Descendants("precipitation").ElementAt(i * 8).Attribute("probability").Value;

                    model[i] = weatherData;
                }
                ViewBag.CityName = cityName;
                return View("Index", model); 
            }
            catch (Exception)
            {
                
                return RedirectToAction("Index"); 
            }
        }
        private async Task<XDocument> LoadXmlFromUrl(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync(url);
                return XDocument.Parse(response);
            }
        }
        private string TryGetTemperatureValue(string temperatureValue)
        {
            if (double.TryParse(temperatureValue, out double temperature))
            {
                int lengthBeforeDecimal = temperatureValue.IndexOf('.'); 
                if (lengthBeforeDecimal == 1)
                {
                    return temperatureValue.Substring(0, 1); 
                }
                else if (lengthBeforeDecimal == 2)
                {
                    return temperatureValue.Substring(0, 2); 
                }
            }
            return temperatureValue;
        }
    }
}



