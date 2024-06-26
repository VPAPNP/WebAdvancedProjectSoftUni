﻿namespace EShopWebApp.Core.ViewModels.CartViewModels
{
    public class ShoppingCartItemViewModel
    {
        public Guid Id { get; set; } = new Guid();
        public Guid ProductId { get; set; }
		public Guid SessionId { get; set; }
		public Guid CartId { get; set; }
        public int Quantity { get; set; }
        public CartProductViewModel Product { get; set; } = new CartProductViewModel();
    }
}