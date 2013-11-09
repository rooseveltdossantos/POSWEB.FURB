$(document).ready(function () {
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