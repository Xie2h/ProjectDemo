using Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Dto.Menu;
using Model.Other;
using WebAPI.Config;

namespace WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _Menu;
        public MenuController(IMenuService Menu)
        {
            _Menu = Menu;
        }
        [HttpPost]
        public ApiResult Add(MenuAdd req)
        {
            long userId = Convert.ToInt32(HttpContext.User.Claims.ToList()[0].Value);
            //获取当前登录人信息 
            return ResultHelper.Success(_Menu.Add(req, userId));
        }
        [HttpPost]
        public ApiResult Edit(MenuEdit req)
        {
            long userId = Convert.ToInt32(HttpContext.User.Claims.ToList()[0].Value);
            //获取当前登录人信息
            return ResultHelper.Success(_Menu.Edit(req, userId));
        }
        [HttpGet]
        public ApiResult Del(long id)
        {
            //获取当前登录人信息 
            return ResultHelper.Success(_Menu.Del(id));
        }
        [HttpGet]
        public ApiResult BatchDel(string ids)
        {
            //获取当前登录人信息 
            return ResultHelper.Success(_Menu.BatchDel(ids));
        }
        [HttpPost]
        public ApiResult GetMenus(MenuReq req)
        {
            return ResultHelper.Success(_Menu.GetMenus(req));
        }
        [HttpGet]
        public ApiResult SettingMenu(long rid, string mids)
        {
            return ResultHelper.Success(_Menu.SettingMenu(rid, mids));
        }
        [HttpGet]
        public ApiResult GetUserMenus()
        {
            long userId = Convert.ToInt32(HttpContext.User.Claims.ToList()[0].Value);
            return ResultHelper.Success(_Menu.GetMenusByUserId(userId));
        }
    }
}
