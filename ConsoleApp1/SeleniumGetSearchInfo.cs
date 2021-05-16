using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.DevTools.V85.Debugger;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace ConsoleApp1
{
    public class SeleniumGetSearchInfo
    {
        private FirefoxDriver _driver;
        private string _path;

        public SeleniumGetSearchInfo()
        {
        }

        public async Task<List<string>> GetAllFirstPageJsonsAsync(string path, string searchTerm, CancellationToken cancellationToken)
        {
            var options = new FirefoxOptions();
            options.AddArgument("--headless");
            options.Profile = new FirefoxProfile();
            options.Profile.SetPreference("webdriver_enable_native_events", false);
            _driver = new FirefoxDriver(options);
            _path = path;
            _driver.Navigate().GoToUrl(_path);

            var bookInfoDicts = new List<Dictionary<string, string>>();
            var jsons = new List<string>();

            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                var originalWindow = _driver.CurrentWindowHandle;

                var searchBox = _driver.FindElement(By.ClassName("form-control"));
                var button = _driver.FindElement(By.ClassName("find_submit"));

                // searchBox.SendKeys("9785360098553");
                searchBox.SendKeys(searchTerm);
                Console.WriteLine(searchBox.GetProperty("value"));

                button.Submit();
                Console.WriteLine("--Searching--");

                var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(10));
                Console.WriteLine($"Windows count = {_driver.WindowHandles.Count}");
                _driver.SwitchTo().Window(_driver.WindowHandles[1]);
                Console.WriteLine($"title: {_driver.Title}");
                wait.Until(c => c.Title != "");
                // var res = wait.Until(c => c.FindElements(By.CssSelector(".search-container")));
                var res1 = wait.Until(c =>
                {
                    var semi = c.FindElement(By.CssSelector(".search-container"));
                    return semi.Displayed && semi.Enabled;
                });
                var res = _driver.FindElements(By.CssSelector(".search-container"));
                Console.WriteLine(res.Count);
                res.ToList().ForEach(
                    bookInfo =>
                    {
                        // var w = wait.Until(c =>
                        // {
                        //     var semi = c.FindElement(By.CssSelector(
                        //         "a[data-target=\"#resultModal\"]"));
                        //     return semi.Displayed && semi.Enabled;
                        // });
                        var r = bookInfo.FindElement(By.CssSelector(
                            "a[data-target=\"#resultModal\"]"));
                        r.Click();
                        var resultModal = _driver.FindElementById("resultModal");
                        wait.Until(ExpectedConditions.ElementToBeClickable(
                            By.CssSelector("a[href=\"#tab_descr\"]")));
                        var a = wait.Until(c=> c.FindElement(By.CssSelector("a[href=\"#tab_descr\"]")));
                        a.Click();
                        Thread.Sleep(100);
                        var descr = _driver.FindElements(By.CssSelector(".card-descr-table tr"));

                        var semiresult = new Dictionary<string, string>();
                        // Console.WriteLine(descr.Count);
                        descr.ToList().ForEach(
                            c =>
                            {
                                var propertyName = c.FindElement(By.TagName("th")).Text;
                                var propertyValue = c.FindElement(By.TagName("td")).Text;
                                semiresult[propertyName] = propertyValue;
                            });

                        bookInfoDicts.Add(semiresult);

                        var json = JsonConvert.SerializeObject(semiresult);
                        jsons.Add(json);
                        Console.WriteLine($"${jsons.Count}: ${json}");

                        var escapeButton = _driver.FindElement(By.CssSelector(".modal-header a[class=\"close\"]"));
                        escapeButton.Click();
                        wait.Until(ExpectedConditions.ElementToBeClickable(
                            By.CssSelector("a[data-target=\"#resultModal\"]")));
                        wait.Until(c =>
                        {
                            var fade = c.FindElements(By.ClassName("modal-backdrop"));
                            return fade.Count == 0;
                        });
                        Thread.Sleep(100);

                        var jsExecutor = _driver as IJavaScriptExecutor;
                        _driver.ExecuteScript("window.scrollBy(0,182)", "");
                        Thread.Sleep(2000);
                        cancellationToken.ThrowIfCancellationRequested();
                    }
                    );


                // Console.WriteLine(res.GetProperty("data-id"));
            }
            catch(Exception e)
            {
                Console.WriteLine($"error: {e.Message}");
            }
            finally
            {
                _driver.Close();
                _driver.Quit();
                Console.WriteLine("--Done--");
            }

            return jsons;
        }
    }
}