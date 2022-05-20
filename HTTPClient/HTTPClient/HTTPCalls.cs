using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HTTPClient
{
    class HTTPCalls
    {
        HttpClient client = new HttpClient();

        public async Task GetCategories()
        {
            var result = await client.GetAsync("https://localhost:44370/api/Category/categories");

            if (PrintIfInsuccessCode(result) == -1)
            {
                return;
            }

            var callData = await result.Content.ReadAsStringAsync();

            JsonConvert.DeserializeObject<List<CategoryData>>(callData);

            Console.WriteLine("List of categories: ");
            Console.WriteLine(callData);
        }

        public async Task PostCategory(string title)
        {
            var category = new CategoryData() { Title = title};
            var categorySerialized = System.Text.Json.JsonSerializer.Serialize(category);
            var stringContent = new StringContent(categorySerialized, Encoding.UTF8, "application/json");
            string url = "https://localhost:44370/api/Category/categories";
            var result = await client.PostAsync(url, stringContent);

            if(PrintIfInsuccessCode(result) == -1)
            {
                return;
            }

            Console.WriteLine("Category with name " + title + " was succesfully posted!");
        }

        public async Task DeleteCategory(int id)
            {
            string url = "https://localhost:44370/api/Category/categories/" + id;
            var result = await client.DeleteAsync(url);
            if (PrintIfInsuccessCode(result) == -1)
            {
                return;
            }

            Console.WriteLine("Category with id " + id + " is succesfully deleted");
        }

        public async Task GetCategory(int id)
        {
            string url = "https://localhost:44370/api/Category/categories/" + id;
            var result = await client.GetAsync(url);

            if (PrintIfInsuccessCode(result) == -1)
            {
                return;
            }

            var callData = await result.Content.ReadAsStringAsync();

            JsonConvert.DeserializeObject<List<CategoryData>>(callData);

            Console.WriteLine($"Category with id {id}: ");
            Console.WriteLine(callData);
        }

        public async Task GetCategory(string name)
        {
            string urlSearch = "https://localhost:44370/api/Category/categories/search?categoryName=" + name;
            var resultId = await client.GetAsync(urlSearch);

            string urlGet = "https://localhost:44370/api/Category/categories/" + resultId.Content.ReadAsStringAsync().Result;
            
            var resultCategory = await client.GetAsync(urlGet);

            if (PrintIfInsuccessCode(resultCategory) == -1)
            {
                return;
            }

            var callData = await resultCategory.Content.ReadAsStringAsync();

            JsonConvert.DeserializeObject<List<CategoryData>>(callData);

            Console.WriteLine($"Category with name {name}: ");
            Console.WriteLine(callData);
        }

        public async Task PutCategoryTitle(int id, string name)
        {
            var category = new CategoryData() { Title = name };
            var categorySerialized = System.Text.Json.JsonSerializer.Serialize(category);
            var stringContent = new StringContent(categorySerialized, Encoding.UTF8, "application/json");

            string url = "https://localhost:44370/api/Category/" + id;
            var result = await client.PutAsync(url, stringContent);

            if (PrintIfInsuccessCode(result) == -1)
            {
                return;
            }

            Console.WriteLine($"Category with id {id} is succesfully modified!");
        }

        public async Task PostProducts(string title, double price, int categoryId)
        {
            var product = new ProductData() { Title = title, Price=price, CategoryId = categoryId };
            var productSerialized = System.Text.Json.JsonSerializer.Serialize(product);
            var stringContent = new StringContent(productSerialized, Encoding.UTF8, "application/json");

            string url = "https://localhost:44370/api/Category/categories/" + categoryId + "/products";
            var result = await client.PostAsync(url, stringContent);

            if (PrintIfInsuccessCode(result) == -1)
            {
                return;
            }

            Console.WriteLine($"Product with category id {categoryId}, title {title} and price {price} was succesfully posted!");
        }

        public async Task GetProductsFromCategory(int id)
        {
            string url = "https://localhost:44370/api/Category/categories/" + id + "/products";
            var result = await client.GetAsync(url);

            if (PrintIfInsuccessCode(result) == -1)
            {
                return;
            }

            var callData = await result.Content.ReadAsStringAsync();

            JsonConvert.DeserializeObject<List<CategoryData>>(callData);

            Console.WriteLine($"Products from category with id {id}: ");
            Console.WriteLine(callData);
        }

        //Helper method
        public int PrintIfInsuccessCode(HttpResponseMessage result)
        {
            if(result.IsSuccessStatusCode)
            {
                return 0;
            }
            Console.WriteLine("Error because of return code " + result.StatusCode);
            return -1;
        }
    }
}
