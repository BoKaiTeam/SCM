define(['text!tpl/Shared/LenovoInputer/OptionTpl.html'],function(tpl) {
    return Backbone.View.extend({
        tagName: 'li',
        initialize:function(userItem) {
            this.model = userItem;
            this.listenTo(this.model, 'change', this.render);
            this.listenTo(this.model, 'destroy', this.remove);
        },
        template: _.template(tpl),
        render:function() {
            this.$el.html(this.template(this.model.toJSON()));
            return this.el;
        }
    });
})