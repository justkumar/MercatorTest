using MercatorTest.Hooks;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow;

namespace MercatorTest.Steps
{
    [Binding]
    public sealed class MercatorTestSteps : BaseSteps
    {
        public MercatorTestSteps(GlobalDriver global) : base(global)
        {
        }

        IWebElement mainMenuDressesLink => this.global.Driver.FindElement(By.XPath("//*[@id='block_top_menu']/ul/li[2]"));
        IList<IWebElement> addToCart => this.global.Driver.FindElements(By.XPath("//span[contains(text(),'Add to cart')]"));
        IWebElement itemAddedToBasketConfirmation => this.global.Driver.FindElement(By.CssSelector("span[class='ajax_cart_product_txt ']"));
        
        IWebElement proceedToCheckoutButton => this.global.Driver.FindElement(By.CssSelector("a[title='Proceed to checkout']"));

        IList<IWebElement> priceOfItems => this.global.Driver.FindElements(By.XPath("//div[@class='right-block']//span[@class='price product-price']"));

      
        [StepDefinition(@"the user adds highest price item to cart")]
        public void WhenTheUserAddsHighestPriceItemToCart()
        {
            AddExpensiveItemToCart();
        }

        [StepDefinition(@"the browser opens on the ""(.*)"" home page")]
        public void GivenTheBrowserOpensOnTheHomePage(string url)
        {
            GoToHomePage(url);
        }

        [StepDefinition(@"the user clicks Dress menu")]
        public void WhenTheUserClicksDressMenu()
        {
            ClickALink();
        }


        [StepDefinition(@"the user see a confirmation message with Proceed to checkout button")]
        public void ThenTheUserSeeAConfirmationMessageWithProceedToCheckoutButton()
        {
            VerifyThatItemAddedToCart();
        }


        public void GoToHomePage(string url)
        {
            this.global.Driver.Navigate().GoToUrl(url);
        }

        public void ClickALink()
        {
            mainMenuDressesLink.Click();
        }

        public void AddExpensiveItemToCart()
        {
            decimal[] allPrices = new Decimal[priceOfItems.Count];

            int i = 0;

            foreach (IWebElement element in priceOfItems)
            {
                allPrices[i++] = decimal.Parse(Regex.Replace(element.Text, "[^0-9.]", ""));
            }

            decimal maxValue = allPrices.Max();
            int indexOfMaxPrice = allPrices.ToList().IndexOf(maxValue);

            Actions actions = new Actions(this.global.Driver);
            actions.MoveToElement(priceOfItems[indexOfMaxPrice]).Build().Perform();
            addToCart[indexOfMaxPrice].Click();
        }

        public void VerifyThatItemAddedToCart()
        {
            WebDriverWait wait = new WebDriverWait(this.global.Driver, TimeSpan.FromSeconds(10));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("span[class='ajax_cart_product_txt ']")));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.CssSelector("a[title='Proceed to checkout']")));
            Assert.IsTrue(itemAddedToBasketConfirmation.Displayed);
            Assert.IsTrue(proceedToCheckoutButton.Displayed);
        }
    }
}
