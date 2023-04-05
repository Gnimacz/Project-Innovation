
var pressedJump = 0,
pressedAttack = 0,
directionX = 0.0,
directionY = 0.0,
directionEnum = "Right",
finalMessage = "";
    // wsUri = "ws://127.0.0.1/",    // works
    // wsUri = "ws://192.168.137.1:8001/",  // This works if this is the LAN address of the web socket server

// button.addEventListener("click", onClickButton);
// document.getElementById("jump").addEventListener("click", onJumpPressed);
document.getElementById("attack").addEventListener("touchstart", onAttackPressed);
document.getElementById("attack").addEventListener("touchend", OnAttackReleased);
document.getElementById("jump").addEventListener("touchstart", onJumpPressed);
document.getElementById("jump").addEventListener("touchend", onJumpReleased);
if(/iPad|iPhone|iPod/.test(navigator.userAgent)){
    document.getElementById("fullscreen").addEventListener("click", IosFullScreen);
    document.getElementById("exit").addEventListener("click", IosExitFullScreen);
}
else{ 
    document.getElementById("fullscreen").addEventListener("click", FullScreen);
    document.getElementsByClassName("hide")[0].classList.remove("hide");
    document.getElementById("exit").addEventListener("click", ExitFullScreen);
}

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

function IosFullScreen(){
    var elem = document.getElementById("Inputs");
    var scrollElem = document.getElementById("jump");
    // elem.style.visible = "visible";
    elem.classList.remove("hide");
    scrollElem.scrollto(elem);
}

function IosExitFullScreen(){   
    var elem = document.getElementById("Inputs");
    elem.classList.add("hide");
}

function ExitFullScreen(){
    if (document.exitFullscreen) {
        document.exitFullscreen();
      } else if (document.mozCancelFullScreen) { /* Firefox */
        document.mozCancelFullScreen();
      } else if (document.webkitExitFullscreen) { /* Chrome, Safari and Opera */
        document.webkitExitFullscreen();
      } else if (document.msExitFullscreen) { /* IE/Edge */
        document.msExitFullscreen();
      }
}

function onJumpPressed() {
    pressedJump = 1;
    vibrate();
}
function onJumpReleased() {
    pressedJump = 0;
}

function onAttackPressed() {
    pressedAttack = 1;
    vibrate();

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

function vibrate(number = 30){
    if(!("vibrate" in navigator)){
    //  alert("Vibrate not supported!");
      return;
     }
    // navigator.vibrate(30);
    navigator.vibrate(number);
   }