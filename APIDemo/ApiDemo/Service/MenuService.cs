using AutoMapper;
using Common;
using Interface;
using Model.Dto.Menu;
using Model.Entitys;
using Model.Other;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class MenuService : IMenuService
    {
        private readonly IMapper _mapper;
        private ISqlSugarClient _db { get; set; }
        public MenuService(IMapper mapper, ISqlSugarClient db)
        {
            _mapper = mapper;
            _db = db;
        }
        public bool Add(MenuAdd input, long MenuId)
        {
            Menu info = _mapper.Map<Menu>(input);
            info.CreateUserId = MenuId;
            info.CreateDate = DateTime.Now;
            info.IsDeleted = 0;
            return _db.Insertable(info).ExecuteCommand() > 0;
        }

        public bool Edit(MenuEdit input, long userId)
        {
            var info = _db.Queryable<Menu>().First(p => p.Id == input.Id);
            _mapper.Map(input, info);
            info.ModifyUserId = userId;
            info.ModifyDate = DateTime.Now;
            return _db.Updateable(info).ExecuteCommand() > 0;
        }

        public bool Del(long id)
        {
            var info = _db.Queryable<Menu>().First(p => p.Id == id);
            return _db.Deleteable(info).ExecuteCommand() > 0;
        }

        public bool BatchDel(string ids)
        {
            return _db.Ado.ExecuteCommand($"DELETE Menu WHERE Id IN({ids})") > 0;
        }

        public PageInfo GetMenus(MenuReq req)
        {
            PageInfo pageInfo = new PageInfo();
            var exp = _db.Queryable<Menu>()
                .WhereIF(!string.IsNullOrEmpty(req.Name), u => u.Name.Contains(req.Name))
                .WhereIF(!string.IsNullOrEmpty(req.Index), u => u.Index.Contains(req.Index))
                .OrderBy((u) => u.Order)
                .Select((u) => new MenuRes
                {
                    Id = u.Id
                ,
                    Name = u.Name
                ,
                    Index = u.Index
                ,
                    FilePath = u.FilePath
                ,
                    ParentId = u.ParentId

                ,
                    Order = u.Order
                ,
                    IsEnable = u.IsEnable
                ,
                    Description = u.Description
                ,
                    CreateDate = u.CreateDate
                }).ToTree(it => it.Children, it => it.ParentId, 0);
            var res = exp
                .Skip((req.PageIndex - 1) * req.PageSize)
                .Take(req.PageSize)
                .ToList();
            pageInfo.Data = _mapper.Map<List<MenuRes>>(res);
            pageInfo.Total = exp.Count();
            return pageInfo;
        }
        public MenuRes GetMenuById(long id)
        {
            var info = _db.Queryable<Menu>().First(p => p.Id == id);
            return _mapper.Map<MenuRes>(info);
        }

        public bool SettingMenu(long rid, string mids)
        {
            List<MenuRoleRelation> list = new List<MenuRoleRelation>();
            foreach (string it in mids.Split(','))
            {
                MenuRoleRelation info = new MenuRoleRelation() { RoleId = rid, MenuId = Convert.ToInt64(it.Replace("'", "")) };
                list.Add(info);
            }
            //删除之前的角色
            _db.Ado.ExecuteCommand($"DELETE MenuRoleRelation WHERE MenuId = {rid}");
            return _db.Insertable(list).ExecuteCommand() > 0;
        }
        #region 查询当前角色所拥有的菜单
        public List<MenuRes> GetMenusByUserId(long userId)
        {
            List<Menu> res = new List<Menu>();
            //查询当前角色所拥有的菜单
            Expression<Func<Menu, MenuRoleRelation, bool>> func = (m, mr) => m.Id == mr.MenuId;
            var currlist = _db.Queryable<Menu>().ToList();
            if (_db.Queryable<Users>().Where(p => p.Id == userId).First().UserType > 0)
            {
                //如果是普通用户，则根据角色获取菜单
                var idarr = _db.Queryable<UserRoleRelation>().Where(P => P.UserId == userId).Select(s => s.RoleId).ToList();
                func = (m, mr) => m.Id == mr.MenuId && idarr.Contains(mr.RoleId);
                currlist = _db.Queryable<Menu>().InnerJoin<MenuRoleRelation>(func).ToList();
            }
            var all = _db.Queryable<Menu>().Where(p => p.IsEnable).ToList();
            if (currlist != null && currlist.Count > 0)
            {
                currlist.ForEach(item =>
                {
                    //找到这些菜单的父级菜单
                    GetParent(all, item, res);
                });
                //将父级菜单和当前的菜单整合在一起组成完整的菜单
                List<MenuRes> list = _mapper.Map<List<MenuRes>>(currlist.Concat(res).Distinct().ToList());
                //再通过递归的方式讲list转tree
                return GetResult(list);
            }
            return new List<MenuRes>();
        }
        /// <summary>
        /// 递归获取所有父级
        /// </summary>
        /// <param name="all"></param>
        /// <param name="curr"></param>
        /// <param name="res"></param>
        private static void GetParent(List<Menu> all, Menu curr, List<Menu> res)
        {
            var pInfo = all.FirstOrDefault(p => p.Id == curr.ParentId);
            if (pInfo != null)
            {
                res.Add(pInfo);
                GetParent(all, pInfo, res);
            }
        }
        private static List<MenuRes> GetResult(List<MenuRes> list)
        {
            //从一级菜单开始往下找子级菜单
            List<MenuRes> newlist = list.Where(p => p.ParentId == 0).ToList();
            //通过递归获取子级
            GetChildren(list, newlist);
            return newlist.Distinct(new DisComparer<MenuRes>("Name")).ToList();
        }
        private static void GetChildren(List<MenuRes> all, List<MenuRes> res)
        {
            if (res != null && res.Count > 0)
            {
                res.ForEach(item =>
                {
                    var child = all.Where(p => p.ParentId == item.Id).Distinct(new DisComparer<MenuRes>("Name")).OrderBy(p => p.Order).ToList();
                    if (child != null && child.Count > 0)
                    {
                        item.Children = child;
                        GetChildren(all, child);
                    }
                });
            }
        }
        #endregion
    }
}
