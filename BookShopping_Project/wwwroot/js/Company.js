var DataTable;
$(document).ready(function () {
    loadDataTable();
})
function loadDataTable() {
    DataTable = $('#tbldata').DataTable({
        "ajax": {
            "url": "/Admin/Company/GetAll"
        },
        "columns": [

            { "data": "name", "width": "10%" },
            { "data": "streetAddress", "width": "10%" },
            { "data": "city", "width": "10%" },
            { "data": "state", "width": "10%" },
            { "data": "postalCode", "width": "10%" },
            { "data": "phoneNumber", "width": "10%" },
            {
                "data": "isAuthorizedCompany",
                "render": function (data) {
                    if (data) {
                        return `<input type="checkbox" disabled checked/>`;

                    }
                    else {
                        return `<input type="checkbox" disable/>`;

                    }
                }
            },
            {
                "data": "id",
                "render": function (data) {
                    return `
<div class="text-center">
<a class="btn btn-primary"href="/Admin/Company/Upsert/${data}">
<i class="fas fa-edit">
</i>
</a>
<a class="btn btn-danger" OnClick=Delete("/Admin/Company/Delete/${data}")>
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
        title: "want to  Delete?",
        buttons: true,
        icon: "wanrning",
        Text: "delete information!!",
        dangerModel: true
    }).then((willdelete) => {
        if (willdelete) {
            $.ajax({
                url: url,
                type: "Delete",
                success: function (data) {
                    if (data) {
                        toastr.success(data.message),
                            dataTable.ajax.reload();

                    }
                    else {
                        toastr.error(data.message);
                    }
                }
            })
        }
    })
}