

// 指定したIDの要素の情報を取得する
window.getComponentInfo = (id, scrollLeft, scrollTop) => {

    var element = document.getElementById(id);
    var rect = element.getBoundingClientRect();
    var scrollX = 0;
    if (typeof scrollLeft !== "undefined") {
        scrollX = scrollLeft;
    }
    var scrollY = 0;
    if (typeof scrollTop !== "undefined") {
        scrollY = scrollTop;
    }
    var obj = {
        ID: element.id,
        X: rect.left,
        Y: rect.top,
        Height: rect.height,
        Width: rect.width,
        ScrollLeft: scrollX,
        ScrollTop: scrollY,
    };
    return obj;
};

// 指定したIDの要素とその子要素を取得する
window.getComponents = (sheetID) => {

    var sheet = document.getElementById(sheetID);
    if (sheet == null) {
        return null;
    }
    var elements = Array.from(sheet.children);
    var components = [];
    // 1つ目にシートを入れておく
    components.push(getComponentInfo(sheet.id));

    elements.forEach((element) => {
        if (element.id == undefined) {
            return null;
        }
        var info = getComponentInfo(element.id)
        components.push(info);
    });

    return Array.from(components);
};

// DLLにチャートエリアの情報を送る
function sendChartAreaInfo() {
    var scrollX = document.documentElement.scrollLeft;
    var scrollY = document.documentElement.scrollTop;
    var info = getComponentInfo('chart-area', scrollX, scrollY);
    DotNet.invokeMethodAsync('WebTCI', 'WindowResized', info);
}

function getCanvas() {
    var container = document.getElementById('chart-area');
    var elements = Array.from(container.children);
    var canvas = null;
    elements.forEach((element) => {
        // １つしかない想定
        canvas = element;
    });
    return canvas;
}

function resizeChartArea() {

    var clientHeight = document.documentElement.clientHeight;
    var canvas = getCanvas();
    var canvasRect = canvas.getBoundingClientRect();

    // 投与量エリアと同じ幅にする
    var dosingArea = document.getElementById('dosing-area');
    var dosingAreaRect = dosingArea.getBoundingClientRect();
    canvas.style.width = dosingAreaRect.width + 'px';

    // 下には投与量エリアしかない想定で高さを決定する
    canvas.style.height = (clientHeight - dosingAreaRect.height - canvasRect.top - 10) + 'px';
}

// 画面サイズが変化したらチャートエリアのサイズをdllに送る
window.onresize = () => {
    resizeChartArea();
    sendChartAreaInfo();
};

window.onscroll = (e) => {
    sendChartAreaInfo();

};

