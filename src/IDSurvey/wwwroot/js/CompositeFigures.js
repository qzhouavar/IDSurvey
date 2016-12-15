﻿$(document).ready(function () {
    if (app === undefined || app === null)
    {
        var app = new MyApp($, AmCharts);
    }
    app.DisplayCompositeOverallChart("/GetCompositeScoreByArea/6,7,8");
    app.DisplayCompositeFigure("/GetCompositeScoreFigure/6,7,8");

    $('#select-quarter-btn').click(function () {
        $(document).ajaxStart();
        //get current select option
        var quarter = $('#select-quarter').val().toString();
        if (quarter != "..") {
            var compositeUrl = encodeURI("/GetCompositeScoreByArea/" + quarter);
            var figureUrl = encodeURI("/GetCompositeScoreFigure/" + quarter);
            app.DisplayCompositeOverallChart(compositeUrl);
            app.DisplayCompositeFigure(figureUrl);
            $('#current-quarter').text('Quarter: ' + $('#select-quarter option:selected').text());
            $('#current-quarter-A1').text('Q' + $('#select-quarter option:selected').text());
            $('#current-quarter-A2').text('Q' + $('#select-quarter option:selected').text());
            $('#current-quarter-B1').text('Q' + $('#select-quarter option:selected').text());
            $('#current-quarter-B2').text('Q' + $('#select-quarter option:selected').text());
            $('#current-quarter-B3').text('Q' + $('#select-quarter option:selected').text());
            $('#current-quarter-B4').text('Q' + $('#select-quarter option:selected').text());
            $('#current-quarter-B5').text('Q' + $('#select-quarter option:selected').text());
            $('#current-quarter-B6').text('Q' + $('#select-quarter option:selected').text());
            $('#current-quarter-B7').text('Q' + $('#select-quarter option:selected').text());
            $('#current-quarter-B8').text('Q' + $('#select-quarter option:selected').text());
        }
        $(document).ajaxStop();
    });

    $('#select-month-btn').click(function () {
        $(document).ajaxStart();
        //get current select option
        var quarter = $('#select-month').val().toString();
        if (quarter != "..") {
            var compositeUrl = encodeURI("/GetCompositeScoreByArea/" + quarter);
            var figureUrl = encodeURI("/GetCompositeScoreFigure/" + quarter);
            app.DisplayCompositeOverallChart(compositeUrl);
            app.DisplayCompositeFigure(figureUrl);
            $('#current-quarter').text('Month: ' + $('#select-month option:selected').text());
            $('#current-quarter-A1').text('Month: ' + $('#select-month option:selected').text());
            $('#current-quarter-A2').text('Month: ' + $('#select-month option:selected').text());
            $('#current-quarter-B1').text('Month: ' + $('#select-month option:selected').text());
            $('#current-quarter-B2').text('Month: ' + $('#select-month option:selected').text());
            $('#current-quarter-B3').text('Month: ' + $('#select-month option:selected').text());
            $('#current-quarter-B4').text('Month: ' + $('#select-month option:selected').text());
            $('#current-quarter-B5').text('Month: ' + $('#select-month option:selected').text());
            $('#current-quarter-B6').text('Month: ' + $('#select-month option:selected').text());
            $('#current-quarter-B7').text('Month: ' + $('#select-month option:selected').text());
            $('#current-quarter-B8').text('Month: ' + $('#select-month option:selected').text());
        }
        $(document).ajaxStop();
    });

});