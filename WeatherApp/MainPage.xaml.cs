using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI;

namespace WeatherApp;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this.InitializeComponent();
        _ = Main();
    }
    async Task Main()
    {
        string apiKey = "YOUR API KEY";
        string city = txtCity.Text;
        string apiUrl = $"http://api.weatherapi.com/v1/current.json?key={apiKey}&q={city}&aqi=no";

        try
        {

            string weatherData = await GetWeatherData(apiUrl);
            // Parse and extract relevant information from the JSON response

            string temperature = Math.Round(Convert.ToDouble(ParseValues(weatherData, "temp_c"))).ToString();
            txtTemp.Text = temperature.ToString() + "°";
            
            string precipitation = Math.Round(Convert.ToDouble(ParseValues(weatherData, "precip_in"))).ToString();
            txtPrecip.Text = precipitation.ToString();

            string wind = Math.Round(Convert.ToDouble(ParseValues(weatherData, "wind_mph"))).ToString();
            txtWind.Text = wind.ToString() + "m/s";

            string humidity = ParseValues(weatherData, "humidity");
            txtHumidity.Text = humidity.ToString();

            string date = ParseValues(weatherData, "localtime");
            txtTime.Text = Convert.ToDateTime(date.Substring(0, date.Length - 2)).ToString("HH:mm");

            if (weatherData.Contains("Sunny") || weatherData.Contains("Clear"))
            {
                weatherIcon.ImageSource = new BitmapImage(new Uri(base.BaseUri, @"/WeatherApp/Assets/Icons/icons8-sun-96.png"));
                gridMain.Background = new SolidColorBrush(Colors.LightGoldenrodYellow);
                recPnl.Fill = new SolidColorBrush(Colors.Gold);
            }
            else if (weatherData.Contains("Light rain"))
            {
                weatherIcon.ImageSource = new BitmapImage(new Uri(base.BaseUri, @"/WeatherApp/Assets/Icons/icons8-light-rain-96.png"));
                gridMain.Background = new SolidColorBrush(Colors.LightGray);
                recPnl.Fill = new SolidColorBrush(Colors.Gray);
            }
            else if (weatherData.Contains("Partly cloudy"))
            {
                weatherIcon.ImageSource = new BitmapImage(new Uri(base.BaseUri, @"/WeatherApp/Assets/Icons/icons8-partly-cloudy-day-96.png"));
                gridMain.Background = new SolidColorBrush(Colors.LightGray);
                recPnl.Fill = new SolidColorBrush(Colors.Gray);
            }
            else if (weatherData.Contains("Light snow"))
            {
                weatherIcon.ImageSource = new BitmapImage(new Uri(base.BaseUri, @"/WeatherApp/Assets/Icons/icons8-light-snow-96.png"));
                gridMain.Background = new SolidColorBrush(Colors.LightGray);
                recPnl.Fill = new SolidColorBrush(Colors.Gray);
            }
            else
            {
                weatherIcon.ImageSource = new BitmapImage(new Uri(base.BaseUri, @"/WeatherApp/Assets/Icons/icons8-clouds-96.png"));
                gridMain.Background = new SolidColorBrush(Colors.LightBlue);
                recPnl.Fill = new SolidColorBrush(Colors.White);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    async Task<string> GetWeatherData(string apiUrl)
    {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string data = await response.Content.ReadAsStringAsync();

                return data;
            }
            else
            {
                throw new HttpRequestException($"Failed to fetch data. Status code: {response.StatusCode}");
            }
        }
    }

    string ParseValues(string jsonData, string field)
    {
        // Parse JSON data to extract the information
        // This depends on the structure of the response from the weather API
        // Here, I'm using a simple string search, which is not robust for all cases
        int startIndex = jsonData.IndexOf("\"" + field + "\":") + ("\"" + field + "\":").Length;
        int endIndex = jsonData.IndexOf(",", startIndex);
        string Str = jsonData.Substring(startIndex, endIndex - startIndex).Trim();
        return Str.Trim('"');
    }

    private void btnSubmit_Click(object sender, RoutedEventArgs e)
    {
        _ = Main();
    }
}
