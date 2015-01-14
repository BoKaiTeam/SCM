define(['Shared/LenovoInputer/OptionCollection', 'Shared/LenovoInputer/OptionView', 'HttpStatusHandle', 'text!tpl/Shared/LenovoInputer/LenovoInputerTpl.html'], function (OptionCollection, OptionView, HttpStatusHandle,tpl) {
    return Backbone.View.extend({
        tagName: 'div',
        className: 'dropdown',
        //初始化函数，输入OptionModel,此model会作为最终结果输出，输入远程url
        initialize: function (model, url) {
            this.model = model;
            this.url = url;
            this.collection = new OptionCollection();
            this.listenTo(this.model, 'change', this.render);
            this.listenTo(this.collection, 'add', this.AddOne);
            this.listenTo(this.collection, 'reset', this.Clear);
        },
        template: _.template(tpl),
        render:function() {
            this.$el.html(this.template(this.model.toJSON()));
            return this.el;
        },
        AddOne:function(option) {
            this.$('ul').append(new OptionView(option).render());
        },
        Clear: function () {
            this.$('ul').empty();
            //this.collection.each(this.AddOne, this);
        },
        events: {
            'change input': 'ConditionChanged',
            'click li':'ClickOption'
        },
        ShowOptions:function() {
            this.$el.addClass('open');
        },
        HideOptions:function() {
            this.$el.removeClass('open');
        },
        ConditionChanged: function () {
            if (this.$('input').val() == '') {
                //条件为空，清空
                this.model.set({
                    'Display':null,
                    'Value': null,
                    'Stamp': new Date().getTime()
                });
                console.info(this.model.toJSON());
                this.collection.reset();
                return;
            }
            var self = this;
            this.collection.fetch({
                url:this.url,
                data: {
                    condition: this.$('input').val()
                },
                success: function (model, rst) {
                    self.ShowOptions();
                },
                error:function(model,rst) {
                    HttpStatusHandle(rst,'');
                },
                wait:true,
                add: true,
                remove: true,
                merge:true
            });
        },
        ClickOption:function(e) {
            this.model.set({
                'Display': $(e.currentTarget).attr('Display'),
                'Value': $(e.currentTarget).attr('Value'),
                'Stamp': new Date().getTime()
            });
            this.collection.reset();
            this.HideOptions();
        }
    });
})