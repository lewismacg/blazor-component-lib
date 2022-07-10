// Create Web Helpers Interop in global scope
var WebHelpers = WebHelpers || {};

WebHelpers.SetElementAbsolutePosition = function (elementId, x, y) {
    var element = document.getElementById(elementId);

    if (!element) return;

    var parent = element.parentElement;
    if (parent) {
        var parentBounding = parent.getBoundingClientRect();
        x -= parentBounding.x;
        y -= parentBounding.y;
    }

    var menuWidth = element.offsetWidth;
    var menuHeight = element.offsetHeight;

    var windowWidth = window.innerWidth;
    var windowHeight = window.innerHeight;

    if ((windowWidth - x) < menuWidth) {
        x = windowWidth - menuWidth;
    }

    if ((windowHeight - y) < menuHeight) {
        y = windowHeight - menuHeight;
    }


    element.style.position = "absolute";
    element.style.left = x + "px";
    element.style.top = y + "px";
    element.style.zIndex = 10;

    return;
}
