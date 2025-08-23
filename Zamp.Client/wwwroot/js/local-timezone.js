function getLocalTimeOffset() {
    return new Date().getTimezoneOffset();
}

function getLocalTimeZoneName() {
    return Intl.DateTimeFormat().resolvedOptions().timeZone;
}