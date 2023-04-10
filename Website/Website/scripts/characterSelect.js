let selectCharacter = false;
console.log(selectCharacter);

let infoCard = document.getElementById("character_info");
let charPic = document.getElementById("character_card");

var currentChar = 0;

var img1 = document.createElement("img");
img1.src = "./images/Taco.png";

var img2 = document.createElement("img");
img2.src = "./images/Ruben.png";

var img3 = document.createElement("img");
img3.src = "./images/Ruben.png";

var charInfo1 = "Very yummy";
var charInfo2 = "Beep boop";
var charInfo3 = "Loves hugs";

class Character{
    constructor(name, info, pic, id){
        this.name = name;
        this.info = info;
        this.pic = pic;
        this.id = id;
    }
}

const characterOne = new Character("Taco", charInfo1, img1, 1);
const characterTwo = new Character("Ruben", charInfo2, img2, 2);
const characterThree = new Character("Joeri", charInfo3, img3, 3);

document.getElementById("left_arrow").addEventListener("click", onClickBack);
document.getElementById("right_arrow").addEventListener("click", onClickNext);
document.getElementById("select_btn").addEventListener("click", onSelect);

const characterList = [characterOne, characterTwo, characterThree];

function onScreenOpen(){
    var charImageHolder = document.getElementById("character_card");
    while(charImageHolder.firstChild){
      charImageHolder.removeChild(charImageHolder.firstChild);
    }
    i = currentChar;
    infoCard.textContent = characterList[i].info;
    document.getElementById("character_card").appendChild(characterList[i].pic);
    console.log(infoCard, currentChar);
}

function onClickBack(){
    var charImageHolder = document.getElementById("character_card");
    while(charImageHolder.firstChild){
      charImageHolder.removeChild(charImageHolder.firstChild);
    }
    i = currentChar;
    i--;
    if(i < 0){
        i = characterList.length - 1;
    }
    infoCard.textContent = characterList[i].info;
    document.getElementById("character_card").appendChild(characterList[i].pic);
    currentChar = i;
    console.log(infoCard, currentChar);
}

function onClickNext(){
    var charImageHolder = document.getElementById("character_card");
    while(charImageHolder.firstChild){
      charImageHolder.removeChild(charImageHolder.firstChild);
    }
    i = currentChar;
    i++;
    if(i >= characterList.length){
        i = 0;
    }
    infoCard.textContent = characterList[i].info;
    document.getElementById("character_card").appendChild(characterList[i].pic);
    currentChar = i;
    console.log(infoCard, currentChar);
}

function onSelect(){
    if(selectCharacter == false){
        selectCharacter = true;
    } else{
        selectCharacter = false;
    }
    console.log(selectCharacter);
}