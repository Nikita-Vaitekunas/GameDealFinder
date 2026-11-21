
namespace GameDealFinder.Models;

public class Store
{
    public string storeID { get; set; }
    public string storeName { get; set; }
    public StoreImages images { get; set; }
}

public class StoreImages
{
    public string banner { get; set; }
    public string logo { get; set; }
    public string icon { get; set; }
}
