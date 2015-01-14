define([], function() {
    return Backbone.Model.extend({
        defaults: {
            'Id': null,
            'GroupCode': null,
            'GroupName': null,
            'GroupFun': [],
            'People': 0,
            'Fun':0,
            'GroupType': 0
        },
        idAttribute: 'Id',
        urlRoot:'/api/UserGroupApi'
    });
});