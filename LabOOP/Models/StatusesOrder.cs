using System;
using System.Collections.Generic;

namespace LabOOP.Models
{
    public partial class StatusesOrder
    {
        public int Id { get; set; }
        public int StatusId { get; set; }
        public int OrderId { get; set; }
        public DateTime DateOfStatus { get; set; }

        public virtual Order Order { get; set; } = null!;
        public virtual Status Status { get; set; } = null!;
    }
}
