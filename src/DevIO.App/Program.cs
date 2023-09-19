using DevIO.Business.Interfaces;
using DevIO.Data.Context;
using DevIO.Data.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//AutoMapper
builder.Services.AddAutoMapper(typeof(Program));

// Add services to the container.
builder.Services.AddControllersWithViews();

//Conex�o do Banco
builder.Services.AddDbContext<MeuDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MinhaConexaoDb")));

builder.Services.AddScoped<MeuDbContext>();
builder.Services.AddScoped<IProdutosRepository , ProdutoRepository>();
builder.Services.AddScoped<IFornecedorRepository, FornecedorRepository>();
builder.Services.AddScoped<IEnderecoRepository, EnderecoRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
