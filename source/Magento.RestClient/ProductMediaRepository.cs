﻿using System.Collections.Generic;
using Magento.RestClient.Repositories.Abstractions;
using RestSharp;

namespace Magento.RestClient
{
    internal class ProductMediaRepository : IProductMediaRepository
    {
        private readonly IRestClient _client;

        public ProductMediaRepository(IRestClient client)
        {
            this._client = client;
        }

        public void Create(string sku, ProductMedia entry)
        {
            throw new System.NotImplementedException();
        }

        public List<ProductMedia> GetForSku(string sku)
        {
            throw new System.NotImplementedException();
        }

        public void Delete(string sku, int entryId)
        {
            throw new System.NotImplementedException();
        }

        public ProductMedia Get(string sku, int entryId)
        {
            throw new System.NotImplementedException();
        }

        public ProductMedia Update(string sku, int entryId, ProductMedia entry)
        {
            throw new System.NotImplementedException();
        }
    }
}