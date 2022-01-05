var cnavas,
    ctx,
    width,
    height;
var animation,
    loop = true;
var ctx_backColor = "#EEE";

window.onload = function () {
    canvas = document.getElementById("myCanvas");
    ctx = canvas.getContext("2d");
    canvas.width = width = window.innerWidth;
    canvas.height = height = window.innerHeight;

    main();
}
var clock;
function main() {
    console.log("Start");
    clock = new Clock(width / 2, height / 2, (width > height ? height / 2 : width / 2) - 100);
    mainLoop();
}

function mainLoop(timestamp) {
    //Clear
    ctx.fillStyle = ctx_backColor;
    ctx.fillRect(0, 0, canvas.width, canvas.height);
    //--------Begin-----------

    clock.update();
    clock.draw(ctx);

    //--------End---------------
    if (loop) {
        animation = window.requestAnimationFrame(mainLoop);
    } else {
        // over
    }
}

function getNow() {
    return new Date().getTime();
}

function Clock(x, y, r) {
    this.pos = { x: x, y: y };
    this.delay = 1000;
    this.lastTime = getNow();
    this.r = r;
    // sec,min,hour
    this.pointer = [
        { length: r, angle: 0 },
        { length: r * 4 / 5, angle: 0 },
        { length: r * 2 / 5, angle: 0 }
    ];
    this.update = function () {
        //console.log(new Date().getHours(), new Date().getMinutes(), new Date().getSeconds());
        let p = this.pointer;
        p[0].angle = new Date().getSeconds() * 6;
        p[1].angle = (new Date().getMinutes()) * 6 + ((p[0].angle % 360) / 360) * 6;
        p[2].angle = (new Date().getHours()) * 30 + ((p[1].angle % 360) / 360) * 30;
    }

    this.draw = function (ctx) {
        ctx.save();
        ctx.translate(this.pos.x, this.pos.y);
        ctx.rotate(-Math.PI / 2);
        //draw background
        ctx.lineWidth = 3;
        ctx.beginPath();
        ctx.arc(0, 0, this.r, 0, Math.PI * 2);
        ctx.stroke();

        for (let i = 1; i <= 12; i++) {
            let angle = i * 30;
            let cos = Math.cos(angle * Math.PI / 180);
            let sin = Math.sin(angle * Math.PI / 180);
            let len = this.r * 0.85;
            let size = 50;
            drawString(ctx, i + "",
                cos * len - size / 3, sin * len,
                "#000", size, "consolas",
                90, "center", 1);
        }

        for (let i = 0; i < 360; i += 6) {
            let cos = Math.cos(i * Math.PI / 180);
            let sin = Math.sin(i * Math.PI / 180);
            let MaxLen = this.r;
            let Len = Math.floor(i % 30) == 0 ? this.r * 0.04 : this.r * 0.015;
            ctx.lineWidth = 2;
            ctx.beginPath();
            ctx.moveTo((MaxLen - Len) * cos, (MaxLen - Len) * sin);
            ctx.lineTo(MaxLen * cos, MaxLen * sin);
            ctx.stroke();
        }
        for (let i = 0; i < this.pointer.length; i++) {
            let p = this.pointer[i];
            let cos = Math.cos(p.angle * Math.PI / 180);
            let sin = Math.sin(p.angle * Math.PI / 180);
            ctx.lineWidth = this.r * 3 / p.length;
            ctx.beginPath();
            ctx.moveTo(0, 0);
            ctx.lineTo(p.length * cos, p.length * sin);
            ctx.stroke();
        }
        ctx.fillStyle = "#333";
        ctx.beginPath();
        ctx.arc(0, 0, 5, 0, Math.PI * 2);
        ctx.fill();

        ctx.restore();
    }
}