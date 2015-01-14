define([
        'Home/MenuCategoryCollection',
        'Home/MenuCategoryView',
        'Home/NavMenuCategoryView',
        'HttpStatusHandle',
        'Home/Router'
    ],
    function (MenuCategoryCollection, MenuCategoryView, NavMenuCategoryView, HttpStatusHandle, Router) {
        return Backbone.View.extend({
            initialize: function() {
                this.menuCategorys = new MenuCategoryCollection();
                var router = new Router();
                this.listenTo(this.menuCategorys, 'add', this.AddMenu);
                this.listenTo( this.menuCategorys, 'reset', this.AddAllMenu);
                this.menuCategorys.fetch({
                    success: function(model, rst) {

                    },
                    error: function(model, rst) {
                        HttpStatusHandle(rst, "功能菜单");
                    },
                    wait: true
                });
            },
            AddMenu: function(menuCategory) {
                this.$('#mainMenuPanel').append(new MenuCategoryView(menuCategory).render());
                this.$('#navMenuStart').after(new NavMenuCategoryView(menuCategory).render());
            },
            AddAllMenu: function() {
                this.menuCategorys.each(this.AddMenuCategory, this);
            }
        });
    });