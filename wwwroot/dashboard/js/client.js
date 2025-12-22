$(document).ready(function () {
    $("#clientTable").DataTable({
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

    $(document).on('click', '.edit-client-btn', function () {
        const button = $(this);
        const id = button.data('id');
        const name = button.data('name');
        const contact = button.data('contact');
        const address = button.data('address');

        Swal.fire({
            title: 'Edit Client',
            html: `
        <div class="inp-fields d-flex gap-4 flex-column text-start">

            <div class="inp-bx">
                <label for="clientName">Client Name</label>
                <div class="input-wrap">
                    <input id="clientName" type="text" value="${name}" placeholder="Client Name" />
                    <span class="material-symbols-rounded">person</span>
                </div>
            </div>

            <div class="inp-bx">
                <label for="clientContact">Contact</label>
                <div class="input-wrap">
                    <input id="clientContact" type="text" value="${contact}" placeholder="03xxxxxxxxx" />
                    <span class="material-symbols-rounded">call</span>
                </div>
            </div>

            <div class="inp-bx full-width">
                <label for="clientAddress">Address</label>
                <div class="input-wrap">
                    <textarea id="clientAddress" placeholder="Client Address">${address || ''}</textarea>
                    <span class="material-symbols-rounded">home</span>
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
                name: $('#clientName').val(),
                contact: $('#clientContact').val(),
                address: $('#clientAddress').val()
            })
        }).then((result) => {
            if (result.isConfirmed) updateClient(result.value);
        });
    });

    function updateClient(data) {
        $.ajax({
            url: '/Client/Edit',
            type: 'POST',
            data: data,
            success: function (res) {
                Swal.fire({
                    toast: true,
                    position: 'top-end',
                    icon: 'success',
                    title: res.message,
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

    $(document).on('click', '.delete-client-btn', function () {
        const button = $(this);
        const id = button.data('id');

        Swal.fire({
            title: 'Are you sure?',
            text: "This will permanently delete the client!",
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
                    url: '/Client/Delete',
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




});// End Point