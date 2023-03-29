import '../node_modules/nipplejs/dist/nipplejs.js';


var joystick = nipplejs.create({
    zone: document.getElementById('zone_joystick'),
    mode: 'semi',
    color: 'white',
    dynamicPage: true,
    catchDistance: 100, 
});

joystick.on('move', function (evt, data) {
    directionX = data.vector.x;
    directionY = data.vector.y;
});