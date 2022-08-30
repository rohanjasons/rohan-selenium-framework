using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using WebDriver = Rohan.Selenium.Framework.Enums.WebDriver;

namespace Rohan.Selenium.Framework.Selenium;

/// <summary>
/// Base class to inject in all tests. This contains the driver factory that will spin up a web driver instance for the test.
/// </summary>
public class WebTestBase
{
    /// <summary>
    /// Constructor where a new instance of the WebDriverManager is initialised. This is used to automatically get the latest version of the web driver that you wish to use.
    /// </summary>
    public WebTestBase()
    {
        new DriverManager().SetUpDriver(new ChromeConfig());
    }

    /// <summary>
    /// Gets instance of the WebDriver used during the tests.
    /// </summary>
    public IWebDriver? WebBrowserDriver { get; private set; }

    /// <summary>
    /// Start up the WebDriver and navigate to the URL specified.
    /// </summary>
    /// <param name="url">The Url that will be loaded in the web page.</param>
    /// <param name="deleteAllCookies">Should the cookies be deleted before starting the browser.</param>
    /// <param name="webDriver">The webdriver that will be used during the test.</param>
    public void WebTestSetup(Uri url, bool deleteAllCookies = true, WebDriver webDriver = WebDriver.Chrome)
    {
        switch (webDriver)
        {
            case WebDriver.Chrome:
                InitialiseChromeLocal(url, deleteAllCookies);
                break;
            case WebDriver.ChromeBuild:
                InitialiseChromeBuild(url, deleteAllCookies);
                break;
            case WebDriver.ChromeHeadless:
                InitialiseChromeHeadless(url, deleteAllCookies);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(webDriver), webDriver, null);
        }
    }

    /// <summary>
    /// Initialise chrome on the local webdriver
    /// </summary>
    /// <param name="url">The url that will be navigated to</param>
    /// <param name="deleteAllCookies">Boolean to determine whether you want to delete cookies prior to opening browser</param>
    private void InitialiseChromeLocal(Uri url, bool deleteAllCookies)
    {
        var options = new ChromeOptions();
        options.AddUserProfilePreference("download.prompt_for_download", false);
        options.AddUserProfilePreference("download.directory_upgrade", true);
        options.AddUserProfilePreference("safebrowsing.enabled", true);
        options.AddUserProfilePreference("credentials_enable_service", false);
        options.AddUserProfilePreference("profile.password_manager_enabled", false);
        options.AddArguments("chrome.switches", "--disable-gpu", "--disable-popup-blocking", "--disable-extensions", "--disable-extensions-http-throttling", "--disable-extensions-file-access-check", "--disable-infobars", "--enable-automation", "--safebrowsing-disable-download-protection ", "--safebrowsing-disable-extension-blacklist", "--start-maximized");
        WebBrowserDriver = new ChromeDriver(options);
        InitialiseWebDriver(url, deleteAllCookies);
    }

    /// <summary>
    /// Initialise chrome on the local webdriver
    /// </summary>
    /// <param name="url">The url that will be navigated to</param>
    /// <param name="deleteAllCookies">Boolean to determin whether you want to delete cookies prior to openinig browser</param>
    private void InitialiseChromeHeadless(Uri url, bool deleteAllCookies)
    {
        var options = new ChromeOptions();
        options.AddUserProfilePreference("download.prompt_for_download", false);
        options.AddUserProfilePreference("download.directory_upgrade", true);
        options.AddUserProfilePreference("safebrowsing.enabled", true);
        options.AddUserProfilePreference("credentials_enable_service", false);
        options.AddUserProfilePreference("profile.password_manager_enabled", false);
        options.AddArguments("chrome.switches", "headless", "--disable-gpu", "--disable-popup-blocking", "--disable-extensions", "--disable-extensions-http-throttling", "--disable-extensions-file-access-check", "--disable-infobars", "--enable-automation", "--safebrowsing-disable-download-protection ", "--safebrowsing-disable-extension-blacklist", "--start-maximized");
        WebBrowserDriver = new ChromeDriver(options);
        InitialiseWebDriver(url, deleteAllCookies);
    }

    /// <summary>
    /// Initialise chrome on the build server
    /// </summary>
    /// <param name="url">The url that will be navigated to</param>
    /// <param name="deleteAllCookies">Boolean to determine whether you want to delete cookies prior to opening browser</param>
    private void InitialiseChromeBuild(Uri url, bool deleteAllCookies)
    {
        var options = new ChromeOptions();
        options.AddUserProfilePreference("download.prompt_for_download", false);
        options.AddUserProfilePreference("download.directory_upgrade", true);
        options.AddUserProfilePreference("safebrowsing.enabled", true);
        options.AddUserProfilePreference("credentials_enable_service", false);
        options.AddUserProfilePreference("profile.password_manager_enabled", false);
        options.AddArguments("chrome.switches", "--disable-popup-blocking", "--disable-extensions", "--disable-extensions-http-throttling", "--disable-extensions-file-access-check", "--disable-infobars", "--enable-automation", "--safebrowsing-disable-download-protection ", "--safebrowsing-disable-extension-blacklist", "--start-maximized");
        options.AddArguments("headless");
        WebBrowserDriver = new ChromeDriver(Environment.GetEnvironmentVariable("ChromeWebDriver"), options);
        InitialiseWebDriver(url, deleteAllCookies);
    }

    /// <summary>
    /// Navigate to inputted Url and maximise browser
    /// </summary>
    /// <param name="url">The url that will be navigated to</param>
    /// <param name="deleteAllCookies">Boolean to determine whether you want to delete cookies prior to opening browser</param>
    private void InitialiseWebDriver(Uri url, bool deleteAllCookies)
    {
        const int maxAttempts = 3;
        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            var message = string.Empty;
            try
            {
                SetTimeout.PageLoad(WebBrowserDriver, TimeSpan.FromSeconds(TimeoutInSeconds.DefaultTimeout));

                if (deleteAllCookies)
                {
                    WebBrowserDriver.Manage().Cookies.DeleteAllCookies();
                }

                WebBrowserDriver.Navigate().GoToUrl(url);
                WebBrowserDriver.Manage().Window.Maximize();
                break;
            }
            catch (WebDriverException exception)
            {
                message = message + $"Exception {attempt}:" + exception.Message;
                if (attempt >= maxAttempts)
                {
                    throw new WebDriverException(string.Format($"Failed to start Web Browser in timely manner. - {message}"));
                }
            }
        }
    }
}