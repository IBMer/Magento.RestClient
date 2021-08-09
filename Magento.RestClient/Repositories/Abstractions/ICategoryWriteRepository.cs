﻿using System.Collections.Generic;
using Magento.RestClient.Models;

namespace Magento.RestClient.Repositories.Abstractions
{
    public interface ICategoryWriteRepository
    {
        public void DeleteCategoryById(int categoryId);
        public void MoveCategory(int categoryId, int parentId, int? afterId = null);
        public List<ProductLink> GetProducts(int categoryId);
        public void AddProduct(int categoryId, ProductLink productLink);
        public void DeleteProduct(int categoryId, string sku);
        public Category CreateCategory(Category category);
    }
}