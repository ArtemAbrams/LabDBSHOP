using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LabOOP.Models
{
    public partial class Deliver
    {
        public Deliver()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Телефон має бути")]
        [RegularExpression(@"^\380\d{9}$", ErrorMessage = "Номер телефону повинен починатися з 380 та містити ще 9 цифр")]
        [StringLength(13, MinimumLength = 12)]
        [Remote(action: "VerifyName", controller: "Delivers", AdditionalFields = nameof(PhoneNumber))]
        public string PhoneNumber { get; set; } = null!;
        [Required(ErrorMessage = "Транспорт має бути")]
        [Display(Name = "How to deliver")]
        public int? TransportId { get; set; }

        [Required(ErrorMessage = "Ім`я має бути")]
        [StringLength(10)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Фамілія має бути")]
        [StringLength(10)]
        public string Surname { get; set; } = null!;

        public virtual Transport? Transport { get; set; }
        public virtual ICollection<Order> Orders { get; set; }

        public string DisplayText
        {
            get => $"Name: {Name} Surname: {Surname} Transport: {Transport?.Name ?? "N/A"}";
        }
    }
}
