# Szoftverteszteles - 2. beadando

Hallgato: Baji Gabor (DISEXN)

## Leiras
Selenium WebDriver alapu UI tesztek a https://www.saucedemo.com/ oldalon.
Microsoft Edge bongeszovel, MSTest keretrendszerben.

A tesztosztaly ket tesztet tartalmaz:
- **SikeresBejelentkezes_KosarbaHelyezes**: pozitiv teszt - bejelentkezes,
  termek kosarba helyezese, kosar tartalmanak ellenorzese
- **SikertelenBejelentkezes_HibasJelszo**: negativ teszt - hibas jelszoval
  valo bejelentkezesi kiserlet, hibauzenet ellenorzese

## Hasznalt technikak
- **Lekerdezesek**: Id, Name, ClassName, XPath, CssSelector
- **Muveletek**: Navigate, SendKeys, Click, WebDriverWait
- **Assertek**: IsTrue, IsFalse, AreEqual, Contains, Displayed
