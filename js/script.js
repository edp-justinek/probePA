jQuery(document).ready(function($) {
    console.log("edp demo-webshop is ready!");
    try {
        StartPageFunction();
    } catch (e) {}

    // navi
    $("#A_Navi_" + getURLParameter("praesgrp")).addClass("active");

    // show warenkorb by loading page
    if (getURLParameter("showCard") == "true") {
        // show warenkorb
        setTimeout(function() { ModuleBool = true; }, 10);
        $('#shoppingCart').addClass('show');
        $('#body').addClass('overflow');
    }

    // title
    try {
        $('#DV_title').html(getURLParameter("title"))
    } catch (e) {
        // no title field
    }
    // default artikel-image
    $(".img-uebersicht").each(function() {
        if (!$.trim($(this).html()).length || $.trim($(this).html()).length < 52) {
            $(this).html("<img class='img-responsive' src='http://demoshop.faros.ch/images/artikel/no-image-edi.png' alt='No image available!' />");
        }
    });
    // Anzahl Warenkorb anzeigen (mobile)
    if ($('#AnzahlArtikel').text() != 0) {
        $('.warenkorb-anzahl').show()
    } else {
        $('.warenkorb-anzahl').hide();
    }

    // Zusatz-Text Check
    $('.artikelZusatzText').each(function() {
        if ($('.artikelZusatzText').children().length == 0) {
            $(this).parent().show();
        } else {
            $(this).parent().hide();
        }
    });


    // Abo-Beginn Show/hide
    $('.AboBeginn').each(function() {
        $(this).hide();

    });

    if (window.location.search.indexOf('praesgrp=ABOS') == 1) {
        $('.AboBeginn').show();
    };
    if (window.location.search.indexOf('navgrp=ABOS') == 1) {
        $(".AboBeginn").show();
    };
    if (window.location.search.indexOf('navgrp=ARTN') > -1) {
        $("#AngebotBtn").hide();
    };
    if (window.location.search.indexOf('navgrp=SPEZ') > -1) {
        $("#AngebotBtn").hide();
    };

    // checkboxes show/hide
    $('.art_merkmale').hide()

    $('.MSPAN_Checkbox').each(function() {
        if ($(this).attr("value") != null && $(this).attr("value") != "") {
            $(this).parent().parent().show();
        }
    });

    // hide empty MSPAN_Checkboxes
    $('.MSPAN_Text').each(function() {
        if ($(this).text().length == 0) {
            $(this).prev().hide();
            $(this).next('br').hide();
        }
    });

    // check if lagerMengeStatus should be shown 
    $('.lager-menge-status.5').show();
    $('.lager-menge-status.5').next().hide();

    // PDF-Download Button / Show Warenkorb
    $('#pdfDownloadButton').click(function() {
        console.log(this);
        window.open(this.dataset.link, '_blank');

    });

    //Check if WarenkorbButtonJS should be show

    $(".showWarenkorb").each(function() {
        var showwarenkorb = $(this).data('showwarenkorb');

        if (showwarenkorb !== undefined) {
            if (showwarenkorb == 0) {
                $('.warenkorb-btn0').hide();
            } else {
                $('.warenkorb-btn').show();
            };
            if (showwarenkorb.length === 0) {
                $('.warenkorb-btn').show();
            };
        }
    });

    // Check if Artikel with Ausgabe
    $('.DV_Checkbox').remove();
    $('.DV_Checkbox1').show();

    // Show/Hide Ausgabe-Picker (check for active ausgabe)
    $('.selectpicker').each(function() {
        var $select = $(this);
        if ($select.find('option').length === 0) {
            $select.hide();
            $(this).parent().parent().find(".WarenkorbButtonJS").attr('disabled', 'disabled');
            // $(this).parent().siblings().find("#WarenkorbButtonJS").attr('disabled', 'disabled');
            $(this).next().removeClass('hidden');
        }
    });

    // Autor, ISBN, Erscheinung-Check 
    $('.informationValue').each(function() {
        if (!$.trim($(this).html()).length) {
            $(this).prev('.informationText').hide();
            $(this).next('br').hide();
        } else {
            $(this).prev('.informationText').show();
            $(this).next('br').show();
        }
    })

    // warenkorb 
    if ($("#SP_AnzArtikelWarenkorb").html() == "0") {
        $("#DV_WarenkorbFull").hide();
        $(".DV_WarenkorbEmpty").show();
    } else {
        $("#DV_WarenkorbFull").show();
        $(".DV_WarenkorbEmpty").hide();
    }

    // check for alert
    if (getURLParameter("error") != "null") {
        if (document.referrer.indexOf(window.location.hostname) >= 0) {
            $('#errorModalContent').html(decodeURIComponent(getURLParameter("error")));
            $('#errorModal').modal('show');
        }
    }

    // check for info
    if (getURLParameter("info") != "null") {
        if (document.referrer.indexOf(window.location.hostname) >= 0) {
            $('#infoModalContent').html(decodeURIComponent(getURLParameter("info")));
            $('#infoModal').modal('show');
        }
    }

    // change subscription
    $('#AboInfos_' + getURLParameter('vorgangsnr') + getURLParameter('posNr')).show();
    $('#AboDetail_' + getURLParameter('vorgangsnr') + getURLParameter('posNr')).show();

    $(".beenden").change(function() {
        if ($('input[name=beenden]').is(':checked')) {
            $('#AboBeendenBtn').removeAttr('disabled');
        } else {
            $('#AboBeendenBtn').attr('disabled');
        }
    });

    $("#comment").change(function() {
        document.getElementById('beenden17').value = "Anderer Grund / " + $('textarea#comment').val();
        // alert($('textarea#comment').val())
    });

    $('.showAktivUmlUntr').each(function() {
        datum = $(this).val();
        var arr = datum.split('.');
        var datum_neu = new Date(arr[2], arr[1] - 1, arr[0]);
        var date = new Date();
        var today = new Date(date.getFullYear(), date.getMonth(), date.getDate(), 0, 0, 0);
        var vorgangsnr = $(this).closest("table").find("tbody>tr").first().attr("vorgangsnr");
        var posnr = $(this).closest("table").find("tbody>tr").first().attr("posnr");

        //console.log(datum_neu >= todayDate);
        if (datum_neu >= today) {
            $(this).show();
            $('#AboStatus' + vorgangsnr + posnr).show();

        } else {
            $(this).parent().hide();
            $(this).parent().prev().hide();

        }

    });

    $().button('toggle')
    $('.dropdown-toggle').dropdown()

    // Benutzerkonto abo bestellungen

    $('#order-table').dataTable({
        "language": {
            "emptyTable": "Keine Bestellungen vorhanden",
            "zeroRecords": "No records to display"
        }
    });

    if ($('#order-table_wrapper > div:first').hasClass('row')) {
        $('#order-table_wrapper > div:first').hide();
    }
    if ($('#order-table_wrapper > div:last').hasClass('row')) {
        $('#order-table_wrapper > div:last').hide();
    }

});

function ShowAboInfos(vorgangsnr, posNr, gesperrt, status) {
    if (status == 0) {
        $("#Aboaktiv_" + vorgangsnr).show();
    }
    if (status == 1) {
        $("#Abogesperrt_" + vorgangsnr).show();
    }
    $('#AboInfos_' + vorgangsnr + posNr).modal('show');
}


function CancelAboUmleitungUnterbruch(arguments) {
    if (arguments == "false") {
        alert("Kann zurzeit nicht gelöscht werden. Ansonsten Support kontaktieren.");
    } else {
        if (confirm("Wirklich entfernen?")) {
            document.location.href = "/shop/cancelAboUmleitungUnterbruch?args=" + arguments;
        }
    }
}

var t_vorgangsnr;
var t_posNr;
var t_artikel;

function AboMutation(mutType, vorgangsnr, posNr, artikel) {
    t_vorgangsnr = vorgangsnr;
    t_posNr = posNr;
    t_artikel = artikel;
    switch (mutType) {
        case "umleiten":
            $('#aboUmleitenModal').modal('show');
            break;

        case "unterbrechen":
            $('#aboUnterbrechenModal').modal('show');
            break;

        case "beenden":
            if (confirm("Wirklich beenden?")) {
                document.location.href = "/shop/aboCancel?vorgangsnr=" + vorgangsnr + "&posNr=" + posNr;
            }
            break;
    }
}

function getURLParameter(name) {
    return decodeURI(
        (RegExp(name + '=' + '(.+?)(&|$)').exec(location.search) || [, null])[1]
    );
}

$(function() {
    $('#von').datepicker({
        format: "dd/mm/yyyy",
        language: "de",
        changeMonth: true,
        changeYear: true
    });

    $('#bis').datepicker({
        format: "dd/mm/yyyy",
        language: "de",
        changeMonth: true,
        changeYear: true
    })

    $('#gebdatum').datepicker({
        format: "dd/mm/yyyy",
        language: "de",
        changeMonth: true,
        changeYear: true
    })
});

function ABOS() {
    setTimeout(function() { ModuleBool = true; }, 10);
    $('#ABOS').addClass('show');
    $('#body').addClass('overflow');
}

function ModalClose() {
    $('#login').removeClass('show');
    $('#body').removeClass('overflow');
    $('#shoppingCart').removeClass('show');
    $('#body').removeClass('overflow');
    $('#ABOS').removeClass('show');
    $('#body').removeClass('overflow');
    ModuleBool = false;
}

// Abo Kündigen

function AboBeenden() {
    var grund = 000;
    jQuery.each($(".beenden"), function(i, myBeendenButton) {
        if ($(myBeendenButton).is(':checked')) {
            grund = $(myBeendenButton).val();
            document.location.href = "/shop/aboCancel?vorgangsnr=" + getURLParameter('vorgangsnr') + "&posNr=" + getURLParameter('posNr') + "&grund=" + grund + "&reUrl=" + escape("/?Benutzerkonto_Daten");
        }
    });
};

function GrundWohnort() {
    $('#GrundWohnort').show();
    $("#WohnortBtn").addClass("BeendenBtn-Aktiv");
    $('#GrundZeitung').hide();
    $("#ZeitungBtn").removeClass("BeendenBtn-Aktiv");
    $('#GrundLesen').hide();
    $("#LesenBtn").removeClass("BeendenBtn-Aktiv");
}

function GrundZeitung() {
    $('#GrundZeitung').show();
    $("#ZeitungBtn").addClass("BeendenBtn-Aktiv");
    $('#GrundWohnort').hide();
    $("#WohnortBtn").removeClass("BeendenBtn-Aktiv");
    $('#GrundLesen').hide();
    $("#LesenBtn").removeClass("BeendenBtn-Aktiv");
}

function GrundLesen() {
    $('#GrundLesen').show();
    $("#LesenBtn").addClass("BeendenBtn-Aktiv");
    $('#GrundWohnort').hide();
    $("#WohnortBtn").removeClass("BeendenBtn-Aktiv");
    $('#GrundZeitung').hide();
    $("#ZeitungBtn").removeClass("BeendenBtn-Aktiv");
}

function getValue(textarea) {
    // Angegebenes Element vom Typ texarea?
    if (textarea.type == 'textarea') {
        // Inhalt des Eingabefeldes
        var value = document.formular.feld.value;

        // Eingabefeld leer?
        if (value.length == 0)
        // Wenn ja, entsprechende Meldung ausgeben
            alert('Texarea ist leer!');
        else
        // Ausgabe des in der Textarea gespeicherten Inhalts
            alert('Inhalt des Feldes: ' + value);
    }
}

function getURLParameter(name) {
    return decodeURI(
        (RegExp(name + '=' + '(.+?)(&|$)').exec(location.search) || [, null])[1]
    );
}

// Abo Umleiten

function GoEditAdresse(myAdress) {
    document.location.href = $(myAdress).attr("formaction") + "&successUrlSession=" + encodeURIComponent(document.location.href);
}

function AboUmleiten() {
    $('#vorgangsnr').val(getURLParameter('vorgangsnr'));
    $('#posNr').val(getURLParameter('posNr'));
    $('#artikel').val(getURLParameter('artikel'));
    $('#adrssnr').val($('input[name=umleitungsadr]:checked').val());
    $('#successUrl').val(document.location.href);
    $('form').submit();
}

// Abo Unterbrechen

function AboUnterbruch() {
    $('#vorgangsnr').val(getURLParameter('vorgangsnr'));
    $('#posNr').val(getURLParameter('posNr'));
    $('#artikel').val(getURLParameter('artikel'));
    $('#successUrl').val(document.location.href);
    $('form').submit();
}

//  Navbar/Module-Buttons
var ModuleBool = false;

function login() {
    setTimeout(function() {
        ModuleBool = true;
    }, 10);
    $('#login').addClass('show');
    $('#body').addClass('overflow');
};

function shoppingCart() {
    setTimeout(function() {
        ModuleBool = true;
    }, 10);
    $('#shoppingCart').addClass('show');
    $('#body').addClass('overflow');
};

function ABOS() {
    setTimeout(function() {
        ModuleBool = true;
    }, 10);
    $('#ABOS').addClass('show');
    $('#body').addClass('overflow');
};

function ModalClose() {
    $('#login').removeClass('show');
    $('#body').removeClass('overflow');
    $('#shoppingCart').removeClass('show');
    $('#body').removeClass('overflow');
    $('#ABOS').removeClass('show');
    $('#body').removeClass('overflow');
    ModuleBool = false;
};

var ModuleBool = false;

function login() {
    setTimeout(function() {
        ModuleBool = true;
    }, 10);
    $('#login').addClass('show');
    $('#body').addClass('overflow');
}

function shoppingCart() {
    setTimeout(function() {
        ModuleBool = true;
    }, 10);
    $('#shoppingCart').addClass('show');
    $('#body').addClass('overflow');
}