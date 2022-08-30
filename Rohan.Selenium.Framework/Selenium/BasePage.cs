namespace Rohan.Selenium.Framework.Selenium;

/// <summary>
/// Base Page that all test pages will inherit to gain access to the Web Driver
/// </summary>
public class BasePage
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BasePage"/> class.
    /// Initialise all the controls on the page so that they are accessible.
    /// </summary>
    /// <param name="webDriver">WebDriver that is in use.</param>
    protected BasePage(IWebDriver webDriver)
    {
        if (WaitForPageToFinishLoading(webDriver) == false)
        {
            throw new Exception("Timed out while waiting for the page to load.");
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BasePage"/> class.
    /// Initialise all the controls on the page so that they are accessible.
    /// </summary>
    /// <param name="webDriver">WebDriver that is in use.</param>
    /// <param name="elementId">The element to initially check to ensure on correct page.</param>
    protected BasePage(IWebDriver webDriver, string elementId)
    {
        if (WaitForPageToFinishLoading(webDriver) == false)
        {
            throw new Exception("Timed out while waiting for the page to load.");
        }

        AssertElementIsDisplayed(elementId);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BasePage"/> class.
    /// Initialise all the controls on the page so that they are accessible.
    /// </summary>
    /// <param name="webDriver">WebDriver that is in use.</param>
    /// <param name="elementId">The element to initially check to ensure on correct page.</param>
    protected BasePage(IWebDriver webDriver, By elementId)
    {
        if (WaitForPageToFinishLoading(webDriver) == false)
        {
            throw new Exception("Timed out while waiting for the page to load.");
        }

        this.AssertElementIsDisplayed(elementId);
    }

    /// <summary>
    /// Gets or sets IWebDriver Instance
    /// </summary>
    private static IWebDriver Driver { get; set; }

    /// <summary>
    /// Wrapped the wait for javascript method for use when loading webpages
    /// </summary>
    /// <param name="webDriver">The web browser driver</param>
    /// <returns>The browser once the javascript load has completed</returns>
    private static bool WaitForPageToFinishLoading(IWebDriver webDriver)
    {
        Driver = webDriver;

        IWait<IWebDriver> wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(TimeoutInSeconds.ExtendedTimeout));
        return wait.Until(driver => ((IJavaScriptExecutor)driver).ExecuteScript("return document.readyState").Equals("complete"));
    }

    /// <summary>
    /// Checks to see if the correct page has been loaded
    /// </summary>
    /// <param name="elementId">the element that is checked to ensure that the correct page is loaded</param>
    /// <returns>Returns a true or false</returns>
    private static bool AssertElementIsDisplayed(string elementId)
    {
        const int timeOut = TimeoutInSeconds.DefaultTimeout;
        for (var time = 0; time < timeOut; time++)
        {
            try
            {
                Driver.FindElement(By.Id(elementId));
                return true;
            }
            catch (NoSuchElementException)
            {
            }

            Thread.Sleep(TimeSpan.FromSeconds(1));
        }

        throw new Exception($"Could not find element with ID - {elementId} on page");
    }

    /// <summary>
    /// Checks to see if the correct page has been loaded
    /// </summary>
    /// <param name="elementId">the element that is checked to ensure that the correct page is loaded</param>
    /// <returns>Returns a true or false</returns>
    private bool AssertElementIsDisplayed(By elementId)
    {
        const int timeOut = TimeoutInSeconds.DefaultTimeout;
        for (var time = 0; time < timeOut; time++)
        {
            try
            {
                Driver.FindElement(elementId);
                return true;
            }
            catch (NoSuchElementException)
            {
            }

            Thread.Sleep(TimeSpan.FromSeconds(1));
        }

        throw new Exception($"Could not find element with ID - {elementId} on page");
    }
}