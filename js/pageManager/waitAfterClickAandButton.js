document.addEventListener("DOMContentLoaded", function(event) {

    //wait symbol   
    var button= document.querySelector("button");
    button.addEventListener("click", check); 
    var a = document.querySelector("a");
    a.addEventListener("click", check); 
    
    function check(){ 
        document.body.style.cursor='wait';
    };
    document.body.style.cursor='wait';
    
    window.onload=function(){document.body.style.cursor='default';}
    
  });