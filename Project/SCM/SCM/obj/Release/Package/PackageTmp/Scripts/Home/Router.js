﻿define([], function() {
    return Backbone.Router.extend({
        initialize:function() {
            Backbone.history.start();
        },
        routes: {
            '': 'Index',
            'goto/:controller/:action': 'Goto',
            '*error': function(error) {
                alert('error hash: ' + error);
            }
        },
        Index: function() {

        },
        Goto: function (controller,action) {
            $('#mainPanel').load(controller + '/' + action);
        }
    });
});