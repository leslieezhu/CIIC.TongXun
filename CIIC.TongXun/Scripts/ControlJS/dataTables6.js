

$(document).ready(function () {

    var table = $('#example').DataTable({
        dom: 'Bfrtip',
        ajax: '/Admin/Demo/DataTable6',
        columns: [
            { data: 'readingOrder', className: 'reorder' },
            { data: 'title' },
            { data: 'author' },
            {
                data: 'duration', render: function (data, type, row) {
                    return parseInt(data / 60, 10) + 'h ' + (data % 60) + 'm';
                }
            }
        ],
        columnDefs: [
            { orderable: false, targets: [1, 2, 3] }
        ],
        rowReorder: {
            dataSrc: 'readingOrder'
            //editor: editor
        },
        select: true
    });



});