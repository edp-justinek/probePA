document.addEventListener("DOMContentLoaded", function(event) {
        //aktuelles Datum anzeigen im Footer
        var aktuellesDatum = new Date();
        var Jahr = aktuellesDatum.getFullYear();
        var x = document.getElementById("aktuellesJahr").innerHTML = Jahr;
        return(x);
      });
        document.addEventListener("DOMContentLoaded", function(event) {

        var aktuellesDatum2 = new Date();
        var Jahr2 = aktuellesDatum2.getFullYear();
        var date = document.getElementById("aktuellesJahr2").innerHTML = Jahr2;
        return(date);
  });
        
        
