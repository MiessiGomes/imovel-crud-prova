using Microsoft.EntityFrameworkCore;

class Program
{
    static void Main(string[] args)
    {
        using (var context = new ImovelContext())
        {
            context.Database.EnsureCreated();

            while (true)
            {
                Console.WriteLine("\n1. Adicionar um Imóvel");
                Console.WriteLine("2. Listar Imóveis Adicionados");
                Console.WriteLine("3. Atualizar um Imóvel");
                Console.WriteLine("4. Excluir um Imóvel");
                Console.WriteLine("5. Adicionar Cômodo ao Imóvel");
                Console.WriteLine("6. Sair");
                Console.Write("Escolhe uma opção aí parceiro: ");

                string opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        AdicionarImovel(context);
                        break;
                    case "2":
                        ListarImoveis(context);
                        break;
                    case "3":
                        AtualizarImovel(context);
                        break;
                    case "4":
                        ExcluirImovel(context);
                        break;
                    case "5":
                        AdicionarComodo(context);
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }
            }
        }
    }

    static void AdicionarImovel(ImovelContext context)
    {
        Console.Write("Descrição: ");
        string descricao = Console.ReadLine();
        
        Console.Write("Data da Compra (dd/mm/aaaa): ");
        DateTime dataCompra = DateTime.Parse(Console.ReadLine());
        
        Console.Write("Endereço: ");
        string endereco = Console.ReadLine();

        var imovel = new Imovel
        {
            Descricao = descricao,
            DataCompra = dataCompra,
            Endereco = endereco
        };

        context.Imoveis.Add(imovel);
        context.SaveChanges();
        Console.WriteLine("Imóvel adicionado com sucesso.");
    }

    static void ListarImoveis(ImovelContext context)
    {
        var imoveis = context.Imoveis.Include(i => i.Comodos).ToList();
        foreach (var imovel in imoveis)
        {
            Console.WriteLine($"ID: {imovel.Id}, " +
                              $"Descrição: {imovel.Descricao}, " +
                              $"Data da Compra: {imovel.DataCompra:dd/MM/yyyy}, " +
                              $"Endereço: {imovel.Endereco}");
            Console.WriteLine("Cômodos:");
            foreach (var comodo in imovel.Comodos)
            {
                Console.WriteLine($"  - {comodo.Nome}");
            }
            Console.WriteLine();
        }
    }

    static void AtualizarImovel(ImovelContext context)
    {
        Console.Write("ID do Imóvel a ser atualizado: ");
        int id = int.Parse(Console.ReadLine());

        var imovel = context.Imoveis.Find(id);
        if (imovel == null)
        {
            Console.WriteLine("Imóvel não encontrado.");
            return;
        }

        Console.Write("Nova Descrição: ");
        imovel.Descricao = Console.ReadLine();
        
        Console.Write("Nova Data da Compra (dd/mm/aaaa): ");
        imovel.DataCompra = DateTime.Parse(Console.ReadLine());
        
        Console.Write("Novo Endereço: ");
        imovel.Endereco = Console.ReadLine();

        context.SaveChanges();
        Console.WriteLine("Imóvel atualizado com sucesso.");
    }

    static void ExcluirImovel(ImovelContext context)
    {
        Console.Write("ID do Imóvel a ser excluído: ");
        int id = int.Parse(Console.ReadLine());

        var imovel = context.Imoveis.Find(id);
        if (imovel == null)
        {
            Console.WriteLine("Imóvel não encontrado.");
            return;
        }

        context.Imoveis.Remove(imovel);
        context.SaveChanges();
        Console.WriteLine("Imóvel excluído com sucesso.");
    }

    static void AdicionarComodo(ImovelContext context)
    {
        Console.Write("ID do Imóvel para adicionar o cômodo: ");
        int imovelId = int.Parse(Console.ReadLine());

        var imovel = context.Imoveis.Find(imovelId);
        if (imovel == null)
        {
            Console.WriteLine("Imóvel não encontrado.");
            return;
        }

        Console.Write("Nome do Cômodo: ");
        string nome = Console.ReadLine();

        var comodo = new Comodo
        {
            Nome = nome,
            ImovelId = imovelId
        };

        context.Comodos.Add(comodo);
        context.SaveChanges();
        Console.WriteLine("Cômodo adicionado com sucesso.");
    }
}