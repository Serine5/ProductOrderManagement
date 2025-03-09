using DAL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ProductOrderManagement.Pages
{
    public class ProductsModel : PageModel
    {
        private readonly IHttpClientFactory _clientFactory;
        public List<Product>? Products { get; set; }

        public ProductsModel(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task OnGetAsync()
        {
            var client = _clientFactory.CreateClient("ApiClient");

            Products = await client.GetFromJsonAsync<List<Product>>("api/products");
        }
        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Category { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
        }

    }
}
