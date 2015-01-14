define(['text!tpl/Home/NavMenuCategoryTpl.html'], function(tpl) {
    return Backbone.View.extend({
        tagName: 'li',
        className: 'dropdown hidden-lg hidden-md hidden-sm',
        initialize: function(menus) {
            this.model = menus;
        },
        template: _.template(tpl),
        render:function() {
            this.$el.html(this.template(this.model.toJSON()));
            return this.el;
        },
        events: {
            'click li':'ClickMenu'
        },
        ClickMenu: function (e) {
            $('#navMenuBtn').click();//收起菜单
        }
});
});