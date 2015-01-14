define([], function() {
    return Backbone.Model.extend({
        defaults: {
            'Id': null,
            'DeptCode': null,
            'DeptName': null,
            'ParentCode': null,
            'Childs': [],
            'People':0
        },
        urlRoot: '/api/DeptApi',
        idAttribute: 'Id'
    });
});