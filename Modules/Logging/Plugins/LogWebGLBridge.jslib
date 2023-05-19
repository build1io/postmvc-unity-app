mergeInto(LibraryManager.library, {
    LogDebug: function(message) {
        console.log(UTF8ToString(message));
    },
  
    LogWarning: function(message) {
        console.warn(UTF8ToString(message));
    },

    LogError: function(message) {
        console.error(UTF8ToString(message));
    },
    
    GetUrlParameters: function() {
        var str = window.location.search;
        var bufferSize = lengthBytesUTF8(str) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(str, buffer, bufferSize);
        return buffer;
    }
});