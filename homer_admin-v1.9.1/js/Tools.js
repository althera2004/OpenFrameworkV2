function guid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
          .toString(16)
          .substring(1);
    }
    return s4() + s4() + '-' + s4() + '-' + s4() + '-' +
      s4() + '-' + s4() + s4() + s4();
}

function GetDateFromTextComplete(value) {
    if (typeof value === "undefined" || value === null || value === "") {
        return "";
    }

    var dateText = value.split('T')[0];
    var year = dateText.split('-')[0];
    var month = dateText.split('-')[1];
    var day = dateText.split('-')[2];
    return new Date(year, month, day);

}

function GetDateFromText(date, separator, nullable) {
    if (date == '') {
        if (nullable == null) {
            return null;
        }
        else {
            if (nullable === false) {
                return null;
            }
            else {
                return new Date(1970, 1, 1);
            }
        }
    }
    if (separator == '/') { separator = '-'; }
    if (separator == null) { separator = '-'; }
    date = date.split('/').join('-');
    var day = date.split(separator)[0];
    var month = (date.split(separator)[1] * 1) - 1;
    var year = date.split(separator)[2];
    return new Date(year, month, day);
}

function DateToText(date) {
    if (typeof date === "undefined" || date === null || date === "") { return ""; }
    function pad(s) { return (s < 10) ? '0' + s : s; }
    return [pad(date.getDate()), pad(date.getMonth() + 1), date.getFullYear()].join('/');
}