using KudissangaSite.Models.ValueObjects;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KudissangaSite.Models;

public class Reserva : Entity
{
    //public Guid FuncionarioId { get; set; }
    //public Guid ClienteId { get; set; }
    public Guid SuiteId { get; set; }
    public DateTime DataReserva { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório!")]
    [DisplayName("Número da reserva")]
    public int Numero { get; set; }

    [DisplayName("Valor a pagar")]
    public decimal Valor { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório!")]
    [DisplayName("Dias de estadia")]
    public int DiasEstadia { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório!")]
    [DisplayName("Valor pago")]
    public decimal ValorPago { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório!")]
    [DisplayName("Troco")]
    public decimal Troco { get; set; }
    public string NomeCliente { get; set; }
    public string NomeFuncionario { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório!")]
    [DisplayName("Tipo de Pagamento")]
    public TipoPagamento TipoPagamento { get; set; }

    [NotMapped]
    public IFormFile ComprovativoUpload { get; set; }

    [DisplayName("Comprovativo")]
    public string Comprovativo { get; set; }

    [Required(ErrorMessage = "O campo {0} é obrigatório!")]
    [DisplayName("Aprovado")]
    public bool Aprovada { get; set; }

    /* Relações do EF Core */
    public Funcionario Funcionario { get; set; }
    public Cliente Cliente { get; set; }
    public Suite Suite { get; set; }
    public IEnumerable<ReservaItem> ItemReservas { get; set; }
}
