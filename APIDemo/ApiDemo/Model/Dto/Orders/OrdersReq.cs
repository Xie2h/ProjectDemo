using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dto.Orders
{
    public class OrdersReq
    {
        public long Id { get; set; }
        public string Index { get; set; }
        public string GoodsName { get; set; }
        public string CustomersName { get; set; }
        public string NickName { get; set; }
        public decimal Price { get; set; }
        public int Num { get; set; }
        public decimal Sum { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
