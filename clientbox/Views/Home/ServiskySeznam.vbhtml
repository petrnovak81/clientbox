@Code
    ViewData("Title") = "Servisky seznam"
    Layout = "~/Views/Shared/_Layout.vbhtml"
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
         data-navigatable="true"
         data-toolbar="[{ template: kendo.template($('#toolbar').html()) }]"
         data-columns="[
         { 'template': '#=btnDetail()#', 'title': ' ', 'width': 44 },
         { 'field': 'NazevFirmy', title: 'Firma - klient' },
         { 'field': 'NazevSmlouvy', title: 'Číslo smlouvy/název' },
         { 'field': 'rr_TypServisniSmlouvyHodnota', title: 'Typ servisní smlouvy' },
         { 'field': 'rr_IntervalFakturaceSSText', title: 'Interval fakturace' },
         { 'field': 'DatumPoslednihoUctovanehoMesice', format: '{0:d}', title: 'Poslední vyúčtovaný měsíc' },
         { 'field': 'MesicniSazbaBezDPH', title: 'Měsíční sazba smlouvy bez DPH' },
         { 'field': 'PlatiOd', format: '{0:d}', title: 'Platí od' },
         { 'field': 'UkoncenaKeDni', title: 'Ukončena ke dni' },
         { 'field': 'TextNafakturu', title: 'Text na fakturu' },
         { 'field': 'rr_FakturovatNaFirmuHodnota', title: 'Fakturovat na firmu' }]"
         data-bind="source: AGsp_Get_SmlouvyServisniSeznam" style="height: 100%;"></div>
</div>

<div class="modal fade" id="modal_servisky">
    <div class="modal-dialog modal-md">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">Editace</h4>
                <button type="button" class="close" data-dismiss="modal">&times;</button>
            </div>
            <div class="modal-body">
                <div class="form-group">
                    <label for="Firma">Firma - klient:</label>
                    <input data-role="dropdownlist" id="Firma" data-value-primitive="true" data-value-field="value" data-text-field="text" data-bind="source: firmy, value: selected.Firma" class="form-control">
                </div>
                <div class="form-group">
                    <label for="NazevSmlouvy">Číslo smlouvy/název:</label>
                    <input type="text" id="NazevSmlouvy" data-bind="value: selected.NazevSmlouvy" class="form-control" />
                </div>
                <div class="form-group">
                    <label for="rr_FakturovatNaFirmu">Fakturovat na firmu:</label>
                    <input data-role="dropdownlist" id="rr_FakturovatNaFirmu" data-value-primitive="true" data-value-field="value" data-text-field="text" data-bind="source: fakturovatna, value: selected.rr_FakturovatNaFirmu" class="form-control">
                </div>
                <div class="form-group">
                    <label for="rr_TypServisniSmlouvy">Typ servisní smlouvy:</label>
                    <input data-role="dropdownlist" id="rr_TypServisniSmlouvy" data-value-primitive="true" data-value-field="value" data-text-field="text" data-bind="source: typsmlouvy, value: selected.rr_TypServisniSmlouvy" class="form-control">
                </div>
                <div class="form-group">
                    <label for="rr_IntervalFakturaceSS">Interval fakturace:</label>
                    <input data-role="dropdownlist" id="rr_IntervalFakturaceSS" data-value-primitive="true" data-value-field="value" data-text-field="text" data-bind="source: intervaly, value: selected.rr_IntervalFakturaceSS" class="form-control">
                </div>
                <div class="form-group">
                    <label for="MesicniSazbaBezDPH">Měsíční sazba smlouvy bez DPH:</label>
                    <input data-role="numerictextbox" min="0" id="MesicniSazbaBezDPH" data-bind="value: selected.MesicniSazbaBezDPH" class="form-control">
                </div>
                <div class="form-group">
                    <label for="PlatiOd">Platí od:</label>
                    <input data-role="datepicker" id="PlatiOd" data-bind="value: selected.PlatiOd" class="form-control">
                </div>
                <div class="form-group">
                    <label for="UkoncenaKeDni">Ukončena ke dni:</label>
                    <input data-role="datepicker" id="UkoncenaKeDni" data-bind="value: selected.UkoncenaKeDni" class="form-control">
                </div>
                <div class="form-group">
                    <label for="TextNafakturu">Text na fakturu:</label>
                    <textarea id="TextNafakturu" rows="2" data-bind="value: selected.TextNafakturu" class="form-control"></textarea>
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
    <h3 class="text-center border-bottom">Seznam servisních smluv</h3>
    <a class="k-button" id="btn-add" href="\#"><span class="k-icon k-i-plus"></span> Přidat záznam</a>
</script>

<script>
    function btnDetail() {
        return '<button class="btn btn-info" data-bind="events: { click: btndetail }"><span class="k-icon k-i-edit"></span></button>';
    };

    $(function () {
        var viewModel = kendo.observable({
            firmy: new kendo.data.DataSource({
                schema: {
                    data: "data",
                    errors: "error"
                },
                transport: {
                    read: {
                        url: "@Url.Action("AGvw_FirmyAPobocky_CBX", "Api/Service")",
                        type: "GET"
                    },
                    parameterMap: function (options, operation) {
                        var pm = kendo.data.transports.odata.parameterMap(options);
                        return pm;
                    }
                }
            }),
            typsmlouvy: new kendo.data.DataSource({
                schema: {
                    data: "data",
                    errors: "error"
                },
                transport: {
                    read: {
                        url: "@Url.Action("AGvwrr_TypServisniSmlouvy_CBX", "Api/Service")",
                        type: "GET"
                    },
                    parameterMap: function (options, operation) {
                        var pm = kendo.data.transports.odata.parameterMap(options);
                        return pm;
                    }
                }
            }),
            intervaly: new kendo.data.DataSource({
                schema: {
                    data: "data",
                    errors: "error"
                },
                transport: {
                    read: {
                        url: "@Url.Action("AGvwrr_IntervalFakturaceSS", "Api/Service")",
                        type: "GET"
                    },
                    parameterMap: function (options, operation) {
                        var pm = kendo.data.transports.odata.parameterMap(options);
                        return pm;
                    }
                }
            }),
            fakturovatna: new kendo.data.DataSource({
                schema: {
                    data: "data"
                },
                transport: {
                    read: "@Url.Action("RegRest", "Api/Service")?Register=rr_FakturovatNaFirmu"
                }
            }),
            AGsp_Get_SmlouvyServisniSeznam: new kendo.data.DataSource({
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true,
                pageSize: 100,
                schema: {
                    data: "data",
                    total: "total",
                    errors: "error",
                    model: {
                        id: "IDSmlouvy",
                        fields: {
                            PlatiOd: { type: "date" },
                            UkoncenaKeDni: { type: "date" },
                            DatumPoslednihoUctovanehoMesice: { type: "date" }
                        }
                    }
                },
                transport: {
                    read: {
                        url: "@Url.Action("AGsp_Get_SmlouvyServisniSeznam", "Api/Service")",
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
                $('#modal_servisky').modal("show");
            },
            save: function (e) {
                var that = this;
                var data = this.get("selected").toJSON();
                if (data.PlatiOd) {
                    data.PlatiOd = kendo.toString(new Date(data.PlatiOd), "yyyy-MM-dd HH:mm:ss");
                }
                if (data.UkoncenaKeDni) {
                    data.UkoncenaKeDni = kendo.toString(new Date(data.UkoncenaKeDni), "yyyy-MM-dd HH:mm:ss");
                }
                $.post('@Url.Action("AGsp_Do_IU_SmlouvaServisni", "Api/Service")', data, function (result) {
                    if (result.error) {
                        alert(result.error);
                    } else {
                        $('#modal_servisky').modal("hide");
                        that.AGsp_Get_SmlouvyServisniSeznam.read();
                    }
                });
            }
        })
        kendo.bind(document.body, viewModel);
         $(document).on('click', '#btn-add', function (e) {
             viewModel.set("selected", {
                 "IDSmlouvy": 0,
                 "Firma": "",
                 "NazevFirmy": "",
                 "NazevSmlouvy": "",
                 "rr_TypServisniSmlouvy": 0,
                 "rr_TypServisniSmlouvyHodnota": "",
                 "rr_IntervalFakturaceSS": 0,
                 "rr_IntervalFakturaceSSText": "",
                 "MesicniSazbaBezDPH": 0,
                 "PlatiOd": null,
                 "UkoncenaKeDni": null,
                 "TextNafakturu": ""
             });
            $('#modal_servisky').modal("show");
        })
    });
</script>
