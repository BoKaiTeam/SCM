define([
        'text!tpl/Config/DeptManage/DeptTpl.html',
        'HttpStatusHandle',
        'Config/DeptManage/DeptModel'
    ],
    function (tpl, HttpStatusHandle, DeptModel) {
        var DeptView= Backbone.View.extend({
            tagName: "li",
            initialize: function(model,depts ) {
                this.model = model;
                this.depts = depts;
                this.listenTo(this.model, 'destroy', this.remove);
                //this.listenTo(this.model, 'change', this.render);
            },
            template: _.template(tpl),
            render: function () {
                this.$el.html(this.template(this.model.toJSON()));
                return this.el;
            },
            AddChild: function (childView) {
                this.$('ul:first').append(childView.render());
            },
            events: {
                'click b:first': 'ShowBtn',
                'click .add:first': 'ClickAdd',
                'click .del:first': 'ClickDel',
                'click .edit:first': 'BeginEdit',
                'click .editSave:first':'ClickEditSave',
                'click .editCancel:first': 'ClickEditCancel',
                'click .btn-group:first': 'ClickBtn'
            },
            ShowBtn: function() {
                $('.btn-group').addClass('hide');
                this.$('.btn-group:first').toggleClass('hide');
            },
            ClickAdd: function () {
                //点击增加
                var deptView = new DeptView(new DeptModel({ 'ParentCode': this.model.get('DeptCode') }), this.depts);
                this.AddChild(deptView);
                deptView.BeginEdit();
            },
            ClickDel: function () {
                if (this.model.get('DeptCode') == 'root') {
                    alert('根部门不允许删除！');
                    return;
                }
                if (!confirm("确认删除该部门？")) {
                    return;
                }
                if (this.model.get('People') > 0) {
                    if (!confirm("该部门下还有员工，删除该部门将使这些员工失去归属部门。确定删除？")) {
                        return;
                    }
                }
                //点击删除
                this.$('.del:first').button('loading');
                if (!this.$('ul:first').html()) {
                    this.model.destroy({
                        success:function(model, rst) {
                            this.$('.del:first').button('reset');
                        },
                        errot: function (model, rst) {
                            this.$('.del:first').button('reset');
                            HttpStatusHandle(rst, '删除部门');
                        },
                        wait: true
                    });
                } else {
                    alert('该部门存在子部门，不允许删除！');
                }
            },
            BeginEdit: function () {
                //点击编辑
                this.$('form:first').removeClass('hide');
            },
            EndEdit:function() {
                this.$('form:first').addClass('hide');
                this.$('.deptCode:first').val(this.model.get('DeptCode'));
                this.$('.deptName:first').val(this.model.get('DeptName'));
            },
            ClickEditSave: function () {
                var me = this;
                this.$('.editSave:first').button('loading');
                this.$('.editCancel:first').button('loading');
                this.model.save({
                    'DeptCode': this.$('.deptCode:first').val(),
                    'DeptName': this.$('.deptName:first').val()
                }, {
                    success: function (model, rst) {
/*                        me.$('.editSave:first').button('reset');
                        me.$('.editCancel:first').button('reset');
                        me.EndEdit();*/
                        me.depts.fetch({ reset: true });
                    },
                    error: function (model, rst) {
                        me.$('.editSave:first').button('reset');
                        me.$('.editCancel:first').button('reset');
                        HttpStatusHandle(rst, "编辑部门");
                    },
                    wait: true
                });
            },
            ClickEditCancel: function () {
                if (this.model.get('Id') == null) {
                    this.model.destroy();
                    return;
                }
                this.EndEdit();
            },
            ClickBtn: function () {
                this.$('.btn-group:first').addClass('hide');
            }
        });
        return DeptView;
    });