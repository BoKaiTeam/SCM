define([], function() {
    return Backbone.Model.extend({
        defaults: {
            'Id': null,
            'GroupCode': null,
            'FunCode': null,
            'Queriable': null,
            'Creatable': null,
            'Changable': null,
            'Deletable': null,
            'Checkable':null
        },
        urlRoot:'/api/UserGroupFunApi'
    });
});