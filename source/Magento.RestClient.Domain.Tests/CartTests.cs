using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Magento.RestClient.Domain.Extensions;
using Magento.RestClient.Domain.Models;
using Magento.RestClient.Domain.Tests.Abstractions;
using Magento.RestClient.Exceptions;
using Magento.RestClient.Models.Carts;
using Magento.RestClient.Models.Common;
using NUnit.Framework;

namespace Magento.RestClient.Domain.Tests
{
	public class CartTests : AbstractDomainObjectTest
    {
	    private long existingCartId;
		private ProductModel existingSimpleProduct;

		public static Address ScunthorpePostOffice => new Address() {
            Firstname = "Scunthorpe",
            Lastname = "Post Office",
            Telephone = "+44 1724 843348",
            Company = "Scunthorpe Post Office",
            City = "Scunthorpe",
            Street = new List<string>() {"148 High St"},
            Postcode = "DN15 6EN",
            CountryId = "GB"
        };

        [SetUp]
        public void SetupCart()
        {

	        var cart = new CartModel(Client.Carts);
	        this.existingCartId = cart.Id;


	        this.existingSimpleProduct = new ProductModel(Client, "TESTSKU-SIMPLE-EXISTING");
	        existingSimpleProduct.Name = "TEST - Existing Simple Product";
			existingSimpleProduct.SetStock(30);
	        existingSimpleProduct.Price = 30;
	        existingSimpleProduct.Save();
			
        }

		[TearDown]
        public void TearDownCart()
        {
	        existingSimpleProduct.Delete();

		}

        [Test]
        public void CreateCart()
        {
            var cart = Client.CreateNewCartModel();
            cart.Id.Should().NotBe(0);
        }

        [Test]
        public void GetExistingCart()
        {
	        var cart = Client.GetExistingCartModel(existingCartId);
			
            cart.Id.Should().NotBe(0);
        }

        [Test]
        public void Cart_AssignCustomer_ValidCustomer()
		{
			var cart = new CartModel(Client.Carts);

			cart.AssignCustomer(1);

            cart.Customer.Should().NotBeNull();
            cart.Customer.Email.Should().NotBeNullOrEmpty();
            cart.Customer.Firstname.Should().NotBeNullOrEmpty();
            cart.Customer.Lastname.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void Cart_AssignCustomer_InvalidCustomer()
		{
			var cart = Client.CreateNewCartModel();


			Assert.Throws<EntityNotFoundException>(
                () => {
                    cart.AssignCustomer(-1);

                    cart.Customer.Should().BeNull();
                }
            );
        }


        [Test]
        public void Cart_AddItem_ValidItem()
		{
			var cart = new CartModel(Client.Carts);
			cart.AddItem(existingSimpleProduct.Sku, 3);
            cart.Items.Any(item => item.Sku == existingSimpleProduct.Sku && item.Qty == 3).Should().BeTrue();
        }

        [Test]
        public void Cart_AddItem_InvalidItem()
        {
	        var cart = new CartModel(Client.Carts);

			Assert.Throws<MagentoException>(() => {
                cart.AddItem("DOESNOTEXIST", 3);
            });
        }

        [Test]
        public void ShippingMethods_GetMethods_ShippingAddressSet()
        {
	        var cart = new CartModel(Client.Carts);
	        cart.AddItem(this.existingSimpleProduct.Sku, 3);

            cart.BillingAddress = ScunthorpePostOffice;
            cart.ShippingAddress = ScunthorpePostOffice;
            var shippingMethods = cart.EstimateShippingMethods();

            Assert.IsNotEmpty(shippingMethods);
        }
        [Test]
        public void ShippingMethods_GetMethods_ShippingAddressSet_ItemsEmpty()
        {
			var cart = new CartModel(Client.Carts);
			cart.BillingAddress = ScunthorpePostOffice;
            cart.ShippingAddress = ScunthorpePostOffice;

            Assert.Throws<ArgumentNullException>(() => {
                var shippingMethods = cart.EstimateShippingMethods();

            });

        }
        [Test]
        public void ShippingMethods_GetMethods_ShippingAddressNotSet()
        {
	        var cart = new CartModel(Client.Carts);

			cart.AddItem(this.existingSimpleProduct.Sku, 3);

            Assert.Throws<ArgumentNullException>(() =>
                cart.EstimateShippingMethods());
        }
        [Test]
        public void ShippingMethods_SetShippingMethod_Cheapest()
		{
			var cart = new CartModel(Client.Carts);

			cart.AddItem(this.existingSimpleProduct.Sku, 3);

            cart.ShippingAddress = ScunthorpePostOffice;
            var shippingMethods = cart.EstimateShippingMethods();
            var cheapestShipping = cart.EstimateShippingMethods()
                .OrderByDescending(method => method.PriceInclTax)
                .First();

            cart.SetShippingMethod(cheapestShipping);
        }

        /// <summary>
        /// ShippingMethods_SetShippingMethod_InvalidShippingMethod
        /// </summary>
        /// <exception cref="InvalidOperationException">Ignore.</exception>
        [Test]
        public void ShippingMethods_SetShippingMethod_InvalidShippingMethod()
        {
	        var cart = new CartModel(Client.Carts);

			cart.AddItem(this.existingSimpleProduct.Sku, 3);

            cart.BillingAddress = ScunthorpePostOffice;
            cart.ShippingAddress = ScunthorpePostOffice;

            Assert.Throws<InvalidOperationException>(() => {
                cart.SetShippingMethod("Yodel", "THISISNOTAVALIDSHIPPINGMETHOD");
            });
        }

        [Test]
        public void PaymentMethods_GetMethods()
		{
			var cart = new CartModel(Client.Carts);

			var methods = cart.GetPaymentMethods();
            methods.Should().NotBeNullOrEmpty();
        }

        [Test]
        public void PaymentMethods_SetPaymentMethods_InvalidPaymentMethod()
		{
			var cart = new CartModel(Client.Carts);


			Assert.Throws<InvalidOperationException>(() => {
                cart.SetPaymentMethod("GALLONOFPCP");
            });
        }

        [Test]
        public void PaymentMethods_SetPaymentMethods_ValidPaymentMethod()
		{
			var cart = new CartModel(Client.Carts);

			var paymentMethod = cart.GetPaymentMethods()
                .First();
            cart.SetPaymentMethod(paymentMethod.Code);
        }

        /// <summary>
        /// CommitOrder_ValidOrder
        /// </summary>
        /// <exception cref="InvalidOperationException">Ignore.</exception>
        public void CommitOrder_ValidOrder()
        {
	        var cart = new CartModel(Client.Carts);

			cart.ShippingAddress = ScunthorpePostOffice;
            cart.BillingAddress = ScunthorpePostOffice;

            cart.AddItem("TESTPRODUCT", 3)
                .AddItem("TESTPRODUCT", 3);

            var cheapestShipping = cart.EstimateShippingMethods()
                .OrderByDescending(method => method.PriceInclTax)
                .First();
            var paymentMethod = cart.GetPaymentMethods()
                .First();

            cart.SetPaymentMethod(paymentMethod.Code)
                .SetShippingMethod(cheapestShipping);
            var orderId = cart.Commit();
            orderId.Should().NotBe(0);
        }
    }
}