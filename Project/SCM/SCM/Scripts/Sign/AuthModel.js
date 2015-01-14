define([], function() {
    return Backbone.Model.extend({
        defaults: {
            'Id':null,
            'UserCode': null,
            'UserName':null,
            'UPwd': null,
            'Remain':false
        },
        urlRoot: '/api/SignApi',
        idAttribute:'Id'
    });
});