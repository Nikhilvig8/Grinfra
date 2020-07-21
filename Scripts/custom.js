$(document).ready(function() {
  $('.sidebar-nav  a').click(function() {
    $(this)
      .parent()
      .find('ul')
      .slideToggle('fast');
  });
  $('.sidebar-nav > li > a').click(function() {
    $(this)
      .parents()
      .find('a')
      .removeClass('active');
    $(this).addClass('active');
    if ($(this).children().toggleClass) {
    }
  });
});
