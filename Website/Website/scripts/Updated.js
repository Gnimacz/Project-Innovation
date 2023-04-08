// setInterval(update, 300);
setInterval(update, 100);

function update() {
    if (websocket.readyState == 1) {
        if (serverState == serverState.Game) {
            finalMessage = directionX + " " + directionY + " " + pressedJump + " " + pressedAttack + " " + directionEnum;
            doSend(finalMessage);
            console.log("Updated: " + finalMessage)
            directionX = 0.0;
            directionY = 0.0;
        }
    }
    else {
        // console.log("Not connected")
        websocket.close();
        websocket = new WebSocket(wsUri)
    }
    // console.log(directionX + " " + directionY);
}