using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.BasketServices.DTOs
{
    public class BasketItemDto
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int ProductId { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        [Range(0.1,double.MaxValue, ErrorMessage ="Price must be greater than ZERO")]
        public decimal Price { get; set; }
        [Required]
        [Range(1,50,ErrorMessage ="Quantity must be between 1 and 10 Pieces")]
        public int Quantity { get; set; }
        [Required]
        public string PictureUrl { get; set; }
        [Required]
        public string BrandName { get; set; }
        [Required]
        public string TypeName { get; set; }    
    }
}
