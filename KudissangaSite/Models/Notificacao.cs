using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KudissangaSite.Models;

public class Notificacao : Entity
{
    public Guid ClienteId { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório!")]
    [StringLength(4000, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres!", MinimumLength = 10)]
    [DisplayName("Mensagem")]
    public string Mensagem { get; set; }

    /* EF Core Relations */
    public Cliente Cliente { get; set; }
}
