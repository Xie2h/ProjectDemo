using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dto.Orders
{
    public class OrdersEdit
    {
        public long Id { get; set; }
        public string GoodsName { get; set; }
        public string CustomersName { get; set; }
        public decimal Price { get; set; }
        public int Num { get; set; }
        public decimal Sum { get; set; }
    }
}
