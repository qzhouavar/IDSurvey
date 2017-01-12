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

        var myTable = $(eleid).DataTable({
            data: data,
            columns: [
                {data: 'formType',},
                { data: 'area' },

                { data: 'overall_Percent' },
                { data: 'overall_N' },
           

                { data: 'communication_Percent' },
                { data: 'communication_N' },
            
                { data: 'courtesyAndRespect_Percent' },
                { data: 'courtesyAndRespect_N' },
           

                { data: 'accessAndResponsiveness_Percent' },
                { data: 'accessAndResponsiveness_N' },
           

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

        myTable.column(0).nodes().each(function (node, index, dt) {
            if(myTable.cell(node).data() == "Kepro"){
                myTable.cell(node).data('Kepro');
            } else if (myTable.cell(node).data() == "Livanta") {
                myTable.cell(node).data('Livanta');
            } else {
                myTable.cell(node).data('');
            }
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
