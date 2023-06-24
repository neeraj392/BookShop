var dataTable;
$(document).ready(function () {
    loadDataTable();
})
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": {
            "url": "/Admin/User/GetAll"
        },
        "columns": [

            { "data": "name", "width": "15%" },
            { "data": "email", "width": "15%" },
            { "data": "phoneNumber", "width": "15%" },
            { "data": "company.name", "width": "15%" },
            { "data": "role", "width": "15%" },
            {
                "data": {
                    id: "id", lockoutEnd: "lockoutEnd"
                },
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockout = new Date(data.lockoutEnd).getTime();
                    if (lockout > today)
                        //User Locked
                        return `
                        <div class="text-center">
                            <a onclick=lockUnclock('${data.id}') class="btn btn-danger text-white" style="cursor:pointer">
                                Unlock
                            </a>
                        </div> 
                        `;

                    else {
                        //User UnLocked
                        return `
                        <div class="text-center">
                            <a onclick=lockUnclock('${data.id}') class="btn btn-success text-white" style="cursor:pointer">
                                lock
                            </a>
                        </div> 
                        `;

                    }
                }
            }

        ]
    })
}
function lockUnclock(id) {
    $.ajax({
        type: "POST",
        url: "/Admin/User/lockUnlock",
        data: JSON.stringify(id),
        contentType: "application/Json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            }
            else {
                toastr.error(data.message);
            }
        }
    })
}