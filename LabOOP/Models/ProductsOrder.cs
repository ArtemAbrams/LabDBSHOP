using System;
using System.Collections.Generic;

namespace LabOOP.Models
{
    public partial class ProductsOrder
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public int Count { get; set; }

        public virtual Order? Order { get; set; }
        public virtual Product? Product { get; set; }
    }
}
