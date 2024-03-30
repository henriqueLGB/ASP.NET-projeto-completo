function AjaxModal() {

    $(document).ready(function () {
        $(function () {
            $.ajaxSetup({ cache: false });

            $("a[data-modal]").on('click', function (e) {
                e.preventDefault();
                var url = $(this).attr("href");
                $('#myModalContent').load(url, function () {
                    $('#myModal').modal('show');
                    bindForm(this);
                });
                return false;
            })

        })

        function bindForm(dialog) {
            $('form', dialog).submit(function () {
                $.ajax({
                    url: this.action,
                    type: this.method,
                    data: $(this).serialize(),
                    success: function (result) {
                        if (result.success) {
                            $('#myModal').modal('hide')
                            $('#EnderecoTarget').load(result.url);
                        } else {
                            $('#myModalContent').html(result);
                            bindForm(dialog)
                        }
                    }
                })

                return false;
            })
        }
    })

}

function BuscaCep() {
    $(document).ready(function () {

        function limpa_formulario_cep() {
            // Limpa valores do formulário de cep.
            $("#Endereco_Logradouro").val("");
            $("#Endereco_Bairro").val("");
            $("#Endereco_Cidade").val("");
            $("#Endereco_Estado").val("");
        }

        //Quando o campo cep perde o foco
        $("#EnderecoCep").blur(function () {

            //Nova variável "cep" somente com dígitos
            var cep = $(this).val().replace(/\D/g, '')

            //Verifica se o campo cep possui valor atrelado
            if (cep != "") {

                //Expressão regular para validar o CEP
                var validacep = /^[0-9]{8}$/;

                //valida o formato do cep
                if (validacep.test(cep)) {

                    //Preencher os campos com '...' enquanto consulta webservice
                    $("#EnderecoLogradouro").val("...")
                    $("#EnderecoBairro").val("...")
                    $("#EnderecoCidade").val("...")
                    $("#EnderecoEstado").val("...")

                    //Consulta o webservice viacep.com.br
                    $.getJSON("https://viacep.com.br/ws/" + cep + "/json/?callback=?",
                        function (dados) {
                            if (!("erro" in dados)) {

                                //Atualiza os campos com os valores de consulta
                                $("#EnderecoLogradouro").val(dados.logradouro)
                                $("#EnderecoBairro").val(dados.bairro)
                                $("#EnderecoCidade").val(dados.localidade)
                                $("#EnderecoEstado").val(dados.uf)
                            } else {
                                limpa_formulario_cep()
                                alert("CEP não encontrado.")
                            }
                        })

                } else {
                    //Cep inválido
                    limpa_formulario_cep()
                    alert("Formatado de cep inválido.")
                }
            } else {
                //Cep sem valor, limpa formulário
                limpa_formulario_cep();
            }
        })

    })
}