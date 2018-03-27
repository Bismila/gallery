
function clickLikeForCurrentUserPage(slider, countSlides) {
    //like photo begin
    $('a .like').click(function (e) {
        e.preventDefault();

        console.log(countSlides);
        $(this).toggleClass('active');

        $.ajax({
            type: "GET",
            url: '/Image/CountLikes',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data:
            {
                id: encodeURIComponent($(this).parent().attr("id"))
            },
            success: function (data) {
                for (var i = 0; i < countSlides; i++) {
                    var previewCurrentSlide = slider.getCurrentSlide();
                    var tmp = $('.imgs-block').eq(i);
                    var forSliderCount = $('.img-slide').eq(i);
                    if (i === previewCurrentSlide) {
                        if (tmp.find($('.like-js')).hasClass('active')) {
                            tmp.find($('.like-js')).removeClass('active');
                            forSliderCount.find($('.countLikes-js')).text(data.CountLikes);
                            tmp.find($('.countLikes-js')).text(data.CountLikes);
                        }
                        else {
                            tmp.find($('.like-js')).addClass('active');
                            forSliderCount.find($('.countLikes-js')).text(data.CountLikes);
                            tmp.find($('.countLikes-js')).text(data.CountLikes);
                        }
                        break;

                    }
                    else {
                        continue;
                    }
                        
                }
            },
            failure: function (errMsg) {
                alert(errMsg);
            }
        });
        //like photo end
    });
}

function clickLikeCurrentUserForFriendsPage(slider, countSlides) {
    //like photo begin
    $('a .like').click(function (e) {
        e.preventDefault();

        console.log(countSlides);
        $(this).toggleClass('active');

        $.ajax({
            type: "GET",
            url: '/Image/CountLikes',
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data:
            {
                id: encodeURIComponent($(this).parent().attr("id"))
            },
            success: function (data) {
                for (var i = 0; i < countSlides; i++) {
                    var previewCurrentSlide = slider.getCurrentSlide();

                    var tmp = $('.img-friendBlockGeneral').eq(i);
                    var forSliderCount = $('.img-slide').eq(i);

                    if (i === previewCurrentSlide) {
                        if (tmp.find($('.like-js')).hasClass('active')) {
                            tmp.find($('.like-js')).removeClass('active');
                            forSliderCount.find($('.countLikes-js')).text(data.CountLikes);
                            tmp.find($('.countLikes-js')).text(data.CountLikes);
                        }
                        else {
                            tmp.find($('.like-js')).addClass('active');
                            tmp.find($('.countLikes-js')).text(data.CountLikes);
                            forSliderCount.find($('.countLikes-js')).text(data.CountLikes);
                        }
                        break;

                    }
                    else {
                        continue;
                    }

                }
            },
            failure: function (errMsg) {
                alert(errMsg);
            }
        });
        //like photo end
    });
}
$(document).ready(function () {

    //search friends and users begin
    $("#searchFriendForm").keyup(function (e) {

        e.preventDefault();

        var searchLoginFriend = $("#searchLoginFriend").val();
        searchLoginFriend = encodeURIComponent(searchLoginFriend);

        $('#results-searchFriendForm').load("http://localhost:39210/Friends/SearchedFriends?searchLoginFriend="
            + searchLoginFriend);
    });

    function keypressHandler(e) {
        if (e.which === 13) {
            e.preventDefault(); //stops default action: submitting form
            $(this).blur();
        }
    }

    $('body').keypress(keypressHandler);
    //search friends and users end



});
