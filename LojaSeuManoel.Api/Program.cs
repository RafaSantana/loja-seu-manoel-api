using LojaSeuManoel.Api.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

// Registro do DbContext para injeção de dependência
builder.Services.AddDbContext<LojaSeuManoelContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Aplica migrations automaticamente (apenas para dev/homologação, não recomendado para produção)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LojaSeuManoelContext>();
    await db.Database.MigrateAsync();

    // Seed das caixas padrão se não existirem
    if (!await db.Caixas.AnyAsync())
    {
        db.Caixas.AddRange(
            new Caixa { Nome = "Caixa 1", Altura = 30, Largura = 40, Comprimento = 80 },
            new Caixa { Nome = "Caixa 2", Altura = 80, Largura = 50, Comprimento = 40 },
            new Caixa { Nome = "Caixa 3", Altura = 50, Largura = 80, Comprimento = 60 }
        );
        await db.SaveChangesAsync();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();


// Mapeia os controllers (necessário para endpoints aparecerem no Swagger)
app.MapControllers();


await app.RunAsync();
