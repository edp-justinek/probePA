document.addEventListener("DOMContentLoaded", function(event) {

    $.get("/ajax?action=GetSesionValue&sValue=loggedIn&defValue=false", function(restObj) {
        if (restObj == "false" || restObj == "False") {
            document.getElementById('loginModul').classList.add('show_Benutzer');
        } else {
            document.getElementById('loggedInModul').classList.add('show_Benutzer');
        }
    });

});