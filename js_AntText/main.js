var ant = "\u0489";
var sourceEle, resultEle;

window.onload = function () {
    console.log("Start");
    sourceEle = document.getElementById("source");
    resultEle = document.getElementById("result");
}

function btnClick() {
    console.log("Click");
    let str = sourceEle.value;
    resultEle.value = str.replace(/(.{0})/g, '$1' + ant);
}
function btnCopy() {
    copy(resultEle.value);
}

function copy(s) {
    var clip_area = document.createElement('textarea');
    clip_area.textContent = s;

    document.body.appendChild(clip_area);
    clip_area.select();

    document.execCommand('copy');
    clip_area.remove();
}