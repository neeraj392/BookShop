var dataTable;
$(document).ready(function () {
    loadDataTable();
})

function loadDataTable() {
    dataTable = $('#tbldata').DataTable({

        "ajax": {
            "url": "/Admin/Product/GetAll"
        },
        "columns": [
         

            { "data": "title", "width": "15%" },
            { "data": "description", "width": "15%" },
            { "data": "author", "width": "15%" },
            { "data": "isbn", "width": "15%" },
            { "data": "price", "width": "15%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
<div class="text-center">
<a class="btn btn-info"href="/Admin/Product/Upsert/${data}">
<i class="fas fa-edit"></i>
<a class="btn btn-danger" OnClick=Delete("/Admin/Product/Delete/${data}")>
<i class="far fa-trash-alt"></i>
</a>
</div>


`;
                }

            }
           


        ]
    })
}
function Delete(url) {
    swal({
        title: "Want to delete data",
        buttons: true,
        icon: "Warning",
        dangerMode: true,
        text: "Delete information!!"


    }).then((willdelete) => {
        if (willdelete) {
            $.ajax({
                url: url,
                type: 'Delete',
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message),
                            datatable.ajax.reload();
                        /*datatable.api(), ajax.reload();*/
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}