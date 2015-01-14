define([
        'text!tpl/Shared/UserGroupSelecter/UserGroupSelecterItemTpl.html'
],
    function (tpl) {
        return Backbone.View.extend({
            tagName: 'li',
            initialize: function (userGroup) {
                this.model = userGroup;
            },
            template: _.template(tpl),
            render: function () {
                this.$el.html(this.template(this.model.toJSON()));
                return this.el;
            }
        });
    });