mergeInto(LibraryManager.library, {

  Hello: function () {
    window.alert("Hello, world!");
  },

  Alert: function (){
    window.alert("Unity to JS Alert!");
  },

  GetToken: function(){
    var baseURL = window.location.href;
    var url = new URL(baseURL);
    var token = url.searchParams.get("token");
    stringToUTF8(token, baseURL);
    console.log(token);

    return token;
  },

  GetBaseURL: function(){
    var thisURL = window.location.href;
    var bufferSize = lengthBytesUTF8(thisURL) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(thisURL, buffer, bufferSize);
    return buffer;

    console.log(thisURL);
  }


});