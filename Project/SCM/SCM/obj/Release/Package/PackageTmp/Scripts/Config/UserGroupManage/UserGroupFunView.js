define(['text!tpl/Config/UserGroupManage/UserGroupFunTpl.html'], function(tpl) {
    return Backbone.View.extend({
        tagName: 'li',
        className: 'list-group-item',
        template:_.template(tpl),
        initialize: function (userGroupFunModel) {
            this.model = userGroupFunModel;
            this.listenTo(this.model, 'change', this.render);
            this.listenTo(this.model, 'destroy', this.move);
        },
        render:function() {
            this.$el.html(this.template(this.model.toJSON()));
            this.$('label').tooltip();
            if (this.model.get('GroupType') == 0) {
                this.$el.addClass('AdminGroup');
            } else {
                this.$el.addClass('UserGroup');
            }
            return this.el;
        },
        events: {
            'change input':'ValChange'
        },
        ValChange:function(e) {
            var dataName = $(e.currentTarget).attr('data-name');
            switch (dataName) {
                case 'Queriable':
                    this.model.set({ 'Queriable': e.currentTarget.checked });
                    break;
                case 'Creatable':
                    this.model.set({ 'Creatable': e.currentTarget.checked });
                    break;
                case 'Deletable':
                    this.model.set({ 'Deletable': e.currentTarget.checked });
                    break;
                case 'Changable':
                    this.model.set({ 'Changable': e.currentTarget.checked });
                    break;
                case 'Checkable':
                    this.model.set({ 'Checkable': e.currentTarget.checked });
                    break;
            }
        }
    });
});