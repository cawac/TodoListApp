$.validator.addMethod("notpastdate", function (value) {
    if (!value) return true;
    const now = new Date();
    const pad = n => String(n).padStart(2, "0");
    const today = `${now.getFullYear()}-${pad(now.getMonth() + 1)}-${pad(now.getDate())}`;
    return value.substring(0, 10) >= today;
});

$.validator.unobtrusive.adapters.addBool("notpastdate");
