using StoreModels.Models;

namespace StoreMVC.Models
{
    public class ProductCartItemVM
    {
        public mProduct Product { get; set; }
        public mShoppingCartItem CartItem { get; set; }
    }
}
