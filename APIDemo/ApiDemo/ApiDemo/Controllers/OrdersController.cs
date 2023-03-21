using Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Dto.Orders;
using Model.Entitys;
using Model.Other;
using WebAPI.Config;

namespace ApiDemo.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrdersService _Orders;
        public OrdersController(IOrdersService Orders)
        {
            _Orders = Orders;
        }
        [HttpPost]
        public ApiResult GetOrders(OrdersReq req)
        {
            return ResultHelper.Success(_Orders.GetOrders(req));
        }

        [HttpGet]
        public ApiResult GetOrders(long id)
        {
            return ResultHelper.Success(_Orders.GetOrdersById(id));
        }

        [HttpPost]
        public ApiResult Add(OrdersAdd req)
        {
            //获取当前登录人信息
            long userId = Convert.ToInt32(HttpContext.User.Claims.ToList()[0].Value);
            string userNickName = Convert.ToString(HttpContext.User.Claims.ToList()[2].Value);
            return ResultHelper.Success(_Orders.Add(req, userId, userNickName));
        }
        [HttpPost]
        public ApiResult Edit(OrdersEdit req)
        {
            //获取当前登录人信息
            long userId = Convert.ToInt32(HttpContext.User.Claims.ToList()[0].Value);
            return ResultHelper.Success(_Orders.Edit(req, userId));
        }
        [HttpGet]
        public ApiResult Del(long id)
        {
            //获取当前登录人信息 
            return ResultHelper.Success(_Orders.Del(id));
        }
        [HttpGet]
        public ApiResult BatchDel(string ids)
        {
            //获取当前登录人信息 
            return ResultHelper.Success(_Orders.BatchDel(ids));
        }
    }
}
