function DeleteChoice(dishID) {
    if (confirm('Are you sure you want to delete???')) {
        window.location.href = "/DataBase/DeleteDish/" + dishID;
    } else {
        return false;
    }
}