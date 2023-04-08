using StoreModels.Models;

namespace StoreMVC.Models
{
    public class ProductsCategoriesVM
    {
        public IEnumerable<mProductCategory> Categories { get; set; }
        public IEnumerable<mProduct> Products { get; set; }
    }
}
