$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url: '/product/getall' },
        "columns": [
            { data: 'imagePath', "width": "15%" },
            { data: 'name', "width": "15%" },
            { data: 'description', "width": "15%" },
            { data: 'specifications', "width": "15%" },
            { data: 'listPrice', "width": "15%" },
            { data: 'standarCost', "width": "15%" },
            { data: 'sellStartDate', "width": "15%" }
        ]
    });
}