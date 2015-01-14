define([], function() {
    return Backbone.Model.extend({
        defaults: {
            'Id': null,
            'CategoryCode': null,
            'CategoryName': null,
            'SerialNo': null,
            'Menus':null
        },
        urlRoot: '/api/MenuApi',
        idAttribute:'Id'
    });
});