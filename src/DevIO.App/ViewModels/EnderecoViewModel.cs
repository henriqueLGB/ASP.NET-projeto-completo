using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace DevIO.App.ViewModels
{
    public class EnderecoViewModel
    {
        [Key]
        public Guid Id { get; set; }

        //Chave Estrangeira
        [HiddenInput]
        public Guid FornecedorId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(200, ErrorMessage = "O campo {0} precisa ter entre {2} e {1}", MinimumLength = 2)]
        public string Logradouro { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(50, ErrorMessage = "O campo {0} precisa ter entre {2} e {1}", MinimumLength = 1)]
        public string Numero { get; set; }
        public string Complemento { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(8, ErrorMessage = "O campo {0} precisa ter entre {2} e {1}", MinimumLength = 8)]
        public string Cep { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1}", MinimumLength = 2)]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1}", MinimumLength = 2)]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1}", MinimumLength = 2)]
        public string Estado { get; set; }

        /* EF RELATIONS */
        //No caso como o Fornecedor terá um produto pode chamar normal.
        public FornecedorViewModel? Fornecedor { get; set; }
    }
}
