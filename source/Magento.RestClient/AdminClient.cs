﻿using System.Collections.Generic;
using AgileObjects.AgileMapper;
using Magento.RestClient.Exceptions;
using Magento.RestClient.Repositories;
using Magento.RestClient.Repositories.Abstractions;
using Magento.RestClient.Search;
using Magento.RestClient.Search.Abstractions;
using RestSharp;

namespace Magento.RestClient
{
	internal class AdminClient : IAdminClient
	{
		private readonly IRestClient _client;

		public AdminClient(IRestClient client)
		{
			this._client = client;
		}


		public IBulkRepository Bulk => new BulkRepository(_client);
		public ISearchService Search => new SearchService(_client);
		public IStoreRepository Stores => new StoreRepository(_client);
		public IProductRepository Products => new ProductRepository(_client);
		public IProductMediaRepository ProductMedia => new ProductMediaRepository(_client);
		public IConfigurableProductRepository ConfigurableProducts => new ConfigurableProductRepository(_client);
		public IOrderRepository Orders => new OrderRepository(_client);
		public ICustomerRepository Customers => new CustomerRepository(_client);
		public ICustomerGroupRepository CustomerGroups => new CustomerGroupRepository(_client);
		public IDirectoryRepository Directory => new DirectoryRepository(_client);
		public IAttributeSetRepository AttributeSets => new AttributeSetRepository(_client);
		public IInvoiceRepository Invoices => new InvoiceRepository(_client);
		public ICategoryRepository Categories => new CategoryRepository(_client);
		public ICartRepository Carts => new CartRepository(_client);
		public IAttributeRepository Attributes => new AttributeRepository(_client);
		public IShipmentRepository Shipments => new ShipmentRepository(_client);

		///<inheritdoc cref="ICanGetModules"/>
		public List<string> GetModules()
		{
			var request = new RestRequest("modules");


			var response = _client.Execute<List<string>>(request);
			if (response.IsSuccessful)
			{
				return response.Data;
			}
			else
			{
				throw MagentoException.Parse(response.Content);
			}
		}
	}
}