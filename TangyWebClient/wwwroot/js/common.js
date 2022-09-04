window.showToastr = (type, message) => {
    if (type === "success") {
        toastr.success(message, 'Success', { timeOut: 5000 });
    }
    else {
        toastr.error(message, 'Warning', { timeOut: 5000 });
    }
}

window.showSwal = (type, message) => {
    if (type === "success") {
        Swal.fire(
            "Good Job", message, 'success');
    }
    else {
        Swal.fire("Failure", message, 'error');
    }
}

function ShowDeleteConfirmationModal() {
    $('#deleteConfirmationModal').modal('show');
}
function HideDeleteConfirmationModal() {
    $('#deleteConfirmationModal').modal('hide');
}