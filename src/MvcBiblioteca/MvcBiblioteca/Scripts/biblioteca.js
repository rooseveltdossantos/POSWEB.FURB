$(document).ready(function () {

    //Método que invoca o método que retorna os livros de um determinado usuário.
    $(".Usuario").change(function () {
        $.ajax({
            url: "Devolucao/ListarLivrosDoUsuario",
            type: 'POST',
            data: "{ id: " + $(this).val() + " }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
            },
            success: function (json) {
                var ddl = $('#selectLivro');
                ddl.empty();
                $(json).each(function () {
                    $(document.createElement('option'))
                        .attr('value', this.LivroId)
                        .text(this.Titulo)
                        .appendTo(ddl);
                });

                $("#selectLivro").removeAttr("disabled");
            }
        });
    });

    $("#selectLivro").change(function () {
        $.ajax({
            url: "Devolucao/CarregarEmprestimo",
            type: 'POST',
            data: "{ idLivro: " + $(this).val() + ", idUsuario:" + $(".Usuario").val() + " }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
            },
            success: function (json) {
                $("#idEmprestimo").val(json.idEmprestimo);
            }
        });


        $("#idLivro").val($(this).val());
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