require.config({
    paths: {
        'jquery': 'jquery-1.11.0.min'
    }
});
define(['jquery'], function (myjquery) {
    return myjquery.noConflict(true);
});