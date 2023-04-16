function next() {
    const page = document.getElementById('Filters_PageNumber');
    page.value++;
    const form = document.getElementById('time-sheets-table');
    form.submit();
}

function back() {
    const page = document.getElementById('Filters_PageNumber');
    page.value--;
    const form = document.getElementById('time-sheets-table');
    form.submit();
}
