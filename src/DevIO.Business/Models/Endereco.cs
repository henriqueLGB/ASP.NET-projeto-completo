using System.ComponentModel.DataAnnotations;

namespace DevIO.Business.Models
{
    public class Endereco : Entity
    {
        //Chave Estrangeira
        public Guid FornecedorId { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Cep { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }

        /* EF RELATIONS */
        //No caso como o Fornecedor terá um produto pode chamar normal.
        public Fornecedor? Fornecedor { get; set; }
    }
}
