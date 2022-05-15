
// 指定したIDの要素の情報を取得する
export function getComponentInfo(id) {
    var element = document.getElementById(id);
    var rect = element.getBoundingClientRect();

    var size = {
        Height: rect.height,
        Width: rect.width
    }
    var obj = {
        Id: element.id,
        X: rect.left,
        Y: rect.top,
        Size: size
    };
    return obj;
}

export function getScreenSize() {
    var size = {
        Height: window.innerHeight,
        Width: window.innerWidth
    };
    return size;
}

//リサイズイベントにセットする
export function setOnResize(dotNetHelper) {
    window.addEventListener('resize', function () {
        var size = getScreenSize();
        dotNetHelper.invokeMethodAsync('ScreenResized', size);
    }, false);
}
