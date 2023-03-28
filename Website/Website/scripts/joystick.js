import nipplejs from 'nipplejs';

var joystick = nipplejs.create({
    zone: document.getElementById('zone_joystick'),
    mode: 'static',
    position: { left: '50%', top: '50%' },
    color: 'white'
});

joystick.on('move', function (evt, data) {
    console.log(data);
});
