function nextTimeSheetList() {
    const page = document.getElementById('Filters_PageNumber');
    page.value++;
    const form = document.getElementById('time-sheets-table');
    form.submit();
}

function backTimeSheetList() {
    const page = document.getElementById('Filters_PageNumber');
    page.value--;
    const form = document.getElementById('time-sheets-table');
    form.submit();
}