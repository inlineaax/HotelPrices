using HotelPrices.Models.Services;
using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    private readonly IWebScraper _webScraper;

    public HomeController(IWebScraper webScraper) => _webScraper = webScraper;

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Search(DateTime checkIn, DateTime checkOut, int adults)
    {
        try
        {
            var hotelPrices = await _webScraper.ScrapeHotelPricesAsync(checkIn, checkOut, adults);
            return View("Index", hotelPrices);
        }
        catch (ArgumentException ex)
        {
            ViewBag.ErrorMessage = ex.Message;
            return View("Index");
        }
    }
}
