using KudissangaSite.Models.ValueObjects;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace KudissangaSite.Models;

public class Pessoa : Entity
{
    [Required(ErrorMessage = "O campo {0} é obrigatório!")]
    [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 5)]
    [DisplayName("Nome")]
    public string Nome { get; set; }

    [DisplayName("Gênero")]
    public Genero Genero { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório!"), DisplayName("Data de Nascimento")]
    public DateTime DataNascimento { get; set; }

    [StringLength(14, ErrorMessage = "O campo {0} precisa ter {1} caracteres!", MinimumLength = 14)]
    [DisplayName("Bilhete de Identidade")]
    public string BI { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório!")]
    [StringLength(13, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres!", MinimumLength = 9)]
    [DisplayName("Número de telefone")]
    public string Telefone { get; set; }

    [StringLength(1000, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres!", MinimumLength = 10)]
    [DisplayName("Morada")]
    public string Mordada { get; set; }

}
