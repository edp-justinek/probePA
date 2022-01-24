document.addEventListener("DOMContentLoaded", function(event) {

/*CAROUSEL HOME*/
$(document).ready(function () {
  if ($('.carousel-inner .carousel-item:first').hasClass('active')) {
      $('#carousel-prev').hide();
  }

  $('#carousel-next').on('click', function (e) {
      if ($('.carousel-inner .carousel-item:first').hasClass('active')) {
          $('#carousel-prev').hide();
      }
      if ($('.carousel-inner .carousel-item:nth-child(1)').hasClass('active')) {
          $('#carousel-prev').show();
          $('#carousel-next').show();
      }
      if ($('.carousel-inner .carousel-item:nth-child(2)').hasClass('active')) {
          $('#carousel-next').hide();
      }
  });

  $('#carousel-prev').on('click', function (e) {
      if ($('.carousel-inner .carousel-item:nth-child(2)').hasClass('active')) {
          $('#carousel-next').show();
          $('#carousel-prev').hide();
      }
      if ($('.carousel-inner .carousel-item:nth-child(3)').hasClass('active')) {
          $('#carousel-next').show();
          $('#carousel-prev').show();
      }
  });
});

    
  });