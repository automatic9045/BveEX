function wait(ms) {
	return new Promise(resolve => setTimeout(resolve, ms));
}

function eval2(obj) {
	if (obj == "") return;
	return Function("\"use strict\"; return (() => {" + obj + "})()")();
}

function evalFromFile(path) {
	const xhr = new XMLHttpRequest();

	xhr.open("GET", path, false);
	xhr.onreadystatechange = (() => {
		if (xhr.readyState === 4 && xhr.status === 200) {
			eval2(xhr.responseText);
		}
	});
	xhr.send();
}

function evalFromHTMLCollection(htmlCollection) {
	for (let i = 0; i < htmlCollection.length; i++) {
		const child = htmlCollection.item(i);
		if (child.nodeName == "SCRIPT") {
			if (child.src != null && child.src != "") evalFromFile(child.src);
			eval2(child.textContent);
		} else {
			evalFromHTMLCollection(child.children);
		}
	}
}

function include(path, element) {
	const xhr = new XMLHttpRequest();

	xhr.open("GET", path, false);
	xhr.onreadystatechange = (() => {
		if (xhr.readyState === 4 && xhr.status === 200) {
			const response = xhr.responseText;
			element.innerHTML = response.replace("<meta name=\"robots\" content=\"noindex,nofollow\" />", "");

			evalFromHTMLCollection(element.children);
		}
	});
	xhr.send();
}

function includeById(path, elementId) {
	const element = document.getElementById(elementId);
	include(path, element);
}

function includeByClasses(path, elementClasses) {
	const elements = document.getElementsByClassName(elementClasses);
	if (elements.length == 1) {
		include(path, elements[0]);
	} else {
		Array.from(elements).forEach(element => include(path, element));
	}
}

function includeByClasses(path, elementClasses, index) {
	const element = document.getElementsByClassName(elementClasses)[index];
	include(path, element);
}

function setHTML(html, element) {
	element.innerHTML = html;
}

function setHTMLById(html, elementId) {
	const element = document.getElementById(elementId);
	setHTML(html, element);
}

function setHTMLByClasses(html, elementClasses, index) {
	const elements = document.getElementsByClassName(elementClasses);

	switch (arguments.length) {
		case 2:
			switch (elements.length) {
				case 0:
					console.error("Cannot find the class '" + elementClasses + "'");
					return;

				case 1:
					setHTML(html, elements[0]);
					break;

				default:
					Array.from(elements).forEach(element => {
						setHTML(html, element);
					});
					break;
			}
			break;

		case 3:
			setHTML(html, elements[index]);
			break;
	}
}

function refresh() {
	const scrollTop = document.documentElement.scrollTop || document.body.scrollTop;
	const bottom = scrollTop + window.innerHeight;

	const clientHeight = document.documentElement.clientHeight; // ブラウザ内の表示域
	const offsetHeight = document.documentElement.offsetHeight; // ドキュメントの高さ

	checkFadeFooterBus(bottom);
	adjustContentHeight(clientHeight, offsetHeight);
}

let isFooterBusVisible = false;
let animationPlayCount = 0;
let footerBusNumber = "";
let footerBus2Number = "";

function checkFadeFooterBus(bottom) {
	const footer = document.getElementsByTagName("footer")[0];
	const top = footer.offsetTop;

	const footerBus = footer.getElementsByClassName("footer-bus")[0];

	const footerBusBody = footerBus.getElementsByClassName("footer-bus-body")[0];
	const footerBusBody2 = footerBus.getElementsByClassName("footer-bus-body-2")[0];
	const footerSNS = footerBus.getElementsByClassName("footer-sns")[0];

	if (top + 150 < bottom) {
		if (!isFooterBusVisible) {
			const busNumbers = getRandomBusNumbers(2);
			footerBusBody.style.backgroundImage = "url('../img/bus_side/" + busNumbers[0] + ".svg')";
			footerBusBody2.style.backgroundImage = "url('../img/bus_side/" + busNumbers[1] + ".svg')";

			footerBusBody.classList.add("fade");
			footerBusBody2.classList.add("fade");
			if (footerSNS !== undefined) footerSNS.classList.add("fade");

			animationPlayCount++;
			footerBusNumber = busNumbers[0];
			footerBus2Number = busNumbers[1];

			footerBusBody2.addEventListener("animationend", onFooterBus2AnimationEnd);
			gtag("event", "footer_bus_begin_play", { "event_category": "engagement", "event_label": footerBusNumber, "value": animationPlayCount });
		}

		isFooterBusVisible = true;
	} else if (top > bottom) {
		footerBusBody.classList.remove("fade");
		footerBusBody2.classList.remove("fade");
		if (footerSNS !== undefined) footerSNS.classList.remove("fade");

		footerBusBody2.removeEventListener("animationend", onFooterBus2AnimationEnd);

		isFooterBusVisible = false;
	}
}

function onFooterBus2AnimationEnd() {
	gtag("event", "footer_bus_finish_play", { "event_category": "engagement", "event_label": footerBus2Number });
}

function adjustContentHeight(clientHeight, offsetHeight) {
	if (offsetHeight == 0) return;

	const content = document.getElementById("content");

	const marginBottom = parseInt(content.style.marginBottom) || 0;
	if (clientHeight > offsetHeight - marginBottom) {
		content.style.marginBottom = (clientHeight - offsetHeight + marginBottom) + "px";
	} else {
		content.style.marginBottom = "0";
	}
}

function scrollTo(targetId) {
	const target = document.getElementById(targetId);

	if (target) {
		location.hash = "#" + targetId;
		document.documentElement.scrollTop = target.offsetTop - 150;
		return true;
	} else {
		return false;
	}
}


/*--Global site tag(gtag.js) - Google Analytics--*/

evalFromFile("https://www.googletagmanager.com/gtag/js?id=UA-173968596-2");
window.dataLayer = window.dataLayer || [];
function gtag() { dataLayer.push(arguments); }
gtag('js', new Date());
gtag('config', 'UA-173968596-2');


window.addEventListener("load", e => {
	if (location.hash != "") scrollTo(location.hash.substring(1));
});

window.addEventListener("DOMContentLoaded", e => {
	const pathName = location.pathname.substring("/AtsEX".length);
	const links = (() => {
		if (pathName == "/") {
			return document.querySelectorAll("a[href^='" + pathName + "#']");
		} else if (pathName.startsWith("/") && pathName.endsWith("/")) {
			return document.querySelectorAll("a[href^='" + pathName + "#'], a[href^='" + pathName.substring(1) + "#'], a[href^='" + pathName.substring(0, pathName.length - 1) + "#'], a[href^='" + pathName.substring(1, pathName.length - 2) + "#']");
		} else if (pathName.startsWith("/")) {
			return document.querySelectorAll("a[href^='" + pathName + "#'], a[href^='" + pathName.substring(1) + "#']");
		} else if (pathName.endsWith("/")) {
			return document.querySelectorAll("a[href^='" + pathName + "#'], a[href^='" + pathName.substring(0, pathName.length - 1) + "#']");
		} else {
			return document.querySelectorAll("a[href^='" + pathName + "#']");
		}
	})();

	links.forEach(link => {
		link.addEventListener("click", e => {
			const href = e.currentTarget.getAttribute("href");
			const targetId = href.split("#")[1];

			scrollTo(targetId);
			e.preventDefault();
		});
	});
});


window.addEventListener("load", e => { refresh(); refresh(); });
window.addEventListener("resize", refresh);
window.addEventListener("orientationchange", refresh);
window.addEventListener("scroll", refresh);