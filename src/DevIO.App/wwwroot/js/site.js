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