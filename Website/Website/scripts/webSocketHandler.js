var origin = window.location.origin;
var words = origin.split(':'); // typically: words[0]= "http", words[1] = something like "//192.168.0.1", words[2] = "8000" (the http server port)	
var wsUri = "ws:" + words[1] + ":8001" + "/";
var websocket = new WebSocket(wsUri)
websocket.binaryType = 'arraybuffer';
console.log(origin);

var button = document.getElementById("jump");
var output = document.getElementById("output");
console.log(output);

var currentServerState = serverState.MainMenu;

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
    else {
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
function HandleCommand(data) {
    var command = data.split(" ");
    switch (command[0]) {
        case 'vibrate':
            PlayerHurt();
            break;
        case "state":
            switch (command[1]) {
                case "MainMenu":
                    currentServerState = serverState.MainMenu;
                    console.log("MainMenu");
                    selectedCharacter = selectedCharacter = 0;
                    selectCharacter = false;
                    document.getElementById("select_btn").innerHTML = "Select";
                    // document.getElementById("CharacterAll").style.color = rgb(255, 255, 255);
                    document.getElementById("CharacterAll").style.backgroundColor = ogColor;
                    // alert("MainMenu");
                    ExitFullScreen();
                    IosExitFullScreen();
                    IosExitCharacterSelector();

                    document.getElementById("main_menu_btn").style.display = "block";
                    document.getElementById("main_menu_btn").style.visibility = "visible";
                    break;
                case "CharacterSelect":
                    currentServerState = serverState.CharacterSelect;
                    console.log("CharacterSelect");
                    IosCharacterSelector();
                    IosExitFullScreen();
                    // alert("CharacterSelect");

                    document.getElementById("main_menu_btn").style.display = "none";
                    document.getElementById("main_menu_btn").style.visibility = "hidden";
                    break;
                case "Game":
                    currentServerState = serverState.Game;
                    console.log("Game");
                    IosExitCharacterSelector();
                    IosFullScreen();

                    document.getElementById("main_menu_btn").style.display = "none";
                    document.getElementById("main_menu_btn").style.visibility = "hidden";
                    // alert("Game");
                    // FullScreen();
                    break;
                case "EndGame":
                    currentServerState = serverState.GameOver;
                    console.log("EndGame");
                    // alert("EndGame");
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