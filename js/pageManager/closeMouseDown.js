document.addEventListener("DOMContentLoaded", function (event) {
    // Close Modules when clickes outside of it
    var specifiedElementLogin = document.getElementById('LoginInner');
    var specifiedElementAbos = document.getElementById('AbosInner');
    var specifiedElementCart = document.getElementById('ShoppingCardInner');

    document.addEventListener('mousedown', function (event) {
        var isClickInsideLogin = specifiedElementLogin.contains(event.target);
        var isClickInsideAbos = specifiedElementAbos.contains(event.target);
        var isClickInsideCart = specifiedElementCart.contains(event.target);
        if (isClickInsideLogin || isClickInsideAbos || isClickInsideCart) {
        }
        else if (ModuleBool == true) {
            document.getElementById('shoppingCart').classList.remove('show');
            document.getElementById('ABOS').classList.remove('show');
            document.getElementById('login').classList.remove('show');
            document.getElementById('body').classList.remove('overflow');
            ModuleBool = false; 
        }
    });

});