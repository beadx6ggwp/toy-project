var cnavas,
    ctx,
    width,
    height;
var animation,
    lastTime = 0,
    Timesub = 0,
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
    mainLoop(1);
}

function mainLoop(timestamp) {
    Timesub = timestamp - lastTime;
    lastTime = timestamp;
    //Clear
    ctx.fillStyle = ctx_backColor;
    ctx.fillRect(0, 0, canvas.width, canvas.height);
    //--------Begin-----------

    clock.update(Timesub);
    clock.draw(ctx);

    //--------End---------------
    let str1 = "Fps:" + 1000 / Timesub, str2 = "Sleep: " + Timesub;
    drawString(ctx, str1 + "\n" + str2,
        0, canvas.height - 21,
        "#000", 10, "consolas",
        0, 0, 0);
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
    this.lastTime = 0;
    this.r = r;
    // sec,min,hour
    this.pointer = [
        { length: r, angle: 0.0 },
        { length: r * 4 / 5, angle: 0.0 },
        { length: r * 2 / 5, angle: 0.0 }
    ];
    this.update = function (delta) {
        //console.log(new Date().getHours(), new Date().getMinutes(), new Date().getSeconds());

        let p = this.pointer;
        if (getNow() - this.lastTime > 1000) {
            //定準位
            p[0].angle = new Date().getSeconds() * 6;
            p[1].angle = (new Date().getMinutes()) * 6 + ((p[0].angle % 360) / 360) * 6;
            p[2].angle = (new Date().getHours()) * 30 + ((p[1].angle % 360) / 360) * 30;
            this.lastTime = getNow();
        }
        // delta might be NaN
        let fps = 1000 / delta;
        p[0].angle += 6 / fps;
        p[1].angle += 6 / 60 / fps;
        p[2].angle += 30 / 60 / 60 / fps;
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