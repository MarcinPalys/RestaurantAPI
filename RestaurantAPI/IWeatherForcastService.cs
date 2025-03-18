namespace RestaurantAPI
{
    public interface IWeatherForcastService
    {
        public IEnumerable<WeatherForecast> Get(int resultCounter, int min, int max);
    }
}
