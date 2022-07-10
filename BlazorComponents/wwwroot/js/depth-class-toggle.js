var ScrollElement = ScrollElement || {};

ScrollElement.showOnScroll = function(elementId, classToToggle, scrollDepth) {
	var rootElement = document.getElementById(elementId);
	var myScrollFunc = function () {
		rootElement.classList.toggle(classToToggle, window.scrollY <= scrollDepth);
	};

	window.addEventListener("scroll", myScrollFunc);
}