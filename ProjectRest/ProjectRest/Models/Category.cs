using System.ComponentModel.DataAnnotations;

namespace ProjectRest.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string Name { get; set; }
    }
}
