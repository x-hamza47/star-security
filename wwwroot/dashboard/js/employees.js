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

    $('.edit-dept-btn').on('click', function () {
        const button = $(this);

        // Get employee data from data attributes
        const id = button.data('id'); // make sure your button has data-id
        const empCode = button.data('code');
        const name = button.data('name');
        const email = button.data('email');
        const department = button.data('department');
        const grade = button.data('grade');
        const client = button.data('client');

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
                    <label for="empDepartment">Department</label>
                    <div class="input-wrap">
                        <input id="empDepartment" type="text" value="${department}" placeholder="HR / IT / FIN" />
                        <span class="material-symbols-rounded">apartment</span>
                    </div>
                </div>

                <div class="inp-bx">
                    <label for="empGrade">Grade</label>
                    <div class="input-wrap">
                        <input id="empGrade" type="text" value="${grade}" placeholder="A / B / C" />
                        <span class="material-symbols-rounded">star</span>
                    </div>
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
                department: document.getElementById('empDepartment').value,
                grade: document.getElementById('empGrade').value,
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


$('.delete-dept-btn').on('click', function () {
    const button = $(this);
    const id = button.data('id');

    Swal.fire({
        title: 'Are you sure?',
        text: "This will permanently delete the department!",
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
                url: '/Department/Delete',
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