using System.ComponentModel.DataAnnotations;

public class Imovel
{
    [Key]
    public int Id { get; set; }
    public string Descricao { get; set; }
    public DateTime DataCompra { get; set; }
    public string Endereco { get; set; }
    public List<Comodo> Comodos { get; set; } = new List<Comodo>();
}

public class Comodo
{
    [Key]
    public int Id { get; set; }
    public string Nome { get; set; }
    public int ImovelId { get; set; }
    public Imovel Imovel { get; set; }
}