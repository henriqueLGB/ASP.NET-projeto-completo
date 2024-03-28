using DevIO.Business.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Data.Context
{
    public class MeuDbContext : IdentityDbContext
    {

        public MeuDbContext(DbContextOptions options): base(options)
        {
            
        }

        public DbSet<Produto> produtos { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //NO CASO DO MAPEAMENTO NÃO TIVER SIDO EXPECIFICADO O TAMANHO DA COLUNA STRING ,VAMOS SETAR O VALOR 100 COMO DEFAULT 
            foreach(var property in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
            {
                property.SetColumnType("varchar(100)");
            }

            //ELE VAI BUSCAR O MAPPING ATRAVÉS DOS DBSET COLOCADO NO DBCONTEXT
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MeuDbContext).Assembly);

            //ELE VERIFICA E NÃO PERMITI CASO OCORRÁ UM EXCLUSÃO DE ALGUMA TABELA QUE POSSUA FOREIGN KEYS
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.ClientSetNull;
            }

            base.OnModelCreating(modelBuilder);
        }

    }
}
