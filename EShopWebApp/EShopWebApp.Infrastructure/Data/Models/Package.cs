using EShopWebApp.Infrastructure.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShopWebApp.Infrastructure.Data.Models
{
	public class Package
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public string Name { get; set; } = null!;
		public string Description { get; set; } = null!;
		public bool IsDeleted { get; set; }
		public Guid? ProductId { get; set; } 

		public Product Product { get; set; } = null!;
		
	}
}
