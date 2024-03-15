namespace EShopWebApp.Core.ViewModels.CartViewModels
{
    public class CartViewModel
    {
        public Guid Id { get; set; } = new Guid();
        public Guid? UserId { get; set; }
        public Guid? SessionId { get; set; }
        public decimal TotalPrice { get; set; }
        
        public IEnumerable<ShoppingCartItemViewModel> ShoppingCartItems { get; set; } = new List<ShoppingCartItemViewModel>();
       
    }
}
