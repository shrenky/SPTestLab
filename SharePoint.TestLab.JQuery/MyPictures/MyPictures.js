$(document).ready(function() {
    //LoadPics();
    LoadNews();
});
var currentPic = 0;
function GetPictures() {
    if (currentPic == MyPictures.length - 1) {
        currentPic = 0;
    }

    $('#MyPicture').attr('src', MyPictures[currentPic]);
    currentPic++;
}

function LoadPics() {
    if (typeof MyPictures != "undefined") {
        GetPictures();
        setInterval('GetPictures()', 3000);
    }
}

function LoadNews() {
    LoadNewsItems();
    //$('.NewsStory').on('click','.NewsStoryHeader', );
}

function LoadNewsItems() {
    var newsUrl = "/sub/_vti_bin/ListData.svc/News/";

    $.ajax({
        type: 'GET',
        url: newsUrl,
        dataType: 'json',
        success: function(data) {
            $.each(data.d.results, function(i, result) {
                var newsClone = $('#NewsStory-Template').clone();
                newsClone.removeAttr('id');
                newsClone.find('.NewsStoryHeader').text(result.Title);
                newsClone.data("articleId", result.Id);
                newsClone.show();
                newsClone.click(function () {
                    var article = $(this).find('.NewsArticle');
                    if (article.is(':visible')) {
                        article.hide();
                    } else {
                        if (article.html().trim() == "") {
                            LoadArticle(article);
                        }
                    }

                    $('.NewsArticle').hide();
                    article.show();
                });
                $('#News').append(newsClone);
            });
        },
        error: function() {
            alert("error");
        }
        });
}

function LoadArticle(art) {
    var id = art.parent().data('articleId');
    var newsUrl = "/sub/_vti_bin/ListData.svc/News(" + id + ")/Body";
    $.getJSON(newsUrl, function(data) {
        art.append($(data.d.Body));
    });
}