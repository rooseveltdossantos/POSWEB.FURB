$(document).ready(function () {

    $(".Usuario").change(function () {

        if ($(this).val() != '') {
            BuscarLivros($(this).val());
            $("#selectLivro").removeAttr("disabled");
        } else {
            $('#selectLivro').empty();
            $("#selectLivro").attr("disabled", "disabled");
            $("#btnDevolver").css("visibility", "hidden");
            
        }
    });

    $("#selectLivro").change(function () {

        if ($(this).val() != '0') {
            LocalizarEmprestimo($(".Usuario").val(), $(this).val());
            $("#idLivro").val($(this).val());
            $("#btnDevolver").removeAttr("disabled");
            $("#btnDevolver").css("visibility", "visible");
        }
        else {
            $("#btnDevolver").attr("disabled", "disabled");
            $("#btnDevolver").css("visibility", "hidden");
        }
    });

    function BuscarLivros(valor) {

        
        $.ajax({
            url: "../Devolucao/ListarLivrosDoUsuario",
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            data: "{ idUsuario: " + valor + " }",
            dataType: "json",
            beforeSend: function () {
                $("#loading").show();
            },
            success: function (json) {
                var ddl = $('#selectLivro');
                ddl.empty();

                if (json != null) {

                    $(document.createElement('option'))
                        .attr('value', "0")
                        .text("Selecione um Livro")
                        .appendTo(ddl);

                    $(json).each(function() {
                        $(document.createElement('option'))
                            .attr('value', this.LivroId)
                            .text(this.Titulo)
                            .appendTo(ddl);
                    });
                    
                    $("#loading").hide();

                }
            }
        });
    }

    function LocalizarEmprestimo(Usuario, Livro) {
        $.ajax({
            url: "../Devolucao/CarregarEmprestimo",
            type: 'POST',
            data: "{ idLivro: " + Livro + ", idUsuario:" + Usuario + " }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            beforeSend: function () {
            },
            success: function (json) {
                $("#idEmprestimo").val(json.idEmprestimo);
            }
        });
    }



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

    $("input#NomeUsuario").autocomplete({
        source: "/Debitos/Procurar",
        minLength: 2
    });

    // Angel Vitor Lopes :: Inserir Titles para alguns botões
    $(".linkComentarios").attr('title', 'Visualizar/Inserir comentários');
    $(".linkEmprestimo").attr('title', 'Realizar empréstimo do Livro');
    $(".linkReserva").attr('title', 'Realizar reserva do Livro');

});