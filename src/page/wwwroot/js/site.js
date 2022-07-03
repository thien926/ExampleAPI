// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function Logout() 
{
    console.log("Thiện");

    $.ajax({
        url: '/Home/Logout',
        type: "POST",
        dataType: "json",
        data: {
        },
        success: function (data) {
            location.href = "/Home";
        },
        error: function (e) {
            // alert(e.responseText);
        }
    });
}