using AutoMapper;
using Interface;
using Model.Dto.Orders;
using Model.Entitys;
using Model.Other;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class OrdersService : IOrdersService
    {
        private readonly IMapper _mapper;
        private ISqlSugarClient _db { get; set; }
        public OrdersService(IMapper mapper, ISqlSugarClient db)
        {
            _mapper = mapper;
            _db = db;
        }
        public bool Add(OrdersAdd req, long userId, string userNickName)
        {
            Orders info = _mapper.Map<Orders>(req);
            info.NickName = userNickName;
            info.CreateUserId = userId;
            info.CreateDate = DateTime.Now;
            info.IsDeleted = 0;
            info.Price = Convert.ToDecimal(req.Price); // 转换为Decimal类型
            return _db.Insertable(info).ExecuteCommand() > 0;
        }

        public bool Del(long id)
        {
            var info = _db.Queryable<Orders>().First(p => p.Id == id);
            return _db.Deleteable(info).ExecuteCommand() > 0;
        }
        public bool BatchDel(string ids)
        {
            return _db.Ado.ExecuteCommand($"DELETE Orders WHERE Id IN({ids})") > 0;
        }

        public bool Edit(OrdersEdit req, long userId)
        {
            var Orders = _db.Queryable<Orders>().First(p => p.Id == req.Id);
            _mapper.Map(req, Orders);
            Orders.ModifyUserId = userId;
            Orders.ModifyDate = DateTime.Now;
            return _db.Updateable(Orders).ExecuteCommand() > 0;
        }

        public OrdersRes GetOrdersById(long id)
        {
            var info = _db.Queryable<Orders>().First(p => p.Id == id);
            return _mapper.Map<OrdersRes>(info);
        }

        public PageInfo GetOrders(OrdersReq req)
        {
            PageInfo pageInfo = new PageInfo();
            var exp = _db.Queryable<Orders>()
                .WhereIF(!string.IsNullOrEmpty(req.CustomersName), u => u.CustomersName.Contains(req.CustomersName))
                //.WhereIF(!string.IsNullOrEmpty(req.NickName), u => u.NickName.Contains(req.NickName))
                .OrderBy((u) => u.CreateDate, OrderByType.Desc);
            var res = exp
                .Skip((req.PageIndex - 1) * req.PageSize)
                .Take(req.PageSize)
                .ToList();
            pageInfo.Data = _mapper.Map<List<OrdersRes>>(res);
            pageInfo.Total = exp.Count();
            return pageInfo;
        }
    }
}
