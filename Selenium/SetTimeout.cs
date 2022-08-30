namespace Rohan.Selenium.Framework.Selenium;

public class SetTimeout
{
    /// <summary>
    /// Set the Implicit Wait Timeout.
    /// </summary>
    /// <param name="driver">WebDriver Instance</param>
    /// <param name="timeSpan">Timespan to wait for</param>
    public static void ImplicitlyWait(IWebDriver driver, TimeSpan timeSpan)
    {
        driver.Manage().Timeouts().ImplicitWait = timeSpan;
    }

    /// <summary>
    /// Set the Page Load Timeout.
    /// </summary>
    /// <param name="driver">WebDriver Instance</param>
    /// <param name="timeSpan">Timespan to wait for</param>
    public static void PageLoad(IWebDriver driver, TimeSpan timeSpan)
    {
        driver.Manage().Timeouts().PageLoad = timeSpan;
    }

    /// <summary>
    /// Set the Script Timeout.
    /// </summary>
    /// <param name="driver">WebDriver Instance</param>
    /// <param name="timeSpan">Timespan to wait for</param>
    public static void Script(IWebDriver driver, TimeSpan timeSpan)
    {
        driver.Manage().Timeouts().AsynchronousJavaScript = timeSpan;
    }
}