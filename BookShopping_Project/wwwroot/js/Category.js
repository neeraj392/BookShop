var dataTable;
$(document).ready(function () {
    loaddatatable();

})
function loaddatatable() {
    dataTable = $('#tbldata').DataTable

        ({
            "ajax": {
                "url": "/Admin/Category/GetAll"
            },
            "columns": [
                { "data": "name", "width": "70%" },
                {
                    "data": "id",
                    "render": function (data) {
                        return `
                          <div class="text-center">
<a class="btn btn-primary"href="/Admin/Category/Upsert/${data}">
<i class="fas fa-edit"></i></a>
<a class="btn btn-danger"Onclick=Delete("/Admin/Category/Delete/${data}")>
      <i class="fas  fa-trash-alt"></i></a>
</div>
     `;
                    }
                }

            ]

        })
}
function Delete(url) {
    swal({
        title: "want to delete?",
        icon: "Warning",
        text: "Delete Information",
        buttons: true,
        dangerModel: true

    }).then((willdelete) => {
        if (willdelete) {
            $.ajax({
                url: url,
                type: "Delete",
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.message);
                        datatable.ajax.reload();
                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}
