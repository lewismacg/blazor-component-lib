// Create the Draggable Interop JS Object in the global scope
var DraggableElement = DraggableElement || {};

DraggableElement.makeDraggable = function (rootElementId, handleElementId) {
    var rootElement = document.getElementById(rootElementId);
    var handleElement = document.getElementById(handleElementId);

    var deltaX = 0, deltaY = 0, startX = 0, startY = 0;

    if (rootElement.offsetTop === 0){
        rootElement.style.top = rootElement.offsetTop + "px";
    }

    if (rootElement.offsetLeft === 0) {
        rootElement.style.left = (window.innerWidth / 2 - rootElement.clientWidth / 2) + "px";
    }

    // magic js null check here. If handleElement != null, it is "truthy" so it will resolve to true when coerced into a bool. If it is null, it is falsy and will be false when coerced into a bool
    if (handleElement) {
        handleElement.onmousedown = dragMouseDown;
    } else {
        rootElement.onmousedown = dragMouseDown;
    }

    function dragMouseDown(e) {
        e = e || window.event;
        e.preventDefault();

        startX = e.clientX;
        startY = e.clientY;

        document.onmouseup = closeDragElement;
        document.onmousemove = doDrag;
    }

    function doDrag(e) {
        e = e || window.event;
        e.preventDefault();

        deltaX = startX - e.clientX;
        deltaY = startY - e.clientY;

        startX = e.clientX;
        startY = e.clientY;

        rootElement.style.top = (rootElement.offsetTop - deltaY) + "px";
        rootElement.style.left = (rootElement.offsetLeft - deltaX) + "px";
    }

    function closeDragElement() {
        document.onmouseup = null;
        document.onmousemove = null;
    }
}