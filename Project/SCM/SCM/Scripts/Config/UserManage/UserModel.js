define([], function() {
    return Backbone.Model.extend({
        defaults: {
            'Id': null,
            'UserCode': null,
            'UserName': null,
            'DeptCode': null,
            'DeptName': null,
            'GroupCode': null,
            'GroupName': null,
            'Enabled': false,
            'BuildDate': null,
            'BuildUser': null,
            'EditDate': null,
            'EditUser': null,
            'Md5':null
        },
        idAttribute: 'Id',
        urlRoot: '/api/UserApi'
    });
});