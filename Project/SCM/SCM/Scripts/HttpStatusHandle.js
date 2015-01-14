define([], function() {
    return function (rst,content) {
        if (rst.status == 200) {
            return true;
        }
        switch (rst.statusText) {
            case 'System Exception':
                alert("系统发生异常！请联系管理员！");
                return false;
            case 'Sigin Failure':
                alert("登陆失效，请重新登陆！");
                location.href = "/Sign/NoAuthority";
                return false;
            case 'Data Not Found':
                alert("未查询到" + content + "数据！");
                return false;
            case 'No Authority':
                alert("您没有权限访问" + content + "数据！");
                return false;
            case 'Deal Failure':
                alert(content + "操作失败");
                return false;
            default:
                alert("系统发生未知错误，请联系管理员！");
                return false;
        }
    }
});