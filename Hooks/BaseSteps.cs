using OpenQA.Selenium.Chrome;
using System;
using TechTalk.SpecFlow;

namespace MercatorTest.Hooks
{
    public class BaseSteps
    {

        protected readonly GlobalDriver global;

        private static bool driverInitialised = false;

        public BaseSteps(GlobalDriver global)
        {
            this.global = global;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {

            if (!driverInitialised)
            {
                this.global.Driver = new ChromeDriver();
                this.global.Driver.Manage().Window.Maximize();
                this.global.Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
                driverInitialised = true;
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {

            if (driverInitialised)
            {
                this.global.Driver.Close();
                this.global.Driver.Quit();
                driverInitialised = false;
            }

        }
        
    }
}
