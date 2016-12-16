function MyApp($, AmCharts) {
    //Private methods
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
    function createCompositeDataTable(eleid, data) {
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


        data = data.splice(0, 5);

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
                    "lineColor": "red",
                    "width": 2
                }]
            }],

            "graphs": [{
                "id": "g1",
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
                "gridCount": 10
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
                "labelFunction": function (valueText, categoryAxis) {
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
                    else if (valueText == "q11" && type == "COMPLAINTS")
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
    var self = this;

    //public methods
    this.DisplayCompositeScore = function DisplayCompositeScore(url) {
        getData(url, function (d) {
            $(document).ajaxStart();
            createCompositeDataTable('#allResult', d['ALL']);
            createCompositeDataTable('#appealResult', d['APPEALS']);
            createCompositeDataTable('#complaintResult', d['COMPLAINTS']);
            $(document).ajaxStop();
        });
    }

    this.DisplayCompositeOverallChart = function DisplayCompositeOverallChart(url) {
        getData(url, function (d) {
            $(document).ajaxStart();
            createChart("appealResultChart", d['APPEALS']);
            createChart("complaintResultChart", d['COMPLAINTS']);
            $(document).ajaxStop();
        });
    }

    this.DisplayOverallScore = function DisplayOverallScore(url) {
        getData(url, function (d) {
            $(document).ajaxStart();
            createOverallDataTable('#overallRating', d);
            $(document).ajaxStop();
        });
    }

    this.DisplayCompositeFigure = function DisplayCompositeFigure(url) {
        getData(url, function (d) {
            $(document).ajaxStart();
            createCompositeScoreFigure('compositesScoresComplaints', d['COMPLAINTS'].splice(0, 3), 'COMPLAINTS');
            createCompositeScoreFigure('compositesScoresAppeals', d['APPEALS'].splice(0, 3), 'APPEALS');
            createCompositeScoreFigure('communicationsItemsComplaints', d['COMPLAINTS'].splice(0, 3), 'COMPLAINTS');
            createCompositeScoreFigure('communicationsItemsAppeals', d['APPEALS'].splice(0, 3), 'APPEALS');
            createCompositeScoreFigure('courtesyItemsComplaints', d['COMPLAINTS'].splice(0, 2), 'COMPLAINTS');
            createCompositeScoreFigure('courtesyItemsAppeals', d['APPEALS'].splice(0, 2), 'APPEALS');
            createCompositeScoreFigure('responsivenessItemsComplaints', d['COMPLAINTS'].splice(0, 2), 'COMPLAINTS');
            createCompositeScoreFigure('responsivenessItemsAppeals', d['APPEALS'].splice(0, 2), 'APPEALS');
            $(document).ajaxStop();
        });
    }
}

$(document).ready(function () {

    AmCharts.addInitHandler(function (chart) {
        // check if there are graphs with autoColor: true set
        for (var i = 0; i < chart.graphs.length; i++) {
            var graph = chart.graphs[i];
            if (graph.autoColor !== true)
                continue;
            var colorKey = "autoColor-" + i;
            graph.lineColorField = colorKey;
            graph.fillColorsField = colorKey;
            chart.dataProvider[0][colorKey] = "#94ccf5";
            chart.dataProvider[1][colorKey] = "#85b707";
            chart.dataProvider[2][colorKey] = "#0fb526";
            chart.dataProvider[3][colorKey] = "#065a12";
            chart.dataProvider[4][colorKey] = "#1f2f8d";
        }

    }, ["serial"]);

    var $loading = $('#loading').hide();
    $(document)
      .ajaxStart(function () {
          $loading.show();
      })
      .ajaxStop(function () {
          $loading.hide();
      });
});
