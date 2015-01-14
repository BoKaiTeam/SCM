define([
    'Shared/DeptSelecter/DeptSelecterView',
    'Config/DeptManage/DeptModel',
    'Config/UserGroupManage/UserGroupModel',
    'Shared/UserGroupSelecter/UserGroupSelecterView',
    'text!tpl/Config/UserManage/UserTpl.html',
    'md5',
    'HttpStatusHandle'
], function (DeptSelecterView, DeptModel, UserGroupModel, UserGroupSelecterView, tpl,md5, HttpStatusHandle) {
    return Backbone.View.extend({
        tagName: 'tr',
        initialize: function(model) {
            this.model = model;
            this.listenTo(this.model, 'change', this.render);
            this.listenTo(this.model, 'destroy', this.remove);
        },
        template: _.template(tpl),
        render: function () {
            this.$el.html(this.template(this.model.toJSON()));
            return this.el;
        },
        BeginEdit: function () {
            this.$('[data-toggleedit=true]').toggleClass('hide');
            this.dept = new DeptModel({
                'DeptCode': this.model.get('DeptCode'),
                'DeptName': this.model.get('DeptName')
            });
            this.userGroup = new UserGroupModel({
                'GroupCode': this.model.get('GroupCode'),
                'GroupName': this.model.get('GroupName')
            });
            this.$('.deptSelecter').html(new DeptSelecterView(this.dept).render());
            this.$('.userGroupSelecter').html(new UserGroupSelecterView(this.userGroup).render());
        },
        EndEdit: function() {
            this.$('[data-toggleedit=true]').toggleClass('hide');
        },
        events: {
            'click .edit': 'BeginEdit',
            'click .del': 'Del',
            'click .status': 'SetStatus',
            'click .cancel': 'EditCancel',
            'click .save':'EditSave'
        },
        EditCancel: function() {
            this.dept.destroy();
            this.userGroup.destroy();
            if (this.model.get('Id') == null) {
                this.model.destroy();
            } else {
                this.EndEdit();
            }
        },
        EditSave: function () {
            var me = this;
            this.$('.save').button('loading');
            this.$('.cancel').addClass('disabled');
            this.model.save({
                    'UserCode': this.$('.userCode').val(),
                    'UserName': this.$('.userName').val(),
                    'DeptCode': this.dept.get('DeptCode'),
                    'DeptName': this.dept.get('DeptName'),
                    'GroupCode': this.userGroup.get('GroupCode'),
                    'GroupName': this.userGroup.get('GroupName'),
                    'Md5': md5.hexMd5(this.$('.userCode').val())
                },
                {
                    success: function() {
                        me.dept.destroy();
                        me.userGroup.destroy();
                    },
                    error: function(model, rst) {
                        me.$('.save').button('reset');
                        this.$('.cancel').removeClass('disabled');
                        HttpStatusHandle(rst, '保存用户数据');
                    },
                    wait: true,
                    change:true
                }
            );
        },
        Del:function() {
            if (!confirm('确定删除该用户？')) {
                return;
            }
            this.$('.edit').addClass('disabled');
            this.$('.status').addClass('disabled');
            this.$('.del').button('loading');
            this.model.destroy({
                error: function(model, rst) {
                    this.$('.edit').removeClass('disabled');
                    this.$('.status').removeClass('disabled');
                    this.$('.del').button('reset');
                    HttpStatusHandle(rst, '删除用户');
                },
                wait:true
            });
        },
        SetStatus:function() {
            var enabled = this.model.get('Enabled');
            this.$('.edit').addClass('disabled');
            this.$('.del').addClass('disabled');
            this.model.save({ 'Enabled': !enabled },
            {
                success:function() {
                    this.$('.edit').removeClass('disabled');
                    this.$('.del').removeClass('disabled');
                },
                error: function(model, rst) {
                    this.$('.edit').removeClass('disabled');
                    this.$('.del').removeClass('disabled');
                    HttpStatusHandle(rst, '修改用户状态');
                },
                wait: true
            });
        }
    });
});