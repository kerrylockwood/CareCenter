using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareModels.Orders
{
    public class OrderCrtUpdRtnStatus
    {
        public bool OrderHeaderCreated { get; set; }
        public bool OrderAllDetailCreated { get; set; }
        public int OrderId { get; set; }
    }
}
