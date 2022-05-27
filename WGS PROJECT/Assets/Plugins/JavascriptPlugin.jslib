mergeInto(LibraryManager.library, {
  GetToken: function(){
    var baseURL = window.location.href;
    var url = new URL(baseURL);
    var token = url.searchParams.get("token");
    console.log(token);
    return `Bearer ${token}`;
  },
});