mergeInto(LibraryManager.library, {
    NextScreen: function () {
        window.location = "./dashboard?neurospect=finished";
    },
    GetToken: function () {
        var returnStr = window.sessionStorage.getItem("token");
        if (!returnStr) {
            returnStr = "-";
        }
        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        return buffer;
    },
    GetBaseAPIURL: function () {
        var returnStr = window.sessionStorage.getItem("baseAPIURL");
        if (!returnStr) {
            returnStr = "-";
        }
        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        return buffer;
    },
    Replay: function () {
        window.location.reload(true);
    }
});