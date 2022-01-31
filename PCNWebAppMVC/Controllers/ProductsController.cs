using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using Newtonsoft.Json;
using PCNApi.Models;
using PCNWebAppMVC.Models;

namespace PCNWebAppMVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public ProductsController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        Service.PCNApi _pcnApi = new Service.PCNApi();

        // GET: ProductsController
        public async Task<IActionResult> Index(int CurrentPage = 1, int pageSize = 10)
        {
            var productResponse = new PaginationModel();
            HttpClient client = _pcnApi.Initial();
            HttpResponseMessage response =
                await client.GetAsync($"api/Products/GetPaginatedProducts/{CurrentPage}/{pageSize}");
            if (!response.IsSuccessStatusCode) return View(productResponse);

            var results = response.Content.ReadAsStringAsync().Result;
            productResponse.Products = JsonConvert.DeserializeObject<IEnumerable<Product>>(results);

            HttpResponseMessage countHttpResponseMessage = await client.GetAsync("api/Products/GetAllProductsCount");
            if (response.IsSuccessStatusCode)
            {
                var res = countHttpResponseMessage.Content.ReadAsStringAsync().Result;
                productResponse.Count = JsonConvert.DeserializeObject<int>(res);
            }
            else
            {
                productResponse.Count = 10;
            }

            productResponse.CurrentPage = CurrentPage;
            return View(productResponse);
        }

        // GET: ProductsController/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var product = new Product();
            HttpClient client = _pcnApi.Initial();
            HttpResponseMessage response = await client.GetAsync($"api/Products/GetProductById/{id}");
            if (!response.IsSuccessStatusCode) return View("NotFound");
            var results = response.Content.ReadAsStringAsync().Result;
            product = JsonConvert.DeserializeObject<Product>(results);
            return View(product);
        }

        // GET: ProductsController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            HttpClient client = _pcnApi.Initial();
            HttpResponseMessage response = await client.GetAsync($"api/Products/GetProductById/{id}");
            if (!response.IsSuccessStatusCode) return View("NotFound");
            var results = response.Content.ReadAsStringAsync().Result;
            var product = JsonConvert.DeserializeObject<Product>(results);
            return View(product);

        }

        // POST: ProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(product);
                }
                HttpClient client = _pcnApi.Initial();
                HttpResponseMessage response = await client.GetAsync($"api/Products/UpdateProduct/{product}");
                if (!response.IsSuccessStatusCode) return View("NotFound");
                return await Task.Run<ActionResult>(() =>
                {
                    if (true)
                    {
                        return RedirectToAction("Index", "Products");
                    }
                    else
                    {
                        return View("Index");
                    }
                });
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductsController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            HttpClient client = _pcnApi.Initial();
            HttpResponseMessage response = await client.GetAsync($"api/Products/GetProductById/{id}");
            if (!response.IsSuccessStatusCode) return View("NotFound");
            var results = response.Content.ReadAsStringAsync().Result;
            var product = JsonConvert.DeserializeObject<Product>(results);
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            HttpClient client = _pcnApi.Initial();
            HttpResponseMessage response = await client.GetAsync($"api/Products/DeleteProductById/{id}");
            if (!response.IsSuccessStatusCode) return View("NotFound");
            return await Task.Run<ActionResult>(() =>
            {
                if (true)
                {
                    return RedirectToAction("Index", "Products");
                }
            });
        }
    }
}
