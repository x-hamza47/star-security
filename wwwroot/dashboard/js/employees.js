$(document).ready(function () {
    $("#employeesTable").DataTable({
        pagingType: "full_numbers",
        pageLength: 10,
        lengthMenu: [5, 10, 25],
        order: [[0, "asc"]],
        language: {
            search: '_INPUT_',
            searchPlaceholder: 'Search records...',
        },
        columnDefs: [
            {
                targets: -1,
                searchable: false,
                orderable: false
            }
        ],
    });

    $('.edit-emp-btn').on('click', function () {
        const button = $(this);

        const id = button.data('id');
        const empCode = button.data('code');
        const name = button.data('name');
        const email = button.data('email');
        const departmentId = button.data('departmentid');
        const gradeId = button.data('gradeid');
        const client = button.data('client');


        let deptOptions = departments.map(d =>
            `<option value="${d.id}" ${d.id === departmentId ? 'selected' : ''}>${d.name}</option>`
        ).join('');

        let gradeOptions = grades.map(g =>
            `<option value="${g.id}" ${g.id === gradeId ? 'selected' : ''}>${g.name}</option>`
        ).join('');

        Swal.fire({
            title: 'Edit Employee',
            html: `
            <div class="inp-fields d-flex gap-4 flex-column text-start">

                <div class="inp-bx">
                    <label for="empCode">Employee Code</label>
                    <div class="input-wrap">
                        <input id="empCode" type="text" value="${empCode}" placeholder="EMP001" />
                        <span class="material-symbols-rounded">badge</span>
                    </div>
                </div>

                <div class="inp-bx">
                    <label for="empName">Name</label>
                    <div class="input-wrap">
                        <input id="empName" type="text" value="${name}" placeholder="Full Name" />
                        <span class="material-symbols-rounded">person</span>
                    </div>
                </div>

                <div class="inp-bx">
                    <label for="empEmail">Email</label>
                    <div class="input-wrap">
                        <input id="empEmail" type="email" value="${email}" placeholder="example@mail.com" />
                        <span class="material-symbols-rounded">email</span>
                    </div>
                </div>

               <div class="inp-bx">
                <label>Department</label>
                <div class="input-wrap">
                <select id="empDepartment">${deptOptions}</select>
                </div>
            </div>

            <div class="inp-bx">
                <label>Grade</label>
                <select id="empGrade">${gradeOptions}</select>
            </div>

                <div class="inp-bx">
                    <label for="empClient">Client</label>
                    <div class="input-wrap">
                        <input id="empClient" type="text" value="${client}" placeholder="Client Name" />
                        <span class="material-symbols-rounded">business</span>
                    </div>
                </div>

            </div>
        `,
            focusConfirm: false,
            showCancelButton: true,
            confirmButtonText: 'Update',
            background: 'rgba(113, 43, 241, 0.24)',
            color: '#fff',
            backdrop: 'blur(20px)',
            customClass: {
                confirmButton: 'btn-update',
                cancelButton: 'btn-cancel',
                popup: 'edit-modal',
            },
            preConfirm: () => ({
                id: id,
                empCode: document.getElementById('empCode').value,
                name: document.getElementById('empName').value,
                email: document.getElementById('empEmail').value,
                departmentId: document.getElementById('empDepartment').value,
                gradeId: document.getElementById('empGrade').value,
                client: document.getElementById('empClient').value
            })
        }).then((result) => {
            if (result.isConfirmed) updateEmployee(result.value);
        });
    });


    function updateEmployee(data) {
        $.ajax({
            url: '/Employee/Edit',
            type: 'POST',
            data: data,
            success: function (res) {
                Swal.fire({
                    toast: true,
                    position: 'top-end',
                    icon: 'success',
                    title: res,
                    color: '#fff',
                    showConfirmButton: false,
                    showCloseButton: true,
                    timer: 1500,
                    timerProgressBar: true,
                    iconColor: '#008000',
                    background: 'rgba(255, 255, 255, 0.2)',
                    backdrop: 'blur(20px)',
                    customClass: {
                        popup: 'glass-toast'
                    }
                }).then(() => location.reload());
            },
            error: function (xhr) {
                Swal.fire({
                    icon: 'error',
                    title: 'Validation Error',
                    text: xhr.responseText
                });
            }
        });
    }


    $('.delete-emp-btn').on('click', function () {
        const button = $(this);
        const id = button.data('id');

        Swal.fire({
            title: 'Are you sure?',
            text: "This will permanently delete the employee!",
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: 'Yes, delete it!',
            cancelButtonText: 'Cancel',
            background: 'rgba(113, 43, 241, 0.24)',
            color: '#ffff',
            backdrop: 'blur(20px)',
            customClass: {
                confirmButton: 'btn-delete',
                cancelButton: 'btn-cancel',
                popup: 'edit-modal',
            },
            reverseButtons: true
        }).then((result) => {
            if (result.isConfirmed) {
                $.ajax({
                    url: '/Employee/Delete',
                    type: 'POST',
                    data: { id: id },
                    success: function (res) {
                        if (res.success) {
                            button.closest('tr').fadeOut(500, function () {
                                $(this).remove();
                            });
                            Swal.fire({
                                toast: true,
                                position: 'top-end',
                                icon: 'success',
                                title: res.message,
                                showConfirmButton: false,
                                timer: 2000,
                                background: 'rgba(255, 255, 255, 0.2)',
                                backdrop: 'blur(20px)',
                                color: '#fff',
                                iconColor: '#008000',
                                customClass: {
                                    popup: 'glass-toast'
                                }
                            });
                        } else {
                            Swal.fire({
                                icon: 'error',
                                title: 'Error',
                                text: res.message
                            });
                        }
                    },
                    error: function (xhr) {
                        Swal.fire({
                            icon: 'error',
                            title: 'Error',
                            text: xhr.responseText
                        });
                    }
                });
            }
        });
    });
});