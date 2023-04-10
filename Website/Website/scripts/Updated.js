// setInterval(update, 300);
setInterval(update, 100);

function update() {
    if (websocket.readyState == 1) {
        switch (currentServerState) {
            case serverState.MainMenu:
                console.log("MainMenu");
                break;
            case serverState.CharacterSelect:
                console.log("CharacterSelect");
                break;
            case serverState.Game:
                console.log("Game");
                finalMessage = directionX + " " + directionY + " " + pressedJump + " " + pressedAttack + " " + directionEnum;
                doSend(finalMessage);
                console.log("Updated: " + finalMessage)
                directionX = 0.0;
                directionY = 0.0;
                break;
            case serverState.EndGame:
                console.log("EndGame");
                break;
            default:
                break;
        }
        
    }
    else {
        // console.log("Not connected")
        websocket.close();
        
    }
    // console.log(directionX + " " + directionY);
}