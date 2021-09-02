﻿using System.Collections.Generic;
using System.Linq;
using Magento.RestClient.Domain.Abstractions;
using Magento.RestClient.Models.Attributes;
using Magento.RestClient.Models.Products;
using Magento.RestClient.Repositories.Abstractions;

namespace Magento.RestClient.Domain.Models
{
	public class AttributeModel : IDomainModel
	{
		private readonly IAdminClient _client;
		private string _frontendInput;
		private bool _frontendInputChanged;
		private List<Option> _options;

		public AttributeModel(IAdminClient client, string attributeCode)
		{
			_client = client;
			this.AttributeCode = attributeCode;
			Refresh();
		}

		public string AttributeCode { get; }

		public string FrontendInput {
			get => _frontendInput;
			set {
				_frontendInput = value;
				_frontendInputChanged = true;
			}
		}

		public string DefaultFrontendLabel { get; set; }


		public bool IsPersisted { get; private set; }

		public void Refresh()
		{
			var existing = _client.Attributes.GetByCode(this.AttributeCode);

			if (existing == null)
			{
				this.IsPersisted = false;
				_options = new List<Option>();
			}
			else
			{
				this.IsPersisted = true;

				this.DefaultFrontendLabel = existing.DefaultFrontendLabel;
				_frontendInput = existing.FrontendInput;
				_options = _client.Attributes.GetProductAttributeOptions(this.AttributeCode);
			}
		}

		public void Save()
		{
			var existing = _client.Attributes.GetByCode(this.AttributeCode);
			var attribute = new ProductAttribute(this.AttributeCode);

			attribute.FrontendInput = this.FrontendInput;
			if (existing != null && _frontendInputChanged)
			{
				_client.Attributes.DeleteProductAttribute(this.AttributeCode);
			}

			attribute = existing != null
				? _client.Attributes.Update(this.AttributeCode, attribute)
				: _client.Attributes.Create(attribute);

			if (_options.Any())
			{
				var existingOptions = _client.Attributes.GetProductAttributeOptions(this.AttributeCode);

				foreach (var option in _options.Where(option =>
					!existingOptions.Select(option1 => option1.Label).Contains(option.Label)))
				{
					_client.Attributes.CreateProductAttributeOption(this.AttributeCode, option);
				}

				foreach (var option in existingOptions.Where(option =>
					!_options.Select(o => o.Label).Contains(option.Label) && !string.IsNullOrEmpty(option.Value)))
				{
					_client.Attributes.DeleteProductAttributeOption(this.AttributeCode, option.Value);
				}
			}

			Refresh();
		}


		public void AddOption(string option)
		{
			if (option != null)
			{
				if (_options.All(o => o.Label != option))
				{
					_options.Add(new Option {Label = option});
				}
			}
		}
	}
}