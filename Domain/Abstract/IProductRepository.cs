﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Abstract
{
    public interface IProductRepository
    {
        IQueryable<Product> Products { get; }

        void SaveProduct(Product product);

        void DeleteProduct(Product product);

        void AddProductCategory(Product product, Category category);

        //void InsertWithData(int productId, int categoryId);
        void InsertWithData(Product product, Category category);

        Category FindID(int id);

        //void RefreshAllShortNames();

        //void RefreshProductShortName(Product product);

        //string GetShortName(string name, int maxID);

        //void UpdateProductSequence(int categoryId, bool every);

        //void ProductSequence(int productId, string actionType);

        //void SetActiveStatus(bool isActive, Product product);

        //void SetDeletedStatus(bool isDeleted, Product product);

        //void RefreshEveryProductSequence(int[] categoryIdArray);

        Product GetProductOrigin(Product product);

        Task SaveProductAsync(Product product);
    }
}
