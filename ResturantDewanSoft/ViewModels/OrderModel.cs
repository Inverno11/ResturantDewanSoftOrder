using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResturantDewanSoft.ViewModels
{
    class OrderModel
    {

        public string Name { set; get; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total { set; get; }
        

    }
}
