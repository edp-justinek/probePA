  // wenn man nicht angemeldet ist wird der Body ausgeblendet

  if (window.location.href.indexOf("?Benutzerkonto") > -1) {
      $.get("/ajax?action=GetSesionValue&sValue=loggedIn&defValue=false", function(restObj) {
          if (restObj == "false" || restObj == "False") {
              $("body").hide();
              window.location.href = "/?home";
          } else {
              $("body").show();
          }
      })
  }