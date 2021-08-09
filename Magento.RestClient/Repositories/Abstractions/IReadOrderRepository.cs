﻿using Magento.RestClient.Models;

namespace Magento.RestClient.Repositories.Abstractions
{
    public interface IReadOrderRepository
    {
        SearchResponse<Order> Search();
        Order CreateOrder(Order order);
        Order GetOrderById(int orderId);
    }
}