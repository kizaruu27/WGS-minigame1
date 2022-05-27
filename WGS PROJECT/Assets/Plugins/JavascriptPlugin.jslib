mergeInto(LibraryManager.library, {
  GetToken: function(){
    var baseURL = window.location.href;
    var url = new URL(baseURL);
    var token = url.searchParams.get("token");

    console.log("ini token di jslib : "+token);

    var bufferSize = lengthBytesUTF8(token) + 1;
    var buffer = _malloc(bufferSize);
    stringToUTF8(token, buffer, bufferSize);

    return buffer;
  },
});