using System.ComponentModel.DataAnnotations;

namespace BibliotecaNacional.Models
{
    public class Libro
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es obligatorio")]
        public string Titulo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El autor es obligatorio")]
        public string Autor { get; set; } = string.Empty;

        [Required(ErrorMessage = "La categoría es obligatoria")]
        public string Categoria { get; set; } = string.Empty;

        [Required(ErrorMessage = "El año de publicación es obligatorio")]
        [Range(1000, 2100, ErrorMessage = "Ingrese un año válido")]
        public int AnioPublicacion { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        public string Estado { get; set; } = string.Empty;
    }
}