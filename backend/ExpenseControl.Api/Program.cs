using ExpenseControl.Api.Data;
using ExpenseControl.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// ---------- Serviços ----------
// AddJsonOptions garante que o enum TipoTransacao seja aceito/devolvido como
// texto ("Despesa"/"Receita") no JSON da API, em vez de número (0/1).
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

// Camada de persistência em JSON (singleton: os dados vivem em disco,
// então só precisamos de uma instância cuidando dos arquivos)
builder.Services.AddSingleton<JsonDataStore>();

// Serviços de domínio
builder.Services.AddScoped<PessoaService>();
builder.Services.AddScoped<TransacaoService>();
builder.Services.AddScoped<TotaisService>();

// CORS liberado para o frontend React rodando localmente (Vite: porta 5173)
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173", "http://127.0.0.1:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// ---------- Pipeline ----------
app.UseCors("FrontendPolicy");
app.UseAuthorization();
app.MapControllers();

app.Run();
