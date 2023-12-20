using KudissangaSite.Models;

namespace KudissangaSite.Extensions;

public static class DadosReserva
{
    public static Guid ReservaId { get; set; }
    public static Cliente ClienteReserva { get; set; }
    public static Funcionario FuncionarioReserva { get; set; }
    public static Guid IdCliente { get; set; }
    public static Guid IdFuncionario { get; set; }
    public static int NumeroReserva { get; set; }
    public static Suite SuiteReserva { get; set; }
    public static Guid IdSuite { get; set; }
}
