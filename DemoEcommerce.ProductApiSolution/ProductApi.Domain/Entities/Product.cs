using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Domain.Entities
{
    public class Product
    {
        public int Id { set; get; }
        public string? Name { set; get; }
        public decimal Price { set; get; }
        public int Quantity { set; get; }
    }
}
