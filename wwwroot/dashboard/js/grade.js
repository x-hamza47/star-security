$(document).ready(function () {
    $("#employeesTable").DataTable({
        pagingType: "full_numbers",
        pageLength: 10,
        lengthMenu: [5, 10, 25],
        order: [[0, "asc"]],
        drawCallback: function (settings) {
            const api = this.api();
            const pageInfo = api.page.info();

            // pagination container
            const pagination = $(api.table().container())
                .find('.dt-paging');

            // show only if more than 1 page
            if (pageInfo.pages > 1) {
                pagination.show();
            } else {
                pagination.hide();
            }
        },
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
            title: 'Edit Grade',
            html: `
               <div class="inp-fields d-flex gap-4 flex-column text-start">
                    <div class="inp-bx">
                        <label for="gradeName">Grade Name</label>
                        <div class="input-wrap">
                            <input id="gradeName"
                                   type="text"
                                   placeholder="Grade I / Supervisor / Officer"
                                   value="${name}" />
                            <span class="material-symbols-rounded">military_tech</span>
                        </div>
                    </div>


                    <div class="inp-bx">
                        <label for="gradeCode">Grade Code</label>
                        <div class="input-wrap">
                            <input id="gradeCode"
                                   type="text"
                                   value="${code}"
                                   placeholder="G1 / G2 / SUP" />
                            <span class="material-symbols-rounded">badge</span>
                        </div>
                    </div>

                    <div class="inp-bx full-width">
                        <label for="gradeDescription">Description</label>
                        <div class="input-wrap">
                            <textarea id="gradeDescription"
                                      placeholder="Brief description of grade responsibilities">${description}</textarea>
                            <span class="material-symbols-rounded">assignment</span>
                        </div>
                    </div>


                    <div class="form-check form-switch">
                        <input class="form-check-input"
                               type="checkbox"
                               id="gradeStatus"
                               ${isActive === true || isActive === "True" ? "checked" : ""}>
                        <label class="form-check-label" for="gradeStatus">
                            Active
                        </label>
                    </div>
                </div>
                `,
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
                name: document.getElementById('gradeName').value,
                code: document.getElementById('gradeCode').value,
                description: document.getElementById('gradeDescription').value,
                isActive: document.getElementById('gradeStatus').checked
            })
        }).then((result) => {
            if (result.isConfirmed) updateDepartment(result.value);
        });
    });



    function updateDepartment(data) {
        $.ajax({
            url: '/Grade/Edit',
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
        text: "This will permanently delete the staff grade!",
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
                url: '/Grade/Delete',
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