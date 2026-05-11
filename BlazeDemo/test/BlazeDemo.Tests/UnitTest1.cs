using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace BlazeDemo.Tests;

[TestFixture]
public class BlazeDemoTests
{
    private IWebDriver driver;
    private const string BaseURL = "https://blazedemo.com";

    [SetUp]
    public void SetupTest()
    {
        driver = new ChromeDriver();
    }

    [TearDown]
    public void TeardownTest()
    {
        driver.Quit();
        driver.Dispose();
    }

    [Test]
    public void FlightSearch_MexicoCityToDublin_ShouldHaveAtLeastThreeFlights()
    {
        // Arrange
        driver.Navigate().GoToUrl(BaseURL);

        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

        var departureDropdown = new SelectElement(
            wait.Until(ExpectedConditions.ElementExists(By.Name("fromPort")))
        );
        departureDropdown.SelectByText("Mexico City");

        var destinationDropdown = new SelectElement(
            wait.Until(ExpectedConditions.ElementExists(By.Name("toPort")))
        );
        destinationDropdown.SelectByText("Dublin");

        // Act
        driver.FindElement(By.CssSelector("input[type='submit']")).Click();

        // Assert
        var flightRows = wait.Until(ExpectedConditions.VisibilityOfAllElementsLocatedBy(
            By.CssSelector("table.table tbody tr")
        ));

        flightRows.Count.Should().BeGreaterThanOrEqualTo(3);
    }
}