using KudissangaSite.Models.ValueObjects;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KudissangaSite.Models;

public class Funcionario : Entity
{
    public Guid PessoaId { get; set; }
    public Funcao Funcao { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório!")]
    [DisplayName("Endereço de Email")]
    public string Email { get; set; }

    /* EF Core Relations */
    public Pessoa Pessoa { get; set; }

}
