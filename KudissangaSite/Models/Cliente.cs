namespace KudissangaSite.Models;

public class Cliente : Entity
{
    public Guid PessoaId { get; set; }

    /* EF Core Relations */
    public IEnumerable<Notificacao> Notificaoes { get; set; }
    public Pessoa Pessoa { get; set; }
}
