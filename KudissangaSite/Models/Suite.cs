using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KudissangaSite.Models;

public class Suite : Entity
{
    [Required(ErrorMessage = "O campo {0} é obrigatório!")]
    [DisplayName("Número da suite")]
    public int Numero { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório!")]
    [DisplayName("Descrição")]
    public string Descricao { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório!")]
    [DisplayName("Preço diário")]
    public decimal PrecoDiario { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório!")]
    [DisplayName("Capacidade")]
    public int Capacidade { get; set; }

    [NotMapped]
    [DisplayName("Imagem da Suíte")]
    public IFormFile ImagemUpload { get; set; }

    public string Imagem { get; set; }

    [DisplayName("Data de Cadastro")]
    public DateTime DataCadastro { get; set; }

    public bool Disponivel { get; set; }
}
