define([
        'text!tpl/Shared/DeptSelecter/DeptSelecterItemTpl.html'
    ],
    function(tpl) {
        return Backbone.View.extend({
            tagName:'li',
            initialize:function(dept) {
                this.model = dept;
            },
            template:_.template(tpl),
            render: function() {
                this.$el.html(this.template(this.model.toJSON()));
                return this.el;
            }
        });
    });