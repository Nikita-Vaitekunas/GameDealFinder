
namespace GameDealFinder.Models;

public class Deal
{
    public string DealID { get; set; }
    public string Title { get; set; }
    public string StoreID { get; set; }
    public string GameID { get; set; }
    public string SalePrice { get; set; }
    public string NormalPrice { get; set; }
    public string Savings { get; set; }
    public string Thumb { get; set; }
}
