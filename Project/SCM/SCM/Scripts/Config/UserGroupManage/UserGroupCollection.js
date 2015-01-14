define(['Config/UserGroupManage/UserGroupModel'], function(UserGroupModel) {
    return Backbone.Collection.extend({
        model: UserGroupModel,
        url:'/api/UserGroupApi'
    });
});