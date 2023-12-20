using KudissangaSite.Models.ValueObjects;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KudissangaSite.Models;

public class ReservaItem : Entity
{
    public Guid ReservaId { get; set; }

    /* EF Core Relations */
    public Reserva Reserva { get; set; }
}
