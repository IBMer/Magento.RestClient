﻿using System.Collections.Generic;
using Magento.RestClient.Repositories.Abstractions.Customers;

namespace Magento.RestClient.Repositories.Abstractions
{
    public interface IAttributeSetRepository : IReadAttributeSetRepository, IWriteAttributeSetRepository
    {
        void Delete(long attributeSetId);
    }
}