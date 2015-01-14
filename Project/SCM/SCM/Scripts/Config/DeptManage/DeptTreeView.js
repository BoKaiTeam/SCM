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
                this.depts = new DeptCollection();
                this.listenTo(this.depts, 'add', this.AddOne);
                this.listenTo(this.depts, 'reset', this.AddAll);
                this.depts.fetch({
                    success: function (model, rst) {
                    },
                    error: function(model, rst) {
                        HttpStatusHandle(rst,'部门');
                    }
                });
            },
            AddOne: function (dept) {
                var deptView = new DeptView(dept,this.depts);
                this.$('#root').append(deptView.render());
                if (dept.get('Childs').length > 0) {
                    //有子节点
                    this.AddChild(deptView, dept.get('Childs'));
                }

            },
            AddAll: function () {
                this.$('#root').empty();
                this.depts.each(this.AddOne, this);
            },
            AddChild: function (parentView, childs) {
                for (var i = 0; i < childs.length; i++) {
                    var deptView = new DeptView(new DeptModel(childs[i]),this.depts);
                    parentView.AddChild(deptView);
                    if (childs[i].Childs.length > 0) {
                        this.AddChild(deptView, childs[i].Childs);
                    }
                }
            }
        });
    });