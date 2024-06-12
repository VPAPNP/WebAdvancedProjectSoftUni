using EShopWebApp.Core.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EShopWebApp.Core.Services
{
	public class EcontService : IEcontService
	{
		private readonly HttpClient _httpClient;
		private const string BaseUrl = "http://ee.econt.com/services/";

		public EcontService(HttpClient httpClient)
		{
			_httpClient = httpClient;
			_httpClient.BaseAddress = new Uri(BaseUrl);
		}

		public async Task<string[]> GetCountriesAsync()
		{
			var response = await _httpClient.GetAsync("Nomenclatures/NomenclaturesService.getCountries.json");
			response.EnsureSuccessStatusCode();
			var content = await response.Content.ReadAsStringAsync();

			// Parse the JSON string into a JsonDocument
			using (JsonDocument doc = JsonDocument.Parse(content))
			{
				// Get the root element
				JsonElement root = doc.RootElement;

				// Get the 'countries' array
				if (root.TryGetProperty("countries", out JsonElement countriesElement) && countriesElement.ValueKind == JsonValueKind.Array)
				{
					// Create a list to store country names
					var countryNames = new List<string>();

					// Iterate through the array elements
					foreach (JsonElement countryElement in countriesElement.EnumerateArray())
					{
						// Get the 'name' property of each country object
						if (countryElement.TryGetProperty("name", out JsonElement nameElement) && nameElement.ValueKind == JsonValueKind.String)
						{
							countryNames.Add(nameElement.GetString());
						}
					}

					// Convert the list to an array and return
					return countryNames.ToArray();
				}
				else
				{
					throw new InvalidOperationException("Unable to find 'countries' array in JSON response.");
				}
				// Add methods for other API endpoints as needed
			}
		}
	}
}

