using System.Text.Json;
using ExpenseControl.Api.Models;

namespace ExpenseControl.Api.Data;


public class JsonDataStore
{
    private readonly string _pessoasPath;
    private readonly string _transacoesPath;
    private readonly object _lock = new();

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter() }
    };

    public JsonDataStore(IWebHostEnvironment env)
    {
        var dataFolder = Path.Combine(env.ContentRootPath, "data");
        Directory.CreateDirectory(dataFolder);

        _pessoasPath = Path.Combine(dataFolder, "pessoas.json");
        _transacoesPath = Path.Combine(dataFolder, "transacoes.json");

        if (!File.Exists(_pessoasPath))
            File.WriteAllText(_pessoasPath, "[]");

        if (!File.Exists(_transacoesPath))
            File.WriteAllText(_transacoesPath, "[]");
    }

    public List<Pessoa> LerPessoas()
    {
        lock (_lock)
        {
            var json = File.ReadAllText(_pessoasPath);
            return JsonSerializer.Deserialize<List<Pessoa>>(json, _jsonOptions) ?? new List<Pessoa>();
        }
    }

    public void SalvarPessoas(List<Pessoa> pessoas)
    {
        lock (_lock)
        {
            var json = JsonSerializer.Serialize(pessoas, _jsonOptions);
            File.WriteAllText(_pessoasPath, json);
        }
    }

    public List<Transacao> LerTransacoes()
    {
        lock (_lock)
        {
            var json = File.ReadAllText(_transacoesPath);
            return JsonSerializer.Deserialize<List<Transacao>>(json, _jsonOptions) ?? new List<Transacao>();
        }
    }

    public void SalvarTransacoes(List<Transacao> transacoes)
    {
        lock (_lock)
        {
            var json = JsonSerializer.Serialize(transacoes, _jsonOptions);
            File.WriteAllText(_transacoesPath, json);
        }
    }
}
