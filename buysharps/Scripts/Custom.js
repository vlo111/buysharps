/* When the user clicks on the button,
toggle between hiding and showing the dropdown content */
function myFunction() {
    document.getElementById("myDropdown").classList.toggle("show");
}

// Close the dropdown menu if the user clicks outside of it
window.onclick = function (event) {
    if (!event.target.matches('.dropbtn')) {
        var dropdowns = document.getElementsByClassName("dropdown-content");
        var i;
        for (i = 0; i < dropdowns.length; i++) {
            var openDropdown = dropdowns[i];
            if (openDropdown.classList.contains('show')) {
                openDropdown.classList.remove('show');
            }
        }
    }
}

jQuery(document).on("click", '#SendForm', function (jsonData) {

    jQuery("form").removeData("validator");
    jQuery("form").removeData("unobtrusiveValidation");
    jQuery.validator.unobtrusive.parse("form");

    if (jQuery("form").valid()) {
        var data = jQuery("form").serialize();
        jQuery.ajax({
            url: '' + jsonData.currentTarget.name,
            type: 'POST',
            data: data,
            success: function (response) {
                alertify.set('notifier', 'position', 'top-right');
                if (response.success) {
                    alertify.success("The data has been successfully " + response.success);
                    $('#myModal').modal('toggle');
                    setTimeout(function () {
                        location.reload();
                    }, 1500);
                }
                else
                    alertify.error(response.error);


            },
            error: function (response) {
                alertify.error(response);
            }
        });
    }
});
