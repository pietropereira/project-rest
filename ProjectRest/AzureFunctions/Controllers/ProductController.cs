using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32.SafeHandles;
using ProjectRest.Data;
using ProjectRest.Models;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace AzureFunctions.Controllers
{
    public class ProductController : IDisposable
    {
        private readonly DbContextOptions<DataContext> _optionsContext;

        public ProductController()
        {
            _optionsContext = new DbContextOptions<DataContext>();
        }

        public async Task<ActionResult<Product>> Post(Product model)
        {
            using (var data = new DataContext(_optionsContext))
            {
                if (model.Id > 0)
                {
                    var product = await data.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == model.Id);
                    if (product.Id > 0)
                    {
                        product.Name = model.Name;
                        product.Price = model.Price > 0 ? model.Price : product.Price;
                        product.CategoryId = model.CategoryId > 0 ? model.CategoryId : product.CategoryId;
                        var result = data.Products.Update(product);
                        data.SaveChanges();
                    }

                }
                else
                {
                    data.Products.Add(model);
                    await data.SaveChangesAsync();
                }

                return model;
            }
        }

        public async Task<ActionResult<List<Product>>> GetAll()
        {
            using (var data = new DataContext(_optionsContext))
            {
                return await data.Products.Include(p => p.Category).AsNoTracking().ToListAsync();
            }
        }

        public async Task<ActionResult<Product>> GetById(int id)
        {
            using (var data = new DataContext(_optionsContext))
            {
                return await data.Products.Include(p => p.Category).AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            }
        }

        public async Task DeleteProduct(int id)
        {
            using (var data = new DataContext(_optionsContext))
            {
                var category = await data.Products.FirstOrDefaultAsync(p => p.Id == id);
                data.Products.Remove(category);
                data.SaveChanges();

            }
        }

        #region Disposed https://docs.microsoft.com/pt-br/dotnet/standard/garbage-collection/implementing-dispose
        // Flag: Has Dispose already been called?
        bool disposed = false;
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);


        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                //
            }

            disposed = true;
        }
        #endregion
    }


}
