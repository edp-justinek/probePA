document.addEventListener("DOMContentLoaded", function(event) {

    $('#AboInfos_' + getURLParameter('vorgangsnr') + getURLParameter('posNr')).show();
    $('#AboDetail_' + getURLParameter('vorgangsnr') + getURLParameter('posNr')).show();

    $('.showAktivUmlUntr').each(function() {
        if ($(this).val() != "false") {
            $(this).show();
        } else {
            $(this).parent().hide();
            $(this).parent().prev().hide();
        }
    });
    $('.showInaktivUmlUntr').each(function() {
        if ($(this).val() != "false") {
            $(this).parent().hide();
            $(this).parent().prev().hide();
        } else {
            $(this).show();
        }
    });

});