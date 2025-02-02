﻿using System.Threading.Tasks;
using Magento.RestClient.Abstractions.Abstractions;
using Magento.RestClient.Abstractions.Repositories.Customers;
using Magento.RestClient.Data.Models.Common;
using Magento.RestClient.Data.Models.Customers;

namespace Magento.RestClient.Abstractions.Repositories
{
	public interface ICustomerRepository : IOwnCustomerRepository, IHasQueryable<Customer>
	{
		Customer GetByEmailAddress(string emailAddress);
		Task<Customer> Create(Customer customer, string password = null);
		Task DeleteById(long id);
		Task<Customer> GetById(long customerId);
		Task<ValidationResult> Validate(Customer customer);
		Task<Address> GetBillingAddress(long customerId);
		Task<Address> GetShippingAddress(long customerId);
		Task<Customer> Update(long id, Customer customer);
	}
}