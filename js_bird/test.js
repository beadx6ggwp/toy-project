var canvas = document.getElementById('myCanvas');
var ctx = canvas.getContext('2d');

var PI = Math.PI;
var Width = canvas.width, Height = canvas.height;

var bird;
var pipes = [], pushTime = 60*1, pushCount = 0;;

var score = 0;

main();

function main(){
    console.log(Width,Height);
    bird = new Bird(Width/6,Height/2,15);
    pipes.push(new Pipe(Width, Height));
    loop();
}


function loop(){
    update();
    draw();
    requestAnimationFrame(loop);
}

function update(){
    bird.update();

    pushCount++;
    if(pushCount > pushTime){
        pipes.push(new Pipe(Width, Height));
        pushCount -= pushTime;
    }
    for(var i = 0;i < pipes.length;i++){
        pipes[i].update();

        if(pipes[i].cheak(bird)){
            console.log("Hit");
        }

        if(pipes[i].offScreen()){
            pipes.splice(i,1);
        }
    }
}

function draw(){
    //Clear
    ctx.fillStyle = "#000";
    ctx.fillRect(0, 0, Width, Height);
    //

    bird.render();
    for(var i = 0;i < pipes.length;i++){
        pipes[i].render();
    }

    ctx.fillStyle = "#FF0";
    //ctx.filltext(10,10,"123");
}

document.addEventListener("keypress",function(e){
    //console.log(e);
    if(e.key == " "){
        bird.up();
        console.log("space");
    }
},false);

document.addEventListener("mousedown",function(e){    
        bird.up();
},false);

document.addEventListener("mousemove",function(e){
    //console.log(e);
    //bird.x = e.clientX - canvas.offsetLeft;
    //bird.y = e.clientY - canvas.offsetTop;
    //console.log(bird.x,bird.y);
},false);

function random(max,min){
	return Math.round(Math.random()*(max-min)+min);
}