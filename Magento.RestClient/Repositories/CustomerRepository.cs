﻿using System;
using System.Linq;
using FluentValidation;
using Magento.RestClient.Exceptions;
using Magento.RestClient.Models;
using Magento.RestClient.Repositories.Abstractions;
using Magento.RestClient.Repositories.Abstractions.Customers;
using Magento.RestClient.Search;
using Magento.RestClient.Search.Extensions;
using Magento.RestClient.Validators;
using RestSharp;
using Serilog;

namespace Magento.RestClient.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IRestClient _client;
        private readonly CustomerValidator _customerValidator;

        public CustomerRepository(IRestClient client)
        {
            this._client = client;
            this._customerValidator = new CustomerValidator();
        }

        public Customer GetByEmailAddress(string emailAddress)
        {
            var results = _client.Search().Customers(builder =>
                builder.Where(customer => customer.Email, SearchCondition.Equals, emailAddress));


            if (!results.Items.Any())
            {
                Log.Warning("Customer by {emailAddress} was not found.", emailAddress);
                return null;
            }
            else
            {
                return results.Items.Single();
            }
        }

        public Customer GetById(long customerId)
        {
            var request = new RestRequest("customers/{id}");

            request.Method = Method.GET;

            request.AddOrUpdateParameter("id", customerId, ParameterType.UrlSegment);
            var response = _client.Execute<Customer>(request);
            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                if (response.ErrorException != null)
                {
                    throw response.ErrorException;
                }
                else
                {
                    throw MagentoException.Parse(response.Content);
                }
            }
        }

        public ValidationResult Validate(Customer customer)
        {
            throw new System.NotImplementedException();
        }

        public Address GetBillingAddress(long customerId)
        {
            throw new System.NotImplementedException();
        }

        public Address GetShippingAddress(long customerId)
        {
            throw new System.NotImplementedException();
        }

        public Customer Create(Customer customer, string password)
        {
            _customerValidator.ValidateAndThrow(customer);
            var request = new RestRequest("customers");
            request.Method = Method.POST;
            request.AddJsonBody(new {customer, password});
            var response = _client.Execute<Customer>(request);

            if (response.IsSuccessful)
            {
                return response.Data;
            }
            else
            {
                throw MagentoException.Parse(response.Content);
            }
        }

        public void DeleteById(long id)
        {
            var request = new RestRequest("customers/{id}");

            request.Method = Method.DELETE;

            request.AddOrUpdateParameter("id", id, ParameterType.UrlSegment);
            _client.Execute(request);
        }

        public Customer GetOwnCustomer()
        {
            throw new System.NotImplementedException();
        }

        public Customer UpdateOwnCustomer(Customer me)
        {
            _customerValidator.ValidateAndThrow(me);

            throw new System.NotImplementedException();
        }
    }
}