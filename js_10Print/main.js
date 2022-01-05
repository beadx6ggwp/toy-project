var ctx,
    width,
    height;
var animation,
    loop = true;
var ctx_backColor = "#222";

window.onload = function () {
    ctx = CreateDisplay("myCanvas", 800, 600);
    width = ctx.canvas.width; height = ctx.canvas.height;

    main();
}

// ----------------------------------------------------------
function main() {
    console.log("Start");
    var str_arr = [
        "-----------------------------",
        "gapW, gapH : 可調整間距       (2 ~ 100)",
        "lineWidth  : 可調整線的寬度    (1 ~ 10)",
        "times      : 可調整速度       (1 ~ 100)",
        "chance     : 可調整歪斜的機率  (0.0 ~ 0.9)",
        "調整完畢後輸入 main() 重新生成"
    ];
    console.log("可調整參數:");
    console.log(str_arr.join("\r\n"));

    loop = true;
    x = 0; y = 0;

    ctx.fillStyle = ctx_backColor;
    ctx.fillRect(0, 0, width, height);
    ctx.lineCap = "round";

    window.requestAnimationFrame(mainLoop);
}

var x = 0, y = 0;
var gapW = 20, gapH = 20;
var lineWidth = 2;
var times = 10;
var chance = 0.5;
var status = 0;
var color = "#FFF";

function mainLoop(timestamp) {
    //Clear
    //ctx.fillStyle = ctx_backColor;
    //ctx.fillRect(0, 0, width, height);
    //--------Begin-----------

    for (var i = 0; i < times; i++) {
        status = Math.random() >= chance ? 1 : 0;
        makeLine(status);

        x += gapW;
        if (x > width) {
            y += gapH;
            x = 0;
        }

        if (y > height) loop = false;
    }

    //--------End---------------
    if (loop) {
        window.requestAnimationFrame(mainLoop);
    } else {
        // over
    }
}
function makeLine(status) {
    if (status == 1) {
        DrawLine(x, y, x + gapW, y + gapH, lineWidth, color);
    }
    else {
        DrawLine(x, y + gapH, x + gapW, y, lineWidth, color);
    }
}
function randomInt(min, max) {
    return Math.floor(Math.random() * (max - min + 1) + min);
}
function random(min, max) {
    return Math.random() * (max - min) + min;
}
function DrawLine(x1, y1, x2, y2, w, color) {
    ctx.lineWidth = w;
    ctx.strokeStyle = color;
    ctx.beginPath();
    ctx.moveTo(x1, y1);
    ctx.lineTo(x2, y2);
    ctx.stroke();
}


//---------------------
function CreateDisplay(id, width, height) {
    let canvas = document.createElement("canvas");
    canvas.id = id;
    canvas.width = width;
    canvas.height = height;
    canvas.style.cssText = [
        "display: block;",
        "margin: 0 auto;",
        "background: #FFF;",
        "border:1px solid #000;"
    ].join("");
    document.body.appendChild(canvas);

    return canvas.getContext("2d");
}