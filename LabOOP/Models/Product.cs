using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LabOOP.Models
{
    public partial class Product
    {
        public Product()
        {
            ProductsOrders = new HashSet<ProductsOrder>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Назва товару обов'язкова")]
        [StringLength(10)]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Ціна товару обов'язкова")]
        [Range(0.01, 70000.00, ErrorMessage = "Ціна повинна бути більше 0 та менше за 70000")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Невірний формат числа")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Опис товару обов'язковий")]
        [StringLength(100)]
        public string? Description { get; set; }
        [Required(ErrorMessage = "Вага товару обов'язкова")]
        [Range(0.01, 15000.00, ErrorMessage = "Ціна повинна бути більше 0 та менше за 15000")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Невірний формат числа")]
        public decimal WeightInKilograms { get; set; }

        [Required(ErrorMessage = "Країна має бути")]
        public int? CountryId { get; set; }

        public virtual Country? Country { get; set; }
        public virtual ICollection<ProductsOrder> ProductsOrders { get; set; }
    }
}
