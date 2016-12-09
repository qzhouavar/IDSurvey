// composite score tables
$(document).ready(function () {
    //get data from server
    function getData(targeturl, callback) {
        $.ajax({
            url: targeturl,
            success: function (d) {
                callback(d);
            }
        });
    }
    //create composite data table
    function createCompositeDataTable(eleid, data)
    {
        //check is datatable or not
        if ($.fn.DataTable.isDataTable(eleid)) {
            $(eleid).DataTable().destroy();
        }
        
        $(eleid).DataTable({
            data: data,
            columns: [
                { data: 'area' },
                { data: 'overall' },
                { data: 'communication' },
                { data: 'courtesyAndRespet' },
                { data: 'accessAndResponsiveness' }
            ],
            searching: false,
            info: false,
            paging: false
        });
    }

    var $loading = $('#loading').hide();
    $(document)
      .ajaxStart(function () {
          $loading.show();
      })
      .ajaxStop(function () {
          $loading.hide();
      });

    //create composite data table
    function createOverallDataTable(eleid, data) {
        //check is datatable or not
        if ($.fn.DataTable.isDataTable(eleid)) {
            $(eleid).DataTable().destroy();
        }
        $(eleid).DataTable({
            data: data,
            columns: [
                { data: 'area' },
                { data: 'total' },
                { data: 'appeals' },
                { data: 'complaints' }
            ],
            searching: false,
            info: false,
            paging: false
        });
    }

    //get initial data from server (quarter 8)
    getData("/GetCompositeScoreByArea/3,4,5", function (d) {
        $(document).ajaxStart();
        createCompositeDataTable('#allResult', d['ALL']);
        createCompositeDataTable('#appealResult', d['APPEALS']);
        createCompositeDataTable('#complaintResult', d['COMPLAINTS']);
        $(document).ajaxStop();
    });
    getData("/GetOverallRatingByArea/3,4,5", function (d) {
        $(document).ajaxStart();
        createOverallDataTable('#overallRating', d);
        $(document).ajaxStop();
    });

    $('#select-quarter-btn').click(function () {
        $(document).ajaxStart();
        //get current select option
        var quarter = $('#select-quarter').val().toString();
        var compositeUrl = encodeURI("/GetCompositeScoreByArea/" + quarter);
        var overallUrl = encodeURI("/GetOverallRatingByArea/" + quarter);
        getData(compositeUrl, function (d) {
            createCompositeDataTable('#allResult', d['ALL']);
            createCompositeDataTable('#appealResult', d['APPEALS']);
            createCompositeDataTable('#complaintResult', d['COMPLAINTS']);
        });
        getData(overallUrl, function (d) {
            createOverallDataTable('#overallRating', d);
        });
        $('#current-quarter').text('Quarter: ' + $('#select-quarter option:selected').text());
        $(document).ajaxStop();
    });

    $('#select-month-btn').click(function () {
        $(document).ajaxStart();
        if ($('#select-month option:selected').text() != '..') {
            //get current select option
            var quarter = $('#select-month').val().toString();
            var compositeUrl = encodeURI("/GetCompositeScoreByArea/" + quarter);
            var overallUrl = encodeURI("/GetOverallRatingByArea/" + quarter);
            getData(compositeUrl, function (d) {
                createCompositeDataTable('#allResult', d['ALL']);
                createCompositeDataTable('#appealResult', d['APPEALS']);
                createCompositeDataTable('#complaintResult', d['COMPLAINTS']);
            });
            getData(overallUrl, function (d) {
                createOverallDataTable('#overallRating', d);
            });
            $('#current-quarter').text('Month: ' + $('#select-month option:selected').text());
        }
        $(document).ajaxStop();
    });

});