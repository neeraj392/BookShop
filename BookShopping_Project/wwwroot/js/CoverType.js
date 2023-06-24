

var Datatable;
$(document).ready(function () {
    loadDataTable();
})
function loadDataTable() {
    Datatable = $('#tbldata').DataTable
        ({
            "ajax": {
                "url": "/Admin/Covertype/GetAll",
            },
            "columns":
                [
                    { "data": "name", "width": "70%" },
                    {
                        "data": "id",
                        "render": function (data) {
                            return `
<div class="text-center">
<a class="btn btn-primary"href="/Admin/Covertype/Upsert/${data}">
<i class="fas fa-edit">
</i>
</a>
<a class="btn btn-danger" OnClick=Delete("/Admin/Covertype/Delete/${data}")>
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