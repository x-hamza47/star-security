$(document).ready(function () {
    $("#subserviceTable").DataTable({
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

    $(document).on('click', '.edit-subservice-btn', function () {
        const button = $(this);

        const id = button.data('id');
        const name = button.data('name');
        const departmentId = parseInt(button.data('department'))
        const description = button.data('description');

        let deptOptions = departments.map(d =>
            `<option value="${d.id}" ${d.id === departmentId ? 'selected' : ''}>${d.name}</option>`
        ).join('');

        Swal.fire({
            title: 'Edit Service',
            html: `
        <div class="inp-fields d-flex gap-4 flex-column text-start">

            <div class="inp-bx">
                <label for="subName">Service Name</label>
                <div class="input-wrap">
                    <input id="subName" type="text" value="${name}" placeholder="Service Name" />
                    <span class="material-symbols-rounded">apartment</span>
                </div>
            </div>

            <div class="inp-bx">
                <label for="subDepartment">Department</label>
                <div class="input-wrap">
                    <select id="subDepartment">${deptOptions}</select>
                    <span class="material-symbols-rounded">apartment</span>
                </div>
            </div>

            <div class="inp-bx full-width">
                <label for="subDescription">Description</label>
                <div class="input-wrap">
                    <textarea id="subDescription" placeholder="Brief description">${description || ''}</textarea>
                    <span class="material-symbols-rounded">description</span>
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
                name: document.getElementById('subName').value,
                departmentId: parseInt(document.getElementById('subDepartment').value),
                description: document.getElementById('subDescription').value
            })
        }).then((result) => {
            if (result.isConfirmed) updateSubService(result.value);
        });
    });

   
    function updateSubService(data) {
        $.ajax({
            url: '/SubService/Edit',
            type: 'POST',
            data: data,
            success: function (res) {
                Swal.fire({
                    toast: true,
                    position: 'top-end',
                    icon: 'success',
                    title: res.message || "Service updated successfully!",
                    color: '#fff',
                    showConfirmButton: false,
                    showCloseButton: true,
                    timer: 1500,
                    timerProgressBar: true,
                    iconColor: '#008000',
                    background: 'rgba(255, 255, 255, 0.2)',
                    backdrop: 'blur(20px)',
                    customClass: { popup: 'glass-toast' }
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



    $(document).on('click', '.delete-subservice-btn', function () {
        const button = $(this);
        const id = button.data('id');

        Swal.fire({
            title: 'Are you sure?',
            text: "This will permanently delete the service!",
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
                    url: '/SubService/Delete',
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