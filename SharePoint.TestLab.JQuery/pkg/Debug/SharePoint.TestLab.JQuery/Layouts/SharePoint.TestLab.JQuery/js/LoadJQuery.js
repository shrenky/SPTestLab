function getJqueryVersion() {
    require(["jquery"], function (myjquery) {
        alert('jquery version： ' + myjquery.fn.jquery);
    });
}

function noJquery() {
    if (typeof ($) == 'undefined') {
        alert('$ is undefined');
    } else {
        alert($);
    }
}