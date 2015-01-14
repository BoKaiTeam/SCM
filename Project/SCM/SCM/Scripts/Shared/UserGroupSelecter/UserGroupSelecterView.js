define([
        'Shared/UserGroupSelecter/UserGroupSelecterItemView',
        'Config/UserGroupManage/UserGroupCollection',
        'text!tpl/Shared/UserGroupSelecter/UserGroupSelecterTpl.html',
        'HttpStatusHandle'
],
    function (UserGroupSelecterItemView, UserGroupCollection, tpl, HttpStatusHandle) {
        return Backbone.View.extend({
            tagName: 'div',
            className: 'btn-group',
            initialize: function (userGroup) {
                this.model = userGroup;
                this.listenTo(this.model, 'change', this.render);
                this.listenTo(this.model, 'destroy', this.remove);
                this.userGroups = new UserGroupCollection();
                this.listenTo(this.userGroups, 'add', this.AddOne);
                this.listenTo(this.userGroups, 'reset', this.AddAll);
                this.userGroups.fetch({
                    error: function (model, rst) {
                        HttpStatusHandle(rst, '读取部门列表');
                    }
                });
            },
            template: _.template(tpl),
            render: function () {
                var items = this.$('ul');
                this.$el.html(this.template(this.model.toJSON()));
                if (items.length > 0) {
                    this.$('ul').html(items.html());
                }
                return this.el;
            },
            AddOne: function (dept) {
                this.$('ul').append(new UserGroupSelecterItemView(dept).render());
            },
            AddAll: function () {
                this.$('ul').empty();
                this.userGroups.each(this.AddOne, this);
            },
            events: {
                'click li':'ChooseOne'
            },
            ChooseOne: function (e) {
                this.model.set({
                    'GroupCode': $(e.currentTarget).attr('groupcode'),
                    'GroupName': $(e.currentTarget).attr('groupname')
                });
            }
        });
    });