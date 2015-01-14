define(['Home/MenuCategoryModel'],function(MenuCategoryModel) {
    return Backbone.Collection.extend({
        model: MenuCategoryModel,
        url: '/api/MenuApi'
    });
})