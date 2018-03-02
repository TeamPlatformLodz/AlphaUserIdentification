// Write your JavaScript code.
var timer, delay = 500;
$('#manageAccountImageInput').bind('input', function (e) {
    var imgUrl = this.value;
    clearTimeout(timer);
    timer = setTimeout(function () {
        if ( !imageExists(imgUrl) ) {
            imgUrl = '/images/default-user-image.png';
        }
        $('#user-image').attr('src', imgUrl);
    }, delay);
});

function imageExists(url) {
    var isReachable;
    var img = new Image();
    img.onload = function () { isReachable = true; };
    img.onerror = function () { isReachable = false; };
    img.src = url;
    return isReachable;
}

$(document).ready(function () {
    $('.js-example-basic-multiple').select2({width:"resolve"});
});