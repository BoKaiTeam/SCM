define(['Config/UserManage/UserModel'], function(UserModel) {
    return Backbone.Collection.extend({
        model: UserModel,
        url: '/api/UserApi'
    });
});