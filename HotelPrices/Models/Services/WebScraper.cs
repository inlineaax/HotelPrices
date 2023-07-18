using HotelPrices.Models.DTO;
using HotelPrices.Models.Services;
using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;

public class WebScraper : IWebScraper
{
    private readonly IConfiguration _configuration;
    private readonly string? _baseUrl;
    private readonly IHotelService _hotelService;

    public WebScraper(IHotelService hotelService, IConfiguration configuration)
    {
        _configuration = configuration;
        _baseUrl = _configuration.GetSection("WebScraperSettings:BaseUrl").Value;
        _hotelService = hotelService;        
    }

    public async Task<List<HotelDTO>> ScrapeHotelPricesAsync(DateTime checkIn, DateTime checkOut, int adults)
    {
        if (checkOut <= checkIn || (checkOut - checkIn).Days > 7)
        {
            throw new ArgumentException("Check-out deve ser maior que o check-in e a diferença de dias não pode ultrapassar 7 dias.");
        }

        // Verifica se os dados já existem no db, se sim, retorna dados do db
        List<HotelDTO> hotelPricesFromDatabase = await _hotelService.GetHotelPricesAsync(checkIn, checkOut);
        if (hotelPricesFromDatabase.Count > 0)
        {
            return hotelPricesFromDatabase;
        }

        // Dados não encontrados no banco de dados, realizar o scraping
        var queryString = $"checkin={checkIn:d/M/yyyy}&checkout={checkOut:d/M/yyyy}&cidade=&hotel=14&adultos={adults}&criancas=&destino=Hotel+QA+Absoluto&promocode=&tarifa=&mesCalendario=";
        var url = $"{_baseUrl}?{queryString}";

        var options = new ChromeOptions();
        options.AddArgument("headless"); // Modo headless
        options.AddArgument("no-sandbox"); // Evitar erro no Linux

        using (var driver = new ChromeDriver(options))
        {
            driver.Navigate().GoToUrl(url);

            // Aguardar um tempo para que a página seja completamente carregada
            await Task.Delay(TimeSpan.FromSeconds(5));

            var content = driver.PageSource;

            var document = new HtmlDocument();
            document.LoadHtml(content);

            var hotelPrices = new List<HotelDTO>();

            // Extraindo dados
            var roomNodes = document.DocumentNode.SelectNodes("//tr[contains(@class, 'row-quarto')]");
            var tarifaNodes = document.DocumentNode.SelectNodes("//tr[contains(@class, 'row-tarifa')]");
            if (roomNodes != null)
            {
                foreach (var roomNode in roomNodes)
                {
                    var roomName = roomNode.SelectSingleNode(".//span[contains(@class, 'quartoNome')]").InnerText;
                    var maxAdults = int.Parse(roomNode.SelectSingleNode(".//div[contains(@class, 'adultos')]").GetAttributeValue("data-n", "0"));

                    foreach (var tarifaNode in tarifaNodes)
                    {
                        var conditionNode = tarifaNode.SelectSingleNode(".//li[contains(@class, 'nomeTarifa')]");
                        var condition = conditionNode?.InnerText.Trim();
                        var averagePricePerNight = decimal.Parse(tarifaNode.SelectSingleNode(".//span[contains(@class, 'valorFinal')]").InnerText.Replace("R$", ""));
                        var totalPrice = decimal.Parse(tarifaNode.SelectSingleNode(".//span[contains(@class, 'valorFinal')]").InnerText.Replace("R$", ""));

                        var checkInDateTimeOffset = new DateTimeOffset(checkIn, TimeSpan.Zero);
                        var checkOutDateTimeOffset = new DateTimeOffset(checkOut, TimeSpan.Zero);

                        var hotelPrice = new HotelDTO
                        {
                            RoomName = roomName,
                            MaxAdults = maxAdults,
                            Condition = condition,
                            AveragePricePerNight = averagePricePerNight,
                            TotalPrice = totalPrice * (checkOut - checkIn).Days,
                            CheckIn = checkInDateTimeOffset,
                            CheckOut = checkOutDateTimeOffset,
                        };

                        hotelPrices.Add(hotelPrice);
                    }                    
                }
            }

            // Salvando os dados no banco de dados
            await _hotelService.SaveHotelPricesAsync(hotelPrices);

            return hotelPrices;
        }
    }
}

