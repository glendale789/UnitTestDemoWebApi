using Xunit;
using WeatherApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using WeatherApi.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using WeatherApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace WeatherApi.Controllers.Tests
{
    public class WeatherForecastsControllerTests
    {
        private readonly Mock<IWeatherService> _serviceMock;
        private readonly WeatherForecastsController _controller;

        public WeatherForecastsControllerTests()
        {
            _serviceMock = new Mock<IWeatherService>();
            _controller = new WeatherForecastsController(new NullLogger<WeatherForecastsController>(), _serviceMock.Object);
        }

        [Fact]
        public async Task GetWeatherForecastNotFound()
        {
            // Arrange
            WeatherForecast weather = null;
            _serviceMock.Setup(x => x.GetWeatherForecastAsync(It.IsAny<int>())).ReturnsAsync(weather);

            // Act
            var result = await _controller.GetWeatherForecast(100);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetWeatherForecastFound()
        {
            // Arrange
            var weather = new WeatherForecast { Id = 1, Date = DateTime.Now, TemperatureC = 25, Summary = "Sunny" };
            _serviceMock.Setup(x => x.GetWeatherForecastAsync(It.IsAny<int>())).ReturnsAsync(weather);

            // Act
            var result = await _controller.GetWeatherForecast(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<WeatherForecast>(okResult.Value);
            Assert.Equal(weather.Id, returnValue.Id);
        }

        [Fact]
        public async Task GetWeatherForecasts()
        {
            // Arrange
            var weathers = new List<WeatherForecast>
            {
                new WeatherForecast { Id = 1, Date = DateTime.Now, TemperatureC = 25, Summary = "Sunny" },
                new WeatherForecast { Id = 2, Date = DateTime.Now, TemperatureC = 20, Summary = "Cloudy" }
            };
            _serviceMock.Setup(x => x.GetWeatherForecastsAsync()).ReturnsAsync(weathers);

            // Act
            var result = await _controller.GetWeatherForecasts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<WeatherForecast>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task AddWeatherForecast()
        {
            // Arrange
            var weather = new WeatherForecast { Id = 1, Date = DateTime.Now, TemperatureC = 25, Summary = "Sunny" };
            _serviceMock.Setup(x => x.AddWeatherForecastAsync(It.IsAny<WeatherForecast>())).ReturnsAsync(weather);

            // Act
            var result = await _controller.AddWeatherForecast(weather);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<WeatherForecast>(createdAtActionResult.Value);
            Assert.Equal(weather.Id, returnValue.Id);
        }
    }
}
