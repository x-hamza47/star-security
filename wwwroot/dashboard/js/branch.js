$(document).ready(function () {
    $("#branchTable").DataTable({
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

    $(document).on('click', '.edit-branch-btn', function () {
        const button = $(this);
        const id = button.data('id');
        const name = button.data('name');
        const area = button.data('area');
        const manager = button.data('manager');
        const contact = button.data('contact');
        const latitude = button.data('latitude');
        const longitude = button.data('longitude');
        const regionId = button.data('regionId');
        const isActive = button.data('active');



        let regionOptions = regions.map(r =>
            `<option value="${r.id}" ${r.id === regionId ? 'selected' : ''}>${r.name}</option>`
        ).join('');

        Swal.fire({
            title: 'Edit Branch',
            html: `
           <div class="inp-fields d-flex gap-4 flex-column text-start">

                <div class="inp-bx">
                    <label for="branchName">Branch Name</label>
                    <div class="input-wrap">
                        <input id="branchName" type="text" value="${name}" placeholder="Branch Name" />
                        <span class="material-symbols-rounded">store</span>
                    </div>
                </div>

                <div class="inp-bx">
                    <label for="branchArea">Area</label>
                    <div class="input-wrap">
                        <input id="branchArea" type="text" value="${area}" placeholder="Area" />
                        <span class="material-symbols-rounded">location_on</span>
                    </div>
                </div>

                <div class="inp-bx">
                    <label for="branchManager">Manager Name</label>
                    <div class="input-wrap">
                        <input id="branchManager" type="text" value="${manager}" placeholder="Manager Name" />
                        <span class="material-symbols-rounded">person</span>
                    </div>
                </div>

                <div class="inp-bx">
                    <label for="branchContact">Contact Number</label>
                    <div class="input-wrap">
                        <input id="branchContact" type="text" value="${contact}" placeholder="03xxxxxxxxx" />
                        <span class="material-symbols-rounded">call</span>
                    </div>
                </div>

                <div class="inp-bx">
                    <label for="branchLat">Latitude</label>
                    <div class="input-wrap">
                        <input id="branchLat" type="number" step="any" value="${latitude}" />
                        <span class="material-symbols-rounded">public</span>
                    </div>
                </div>

                <div class="inp-bx">
                    <label for="branchLng">Longitude</label>
                    <div class="input-wrap">
                        <input id="branchLng" type="number" step="any" value="${longitude}" />
                        <span class="material-symbols-rounded">public</span>
                    </div>
                </div>

                <div class="inp-bx">
                    <label>Region</label>
                    <div class="input-wrap">
                        <select id="branchRegion">${regionOptions}</select>
                    </div>
                </div>

                 <div class="form-check form-switch">
                        <input class="form-check-input" type="checkbox" id="branchActive" ${isActive === true || isActive === "True" ? "checked" : ""}>
                        <label class="form-check-label" for="branchActive">Active</label>
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
                name: $('#branchName').val(),
                area: $('#branchArea').val(),
                managerName: $('#branchManager').val(),
                contactNumber: $('#branchContact').val(),
                latitude: $('#branchLat').val(),
                longitude: $('#branchLng').val(),
                regionId: $('#branchRegion').val(),
                isActive: $('#branchActive').is(':checked')
            })
        }).then((result) => {
            if (result.isConfirmed) updateBranch(result.value);
        });
    });

    function updateBranch(data) {
        $.ajax({
            url: '/Branch/Edit',
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

    $(document).on('click', '.delete-branch-btn', function () {
        const button = $(this);
        const id = button.data('id');

        Swal.fire({
            title: 'Are you sure?',
            text: "This will permanently delete the branch!",
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
                    url: '/Branch/Delete',
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