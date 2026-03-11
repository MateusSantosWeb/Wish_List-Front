using System.ComponentModel.DataAnnotations;

namespace WishListWeb.Models
{
    public class WishViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório")]
        [StringLength(100, ErrorMessage = "O título deve ter no máximo 100 caracteres")]
        public string Titulo { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
        public string? Descricao { get; set; }

        [Required(ErrorMessage = "A categoria é obrigatória")]
        public string Categoria { get; set; } = string.Empty;

        [Required(ErrorMessage = "A prioridade é obrigatória")]
        public string Prioridade { get; set; } = string.Empty;

        // CAMPOS OPCIONAIS (com ?)
        public string? Link { get; set; }

        public string? ImagemUrl { get; set; }

        // Status é definido pela API, não pelo usuário
        public string? Status { get; set; }

        public DateTime? DataCriacao { get; set; }
        
        public int? UsuarioId { get; set; }

        // Para o namorado ver o status secreto
        public string? StatusSecreto { get; set; }
    }

    // ViewModel para quando o namorado vê a lista com progresso
    public class WishWithProgressViewModel
    {
        public WishViewModel Wish { get; set; } = new();
        public string? StatusSecreto { get; set; }
        public string? NotaPrivada { get; set; }
        public string? NotaRealizacao { get; set; }
        public string? FotosRealizacao { get; set; }
        public DateTime? DataRealizacao { get; set; }
    }

    public class ProgressViewModel
    {
        public int Id { get; set; }
        public int WishId { get; set; }
        public string StatusSecreto { get; set; } = string.Empty;
        public string? NotaPrivada { get; set; }
        public string? NotaRealizacao { get; set; }
        public string? FotosRealizacao { get; set; }
        public DateTime? DataRealizacao { get; set; }
        public WishViewModel? Wish { get; set; }
    }
}