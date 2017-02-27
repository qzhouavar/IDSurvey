// composite score tables
$(document).ready(function () {
    if (app === undefined || app === null) {
        var app = new MyApp2($, AmCharts);
    }

    app.DisplayCompositeScore("/GetFormTypeByArea/9,10,11");
   

    $('#select-quarter-btn').click(function () {
        $(document).ajaxStart();
        //get current select option
        var quarter = $('#select-quarter').val().toString();
        if (quarter != "..") {
            var compositeUrl = encodeURI("/GetFormTypeByArea/" + quarter);
          
            app.DisplayCompositeScore(compositeUrl);
           
            $('#current-quarter').text('Quarter: ' + $('#select-quarter option:selected').text());
        }
        $(document).ajaxStop();
    });

    $('#select-month-btn').click(function () {
        $(document).ajaxStart();
        //get current select option
        var quarter = $('#select-month').val().toString();
        if (quarter != "..") {
            var compositeUrl = encodeURI("/GetFormTypeByArea/" + quarter);
           
            app.DisplayCompositeScore(compositeUrl);
           
            $('#current-quarter').text('Month: ' + $('#select-month option:selected').text());
        }
        $(document).ajaxStop();
    });
});