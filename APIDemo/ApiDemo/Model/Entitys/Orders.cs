using Model.Common;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Entitys
{
    public class Orders : IEntity
    {
        /// <summary>
        /// 商品名称
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public string GoodsName { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public string CustomersName { get; set; }
        /// <summary>
        /// 商品单价
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public decimal Price { get; set; }
        /// <summary>
        /// 商品数量
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public int Num { get; set; }
        public decimal Sum { get; set; }
        public string NickName { get; set; }
    }
}
