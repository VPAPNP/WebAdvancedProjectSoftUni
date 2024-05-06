using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShopWebApp.Core.Contracts
{
	public interface IEcontService
	{
		Task<string[]> GetCountriesAsync();
	}
}
