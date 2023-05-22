mergeInto(LibraryManager.library, {
    LogDebug: function(message) {
        console.log(UTF8ToString(message));
    },
  
    LogWarning: function(message) {
        console.warn(UTF8ToString(message));
    },

    LogError: function(message) {
        console.error(UTF8ToString(message));
    }
});