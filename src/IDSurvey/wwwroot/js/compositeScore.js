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

    AmCharts.addInitHandler(function (chart) {
        // check if there are graphs with autoColor: true set
        for (var i = 0; i < chart.graphs.length; i++) {
            var graph = chart.graphs[i];
            if (graph.autoColor !== true)
                continue;
            var colorKey = "autoColor-" + i;
            graph.lineColorField = colorKey;
            graph.fillColorsField = colorKey;
            //for (var x = 0; x < chart.dataProvider.length; x++) {
            //    var color = chart.colors[x];
            //    chart.dataProvider[x][colorKey] = color;
            //}
            chart.dataProvider[0][colorKey] = "#94ccf5";
            chart.dataProvider[1][colorKey] = "#85b707";
            chart.dataProvider[2][colorKey] = "#0fb526";
            chart.dataProvider[3][colorKey] = "#065a12";
            chart.dataProvider[4][colorKey] = "#1f2f8d";
        }

    }, ["serial"]);

    function createChart(eleid, data) {
       

        data = data.splice(0,5);
       
        var chart = AmCharts.makeChart(eleid, {
            "type": "serial",
            "theme": "light",
            "rotate": true,  
            "dataProvider": data,
            "valueAxes": [{
                "minimum": 0,
                "maximum": 100, 
                "position": "left",
                "guides": [{
                    "dashLength": 6,
                    "inside": true,
                    "lineAlpha": 1,
                    "value": 70,
                    "lineColor":"red",
                    "width": 2
                }]
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
                "fixedColumnWidth": 25
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
 

    function createCompositeScoreFigure(eleid, data, type) {
      
        var chart = AmCharts.makeChart(eleid, {
            "type": "serial",
            "theme": "light",
            "rotate": false,
            "dataProvider": data,
            "valueAxes": [{
                "minimum": 0,
                "maximum": 100,
                "axisAlpha": 0.1,
                "autoGridCount": false,
                "gridCount":10
            }],
            "legend": {
                "horizontalGap": 10,
                "useGraphSettings": true,
                "markerSize": 10,
                "align": "center",
                "valueAlign": "left"
            },

            "graphs": [{
                "id": "v1",
                //"balloonText": "<b>[[title]]</b><br><span>[[category]]: <b>[[area1]]</b></span>",
                "fillAlphas": 1,
                //"labelText": "[[value]]",
              
                "title": "Area 1",
                "type": "column",
                
               
                "valueField": "area1",
                "fillColorsField": "lineColor",
                "lineColorField": "lineColor",
                "lineColor": "#94ccf5"
            }, {
                "id": "v2",//"balloonText": "<b>[[title]]</b><br><span style='font-size:14px'>[[category]]: <b>[[area2]]</b></span>",
            
                //"labelText": "[[value]]",
                "fillAlphas": 1,
                "title": "Area 2",
                "type": "column",
              
                
                "valueField": "area2",
                "fillColorsField": "lineColor",
                "lineColorField": "lineColor",
                "lineColor": "#85b707"
            }, {
                "id": "v3",//"balloonText": "<b>[[title]]</b><br><span style='font-size:14px'>[[category]]: <b>[[are3]]</b></span>",
              
                //"labelText": "[[value]]",
                "fillAlphas": 1,
                "title": "Area 3",
                "type": "column",
              
               
                "valueField": "area3",
               
                "fillColorsField": "lineColor",
                "lineColorField": "lineColor",
                "lineColor": "#0fb526"

            }, {
                "id": "v4",//"balloonText": "<b>[[title]]</b><br><span style='font-size:14px'>[[category]]: <b>[[area4]]</b></span>",
               "fillAlphas": 1,
                //"labelText": "[[value]]",
             
                "title": "Area 4",
                "type": "column",
               
              
                "valueField": "area4",
                "fillColorsField": "lineColor",
                "lineColorField": "lineColor",
                "lineColor": "#065a12"
            }, {
                "id": "v5", //"balloonText": "<b>[[title]]</b><br><span style='font-size:14px'>[[category]]: <b>[[area5]]</b></span>",
                "fillAlphas": 1,
                //"labelText": "[[value]]",
              
                "title": "Area 5",
                "type": "column",
                "valueField": "area5",
                "fillColorsField": "lineColor",
                "lineColorField": "lineColor",
                "lineColor": "#1f2f8d"
            }],

            "chartCursor": {
                "categoryBalloonEnabled": false,
                "cursorAlpha": 0,
                "zoomable": false
            },
            "categoryField": "chartCategory",
            "columnSpacing": 0,
            "categoryAxis": {
                "parseDates": false,
                "axisAlpha": 0,
                "minHorizontalGap": 60,
                "gridThickness": 0,
                "gridAlpha": 0,
             
                "autoWrap": true,
                "labelFunction": function (valueText, chartCategory, categoryAxis) {
                    if (valueText == "CommunicationComp")
                        return "Beneficiary-Centered Communications";
                    else if (valueText == "ResponsivenessComp")
                        return "Access and Responsiveness";
                    else if (valueText == "CourtesyComp")
                        return "Courtesy and Respect";
                    else if (valueText == "q7")
                        return "Explained things in a way you could understand";
                    else if (valueText == "q8")
                        return "Spent enough time with you";
                    else if (valueText == "q9")
                        return "Listend carefully to you";
                    else if (valueText == "q6")
                        return "Was as helpful as you thought he or she should be";
                    else if (valueText == "q10")
                        return "Showed respect for what you said";
                    else if (valueText == "q11" && type=="COMPLAINTS")
                        return "Was as responsive to your quality of care complaint as you thought he or she should be";
                    else if (valueText == "q12" && type == "COMPLAINTS")
                        return "Understood the situation related to your quality of care complaint";
                    else if (valueText == "q11" && type == "APPEALS")
                        return "Was as responsive to your appeal as you thought he or she should be";
                    else if (valueText == "q12" && type == "APPEALS")
                        return "Understood the situation related to your appeal";
                     return valueText;

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
    getData("/GetCompositeScoreByArea/6,7,8", function (d) {
        $(document).ajaxStart();
        createCompositeDataTable('#allResult', d['ALL']);
       
        createCompositeDataTable('#appealResult', d['APPEALS']);
        createChart("appealResultChart", d['APPEALS']);
        
        createCompositeDataTable('#complaintResult', d['COMPLAINTS']);
        createChart("complaintResultChart", d['COMPLAINTS']);

        $(document).ajaxStop();
    });
    getData("/GetOverallRatingByArea/6,7,8", function (d) {
        $(document).ajaxStart();
        createOverallDataTable('#overallRating', d);
        $(document).ajaxStop();
    });
    getData("/GetCompositeScoreFigure/6,7,8", function (d) {
        $(document).ajaxStart();
       
        createCompositeScoreFigure('compositesScoresComplaints', d['COMPLAINTS'].splice(0, 3), 'COMPLAINTS');
        createCompositeScoreFigure('compositesScoresAppeals', d['APPEALS'].splice(0, 3), 'APPEALS');

        createCompositeScoreFigure('communicationsItemsComplaints', d['COMPLAINTS'].splice(0, 3), 'COMPLAINTS');
        createCompositeScoreFigure('communicationsItemsAppeals', d['APPEALS'].splice(0, 3), 'APPEALS');

        createCompositeScoreFigure('courtesyItemsComplaints', d['COMPLAINTS'].splice(0, 2), 'COMPLAINTS');
        createCompositeScoreFigure('courtesyItemsAppeals', d['APPEALS'].splice(0, 2), 'APPEALS');
        createCompositeScoreFigure('responsivenessItemsComplaints', d['COMPLAINTS'].splice(0, 2), 'COMPLAINTS');
        createCompositeScoreFigure('responsivenessItemsAppeals', d['APPEALS'].splice(0, 2),'APPEALS');
        $(document).ajaxStop();
    });


    $('#select-quarter-btn').click(function () {
        $(document).ajaxStart();
        //get current select option
       
            var quarter = $('#select-quarter').val().toString();
            if (quarter != "..") {
            var compositeUrl = encodeURI("/GetCompositeScoreByArea/" + quarter);
            var overallUrl = encodeURI("/GetOverallRatingByArea/" + quarter);
            var figureUrl = encodeURI("/GetCompositeScoreFigure/" + quarter);
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
            getData(figureUrl, function (d) {
                createCompositeScoreFigure('compositesScoresComplaints', d['COMPLAINTS'].splice(0, 3), 'COMPLAINTS');
                createCompositeScoreFigure('compositesScoresAppeals', d['APPEALS'].splice(0, 3), 'APPEALS');

                createCompositeScoreFigure('communicationsItemsComplaints', d['COMPLAINTS'].splice(0, 3), 'COMPLAINTS');
                createCompositeScoreFigure('communicationsItemsAppeals', d['APPEALS'].splice(0, 3), 'APPEALS');

                createCompositeScoreFigure('courtesyItemsComplaints', d['COMPLAINTS'].splice(0, 2), 'COMPLAINTS');
                createCompositeScoreFigure('courtesyItemsAppeals', d['APPEALS'].splice(0, 2), 'APPEALS');
                createCompositeScoreFigure('responsivenessItemsComplaints', d['COMPLAINTS'].splice(0, 2), 'COMPLAINTS');
                createCompositeScoreFigure('responsivenessItemsAppeals', d['APPEALS'].splice(0, 2), 'APPEALS');
            });

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
            var overallUrl = encodeURI("/GetOverallRatingByArea/" + quarter);
            var figureUrl = encodeURI("/GetCompositeScoreFigure/" + quarter);
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
            getData(figureUrl, function (d) {
                createCompositeScoreFigure('compositesScoresComplaints', d['COMPLAINTS'].splice(0, 3), 'COMPLAINTS');
                createCompositeScoreFigure('compositesScoresAppeals', d['APPEALS'].splice(0, 3), 'APPEALS');

                createCompositeScoreFigure('communicationsItemsComplaints', d['COMPLAINTS'].splice(0, 3), 'COMPLAINTS');
                createCompositeScoreFigure('communicationsItemsAppeals', d['APPEALS'].splice(0, 3), 'APPEALS');

                createCompositeScoreFigure('courtesyItemsComplaints', d['COMPLAINTS'].splice(0, 2), 'COMPLAINTS');
                createCompositeScoreFigure('courtesyItemsAppeals', d['APPEALS'].splice(0, 2), 'APPEALS');
                createCompositeScoreFigure('responsivenessItemsComplaints', d['COMPLAINTS'].splice(0, 2), 'COMPLAINTS');
                createCompositeScoreFigure('responsivenessItemsAppeals', d['APPEALS'].splice(0, 2), 'APPEALS');

            });
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