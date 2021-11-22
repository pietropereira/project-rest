using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectRest.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "O campo nome é obrigatório")]

        public string Name { get; set; }

        [Required(ErrorMessage = "O campo é preço obrigatório")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "O Id da Categoria é obrigatório")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
