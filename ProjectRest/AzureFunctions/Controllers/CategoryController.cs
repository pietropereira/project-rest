using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32.SafeHandles;
using ProjectRest.Data;
using ProjectRest.Models;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AzureFunctions.Controllers
{
    public class CategoryController : IDisposable
    {
        private readonly DbContextOptions<DataContext> _optionsContext;

        public CategoryController()
        {
            _optionsContext = new DbContextOptions<DataContext>();
        }

        public async Task<ActionResult<Category>> Post(Category model)
        {
            using (var data = new DataContext(_optionsContext))
            {
                if(model.Id > 0)
                {
                    var category = await data.Categories.FirstOrDefaultAsync(p => p.Id == model.Id);
                    if(category.Id > 0)
                    {
                        category.Name = model.Name;
                        data.Categories.Update(category);
                        data.SaveChanges();
                    }
                    
                }
                else
                {
                    data.Categories.Add(model);
                    await data.SaveChangesAsync();
                }
                
                return model;
            }
        }

        public async Task<ActionResult<List<Category>>> GetAll()
        {
            using (var data = new DataContext(_optionsContext))
            {
                return await data.Categories.ToListAsync();
            }
        }

        public async Task<ActionResult<Category>> GetById(int id)
        {
            using (var data = new DataContext(_optionsContext))
            {
                return await data.Categories.FirstOrDefaultAsync(p => p.Id == id);
            }
        }

        public async Task DeleteCategory(int id)
        {
            using (var data = new DataContext(_optionsContext))
            {
                var category = await data.Categories.FirstOrDefaultAsync(p => p.Id == id);
                data.Categories.Remove(category);
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
