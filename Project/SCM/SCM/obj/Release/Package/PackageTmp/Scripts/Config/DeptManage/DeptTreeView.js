define([
        "Config/DeptManage/DeptCollection",
        'Config/DeptManage/DeptView',
        'Config/DeptManage/DeptModel',
        "HttpStatusHandle",
        "css!style/tree.css"
    ],
    function (DeptCollection, DeptView,DeptModel, HttpStatusHandle) {
        return Backbone.View.extend(
        {
            initialize: function() {
                var me = this;
                this.depts = new DeptCollection();
                this.listenTo(this.depts, 'add', this.AddOne);
                this.depts.fetch({
                    success: function (model, rst) {
                    },
                    error: function(model, rst) {
                        HttpStatusHandle(rst,'部门');
                    }
                });
            },
            AddOne: function (dept) {
                var deptView = new DeptView(dept);
                this.$('#root').append(deptView.render());
                if (dept.get('Childs').length > 0) {
                    //有子节点
                    this.AddChild(deptView, dept.get('Childs'));
                }

            },
            AddChild: function (parentView, childs) {
                //递归有问题
                for (var i = 0; i < childs.length; i++) {
                    var deptView = new DeptView(new DeptModel(childs[i]));
                    parentView.AddChild(deptView);
                    if (childs[i].Childs.length > 0) {
                        this.AddChild(deptView, childs[i].Childs);
                    }
                }
            }
        });
    });