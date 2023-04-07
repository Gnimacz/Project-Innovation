var origin = window.location.origin;
var words = origin.split(':'); // typically: words[0]= "http", words[1] = something like "//192.168.0.1", words[2] = "8000" (the http server port)	
var wsUri = "ws:" + words[1] + ":8001" + "/";
var websocket = new WebSocket(wsUri)
websocket.binaryType = 'arraybuffer';
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
    const data = e.data;
    if (typeof data === 'string') {
        console.log("string data: " + data);
        HandleCommand(data);
    } else if (data instanceof ArrayBuffer) {
        console.log("arraybuffer data: " + data);
    }
    else{
        alert("Unknown data type received");
    }

    writeToScreen("<span>RESPONSE: " + e.data + "</span>");
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

// command handlers
function HandleCommand(data){
    var command = data.split(" ");
    switch(command[0]){
        case 'vibrate':
            PlayerHurt();
            break;
        case "set":
            switch(command[1]){
                case "fullscreen":
                    FullScreen();
                    break;
                case "exitfullscreen":
                    ExitFullScreen();
                    break;
            }
            break;
    }
}

function PlayerHurt() {
    var element = document.getElementById("Inputs");
    vibrate(50);
    var originalColor = element.style.backgroundColor;
    element.style.backgroundColor = 'red';
    setTimeout(() => {
      element.style.backgroundColor = originalColor;
    }, 100);
  }