using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using AzureFunctions.Controllers;
using System.Net.Http;
using ProjectRest.Models;

namespace AzureFunctions
{
    public static class AzureFunctions
    {
        /*[FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }*/


    }

    #region Category
    public static class Categories
    {
        [FunctionName("Categories")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, nameof(HttpMethods.Get), Route = null)] HttpRequest req,
            ILogger log)
        {

            log.LogInformation("C# HTTP trigger function processed a request.");
            ObjectResult result;
            string responseMessage;
            //string name = default;

            using (CategoryController _categoryController = new CategoryController())
            {
                responseMessage = "Hey...";
                var lis = await _categoryController.GetAll();
                result = new OkObjectResult(lis);
            }

            return result;
        }

    }

    public static class InsertCategory
    {

        [FunctionName("CategoryInsertOrUpdate")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, nameof(HttpMethods.Post), Route = null)] HttpRequestMessage req,
            ILogger log)
        {

            log.LogInformation("C# HTTP trigger function processed a request.");

            var category = await req.Content.ReadAsAsync<Category>();

            ObjectResult result;
            string responseMessage = default;


            if (string.IsNullOrEmpty(category.Name))
            {

                responseMessage = "Please...";

                result = new BadRequestObjectResult(responseMessage);
            }
            else
            {
                responseMessage = "Hey...";

                using (CategoryController _categoryController = new CategoryController())
                {
                    await _categoryController.Post(category);

                }
                result = new OkObjectResult(responseMessage);

            }
            return result;

        }
    }

    public static class CategoryById
    {

        [FunctionName("Category")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, nameof(HttpMethods.Get), Route = null)] HttpRequest req,
            ILogger log)
        {

            log.LogInformation("C# HTTP trigger function processed a request.");
            ObjectResult result = default;
            var idReq = req.Query["id"];
            //string name = default;
            var id = int.Parse(idReq);
            using (CategoryController c = new CategoryController())
            {
                if (id > 0)
                {
                    var category = await c.GetById(id);
                    result = new OkObjectResult(category);
                }


            }
            return result;

        }

    }

    public static class DeleteCategory
    {

        [FunctionName("DeleteCategory")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, nameof(HttpMethods.Delete), Route = null)] HttpRequest req,
            ILogger log)
        {

            log.LogInformation("C# HTTP trigger function processed a request.");
            ObjectResult result = default;
            string responseMessage = default;
            var id = req.Query["id"];
            //string name = default;
            var idd = int.Parse(id);
            using (CategoryController c = new CategoryController())
            {
                if (idd > 0)
                {
                    var category = c.DeleteCategory(idd);
                    responseMessage = "Deletado com sucesso";
                    result = new OkObjectResult(responseMessage);
                }


            }
            return result;

        }

    }
    #endregion Category


    #region Product
    public static class Products
    {
        [FunctionName("Products")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, nameof(HttpMethods.Get), Route = null)] HttpRequest req,
            ILogger log)
        {

            log.LogInformation("C# HTTP trigger function processed a request.");
            ObjectResult result;
            string responseMessage;
            //string name = default;

            using (ProductController _productController = new ProductController())
            {
                responseMessage = "Hey...";
                var lis = await _productController.GetAll();
                result = new OkObjectResult(lis);
            }

            return result;
        }

    }

    public static class InsertorUpdateProduct
    {

        [FunctionName("Product")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, nameof(HttpMethods.Post), Route = null)] HttpRequestMessage req,
            ILogger log)
        {

            log.LogInformation("C# HTTP trigger function processed a request.");

            var product = await req.Content.ReadAsAsync<Product>();

            ObjectResult result;
            string responseMessage = default;


            if (string.IsNullOrEmpty(product.Name))
            {

                responseMessage = "Please...";

                result = new BadRequestObjectResult(responseMessage);
            }
            else
            {
                responseMessage = "Hey...";

                using (ProductController _productController = new ProductController())
                {
                    await _productController.Post(product);

                }
                result = new OkObjectResult(responseMessage);

            }
            return result;

        }
    }

    public static class ProductById
    {

        [FunctionName("productById")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, nameof(HttpMethods.Get), Route = null)] HttpRequest req,
            ILogger log)
        {

            log.LogInformation("C# HTTP trigger function processed a request.");
            ObjectResult result = default;
            var idReq = req.Query["id"];
            //string name = default;
            var id = int.Parse(idReq);
            using (ProductController c = new ProductController())
            {
                if (id > 0)
                {
                    var category = await c.GetById(id);
                    result = new OkObjectResult(category);
                }


            }
            return result;

        }

    }

    public static class DeleteProduct
    {

        [FunctionName("DeleteProduct")]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, nameof(HttpMethods.Delete), Route = null)] HttpRequest req,
            ILogger log)
        {

            log.LogInformation("C# HTTP trigger function processed a request.");
            ObjectResult result = default;
            string responseMessage = default;
            var idReq = req.Query["id"];
            //string name = default;
            var id = int.Parse(idReq);
            using (ProductController c = new ProductController())
            {
                if (id > 0)
                {
                    var category = c.DeleteProduct(id);
                    responseMessage = "Deletado com sucesso";
                    result = new OkObjectResult(responseMessage);
                }


            }
            return result;

        }

    }
    #endregion
}
