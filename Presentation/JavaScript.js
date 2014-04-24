function generatePostBack() {
    var o = window.event.srcElement;
    if (o.tagName == "INPUT" && o.type == "checkbox") {
        __doPostBack("", "");
    }
}