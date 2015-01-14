define(['Config/UserGroupManage/UserGroupFunModel'], function (UserGroupFunModel) {
    return Backbone.Collection.extend({
        model: UserGroupFunModel,
        url: '/api/UserGroupFunApi'
    });
})