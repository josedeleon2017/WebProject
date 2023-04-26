using StoreModels.Models;

namespace StoreMVC.Models
{
    public class OrderVM
    {
        public mAddress Address { get; set; }
        public IEnumerable<ProductCartItemVM> CartVM { get; set; }
        public mSalesOrderHeader OrderHeader { get; set; }
    }
}
