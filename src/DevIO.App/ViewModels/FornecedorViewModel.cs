using DevIO.Business.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace DevIO.App.ViewModels
{
    public class FornecedorViewModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1}", MinimumLength = 2)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(14, ErrorMessage = "O campo {0} precisa ter entre {2} e {1}", MinimumLength = 11)]
        public string Documento { get; set; }
        public int TipoFornecedor { get; set; }
        public EnderecoViewModel Endereco { get; set; }

        [DisplayName("Ativo?")]
        public bool Ativo { get; set; }

        /* EF RELATIONS */
        //No caso como o fornecedor poderá ter mais de um produto tem que colocar o IEnumerable
        public IEnumerable<ProdutoViewModel>? Produtos { get; set; }
    }
}
