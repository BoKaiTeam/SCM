define([],function() {
    return Backbone.Model.extend({
        defaults: {
            'Value': null,
            'Display':null,
        },
        idAttribute:'Id'
    });
})