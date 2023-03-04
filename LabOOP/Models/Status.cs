using System;
using System.Collections.Generic;

namespace LabOOP.Models
{
    public partial class Status
    {
        public Status()
        {
            StatusesOrders = new HashSet<StatusesOrder>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<StatusesOrder> StatusesOrders { get; set; }
    }
}
