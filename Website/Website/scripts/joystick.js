import '../node_modules/nipplejs/dist/nipplejs.js';


var joystick = nipplejs.create({
    zone: document.getElementById('zone_joystick'),
    mode: 'semi',
    color: 'white',
    dynamicPage: true,
    catchDistance: 100, 
});

var hasVibrated = false;

joystick.on('move', function (evt, data) {
    directionX = data.vector.x;
    directionY = data.vector.y;
    if(!hasVibrated && data.force >= 1){
        vibrate(10);
        hasVibrated = true;
    }
    if(data.force < 1){
        hasVibrated = false;
    }
});

joystick.on('end', function (evt, data) {
    directionX = 0;
    directionY = 0;
    hasVibrated = false;
});