function MyApp2($, AmCharts) {
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
                { data: 'overall_N' },
                { data: 'overallBeneficiary' },
                { data: 'overallBeneficiary_N' },
                { data: 'overallRepresentative' },
                { data: 'overallRepresentative_N' },

                { data: 'communication' },
                { data: 'communication_N' },
                { data: 'communicationBeneficiary' },
                { data: 'communicationBeneficiary_N' },
                { data: 'communicationRepresentative' },
                { data: 'communicationRepresentative_N' },

                { data: 'courtesyAndRespect' },
                { data: 'courtesyAndRespect_N' },
                { data: 'courtesyBeneficiary' },
                { data: 'courtesyBeneficiary_N' },
                { data: 'courtesyRepresentative' },
                { data: 'courtesyRepresentative_N' },

                { data: 'accessAndResponsiveness' },
                { data: 'accessAndResponsiveness_N' },
                { data: 'responsivenessBeneficiary' },
                { data: 'responsivenessBeneficiary_N' },
                { data: 'responsivenessRepresentative' },
                { data: 'responsivenessRepresentative_N' }

            ],
            searching: false,
            info: false,
            paging: false,
            ordering: false,
            responsive: true,
            //dom: 'Bfrtip',
            //buttons: [
            //    'copy', 'csv', 'excel', 'pdf', 'print'
            //]
        });
    }

    function createQ19DataTable(eleid, data) {
        //check is datatable or not
        if ($.fn.DataTable.isDataTable(eleid)) {
            $(eleid).DataTable().destroy();
        }

        $(eleid).DataTable({
            data: data,
            columns: [
                { data: 'area' },
                { data: 'q19Total' },
                { data: 'q19Total_N' },
                { data: 'q19Beneficiary' },
                { data: 'q19Beneficiary_N' },
                { data: 'q19Representative' },
                { data: 'q19Representative_N' }

            ],
            searching: false,
            info: false,
            paging: false,
            "ordering": false,
            responsive: true
        });
    }
   
    function createCompositeScoreFigure(eleid, data) {

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
                "valueAlign": "left",
                "autoMargins": false,
                "marginTop":10
              
            },
           
            "graphs": [{
                "id": "v1",
                "fillAlphas": 1,
                "title": "Beneficiary",
                "type": "column",
                "valueField": "beneficiary",
                "fillColorsField": "lineColor",
                "lineColorField": "lineColor",
                "lineColor": "#6b95e0",
                "fixedColumnWidth": 35
            }, {
                "id": "v2",
                "fillAlphas": 1,
                "title": "Beneficiary Representative",
                "type": "column",
                "valueField": "beneRepresentative",
                "fillColorsField": "lineColor",
                "lineColorField": "lineColor",
                "lineColor": "maroon",
                "fixedColumnWidth": 35
            }],
                "allLabels": [
            {
                "text": "Appeals(N=" +data[0].number+")",
                "x": "!650",
                "y": "!11",
                "width": "20%",
                "size": 8,
                "bold": true,
                "align": "right",
             
            }, {
                "text": "Complaints(N=" + data[4].number + ")",
                "x": "!230",
                "y": "!11",
                "width": "20%",
                "size": 8,
                "bold": true,
                "align": "right",

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
                //"axisAlpha": 0,
                //"minHorizontalGap": 80,
                //"gridThickness": 0,
                //"gridAlpha": 0,
                "autoWrap": true,
                "gridPosition": "start",
                "gridAlpha": 0,
                "tickPosition": "start",
                "tickLength": 12.5,
               
                "labelFunction": function (valueText, categoryAxis) {
                    if (valueText == "OverallAppeals")
                        return "Overall";
                    else if (valueText == "CommunicationAppeals")
                        return "Communication";
                    else if (valueText == "CourtesyAppeals")
                        return "Courtesy and Respect";
                    else if (valueText == "ResponsivenessAppeals")
                        return "Access and Responsiveness";
                    else if (valueText == "OverallComplaints")
                        return "Overall";
                    else if (valueText == "CommunicationComplaints")
                        return "Communication";
                    else if (valueText == "CourtesyComplaints")
                        return "Courtesy and Respect";
                    else if (valueText == "ResponsivenessComplaints")
                        return "Access and Responsiveness";
                    return valueText;
                }
            },
            "export": {
                "enabled": true
            }
        });
    }
    //create composite data table
  
    var self = this;

    //public methods
    this.DisplayCompositeScore = function DisplayCompositeScore(url) {
        getData(url, function (d) {
            $(document).ajaxStart();
            createCompositeDataTable('#allResult', d['ALL']);
            createCompositeDataTable('#appealResult', d['APPEALS']);
            createCompositeDataTable('#complaintResult', d['COMPLAINTS']);

            createQ19DataTable('#allResultQ19', d['ALL']);
            createQ19DataTable('#appealResultQ19', d['APPEALS']);
            createQ19DataTable('#complaintResultQ19', d['COMPLAINTS']);
            $(document).ajaxStop();
        });
    }

   



    this.DisplayCompositeFigure = function DisplayCompositeFigure(url) {
        getData(url, function (d) {
            $(document).ajaxStart();
            createCompositeScoreFigure('beneAnalysisChart', d['ALL']);
          
            $(document).ajaxStop();
        });
    }
}

$(document).ready(function () {

   

    var $loading = $('#loading').hide();
    $(document)
      .ajaxStart(function () {
          $loading.show();
      })
      .ajaxStop(function () {
          $loading.hide();
      });
});
