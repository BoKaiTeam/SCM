define([
        'Shared/DeptSelecter/DeptSelecterItemView',
        'Config/DeptManage/DeptCollection',
        'text!tpl/Shared/DeptSelecter/DeptSelecterTpl.html',
        'HttpStatusHandle'
    ],
    function (DeptSelecterItemView, DeptCollection,tpl, HttpStatusHandle) {
        return Backbone.View.extend({
            tagName: 'div',
            className: 'btn-group',
            initialize: function (dept) {
                this.model = dept;
                this.listenTo(this.model, 'change', this.render);
                this.listenTo(this.model, 'destroy', this.remove);

                this.depts = new DeptCollection();
                this.listenTo(this.depts, 'add', this.AddOne);
                this.listenTo(this.depts, 'reset', this.AddAll);
                this.depts.fetch({
                    error: function(model, rst) {
                        HttpStatusHandle(rst, '读取部门列表');
                    }
                });
            },
            template: _.template(tpl),
            render: function () {
                var item = this.$('ul');
                this.$el.html(this.template(this.model.toJSON()));
                if (item.length > 0) {
                    this.$('ul').html(item.html());
                }
                return this.el;
            },
            AddOne:function(dept) {
                this.$('ul').append(new DeptSelecterItemView(dept).render());
            },
            AddAll: function () {
                this.$('ul').empty();
                this.depts.each(this.AddOne, this);
            },
            events: {
                'click li': 'ChooseOne'
            },
            ChooseOne: function (e) {
                this.model.set({
                    'DeptCode': $(e.currentTarget).attr('deptcode'),
                    'DeptName': $(e.currentTarget).attr('deptname')
                });
            }
        });
    });