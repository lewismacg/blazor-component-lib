// Create the Draggable Interop JS Object in the global scope
var DraggableElement = DraggableElement || {};

DraggableElement.makeDraggable = function (rootElementId, handleElementId) {
	var updatedX = 0, updatedY = 0, initialX = 0, initialY = 0;
	document.getElementById(handleElementId).onmousedown = dragMouseDown;
	var root = document.getElementById(rootElementId);

	function dragMouseDown(e) {
		e = e || window.event;
		e.preventDefault();
		initialX = e.clientX;
		initialY = e.clientY;
		document.onmouseup = closeDragElement;
		document.onmousemove = elementDrag;
	}

	function elementDrag(e) {
		e = e || window.event;
		e.preventDefault();
		updatedX = initialX - e.clientX;
		updatedY = initialY - e.clientY;
		initialX = e.clientX;
		initialY = e.clientY;
		root.style.top = (root.offsetTop - updatedY) + "px";
		root.style.left = (root.offsetLeft - updatedX) + "px";
	}

	function closeDragElement() {
		document.onmouseup = null;
		document.onmousemove = null;
	}
}

DraggableElement.dragContainerCount = 0;
DraggableElement.increaseDragScrollBoundary = function () {
	DraggableElement.dragContainerCount++;

	if (document.getElementById("draggableScrollTop") || document.getElementById("draggableScrollBottom")) return;
	
	var appendElementWithId = function appendElementWithId(id, top) {
		var element = document.createElement('div');
		element.setAttribute('id', id);
		element.classList.add("drag-scroll-boundary");
		var icon = document.createElement('i');
		element.appendChild(icon);

		if (top) {
			element.style.cssText += "top: 0;"
			icon.innerHTML = "arrow_upward";
		} else {
			element.style.cssText += "bottom: 0;"
			icon.innerHTML = "arrow_downward";
		}

		document.body.appendChild(element);

		return element;
	};

	function hide() {
		topElement.style.display = 'none';
		bottomElement.style.display = 'none';
	};

	function show() {
		topElement.style.display = 'block';
		bottomElement.style.display = 'block';
	};

	function triggerScroll(amount) {
		return window.scrollBy(0, amount);
	};

	function clearScrollInterval() {
		clearInterval(doScroll);
		doScroll = null;
	};

	var topElement = appendElementWithId("draggableScrollTop", true);
	var bottomElement = appendElementWithId("draggableScrollBottom", false);

	hide();

	var doScroll = null;

	[topElement, bottomElement].forEach(function (el) {
		return el.addEventListener('dragover', function (e) {
			if (doScroll == null) {
				doScroll = setInterval(function () {
					return triggerScroll(e.target === topElement ? -25 : 25);
				}, 25);
			}
		});
	});

	[topElement, bottomElement].forEach(function (el) {
		return el.addEventListener('dragleave', function () {
			return clearScrollInterval();
		});
	});

	document.addEventListener('dragover', function (e) {
		return show();
	});

	document.addEventListener('dragend', function () {
		clearScrollInterval();
		hide();
	});
}

DraggableElement.destroyDragScrollBoundary = function () {
	DraggableElement.dragContainerCount--;
	if (DraggableElement.dragContainerCount == 0) {
		var topElement = document.getElementById("draggableScrollTop");
		var bottomElement = document.getElementById("draggableScrollBottom");

		if (topElement) {
			topElement.remove();
		}
		if (bottomElement) {
			bottomElement.remove();
		}
	}
}

DraggableElement.createTopTableScrollbar = function (scrollableId, leftHandleId, rightHandleId) {
	var scrollableElement = document.getElementById(scrollableId);
	var leftHandleButton = document.getElementById(leftHandleId);
	var rightHandleButton = document.getElementById(rightHandleId);

	if (rightHandleButton && leftHandleButton) {
		function scrollElementLeft() {
			scrollableElement.scrollLeft -= 30;
		}

		function scrollElementRight() {
			scrollableElement.scrollLeft += 30;
		}

		var leftButtonInterval = null
		leftHandleButton.addEventListener("mousedown", () => {
			scrollElementLeft();
			leftButtonInterval = setInterval(scrollElementLeft, 25)
		});

		var rightButtonInterval = null
		rightHandleButton.addEventListener("mousedown", () => {
			scrollElementRight();
			rightButtonInterval = setInterval(scrollElementRight, 25)
		});

		document.addEventListener("mouseup", () => {
			if (leftButtonInterval) clearInterval(leftButtonInterval);
			if (rightButtonInterval) clearInterval(rightButtonInterval);
		});
	}	
}