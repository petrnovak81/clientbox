@Code
    ViewData("Title") = "MojeServisky"
    Layout = "~/Views/Shared/_Layout.vbhtml"
    Dim db As New Data4995Entities
    Dim _user = db.AG_tblUsers.FirstOrDefault(Function(e) e.UserLogin = User.Identity.Name)
    Dim editor As Boolean = (User.IsInRole("hanzl@agilo.cz") Or User.IsInRole("fakturace@agilo.cz") Or User.IsInRole("novak@agilo.cz"))
End Code

<div id="main" style="height: 100%;">
    <div data-role="grid"
         data-filterable="true"
         data-column-menu="true"
         data-pageable="true"
         data-editable="false"
         data-sortable="true"
         data-selectable="true"
         data-scrollable="true"
         data-resizable="true"
         data-no-records="{ template: '<h3 style=\'text-align:center;margin-top:16px;\'>Žádné záznamy</h3>' }"
         data-toolbar="[{ template: kendo.template($('#toolbar').html()) }]"
         data-navigatable="true"
         data-columns="[
         @Code
             If editor Then
                 @<text>{ 'template': '#=btnDetail()#', 'title': ' ', 'width': 44 },</text>
             End If
         End Code
         { 'field': 'UserLastName', title: 'Příjemce provize' },
         { 'field': 'NazevFirmy', title: 'Firma - klient' },
         { 'field': 'NazevSmlouvy', title: 'Název smlouvy' },
         { 'field': 'rr_TypServisniSmlouvyHodnota', title: 'Typ Smlouvy' },
         { 'field': 'PlatiOd', format: '{0:d}', title: 'Platí Od' },
         { 'field': 'UkoncenaKeDni', format: '{0:d}', title: 'Ukončena ke dni' },
         { 'field': 'rr_TypSSProvizeText', title: 'Typ provize' },
         { 'field': 'FixniCastka', title: 'Fixní částka' },
         { 'field': 'ProcentoProvizeSS', title: 'Procento provize' }]"
         data-bind="source: AGsp_Get_SmlouvyServisniProvizeDefSeznam }" style="height: 100%;"></div>
</div>

<div class="modal fade" id="modal_moje_servisky">
    <div class="modal-dialog modal-md">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">Editace</h4>
                <button type="button" class="close" data-dismiss="modal">&times;</button>             
            </div>
            <div class="modal-body">
                <div class="form-group">
                    <label for="UserLastName">Příjemce provize:</label>
                    <input data-role="dropdownlist" id="UserLastName" data-value-primitive="true" data-value-field="value" data-text-field="text" data-bind="source: users, value: selected.IDUserTechnika" class="form-control">
                </div>
                <div class="form-group">
                    <label for="NazevSmlouvy">Název smlouvy:</label>
                    <input data-role="dropdownlist" id="NazevSmlouvy" data-value-primitive="true" data-value-field="value" data-text-field="text" data-bind="source: smlouvy, value: selected.IDSmlouvy" class="form-control">
                </div>
                <div class="form-group">
                    <label for="rr_SSTypProvize">Typ provize:</label>
                    <input data-role="dropdownlist" id="rr_SSTypProvize" data-value-primitive="true" data-value-field="value" data-text-field="text" data-bind="source: provize, value: selected.rr_SSTypProvize" class="form-control">
                </div>
                <div class="form-group">
                    <label for="FixniCastka">Fixní částka:</label>
                    <input data-role="numerictextbox" min="0" class="form-control" id="FixniCastka" data-bind="value: selected.FixniCastka">
                </div>
                <div class="form-group">
                    <label for="ProcentoProvizeSS">Procento provize:</label>
                    <input data-role="numerictextbox" min="0" class="form-control" id="ProcentoProvizeSS" data-bind="value: selected.ProcentoProvizeSS">
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-success" data-bind="events: { click: save }"><span class="k-icon k-i-save"></span> Uložit</button>
                <button type="button" class="btn btn-default" data-dismiss="modal"><span class="k-icon k-i-cancel-outline"></span> Zavřít</button>
            </div>
        </div>
    </div>
</div>

<script id="toolbar" type="text/x-kendo-template">
    <h3 class="text-center border-bottom">Seznam servisních smluv technika - moje servisky</h3>
    <a class="k-button" id="btn-add" href="\#"><span class="k-icon k-i-plus"></span> Přidat záznam</a>
</script>

<script>
    function btnDetail() {
        return '<button class="btn btn-info" data-bind="events: { click: btndetail }"><span class="k-icon k-i-edit"></span></button>';
    };

    $(function () {
        var viewModel = kendo.observable({
            users: new kendo.data.DataSource({
                schema: {
                    data: "data",
                    errors: "error"
                },
                transport: {
                    read: {
                        url: "@Url.Action("Uzivatele", "Api/Service")",
                        type: "GET"
                    },
                    parameterMap: function (options, operation) {
                        var pm = kendo.data.transports.odata.parameterMap(options);
                        return pm;
                    }
                }
            }),
            smlouvy: new kendo.data.DataSource({
                schema: {
                    data: "data",
                    errors: "error"
                },
                transport: {
                    read: {
                        url: "@Url.Action("AGsp_Get_SmlouvyServisniSeznam_CBX", "Api/Service")",
                        type: "GET"
                    },
                    parameterMap: function (options, operation) {
                        var pm = kendo.data.transports.odata.parameterMap(options);
                        return pm;
                    }
                }
            }),
            provize: new kendo.data.DataSource({
                schema: {
                    data: "data",
                    errors: "error"
                },
                transport: {
                    read: {
                        url: "@Url.Action("AGsp_Do_VyuctovaniDoplnitUctovaciObdobi_CBX", "Api/Service")",
                        type: "GET"
                    },
                    parameterMap: function (options, operation) {
                        var pm = kendo.data.transports.odata.parameterMap(options);
                        return pm;
                    }
                }
            }),
            AGsp_Get_SmlouvyServisniProvizeDefSeznam: new kendo.data.DataSource({
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true,
                pageSize: 100,
                schema: {
                    data: "data",
                    total: "total",
                    errors: "error",
                    model: {
                        id: "IDSSProvizeDef",
                        fields: {
                            PlatiOd: { type: "date" },
                            UkoncenaKeDni: { type: "date" }
                        }
                    }
                },
                transport: {
                    read: {
                        url: "@Url.Action("AGsp_Get_SmlouvyServisniProvizeDefSeznam", "Api/Service")",
                        type: "GET"
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
                error: function (e) {
                    e.sender.cancelChanges();
                    if (e.errors) {
                        alert(e.errors);
                    } else {
                        alert("Systémová chyba aplikace. Kontaktujte vývojáře.");
                    }
                }
            }),
            selected: null,
            btndetail: function (e) {
                var data = e.data.toJSON();
                this.set("selected", data);
                $('#modal_moje_servisky').modal("show");
            },
            save: function (e) {
                var that = this;
                var data = this.get("selected").toJSON();
                $.post('@Url.Action("AGsp_Do_IU_SmlouvaServisniProvizeDef", "Api/Service")', data, function (result) {
                    if (result.error) {
                        alert(result.error);
                    } else {
                        $('#modal_moje_servisky').modal("hide");
                        that.AGsp_Get_SmlouvyServisniProvizeDefSeznam.read();
                    }
                });
            }
        })
        kendo.bind(document.body, viewModel);
        $(document).on('click', '#btn-add', function (e) {
            viewModel.set("selected", {
                "IDSSProvizeDef": 0,
                "UserLastName": "@_user.UserLastName",
                "IDSmlouvy": 0,
                "NazevSmlouvy": "",
                "Firma": "",
                "NazevFirmy": "",
                "rr_TypServisniSmlouvyHodnota": "",
                "PlatiOd": new Date(),
                "UkoncenaKeDni": null,
                "IDUserTechnika": parseInt("@_user.IDUser"),
                "rr_SSTypProvize": 0,
                "rr_TypSSProvizeText": "",
                "FixniCastka": 0,
                "ProcentoProvizeSS": 0
            })
            $('#modal_moje_servisky').modal("show");
        })
    });
</script>

