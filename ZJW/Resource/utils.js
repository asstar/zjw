
function cnDateFormat(date) {
        var y = date.getFullYear();
        var m = date.getMonth() + 1;
        var d = date.getDate();
        var h = date.getHours();
        return y + '年' + m + '月' + d + '日' ;

    }
function cnDateParse(s) {
    if (s != null && s != '') {
        var reg = /[\u4e00-\u9fa5]/  //利用正则表达式分隔
        var ss = (s.split(reg));
        var y = parseInt(ss[0], 10);
        var m = parseInt(ss[1], 10);
        var d = parseInt(ss[2], 10);
        var h = parseInt(ss[3], 10);
        if (!isNaN(y) && !isNaN(m) && !isNaN(d) && !isNaN(h)) {
            return new Date(y, m - 1, d, h);
        } else {
            return new Date();
        }
    } return new Date();
}
function parseDate(s) {
    if (s!=null&&s != '') {
        var dateMilliseconds = parseInt(s.replace(/\D/igm, ""));
        var result = new Date(dateMilliseconds);
        return result.getFullYear() + "-" + (result.getMonth() + 1) + "-" + result.getDate();
    }
}
