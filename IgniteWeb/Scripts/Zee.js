
$(document).ready(function() {
 
    $('.login-link').on("click", function (e) {
        $('.login-box').fadeIn("fast");
        return false;
    });
    $('.login-box-close').on("click", function (e) {
        $(this).closest(".login-box").fadeOut("fast");
        return false;
    });
});