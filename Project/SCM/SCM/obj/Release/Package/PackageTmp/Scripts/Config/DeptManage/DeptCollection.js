define(['Config/DeptManage/DeptModel'], function (DeptModel) {
    return Backbone.Collection.extend({
        model: DeptModel,
        url:'/api/DeptApi'
    });
});