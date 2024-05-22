namespace EShopWebApp.Core.ViewModels.PhotoViewModels
{
    public class PhotoViewModel
    {
        public string Id { get; set; } =null!;
        public required string Name { get; set; }
        public required byte[] Picture { get; set; }
    }
}
