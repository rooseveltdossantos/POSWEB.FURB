$(document).ready(function () {

    $("#Usuarios").change(function () {

        $.ajax({
            url: "Devolucao/ListarLivros",
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
            },
            success: function (json) {
                var ddl = $('#selectLivro');
                ddl.empty();
                $(json).each(function () {
                    $(document.createElement('option'))
                        .attr('value', this.Id)
                        .text(this.Value)
                        .appendTo(ddl);
                });
            }
        });

        $("#selectLivro").removeAttr("disabled");

    });

    $(".descricaoLink").click(function (event) {

        event.preventDefault();

        var url = $(this).attr("href");

        $("#descricaoLivro").load(url, function (event) {

            $("#fecharDescricao").click(function (event) {
                event.preventDefault();

                $("#descricaoLivro").html("");
            });
        });
    });


    $("#formComentarios").submit(function (event) {
        event.preventDefault();

        var dados = $(this).serialize();
        var url = $(this).attr("action");

        $.post(url, dados, function (resposta) {
            $("#comentarios").append(resposta);
        });
    });

    var urlCompletar = "/Livros/Procurar";

    $("input#localizar").autocomplete({
        source: urlCompletar,
        minLength: 2
    });
});