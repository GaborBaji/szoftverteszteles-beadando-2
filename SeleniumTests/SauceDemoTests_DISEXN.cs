using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;

namespace SeleniumTests
{
    [TestClass]
    public sealed class SauceDemoTests_DISEXN
    {
        IWebDriver driver;
        WebDriverWait wait;

        // A SauceDemo oldal nyilvanosan elerheto teszt felhasznaloja
        const string Url = "https://www.saucedemo.com/";
        const string ValidUser = "standard_user";
        const string ValidPassword = "secret_sauce";

        [TestInitialize]
        public void Setup()
        {
            // Edge inditas InPrivate modban, hogy ne utkozzunk a ceges profillal
            var options = new EdgeOptions();
            options.AddArgument("inprivate");
            options.AddArgument("--start-maximized");

            driver = new EdgeDriver(options);
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            // Nyitomenu - megnyitjuk a teszt oldalt
            driver.Navigate().GoToUrl(Url);
        }

        [TestCleanup]
        public void Teardown()
        {
            driver?.Quit();
        }

        //  POZITIV TESZT
        //  Sikeres bejelentkezes, termek kosarba helyezese, kosar tartalmanak ellenorzese
        [TestMethod]
        public void SikeresBejelentkezes_KosarbaHelyezes()
        {
            // 1. Bejelentkezes

            // Felhasznalonev mezo kitoltese (Id alapu lekerdezes)
            var userField = driver.FindElement(By.Id("user-name"));
            userField.SendKeys(ValidUser);

            // Jelszo mezo kitoltese (Name alapu lekerdezes)
            var passwordField = driver.FindElement(By.Name("password"));
            passwordField.SendKeys(ValidPassword);

            // Login gomb megnyomasa (Id alapu lekerdezes, Click muvelet)
            var loginButton = driver.FindElement(By.Id("login-button"));
            loginButton.Click();

            // 2. Bejelentkezes sikerenek ellenorzese

            // URL ellenorzes - bejelentkezes utan inventory oldal
            Assert.IsTrue(driver.Url.Contains("/inventory.html"),
                "A bejelentkezes utan nem a varakozott inventory oldalra erkeztunk.");

            // Fejlec szovegenek ellenorzese (ClassName lekerdezes)
            var pageTitle = driver.FindElement(By.ClassName("title"));
            Assert.AreEqual("Products", pageTitle.Text,
                "A fejlec szovege nem a varakozott 'Products'.");

            // 3. Termek kosarba helyezese

            // Az elso termeket kosarba tesszuk (CssSelector lekerdezes)
            var addToCartButton = driver.FindElement(
                By.CssSelector("button[id^='add-to-cart']"));
            addToCartButton.Click();

            // 4. Kosar badge ellenorzese

            // A kosar ikonon megjelenik egy szam (1), ezt ellenorizzuk
            var cartBadge = wait.Until(
                ExpectedConditions.ElementIsVisible(By.ClassName("shopping_cart_badge")));
            Assert.AreEqual("1", cartBadge.Text,
                "A kosar badge nem 1-et mutat a termek hozzaadasa utan.");

            // 5. Kosar oldalra navigalas es ellenorzes

            // Klikk a kosar ikonra (XPath lekerdezes)
            var cartLink = driver.FindElement(By.XPath("//a[@class='shopping_cart_link']"));
            cartLink.Click();

            // A kosar oldalon ellenorizzuk a termekek szamat (XPath lekerdezes)
            var cartItems = driver.FindElements(By.XPath("//div[@class='cart_item']"));
            Assert.AreEqual(1, cartItems.Count,
                "A kosarban nem pontosan 1 termek van.");

            // Termek lathatosaganak ellenorzese (Displayed assert)
            var itemName = driver.FindElement(By.ClassName("inventory_item_name"));
            Assert.IsTrue(itemName.Displayed,
                "A termek neve nem latszik a kosarban.");
        }

        //  NEGATIV TESZT
        //  Sikertelen bejelentkezes hibas jelszoval
        [TestMethod]
        public void SikertelenBejelentkezes_HibasJelszo()
        {
            // Helyes felhasznalonev (Id), de hibas jelszo (Name)
            driver.FindElement(By.Id("user-name")).SendKeys(ValidUser);
            driver.FindElement(By.Name("password")).SendKeys("rosszJelszo123");
            driver.FindElement(By.Id("login-button")).Click();

            // Hibauzenet kontener megjelenik (CssSelector lekerdezes)
            var errorContainer = wait.Until(
                ExpectedConditions.ElementIsVisible(
                    By.CssSelector("h3[data-test='error']")));
            Assert.IsTrue(errorContainer.Displayed,
                "A hibauzenet nem jelent meg.");

            // A hibauzenet szovegenek ellenorzese (Contains assert)
            Assert.IsTrue(
                errorContainer.Text.Contains("Username and password do not match"),
                "A hibauzenet nem a varakozott szoveget tartalmazza.");

            // URL ellenorzes - meg mindig a login oldalon kell lennunk
            Assert.IsFalse(driver.Url.Contains("/inventory.html"),
                "Hibas jelszo eseten is bekerultunk az inventory oldalra.");
        }
    }
}