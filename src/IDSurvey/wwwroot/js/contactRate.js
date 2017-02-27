$(document).ready(function() {
    $('#contactRate').DataTable({
        searching: false,
        info: false,
        paging: false,
        columnDefs: [
            { type: 'natural', targets: 0 }
        ]
    });
} );