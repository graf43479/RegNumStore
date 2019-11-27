using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Abstract;
using Domain.Entities;

namespace Domain.Concrete
{
    public class EFProductRepository : IProductRepository
    {
        private RegNumDBContext context;

        public EFProductRepository(RegNumDBContext context)
        {
            this.context = context;
        }

        public IQueryable<Product> Products {
            get {  return  context.Products.Include(x=>x.Categories).Include(x=>x.User).Include(x=>x.Region);} 
        }

        public void SaveProduct(Product product)
        {
            if (product.ProductID == 0)
            {
                context.Products.Add(product);
            }
            else
            {
                context.Entry(product).State = EntityState.Modified;
            }

            context.SaveChanges();
        }

        public void DeleteProduct(Product product)
        {
                try
                {
                    context.Products.Remove(product);
                    context.SaveChanges();
                }
                catch (Exception)
                {
                    throw;
                }
        }

        
        public Product GetProductOrigin(Product product)
        {
            context.Entry(product).State = EntityState.Detached;
            return product;
        }

        public void AddProductCategory(Product product, Category category)
        {
           // product.Categories.Add(category);
          //  category.Products.Add(product);
            context.SaveChanges();
        }

        public void InsertWithData(Product product, Category category)
        {
            throw new NotImplementedException();
        }

        public Category FindID(int id)
        {
            return context.Categories.Find(id);
        }




        public async Task SaveProductAsync(Product product)
        {
            if (product.ProductID == 0)
            {
                context.Products.Add(product);
            }
            else
            {
                context.Entry(product).State = EntityState.Modified;
            }

            await context.SaveChangesAsync();
        }

        //public void InsertWithData(int productId, int categoryId)
        //{
            
        //    if (b)
        //    {
                
        //    }
            
        //}	
    }
}
