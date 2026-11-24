namespace GameDealFinder.Models;

public class GamePriceHistory
{
    public CheapestPriceEver CheapestPriceEver { get; set; }
}

public class CheapestPriceEver
{
    public string Price { get; set; }
    public long Date { get; set; }
}
