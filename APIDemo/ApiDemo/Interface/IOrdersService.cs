using Model.Dto.Orders;
using Model.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interface
{
    public interface IOrdersService
    {
        bool Add(OrdersAdd role, long userId, string userNickName);
        bool Edit(OrdersEdit role, long userId);
        bool Del(long id);
        bool BatchDel(string ids);
        PageInfo GetOrders(OrdersReq req);
        OrdersRes GetOrdersById(long id);
    }
}

