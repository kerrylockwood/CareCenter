using CareData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareModels.Orders
{
    public class JoinItemToOrderDetail
    {
        public Item Item { get; set; }
        public OrderDetail OrderDetail { get; set; }
    }
}
