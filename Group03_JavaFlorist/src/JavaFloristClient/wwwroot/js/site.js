// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function showToast(message) {
    var toast = document.getElementById('toast');
    toast.innerText = message;
    toast.style.opacity = 1;

    setTimeout(function () {
        toast.style.opacity = 0;
    }, 3000); // Hiển thị toast trong 3 giây
}