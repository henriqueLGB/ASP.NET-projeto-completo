using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DevIO.Business.Models
{
    public class Fornecedor : Entity
    {
        public string Nome { get; set; }
        public string Documento { get; set; }
        public TipoFornecedor TipoFornecedor { get; set; }
        public Endereco Endereco { get; set; }
        public bool Ativo { get; set; }

        /* EF RELATIONS */
        //No caso como o fornecedor poderá ter mais de um produto tem que colocar o IEnumerable
        public IEnumerable<Produto>? Produtos { get; set; } 
    }
}
