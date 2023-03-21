using AutoMapper;
using Model.Dto.Menu;
using Model.Dto.Role;
using Model.Dto.User;
using Model.Dto.Orders;
using Model.Entitys;

namespace WebAPI.Config
{
    /// <summary>
    /// Dto的映射配置
    /// </summary>
    public class AutoMapperConfigs : Profile
    {
        public AutoMapperConfigs()
        {
            //角色
            CreateMap<Role, RoleRes>();
            CreateMap<RoleAdd, Role>();
            CreateMap<RoleEdit, Role>();
            //用户
            CreateMap<Users, UserRes>();
            CreateMap<UserAdd, Users>();
            CreateMap<UserEdit, Users>();
            //菜单
            CreateMap<Menu, MenuRes>();
            CreateMap<MenuAdd, Menu>();
            CreateMap<MenuEdit, Menu>();
            //订单
            CreateMap<Orders, OrdersRes>();
            CreateMap<OrdersAdd, Orders>();
            CreateMap<OrdersEdit, Orders>();
        }
    }
}
