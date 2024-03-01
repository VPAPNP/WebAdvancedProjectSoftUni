using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShopWebApp.Core.ViewModels.ImageViewModels
{
    public class PhotoViewModel
    {
        public string Id { get; set; } =null!;
        public required string Name { get; set; }
        public required byte[] Picture { get; set; }
    }
}
