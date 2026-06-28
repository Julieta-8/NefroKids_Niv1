mergeInto(LibraryManager.library, {

  SendToReactNative: function (jsonPtr) {
    var json = UTF8ToString(jsonPtr);

    if (window.ReactNativeWebView && window.ReactNativeWebView.postMessage) {
      window.ReactNativeWebView.postMessage(json);
    } else {
      console.log("ReactNativeWebView no disponible:", json);
    }
  }

});