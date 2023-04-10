
var finalMessage = "",
  pressedJump = 0,
  pressedAttack = 0,
  directionX = 0.0,
  directionY = 0.0,
  directionEnum = "Right",
  characterSelected = 0;
// wsUri = "ws://127.0.0.1/",    // works
// wsUri = "ws://192.168.137.1:8001/",  // This works if this is the LAN address of the web socket server

// button.addEventListener("click", onClickButton);
// document.getElementById("jump").addEventListener("click", onJumpPressed);
document.getElementById("attack").addEventListener("touchstart", onAttackPressed);
document.getElementById("attack").addEventListener("touchend", OnAttackReleased);
document.getElementById("jump").addEventListener("touchstart", onJumpPressed);
document.getElementById("jump").addEventListener("touchend", onJumpReleased);
if (/iPad|iPhone|iPod/.test(navigator.userAgent)) {
  document.getElementById("fullscreen").addEventListener("click", IosFullScreen);
  document.getElementById("exit").addEventListener("click", IosExitFullScreen);
  document.getElementById("char_select").addEventListener("click", IosCharacterSelector);
  document.getElementById("exit_char").addEventListener("click", IosExitCharacterSelector);
  console.warn("IOS Detected");
}
else {
  document.getElementById("fullscreen").addEventListener("click", FullScreen);
  document.getElementById("exit").addEventListener("click", ExitFullScreen);
  document.getElementById("exit_char").addEventListener("click", ExitCharacterSelector);
  document.getElementById("char_select").addEventListener("click", CharacterSelector);
  var hiddenElems = document.querySelectorAll(".hide");
    for(var i = 0; i < hiddenElems.length; i++){
        hiddenElems[i].classList.remove("hide");
    }
}

function FullScreen() {
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

function IosFullScreen() {
  var elem = document.getElementById("Inputs");
  elem.classList.remove("hide");
  // var scrollElem = document.getElementById("jump");
  // elem.style.visible = "visible";
  // scrollElem.scrollto(elem);
}

function IosExitFullScreen() {
  var elem = document.getElementById("Inputs");
  elem.classList.add("hide");
}

function ExitFullScreen() {
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

function CharacterSelector() {
  onScreenOpen();
  var elem = document.getElementById("character_select");
  if (elem.requestFullscreen) {
    elem.requestFullscreen();
  } else if (elem.mozrequestFullscreen) {
    elem.mozrequestFullscreen();
  } else if (elem.webkitrequestFullscreen) {
    elem.webkitrequestFullscreen();
  } else if (elem.msrequestFullscreen) {
    elem.msrequestFullscreen();
  }
  elem.classList.remove("hide");
}

function ExitCharacterSelector() {
  if (document.exitFullscreen) {
    document.exitFullscreen();
  } else if (document.mozCancelFullScreen) { /* Firefox */
    document.mozCancelFullScreen();
  } else if (document.webkitExitFullscreen) { /* Chrome, Safari and Opera */
    document.webkitExitFullscreen();
  } else if (document.msExitFullscreen) { /* IE/Edge */
    document.msExitFullscreen();
  }
  var elem = document.getElementById("character_select");
  elem.classList.add("hide");
}

function IosCharacterSelector() {
  var elem = document.getElementById("character_select");
  elem.classList.remove("hide");
}

function IosExitCharacterSelector() {
  var elem = document.getElementById("character_select");
  elem.classList.add("hide");
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

// function onEndFrame() {
//   finalMessage = "";
//   finalMessage = directionX + " " + directionY + " " + pressedJump;
//   pressedJump = 0;
//   doSend(finalMessage);
// }

function vibrate(number = 30) {
  if (!("vibrate" in navigator)) {
    //  alert("Vibrate not supported!");
    return;
  }
  // navigator.vibrate(30);
  navigator.vibrate(number);
}
function vibrate(pattern = [30]) {
  if (!("vibrate" in navigator)) {
    //  alert("Vibrate not supported!");
    return;
  }
  navigator.vibrate(pattern);
}