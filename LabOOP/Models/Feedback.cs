using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LabOOP.Models
{
    public partial class Feedback
    {
        public int Id { get; set; }
        public DateTime DateOfPublication { get; set; }
        public int? OrderId { get; set; }
        [Required(ErrorMessage = "Коментар має бути")]
        [Display(Name = "Text")]
        [StringLength(200)]
        public string Description { get; set; } = null!;

        public virtual Order? Order { get; set; }
    }
}
