using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevIO.Data.Repository
{
    public class ProdutoRepository : Repository<Produto>, IProdutosRepository
    {
        public ProdutoRepository(MeuDbContext context) : base(context)
        {
        }

        public async Task<Produto> ObterProdutoFornecedor(Guid id)
        {
            return await db.produtos.AsNoTracking().Include(f => f.Fornecedor)
                .FirstOrDefaultAsync( p => p.Id == id);
        }

        public async Task<IEnumerable<Produto>> ObterProdutosFornecedores()
        {
            return await db.produtos.AsNoTracking().Include(f => f.Fornecedor)
                 .OrderBy(p => p.Nome).ToListAsync();
        }

        public async Task<IEnumerable<Produto>> ObterProdutosPorFornecedor(Guid FornecedorId)
        {
            return await Buscar(p => p.FornecedorId == FornecedorId);
        }
    }
}
    