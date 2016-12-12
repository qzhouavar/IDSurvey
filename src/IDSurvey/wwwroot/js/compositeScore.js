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

    function createChart(eleid, data) {
   
        AmCharts.addInitHandler(function(chart) {
            // check if there are graphs with autoColor: true set
            for(var i = 0; i < chart.graphs.length; i++) {
                var graph = chart.graphs[i];
                if (graph.autoColor !== true)
                    continue;
                var colorKey = "autoColor-"+i;
                graph.lineColorField = colorKey;
                graph.fillColorsField = colorKey;
                for(var x = 0; x < chart.dataProvider.length; x++) {
                    var color = chart.colors[x]
                    chart.dataProvider[x][colorKey] = color;
                }
            }
  
        }, ["serial"]);


        data = data.splice(0,5);
       
        var chart = AmCharts.makeChart(eleid, {
            "type": "serial",
            "theme": "light",
            "rotate": true,
           
            "dataProvider": data,
            "valueAxes": [{
                "minimum": 0,
                "maximum": 100,
                "axisAlpha": 0,
                "position": "left",
            }],

            "graphs": [{
                "id": "g1",
                "balloonText": "Area [[area]]<br><b><span style='font-size:14px;'>[[overall]]</span></b>",
                "bullet": "round",
                "bulletSize": 8,
                "autoColor": true,
                "lineThickness": 2,
                "fillAlphas": 0.8,
                "valueField": "overall",
                "type": "column",
                "precision": 1,
                "labelText": "[[value]]",
            }],


            "chartCursor": {
                "categoryBalloonEnabled": false,
                "cursorAlpha": 0,
                "zoomable": false
            },
            "categoryField": "area",
            "categoryAxis": {
                "parseDates": false,
                "axisAlpha": 0,
                "minHorizontalGap": 60,
                "gridThickness": 0,
                "gridAlpha": 0,
                "labelFunction": function (valueText, quarter, categoryAxis) {
                    return "Area " + valueText;
                }
            },
            "export": {
                "enabled": true
            }
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
        createChart("appealResultChart", d['APPEALS']);
        
        createCompositeDataTable('#complaintResult', d['COMPLAINTS']);
        createChart("complaintResultChart", d['COMPLAINTS']);

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
            createChart("appealResultChart", d['APPEALS']);
            
            createCompositeDataTable('#complaintResult', d['COMPLAINTS']);
            createChart("complaintResultChart", d['COMPLAINTS']);
        });
        getData(overallUrl, function (d) {
            createOverallDataTable('#overallRating', d);
        });
        $('#current-quarter').text('Quarter: ' + $('#select-quarter option:selected').text());
        $(document).ajaxStop();
    });

    $('#select-month-btn').click(function () {
        $(document).ajaxStart();
        //get current select option
        var quarter = $('#select-month').val().toString();
        var compositeUrl = encodeURI("/GetCompositeScoreByArea/" + quarter);
        var overallUrl = encodeURI("/GetOverallRatingByArea/" + quarter);
        getData(compositeUrl, function (d) {
            createCompositeDataTable('#allResult', d['ALL']);


            createCompositeDataTable('#appealResult', d['APPEALS']);
            createChart("appealResultChart", d['APPEALS']);

            createCompositeDataTable('#complaintResult', d['COMPLAINTS']);
            createChart("complaintResultChart", d['COMPLAINTS']);
        });
        getData(overallUrl, function (d) {
            createOverallDataTable('#overallRating', d);
        });
        $('#current-quarter').text('Month: ' + $('#select-month option:selected').text());
        $(document).ajaxStop();
    });

});