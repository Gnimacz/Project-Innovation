var origin = window.location.origin;
var words = origin.split(':'); // typically: words[0]= "http", words[1] = something like "//192.168.0.1", words[2] = "8000" (the http server port)	
var wsUri = "ws:" + words[1] + ":8001" + "/";
var websocket = new WebSocket(wsUri)
console.log(origin);

var button = document.getElementById("jump");
var output = document.getElementById("output");
console.log(output);

// http://www.websocket.org/echo.html
websocket.onopen = function (e) {
    writeToScreen("CONNECTED");
};

websocket.onclose = function (e) {
    writeToScreen("DISCONNECTED");
};

websocket.onmessage = function (e) {
    writeToScreen("<span>RESPONSE: " + e.data + "</span>");
    if(e.data == "vibrate"){
        vibrate();
    }
};

websocket.onerror = function (e) {
    writeToScreen("<span class=error>ERROR:</span> " + e.data);
};

function doSend(message) {
    // writeToScreen("SENT: " + message);
    websocket.send(message);
}

function writeToScreen(message) {
    output.insertAdjacentHTML("afterbegin", "<p>" + message + "</p>");
}

writeToScreen("Websocket address: " + wsUri);
writeToScreen(origin);