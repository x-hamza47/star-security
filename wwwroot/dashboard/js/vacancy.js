$(document).ready(function () {
    $("#vacancyTable").DataTable({
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

    $(document).on('click', '.edit-vacancy-btn', function () {
        const b = $(this);
        const id = b.data('id');
        const title = b.data('title');
        const departmentId = b.data('departmentid');
        const subServiceId = b.data('subserviceid');
        const gradeId = b.data('gradeid');
        const requiredStaff = b.data('requiredstaff');
        const lastDate = b.data('lastdate');
        const isActive = b.data('active');

        let deptOptions = departments.map(d =>
            `<option value="${d.Id}" ${d.Id == departmentId ? 'selected' : ''}>${d.Name}</option>`
        ).join('');

        let gradeOptions = grades.map(g =>
            `<option value="${g.Id}" ${g.Id == gradeId ? 'selected' : ''}>${g.Name}</option>`
        ).join('');

        // Initially empty SubService options; will load via AJAX
        let subServiceOptions = '<option value="">-- Select SubService --</option>';


        Swal.fire({
            title: 'Edit Branch',
            html: `
           <div class="inp-fields d-flex gap-4 flex-column text-start">

            <div class="inp-bx">
                <label for="vacTitle">Job Title</label>
                <div class="input-wrap">
                    <input id="vacTitle" type="text" value="${title}" placeholder="Job Title" />
                    <span class="material-symbols-rounded">work</span>
                </div>
            </div>

            <div class="inp-bx">
                <label>Department</label>
                <div class="input-wrap">
                    <select id="vacDepartment">${deptOptions}</select>
                    <span class="material-symbols-rounded">apartment</span>
                </div>
            </div>

            <div class="inp-bx">
                <label>SubService (Optional)</label>
                <div class="input-wrap">
                    <select id="vacSubService">
                        <option value="">-- Select SubService --</option>
                        ${subServiceOptions}
                    </select>
                    <span class="material-symbols-rounded">category</span>
                </div>
            </div>

            <div class="inp-bx">
                <label>Grade</label>
                <div class="input-wrap">
                    <select id="vacGrade">${gradeOptions}</select>
                    <span class="material-symbols-rounded">badge</span>
                </div>
            </div>

            <div class="inp-bx">
                <label for="vacRequired">Required Staff</label>
                <div class="input-wrap">
                    <input id="vacRequired" type="number" value="${requiredStaff}" min="1" />
                    <span class="material-symbols-rounded">group</span>
                </div>
            </div>

            <div class="inp-bx">
                <label for="vacLastDate">Last Date</label>
                <div class="input-wrap">
                    <input id="vacLastDate" type="date" value="${lastDate.split('T')[0]}" />
                    <span class="material-symbols-rounded">calendar_today</span>
                </div>
            </div>

            <div class="form-check form-switch">
                <input class="form-check-input" type="checkbox" id="vacActive" ${isActive ? "checked" : ""}>
                <label class="form-check-label" for="vacActive">Active</label>
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
            didOpen: () => {
           
                if (departmentId) {
                    loadSubServices(departmentId, subServiceId);
                }

                $('#vacDepartment').on('change', function () {
                    const deptId = $(this).val();
                    loadSubServices(deptId, null);
                });
            },
            preConfirm: () => ({
                id: id,
                title: $('#vacTitle').val(),
                departmentId: $('#vacDepartment').val(),
                subServiceId: $('#vacSubService').val() || null,
                gradeId: $('#vacGrade').val(),
                requiredStaff: $('#vacRequired').val(),
                lastDate: $('#vacLastDate').val(),
                isActive: $('#vacActive').is(':checked')
            })
        }).then((result) => {
            if (result.isConfirmed) updateVacancy(result.value);
        });
    });

    function loadSubServices(deptId, selectedSubServiceId) {
        if (!deptId) {
            $('#vacSubService').html('<option value="">-- Select SubService --</option>');
            return;
        }

        $.ajax({
            url: '/Vacancy/GetSubServicesByDepartment',
            type: 'GET',
            data: { departmentId: deptId },
            success: function (data) {
                let options = '<option value="">-- Select SubService --</option>';
                data.forEach(s => {
                    const selected = s.id == selectedSubServiceId ? 'selected' : '';
                    options += `<option value="${s.id}" ${selected}>${s.name}</option>`;
                });
                $('#vacSubService').html(options);
            }
        });
    }
    function updateVacancy(data) {
        $.ajax({
            url: '/Vacancy/Edit',
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

    $(document).on('click', '.delete-vacancy-btn', function () {
        const button = $(this);
        const id = button.data('id');

        Swal.fire({
            title: 'Are you sure?',
            text: "This will permanently delete the vacancy!",
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
                    url: '/Vacancy/Delete',
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