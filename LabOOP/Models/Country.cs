using System;
using System.Collections.Generic;

namespace LabOOP.Models
{
    public partial class Country
    {
        public Country()
        {
            Products = new HashSet<Product>();
        }

        public int Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
