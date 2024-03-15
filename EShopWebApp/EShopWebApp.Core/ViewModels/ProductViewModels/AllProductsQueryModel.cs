using EShopWebApp.Core.ViewModels.ProductViewModels.Enums;
using System.ComponentModel.DataAnnotations;
using static EShopWebApp.Core.DataConstants.GeneralApplicationConstants.Product;

namespace EShopWebApp.Core.ViewModels.ProductViewModels 
{
    public class AllProductsQueryModel
    {
        public string? Category { get; set; }
        [Display(Name = "Search")]
        public string? SearchTerm { get; set; }
        [Display(Name = "Sort By")]
        public ProductSorting ProductSorting { get; set; }
        public int CurrentPage { get; set; } = DefaultPageNumber;
        public int PageSize { get; set; } = ProductsPerPage;
        public int TotalPages => (int)System.Math.Ceiling((double)this.TotalProducts / this.PageSize);
        public int PageCount { get; set; } = 0;
        public int TotalProducts { get; set; }
        public string? Brand { get; set; }


        public IEnumerable<string> Categories { get; set; } = new HashSet<string>();
        public IEnumerable<string> Brands { get; set; } = new HashSet<string>();
        public IEnumerable<AllProductViewForSearch> Products { get; set; } = new HashSet<AllProductViewForSearch>();




    }


}


