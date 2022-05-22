

function fnPrevious(pageNumber) {
    let selectValue = document.getElementById('categoriaId').value;
    let searchValue = document.getElementById('search').value;
    let pageId = pageNumber

    $.ajax({
        url: '/Pelicula/GetPeliculas',
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        data: {
            pageId: pageId,
            categoriaId: selectValue,
            search: searchValue
        },
        dataType: 'html'
    })
        .success(function (result) {
            $('#result').html(result);
        })
        .error(function (xhr, status) {
            alert(status);
        })
}

function fnNext(pageNumber) {
    let selectValue = document.getElementById('categoriaId').value;
    let searchValue = document.getElementById('search').value;
    let pageId = pageNumber

    $.ajax({
        url: '/Pelicula/GetPeliculas',
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        data: {
            pageId: pageId,
            categoriaId: selectValue,
            search: searchValue
        },
        dataType: 'html'
    })
        .success(function (result) {
            $('#result').html(result);
        })
        .error(function (xhr, status) {
            alert(status);
        })
}