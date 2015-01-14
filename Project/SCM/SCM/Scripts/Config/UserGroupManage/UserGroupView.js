define([
    'Config/UserGroupManage/UserGroupFunView',
    'Config/UserGroupManage/UserGroupFunCollection',
    'text!tpl/Config/UserGroupManage/UserGroupTpl.html',
    'HttpStatusHandle'
],
    function (UserGroupFunView,UserGroupFunCollection, tpl, HttpStatusHandle) {
    return Backbone.View.extend({
        tagName: 'div',
        className: 'col-xs-12 col-sm-6 col-md-6 col-lg-6 userGroupPanel',
        initialize:function(userGroup) {
            this.model = userGroup;
            this.funCollection = new UserGroupFunCollection();
            this.listenTo(this.model, 'change', this.render);
            this.listenTo(this.model, 'destroy', this.remove);
            this.listenTo(this.funCollection, 'add', this.AddOneFun);
            this.listenTo(this.funCollection, 'reset', this.AddAllFun);
        },
        template:_.template(tpl),
        render: function () {
            this.$el.html(this.template(this.model.toJSON()));
            return this.el;
        },
        events: {
            'click .edit': 'BeginEdit',
            'click .cancel': 'Cencel',
            'click .remove': 'Remove',
            'click .save':'Save'
        },
        Save: function () {
            var me = this;
            if (this.$('.groupCode').val() == '') {
                alert('用户组编码不能为空!');
                return;
            }
            if (this.$('.groupName').val() == '') {
                alert('用户组名不能为空！');
                return;
            }
            this.$('.save').button('loading');
            this.$('.cancel').addClass('disabled');
            this.model.save({
                'GroupCode': this.$('[name="GroupCode"]').val(),
                'GroupName': this.$('[name="GroupName"]').val(),
                'GroupFun': this.funCollection.toJSON()
            }, {
                success: function () {
                    me.EndEdit(true);
                    me.$('.save').button('reset');
                    me.$('.cancel').removeClass('disabled');
                },
                error: function (model, rst) {
                    me.$('.save').button('reset');
                    me.$('.cancel').removeClass('disabled');
                    HttpStatusHandle(rst, "保存用户组");
                },
                wait:true
            });
        },
        BeginEdit: function () {
            $('.userGroupPanel').addClass('hide');
            this.$el.removeClass('col-sm-6 col-md-6 col-lg-6 hide');
            this.$('[data-toggleedit="true"]').toggleClass('hide');
            //编辑用户组
            this.funCollection.fetch({
                data: {
                    'GroupCode': this.model.get('GroupCode') == null ? '*' : this.model.get('GroupCode')
                },
                success: function (model, rst) {
                },
                error: function(model, rst) {
                    HttpStatusHandle(rst, '获取用户组菜单');
                },
                reset:true
            });
        },
        EndEdit: function (save) {
            $('.userGroupPanel').removeClass('hide');
            this.$el.addClass('col-sm-6 col-md-6 col-lg-6');
            if (!save) {
                this.$('[data-toggleedit="true"]').toggleClass('hide');
            }
        },
        Cencel: function () {
            this.EndEdit(false);
            if (this.model.get('Id') == null) {
                this.model.destroy();
            }
        },
        AddOneFun: function (fun) {
            this.$('.list-group').append(new UserGroupFunView(fun).render());
        },
        AddAllFun: function () {
            this.$('.list-group').empty();
            this.funCollection.each(this.AddOneFun, this);
        },
        Remove: function () {
            if (!confirm("确认删除该用户组？")) {
                return;
            }
            if (this.model.get('People') > 0) {
                if (!confirm("该用户组下还有员工，删除该组将使这些员工失去归属组。确定删除？")) {
                    return;
                }
            }
            //删除
            this.$('.remove').button('loading');
            this.$('.edit').addClass('disabled');
            this.model.destroy(
            {
                error: function (model, rst) {
                    this.$('.remove').button('reset');
                    this.$('.edit').removeClass('disabled');
                    HttpStatusHandle(rst, '删除用户组');
                },
                wait:true
            });
        }
    });
});