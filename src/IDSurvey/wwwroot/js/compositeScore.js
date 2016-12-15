// composite score tables
$(document).ready(function () {
    if (app === undefined || app === null) {
        var app = new MyApp($, AmCharts);
    }

    app.DisplayCompositeScore("/GetCompositeScoreByArea/6,7,8");
    app.DisplayOverallScore("/GetOverallRatingByArea/6,7,8");

    $('#select-quarter-btn').click(function () {
        $(document).ajaxStart();
        //get current select option
        var quarter = $('#select-quarter').val().toString();
        if (quarter != "..") {
            var compositeUrl = encodeURI("/GetCompositeScoreByArea/" + quarter);
            var overallUrl = encodeURI("/GetOverallRatingByArea/" + quarter);
            app.DisplayCompositeScore(compositeUrl);
            app.DisplayOverallScore(overallUrl);
            $('#current-quarter').text('Quarter: ' + $('#select-quarter option:selected').text());
        }
        $(document).ajaxStop();
    });

    $('#select-month-btn').click(function () {
        $(document).ajaxStart();
        //get current select option
        var quarter = $('#select-month').val().toString();
        if (quarter != "..") {
            var compositeUrl = encodeURI("/GetCompositeScoreByArea/" + quarter);
            var overallUrl = encodeURI("/GetOverallRatingByArea/" + quarter);
            app.DisplayCompositeScore(compositeUrl);
            app.DisplayOverallScore(overallUrl);
            $('#current-quarter').text('Month: ' + $('#select-month option:selected').text());
        }
        $(document).ajaxStop();
    });
});