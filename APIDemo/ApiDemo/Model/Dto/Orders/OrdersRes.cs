using Npgsql.TypeHandlers.NumericHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dto.Orders
{
    public class OrdersRes
    {
        public long Id { get; set; }
        public string GoodsName { get; set; }
        public string CustomersName { get; set; }
        public string NickName { get; set; }
        public decimal Price { get; set; }
        public int Num { get; set; }
        public decimal Sum { get { return Price * Num; } }
        /// <summary>
        /// 创建人Id
        /// </summary> 
        public string CreateUserId { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary> 
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 修改人Id
        /// </summary> 
        public string ModifyUserId { get; set; }
        /// <summary>
        /// 修改日期
        /// </summary> 
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary> 
        public int IsDeleted { get; set; }
    }
}
