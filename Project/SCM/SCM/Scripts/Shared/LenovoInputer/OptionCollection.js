define(['Shared/LenovoInputer/OptionModel'], function (OptionModel) {
    return Backbone.Collection.extend({
        model: OptionModel
    });
});