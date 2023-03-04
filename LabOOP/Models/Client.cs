using System;
using System.Collections.Generic;

namespace LabOOP.Models
{
    public partial class Client
    {
        public Client()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string MobilePhone { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; }
    }
}
