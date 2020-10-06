using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

// 182-1 dto for customer basket
namespace API.Dtos
{
    public class CustomerBasketDto
    {
        [Required]
       public string Id { get; set; } 

       public List<BasketItemDto> Items { get; set; }
    }
}