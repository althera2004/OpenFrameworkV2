function guid() {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
          .toString(16)
          .substring(1);
    }

    return s4() + s4() + "-" + s4() + "-" + s4() + "-" + s4() + "-" + s4() + s4() + s4();
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
    if (date === "") {
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

    if (separator == "/") { separator = "-"; }
    if (separator == null) { separator = "-"; }
    date = date.split("/").join("-");
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

function JavaScriptText(text) {
    text = text.replace(/à/, "&agrave;");
    text = text.replace(/è/, "&egrave;");
    text = text.replace(/ì/, "&igrave;");
    text = text.replace(/ò/, "&ograve;");
    text = text.replace(/ù/, "&ugrave;");
    text = text.replace(/á/, "&aacute;");
    text = text.replace(/é/, "&eacute;");
    text = text.replace(/í/, "&iacute;");
    text = text.replace(/ó/, "&oacute;");
    text = text.replace(/ú/, "&uacute;");
    text = text.replace(/ñ/, "&ntilde;");
    text = text.replace(/ç/, "&ccedil;");
    console.log(text);
    return text;
}

function humanFileSize(bytes, si) {
    var thresh = si ? 1000 : 1024;
    if (Math.abs(bytes) < thresh) {
        return bytes + ' B';
    }
    var units = si
        ? ['kB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB']
        : ['KiB', 'MiB', 'GiB', 'TiB', 'PiB', 'EiB', 'ZiB', 'YiB'];
    var u = -1;
    do {
        bytes /= thresh;
        ++u;
    } while (Math.abs(bytes) >= thresh && u < units.length - 1);
    return bytes.toFixed(1) + ' ' + units[u];
}