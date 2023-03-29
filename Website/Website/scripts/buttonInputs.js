
var pressedJump = 0,
pressedAttack = 0,
directionX = 0.0,
directionY = 0.0,
finalMessage = "";
    // wsUri = "ws://127.0.0.1/",    // works
    // wsUri = "ws://192.168.137.1:8001/",  // This works if this is the LAN address of the web socket server

// button.addEventListener("click", onClickButton);
// document.getElementById("jump").addEventListener("click", onJumpPressed);
document.getElementById("attack").addEventListener("touchstart", onAttackPressed);
document.getElementById("attack").addEventListener("touchend", OnAttackReleased);
document.getElementById("jump").addEventListener("touchstart", onJumpPressed);
document.getElementById("jump").addEventListener("touchend", onJumpReleased);
document.getElementById("fullscreen").addEventListener("click", FullScreen);

function FullScreen(){
    var elem = document.getElementById("Inputs");
    if (elem.requestFullscreen) {
      elem.requestFullscreen();
    } else if (elem.mozRequestFullScreen) { /* Firefox */
      elem.mozRequestFullScreen();
    } else if (elem.webkitRequestFullscreen) { /* Chrome, Safari and Opera */
      elem.webkitRequestFullscreen();
    } else if (elem.msRequestFullscreen) { /* IE/Edge */
      elem.msRequestFullscreen();
    }
}

function onClickButton() {
    var text = textarea.value;

    text && doSend(text);
    textarea.value = "";
    textarea.focus();
}

function onJumpPressed() {
    pressedJump = 1;
}
function onJumpReleased() {
    pressedJump = 0;
}

function onAttackPressed() {
    pressedAttack = 1;
}
function OnAttackReleased() {
    pressedAttack = 0;
}

function onEndFrame() {
    finalMessage = "";
    finalMessage = directionX + " " + directionY + " " + pressedJump;
    pressedJump = 0;
    doSend(finalMessage);
}

function vibrate(){
    if(!("vibrate" in navigator)){
     alert("Vibrate not supported!");
      return;
     }
    navigator.vibrate(5500);
   }