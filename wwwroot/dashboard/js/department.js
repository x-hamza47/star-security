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

        const id = button.data('id');
        const name = button.data('name');
        const code = button.data('code');
        const description = button.data('description');
        const isActive = button.data('active');

        Swal.fire({
            title: 'Edit Department',
            html: `
                <div class="inp-fields d-flex gap-4 flex-column text-start">
                    <div class="inp-bx">
                        <label for="deptName">Department Name</label>
                        <div class="input-wrap">
                            <input id="deptName" type="text" placeholder="Department Name" value="${name}" />
                            <span class="material-symbols-rounded">apartment</span>
                        </div>
                    </div>

                    <div class="inp-bx">
                        <label for="deptCode">Department Code</label>
                        <div class="input-wrap">
                            <input id="deptCode" type="text" value="${code}" placeholder="HR / IT / FIN" />
                            <span class="material-symbols-rounded">badge</span>
                        </div>
                    </div>

                    <div class="inp-bx full-width">
                        <label for="deptDescription">Description</label>
                        <div class="input-wrap">
                            <textarea id="deptDescription" placeholder="Brief description">${description}</textarea>
                            <span class="material-symbols-rounded">description</span>
                        </div>
                    </div>

                    <div class="form-check form-switch">
                        <input class="form-check-input" type="checkbox" id="deptStatus" ${isActive === true || isActive === "True" ? "checked" : ""}>
                        <label class="form-check-label" for="deptStatus">Active</label>
                    </div>
                </div>`,
            focusConfirm: false,
            showCancelButton: true,
            confirmButtonText: 'Update',
            background: 'rgba(113, 43, 241, 0.24)',
            color: '#ffff',
            backdrop: 'blur(20px)',
            customClass: {
                confirmButton: 'btn-update',
                cancelButton: 'btn-cancel',
                popup: 'edit-modal',
            },
            preConfirm: () => ({
                id: id,
                name: document.getElementById('deptName').value,
                code: document.getElementById('deptCode').value,
                description: document.getElementById('deptDescription').value,
                isActive: document.getElementById('deptStatus').checked
            })
        }).then((result) => {
            if (result.isConfirmed) updateDepartment(result.value);
        });
    });



    function updateDepartment(data) {
        $.ajax({
            url: '/Department/Edit',
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

});



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