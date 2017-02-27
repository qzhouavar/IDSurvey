$(document).ready(function () {
    if (app === undefined || app === null)
    {
        var app = new MyApp($, AmCharts);
    }
    app.DisplayCompositeOverallChart("/GetCompositeScoreByArea/9,10,11");
    app.DisplayCompositeFigure("/GetCompositeScoreFigure/9,10,11");

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



    (function () {
        var beforePrint = function () {
            for (var i = 0; i < AmCharts.charts.length-3; i++) {
                var chart = AmCharts.charts[i];
                chart.div.style.width = "1000px";
                chart.validateNow();
            }
        };

        var afterPrint = function () {
            for (var i = 0; i < AmCharts.charts.length; i++) {
                var chart = AmCharts.charts[i];
                chart.div.style.width = "100%";
                chart.validateNow();
            }
        };

        if (window.matchMedia) {
            var mediaQueryList = window.matchMedia('print');
            mediaQueryList.addListener(function (mql) {
                if (mql.matches) {
                    beforePrint();
                } else {
                    afterPrint();
                }
            });
        }

        window.onbeforeprint = beforePrint;
        window.onafterprint = afterPrint();
    }());
  
});