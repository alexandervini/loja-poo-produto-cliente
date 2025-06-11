using Loja.Shared.Models;
using System.Text.Json;

namespace Loja.Shared.Contexts;

public static class LojaContext
{
    public static List<Produto> Produtos { get; set; } = new();
    public static List<Cliente> Clientes { get; set; } = new();

    public static void Inicializar() 
    {
        //
        Produtos.Recuperar();
        Clientes.Recuperar();
    }

    public static void Finalizar()
    {
        //
        Produtos.Salvar();
        Clientes.Salvar();
    }

    public static void Salvar<T>(this List<T> lista) 
    {
        // retorna o nome do tipo generico T 
        var tipo = typeof(T).Name;

        // pasta onde os arquivos serao salvos
        var pasta = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        // cria o diretorio "Loja" dentro da pasta Documentos do usuario, se nao existir
        if (!Directory.Exists(Path.Combine(pasta, "Loja")))
        {
            Directory.CreateDirectory(Path.Combine(pasta, "Loja"));        
        }

        // o nome e caminho do arquivo que sera salvo
        var arquivo = Path.Combine(pasta, "Loja", $"{tipo}.json");
        var opcoes = new JsonSerializerOptions { WriteIndented = true };
        
        // salvar os dados em json no arquivo
        var json = JsonSerializer.Serialize(lista, opcoes);
        File.WriteAllText(arquivo, json);

        // exibir mensagem de sucesso
        Console.WriteLine($"lista de {tipo} salvo com sucesso em {arquivo}");
    }

    public static void Recuperar<T>(this List<T> lista)
    {
        // retorna o nome do tipo generico T 
        var tipo = typeof(T).Name;

        // pasta onde os arquivos serao salvos
        var pasta = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        // o nome e caminho do arquivo que sera salvo
        var arquivo = Path.Combine(pasta, "Loja", $"{tipo}.json");

        if (File.Exists(arquivo)) 
        {
            // ler o arquivo json e desserializar para a lista
            var json = File.ReadAllText(arquivo);
            var listaRecuperada = JsonSerializer.Deserialize<List<T>>(json);
            if (listaRecuperada != null) 
            {
                lista.AddRange(listaRecuperada);
            }
        }

        // exibir mensagem de sucesso
        Console.WriteLine($"lista de {tipo} recuperado com sucesso de {arquivo}");
    }


}
