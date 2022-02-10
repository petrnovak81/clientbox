@Code
    ViewData("Title") = "Uživatelé"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div id="main">
    <div data-role="grid"
         data-column-menu="true"
         data-resizable="true"
         data-editable="inline"
         data-toolbar="['create', 'cancel']"
         data-filterable="true"
         data-sortable="true"
         data-selectable="true"
         data-no-records="{ template: '<h3 style=\'text-align:center;\'>Žádné záznamy</h3>' }"
         data-navigatable="true"
         data-columns="[
         { 'template': '#=btnDetail()#', 'title': ' ', 'width': 80 },
{ 'field': 'IDRole', 'template': '#=foreignKeyDs(IDRole, $.app.model.AG_tbRole, IDUser, \'RoleName\', \'IDRole\')#', 'editor': role_edit, 'title': 'Role' },
{ 'field': 'UserFirstName', 'title': 'Jméno' },
{ 'field': 'UserLastName', 'title': 'Příjmení' },
{ 'field': 'UserLogin', 'template': '#=input_email(UserLogin)#', 'editor': input_email_edit, 'title': 'Login', 'width': 120 },
{ 'field': 'UserMobilePhone', 'template': '#=input_phone(UserMobilePhone)#', 'editor': input_phone_edit, 'title': 'Telefon' },
{ 'field': 'ProcentoZHodinoveSazby', 'title': 'Procento z hod. sazby' },
{ 'field': 'ProcentoZMaleVzdalenky', 'title': 'Procento z malé vzdálenky' },
{ 'field': 'ProcentoZVelkeVzdalenky', 'title': 'Procento z velké vzdálenky' },
{ 'field': 'UserAccountEnabled', 'template': '#=input_switch(UserAccountEnabled)#', 'editor': input_switch_edit, 'title': 'Povoleno', 'width': 75 },
{ 'template': '#=resetButton()#', 'title': 'Reset hesla', 'width': 80 },
{ 'command': [ 'destroy', 'edit' ], 'width': 85 },
]"
         data-bind="source: AG_tblUsers,events: { dataBound: onDataBound, change: select }"></div>
</div>

<div class="modal fade" id="modal_detail">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-body">
                <h4>
                    Provize uživatele <span data-bind="text: UserName"></span>
                </h4>
                <div class="row">
                    <div class="col">
                        <div class="form-group">
                            <label class="text-muted">Procento z hod. sazby:</label>
                            <input data-role="numerictextbox" data-format="p2" data-step="0.1" data-min="0" data-bind="value: ds.ProcentoZHodinoveSazby" class="form-control">
                        </div>
                    </div>
                    <div class="col">
                        <div class="form-group">
                            <label class="text-muted">Procento z malé vzdálenky</label>
                            <input data-role="numerictextbox" data-format="p2" data-step="0.1" data-min="0" data-bind="value: ds.ProcentoZMaleVzdalenky" class="form-control">
                        </div>
                    </div>
                    <div class="col">
                        <div class="form-group">
                            <label class="text-muted">Procento z velké vzdálenky</label>
                            <input data-role="numerictextbox" data-format="p2" data-step="0.1" data-min="0" data-bind="value: ds.ProcentoZVelkeVzdalenky" class="form-control">
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col">
                        <label class="text-muted">Provize:</label>
                        <div data-role="grid"
                             data-filterable="false"
                             data-pageable="false"
                             data-editable="true"
                             data-sortable="false"
                             data-selectable="false"
                             data-scrollable="true"
                             data-resizable="true"
                             data-auto-bind="false"
                             data-toolbar="['save', 'cancel']"
                             data-no-records="{ template: '<h3 style=\'text-align:center;margin-top:16px;\'>Žádné položky</h3>' }"
                             data-navigatable="false"
                             data-columns="[
            { 'field': 'Produkt', 'title': 'Produkt' },
            { 'field': 'Popis', 'title': 'Popis' },
            { 'field': 'ProcentoProvize', 'format': '{0:p2}', 'title': 'Procento provize' }]"
                             data-bind="source: provize">
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-bind="events: { click: previous }"><span class="k-icon k-i-arrow-chevron-left"></span> Předchozí</button>
                    <button type="button" class="btn btn-default" data-bind="events: { click: next }">Následující <span class="k-icon k-i-arrow-chevron-right"></span></button>
                    <button type="button" class="btn btn-success" data-bind="events: { click: procentoSave }"><span class="k-icon k-i-save"></span> Uložit</button>
                    <button type="button" class="btn btn-default" data-dismiss="modal"><span class="k-icon k-i-cancel-outline"></span> Zavřít</button>
                </div>
            </div>
        </div>
    </div>
</div>

    <script>
        function btnDetail(e) {
            return '<button class="btn btn-info" data-bind="events: { click: btndetail }">Detail</button>';
        };

        function btnChat() {
            return '<button class="btn btn-success" data-bind="events: { click: btnchat }">Chat</button>';
        };

    function role_edit(container, options) {
        $('<input required name="' + options.field + '"/>')
            .appendTo(container)
            .kendoDropDownList({
                dataTextField: "RoleName",
                dataValueField: "IDRole",
                dataSource: {
                    transport: {
                        read: "@Url.Action("AG_tbRole_Read", "Api/Service")"
                    }
                }
            });
        };

    function foreignKeyDs(value, source, id, textField, valueField) {
        setTimeout(function () {
            var text = "NULL";
            var data = [];
            if (typeof source.view() !== 'undefined') {
                data = source.view();
            } else {
                data = source;
            }
            if (data.length === 0) {
                foreignKeyDs(value, source, id, textField, valueField);
            }
            if (textField === undefined) {
                textField = 'text';
            }
            if (valueField === undefined) {
                valueField = 'value';
            }
            var arr = $.grep(data, function (item, index) {
                return item[valueField] == value;
            });
            if (arr.length > 0) {
                text = arr[0][textField];
            }
            $('.fk_' + valueField + id).html(text)
        }, 500);
        return '<span class="fk_' + valueField + id +'"><i style="color:silver;">NULL</i></span>';
    }

    function resetButton() {
        return '<button type="button" class="btn btn-danger" data-bind="events: { click: resetPWD }"><span class="k-icon k-i-reset"></span> Reset</button>'
    }

    $(function (global) {
        "use strict";

        var app = global.app = global.app || {};

        app.model = kendo.observable({
            btndetail: function (e) {
                var grid = $(e.currentTarget).closest(".k-grid").data("kendoGrid");
                var row = $(e.currentTarget).closest("tr");
                grid.select(row);
                $('#modal_detail').modal("show");
            },
            next: function (e) {
                var di = this.get("ds");
                var cur = $('tr[data-uid="' + di.uid + '"]')
                var nex = cur.next();
                nex.click();
            },
            previous: function (e) {
                var di = this.get("ds");
                var cur = $('tr[data-uid="' + di.uid + '"]')
                var pre = cur.prev();
                pre.click();
            },
            resetPWD: function (e) {
                $.post("@Url.Action("AG_tblUsers_ResetPWD", "Api/Service")", { "IDUser": e.data.IDUser, "UserLogin": e.data.UserLogin }, function (res) {
                    if (res.message) {
                        alert(res.message)
                    } else {
                        alert("Heslo bylo úspěšně resetováno")
                    }
                })
            },
            onDataBound: function (e) {
                var that = this;
                var row = e.sender.tbody.find('tr:first');
                if (row.length > 0) {
                    e.sender.select(row);
                } else {
                    for (var item in original) {
                        that.set(item, original[item]);
                    }
                }
            },
            AG_tbRole: new kendo.data.DataSource({
                transport: {
                    read: {
                        url: "@Url.Action("AG_tbRole_Read", "Api/Service")",
                        type: "GET"
                    }
                }
            }),
            AG_tblUsers: new kendo.data.DataSource({
                schema: {
                    model: {
                        id: "IDUser",
                        fields: {
                            IDRole: { type: "number" },
                            UserFirstName: { type: "string" },
                            UserLastName: { type: "string" },
                            UserLogin: { type: "string" },
                            UserMobilePhone: { type: "string" },
                            ProcentoZHodinoveSazby: { type: "number" },
                            ProcentoZMaleVzdalenky: { type: "number" },
                            ProcentoZVelkeVzdalenky: { type: "number" },
                            UserAccountEnabled: { type: "boolean" }
                        }
                    }
                },
                batch: true,
                transport: {
                    read: {
                        url: "@Url.Action("AG_tblUsers_Read", "Api/Service")",
                        type: "GET"
                    },
                    create: {
                        url: "@Url.Action("AG_tblUsers_Create", "Api/Service")",
                        type: "POST"
                    },
                    update: {
                        url: "@Url.Action("AG_tblUsers_Update", "Api/Service")",
                        type: "POST"
                    },
                    destroy: {
                        url: "@Url.Action("AG_tblUsers_Destroy", "Api/Service")",
                        type: "POST"
                    },
                    parameterMap: function (options, operation) {
                        if (operation === "read") {
                            var pm = kendo.data.transports.odata.parameterMap(options);
                            if (pm.$filter) {
                                pm.$filter = pm.$filter.replace("eq ''", "eq null");
                            }
                            return pm;
                        } else {
                            return options.models[0];
                        }
                    }
                },
                requestEnd: function (e) {
                    app.model.AG_tbRole.read();
                    if (e.response) {
                        if (e.response.message) {
                            alert(e.response.message);
                        }
                    }
                }
            }),
            provize: new kendo.data.DataSource({
                schema: {
                    model: {
                        id: "IDProduktProvize",
                        fields: {
                            Produkt: { type: 'string' },
                            Popis: { type: 'string' },
                            ProcentoProvize: { type: 'number' }
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        var that = app.model.ds;
                        $.get("@Url.Action("AGsp_GetProduktProcentoProvizeSeznam", "Api/Service")", { iDUser: that.IDUser }, function (result) {
                            options.success(result.data);
                        });
                    },
                    update: function (options) {
                        var that = viewModel;
                        $.get("@Url.Action("AGsp_AddNewOrEditProduktProcentoProvize", "Api/Service")", {
                            IDProduktProvize: (options.data.IDProduktProvize ? options.data.IDProduktProvize : 0),
                            IDUser: that.ds.get("IDUser"),
                            Produkt: options.data.Produkt,
                            ProcentoProvize: options.data.ProcentoProvize,
                        }, function (result) {
                            if (result.error) {
                                alert(result.error);
                            };
                            options.success();
                        });
                    }
                },
                pageSize: 50,
                batch: false
            }),
            ds: {},
            UserName: "",
            select: function (e) {
                var that = this;
                var grid = e.sender;
                var item = $.map(grid.select(), function (a, b) {
                    return grid.dataItem(a);
                })[0];
                that.set("ds", item);
                that.set("UserName", " - " + item.UserFirstName + " " + item.UserLastName);
                that.provize.read();
            },
            procentoSave: function (e) {
                this.AG_tblUsers.sync();
            }
        });
        var original = app.model.toJSON();
        kendo.bind($("body"), app.model);
    })
    </script>
