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
    /*public static class AzureFunctions
    {
        [FunctionName("Function1")]
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
        }


    }*/

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

                var listCategories = _categoryController.GetAll().Result;
                if (listCategories.Value.Count > 0)
                {
                    result = new OkObjectResult(listCategories);
                }
                else
                {
                    responseMessage = "Erro ao carregar Categorias";
                    result = new NotFoundObjectResult(responseMessage);
                }
                
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

                responseMessage = "";

                result = new BadRequestObjectResult(responseMessage);
            }
            else
            {

                using (CategoryController _categoryController = new CategoryController())
                {
                    var resp = _categoryController.Post(category).Result;
                    if (resp.Value.Id > 0)
                    {
                        responseMessage = $"Categoria {resp.Value.Name} cadastrada com sucesso";
                    }
                    else
                    {
                        responseMessage = "Erro ao cadastrar categoria";
                    }
                    
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
                    var category = c.GetById(id).Result;
                    if(category.Value.Id > 0)
                    {
                        result = new OkObjectResult(category);
                    }
                    else
                    {
                        result = new NotFoundObjectResult("Categoria não encontrada");
                    }

                    
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
            var idReq = req.Query["id"];
            var id = int.Parse(idReq);
            using (CategoryController _categoryController = new CategoryController())
            {
                if (id > 0)
                {
                    var category = _categoryController.DeleteCategory(id);
                    if (category.Status.Equals("200"))
                    {
                        responseMessage = "Categoria deletada com sucesso";
                        result = new OkObjectResult(responseMessage);
                    }
                    else
                    {
                        responseMessage = "Erro ao deletar Categoria";
                        result = new NotFoundObjectResult(responseMessage);
                    }
                    
                }
                else
                {
                    responseMessage = "Erro ao deletar Categoria";
                    result = new NotFoundObjectResult(responseMessage);
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
                
                var listProducts = _productController.GetAll().Result;
                if(listProducts.Value.Count > 0)
                {
                    result = new OkObjectResult(listProducts);
                }
                else
                {
                    responseMessage = "Erro ao carregar Produtos";
                    result = new ObjectResult(responseMessage);
                }
                
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

            ObjectResult result = default;
            string responseMessage = default;
            if(product.Id <= 0)
            {
                if (string.IsNullOrEmpty(product.Name) || product.Price <= 0 || product.CategoryId <=0)
                {
                    responseMessage = "Os campos Name, Price e CategoryId são obrigatórios";
                    result = new BadRequestObjectResult(responseMessage);
                }
            }
            else
            {
                using (ProductController _productController = new ProductController())
                {
                    var prod = _productController.Post(product).Result;
                    if (prod.Value.Id > 0)
                    {
                        result = new OkObjectResult(prod);
                    }
                    else
                    {
                        responseMessage = "Não foi possível cadastrar/editar produto";
                        result = new BadRequestObjectResult(responseMessage);
                    }
                }
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
            using (ProductController prod = new ProductController())
            {
                if (id > 0)
                {
                    var produto = prod.GetById(id).Result;
                    if (produto.Value.Id > 0)
                    {
                        result = new OkObjectResult(produto);
                    }
                    else
                    {
                        result = new BadRequestObjectResult("Produto não encontrar");
                    }
                    
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
            using (ProductController prod = new ProductController())
            {
                if (id > 0)
                {
                    var category = prod.DeleteProduct(id);
                    if(category.Status.Equals("200"))
                    {
                        responseMessage = "Deletado com sucesso";
                        result = new OkObjectResult(responseMessage);
                    }
                    else
                    {
                        responseMessage = "Erro ao deletar Produto";
                        result = new OkObjectResult(responseMessage);
                    }
                }
                else
                {
                    responseMessage = "Erro ao deletar Produto";
                    result = new OkObjectResult(responseMessage);
                }


            }
            return result;

        }

    }
    #endregion Product
}
