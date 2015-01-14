define([
        'Config/UserGroupManage/UserGroupCollection',
        'Config/UserGroupManage/UserGroupModel',
        'Config/UserGroupManage/UserGroupView',
        'HttpStatusHandle'
    ],
    function (UserGroupCollection,UserGroupModel, UserGroupView, HttpStatusHandle) {
        return Backbone.View.extend({
            initialize: function() {
                this.userGroups = new UserGroupCollection();
                this.listenTo(this.userGroups, 'add', this.AddOne);
                this.userGroups.fetch({
                    error:function(model, rst) {
                        HttpStatusHandle(rst, '读取用户组');
                    }
                });
            },
            AddOne: function (userGroup) {
                this.$('#userGrpPanel').append(new UserGroupView(userGroup).render());
            },
            events: {
                'click #btnAddGroup':'AddGroup'
            },
            AddGroup: function () {
                var userGroup = new UserGroupModel();
                this.userGroups.add(userGroup, {
                    silent:true
                });
                var view = new UserGroupView(userGroup);
                this.$('#userGrpPanel').prepend(view.render());
                view.BeginEdit();
            }
        });
    });