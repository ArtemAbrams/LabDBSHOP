using System;
using System.Collections.Generic;

namespace LabOOP.Models
{
    public partial class Transport
    {
        public Transport()
        {
            Delivers = new HashSet<Deliver>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Deliver> Delivers { get; set; }
    }
}
