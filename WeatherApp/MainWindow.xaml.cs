using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WeatherApp
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string apiKey = "a96c831bea8df6d395eaf8c9afd8e76f";
        // Link aus Website -> https://api.openweathermap.org/data/2.5/weather?q={city name}&appid={API key} -> Beispiel:  ?q=Berlin&appid=a96c831bea8df6d395eaf8c9afd8e76f
        private readonly string requestUrl = @"https://api.openweathermap.org/data/2.5/weather";  // Alles ab dem ? wird gelöscht da man das später eh mit den passenden Werten befüllen muss
        string cityName = "Köln";

        public MainWindow()
        {
            InitializeComponent();

            
            WeatherMapResponse result = GetWeatherData(cityName);

            string finalImage = "sun";

            if (result.weather[0].main.Contains("Clouds"))
            {
                finalImage = "cloud.png";
            }
            else if (result.weather[0].main.Contains("Snow"))
            {
                finalImage = "sun.png";
            }
            else if (result.weather[0].main.Contains("Rain"))
            {
                finalImage = "rain.png";
            }
            else if(result.weather[0].main.Contains("Clear"))
            {
                finalImage = "sun.png";
            }
            
            backgroundImage.ImageSource = new BitmapImage(new Uri("Images/" + finalImage, UriKind.Relative));

            

            // Temperatur auf GUI anzeigen
            labelTemperature.Content = (result.main.temp).ToString("F1") + "°C";    // ToString("F1") ist Float mit 1 nachkomma Zahl
            labelInfo_min.Content = "min: " + (result.main.temp_min).ToString("F1") + "°C";
            labelInfo_max.Content = "max: " + (result.main.temp_max).ToString("F1") + "°C";
            labelInfo_weather.Content = result.weather[0].main;
        }

        public WeatherMapResponse GetWeatherData(string city)
        {
            HttpClient httpClient = new HttpClient(); // Neuen Client erstellen -> Objetk Instanzieren von der Klasse HttpClient

            // Query wird jetzt aus Strings Konkatiniert
            string cityName = city;

            var finalUri = Convert.ToString(requestUrl + "?q=" + cityName + "&appid=" + apiKey + "&units=metric");
            // Programm läuft auf einem Thread ab aber wenn was Async ist öffnet sich ein anderer Thread und es läuft Parallel und nach abschluss schließt sich der Async Threat und kommt zum Main Thread zurück
            HttpResponseMessage httpResponse = httpClient.GetAsync(finalUri).Result; // Durch das Result wartet man bis der Thread Fertig ist und dann geht das Mainprogramm weiter   

            string response = httpResponse.Content.ReadAsStringAsync().Result;  // Da Content alles mögliche sein kann, weil wir es nicht wissen nutzt man die Methode ReadAsString und Async wegen schritt davor

            // NUGET PACKAGE NEWTON.SOFT.JASON installieren um die JSON API response in C# Konvertieren zu können
            WeatherMapResponse weatherMapResponse = JsonConvert.DeserializeObject<WeatherMapResponse>(response);

            return weatherMapResponse;

            //string aktuelleTemperatur = weatherMapResponse.main.temp.ToString();
            //string minTemperatur = weatherMapResponse.main.temp_min.ToString();
            //string maxTemperatur = weatherMapResponse.main.temp_max.ToString();

        }
    }
}
