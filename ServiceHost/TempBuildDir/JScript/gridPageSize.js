
var combo = new Ext.form.ComboBox({
    name: 'perpage',
    width: 60,
    store: new Ext.data.ArrayStore({
        fields: ['id'],
        data: [
        ['10'],
        ['15'],
        ['25'],
        ['50']
        ]
    }),

    mode: 'local',
    value: '15',
    listWidth: 40,
    triggerAction: 'all',
    displayField: 'id',
    valueField: 'id',
    editable: false,
    forceSelection: true
});

combo.on('select', function (combo, record) {
    pageSize = parseInt(record.get('id'), 10);
    var lastOptions = store.lastOptions;
    if (lastOptions.params != null) {
        Ext.apply(lastOptions.params, {
            'limit': pageSize
        });
        store.load(lastOptions);
    }
    else {
        store.load({ 'limit': pageSize });
    }
},
this);