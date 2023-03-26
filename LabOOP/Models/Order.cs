using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LabOOP.Models
{
    public partial class Order
    {
        public Order()
        {
            Feedbacks = new HashSet<Feedback>();
            ProductsOrders = new HashSet<ProductsOrder>();
        }

        public int Id { get; set; }
        public int? ClientId { get; set; }
        public DateTime? DateOrder { get; set; }

        [Required(ErrorMessage = "Постачальник має бути")]
        public int? DeliverId { get; set; }

        [Required(ErrorMessage = "Адреса має бути")]
        [StringLength(40, MinimumLength = 7)]
        public string? Address { get; set; }

        public virtual Client? Client { get; set; }
        public virtual Deliver? Deliver { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<ProductsOrder> ProductsOrders { get; set; }
    }
}
