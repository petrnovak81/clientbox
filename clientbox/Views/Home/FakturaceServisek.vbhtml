@Code
    ViewData("Title") = "Fakturace servisek"
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
         { 'field': 'IDSSFaktury', title: 'ID smluvní faktura' },
         { 'field': 'IDSmlouvy', title: 'ID smlouvy' },
         { 'field': 'Firma', title: 'Název firmy' },
         { 'field': 'UctovanoOd', format: '{0:d}', title: 'Účtováno od' },
         { 'field': 'UctovanoDo', format: '{0:d}', title: 'Účtováno do' },
         { 'field': 'IDvykazPrace', title: 'Číslo pracáku', template: '#=jitNaPracak(IDvykazPrace)#' },
         { 'field': 'rr_StavPracakuHodnota', title: 'Stav pracáku' },
         { 'field': 'CisloFaktury', title: 'Č.fakt. Vario' },
         { 'field': 'rr_FakturovatNaFirmuHodnota', title: 'Fakturovat na' }]"
         data-bind="source: AGsp_Get_SmlouvyServisniGenerovanePausalky, bindToolbar: toolbar }" style="height: 100%;"></div>
</div>

<script id="toolbar" type="text/x-kendo-template">
    <h3 class="text-center border-bottom">Fakturace servisek</h3>
    <a class="k-button k-primary" href="\#" data-bind="events: { click: vygenerovat }">Vygenerovat faktury ze smluv za další období</a>
    <span class="ml-3">Další období k vyúčtování: </span>
    <input data-role="dropdownlist" data-value-primitive="true" data-value-field="value" data-text-field="text" data-bind="source: datumy, value: datum, events: { dataBound: databound }">
</script>

<script>
    function jitNaPracak(IDvykazPrace) {
        return '<a href="/Home/Fakturace?i=' + IDvykazPrace + '" target="_blank" class="text-primary">' + IDvykazPrace + '</a>';
    }

    $(function () {
        kendo.data.binders.widget.bindToolbar = kendo.data.Binder.extend({
            init: function (widget, bindings, options) {
                kendo.data.Binder.fn.init.call(this, widget.element[0], bindings, options);
            },
            refresh: function () {
                var model = this.bindings["bindToolbar"].get();
                kendo.bind($(this.element).find(".k-grid-toolbar"), model);
            }
        });

        var viewModel = kendo.observable({
            AGsp_Get_SmlouvyServisniGenerovanePausalky: new kendo.data.DataSource({
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
                            UctovanoOd: { type: "date" },
                            UctovanoDo: { type: "date" }
                        }
                    }
                },
                transport: {
                    read: {
                        url: "@Url.Action("AGsp_Get_SmlouvyServisniGenerovanePausalky", "Api/Service")",
                        type: "GET"
                    },
                    parameterMap: function (options, operation) {
                        var pm = kendo.data.transports.odata.parameterMap(options);
                        return pm;
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
            toolbar: kendo.observable({
                datum: null,
                datumy: new kendo.data.DataSource({
                    schema: {
                        data: "data",
                        errors: "error"
                    },
                    transport: {
                        read: {
                            url: "@Url.Action("AGsp_Get_SmlouvyServisniDatumDalsihoObdobi_CBX", "Api/Service")",
                            type: "GET"
                        },
                        parameterMap: function (options, operation) {
                            var pm = kendo.data.transports.odata.parameterMap(options);
                            return pm;
                        }
                    }
                }),
                databound: function (e) {
                    e.sender.select(0);
                },
                vygenerovat: function (e) {
                    var d = this.get("datum");
                    if (d) {
                        $.get('@Url.Action("AGsp_Do_SmlouvyServisniGenerujMesicniPausalky", "Api/Service")', { uctovanyMesic: d }, function (result) {
                            if (result.error) {
                                alert(result.error);
                            } else {
                                viewModel.AGsp_Get_SmlouvyServisniGenerovanePausalky.read();
                            }
                        });
                    } else {
                        alert("Vyberte (období k vyúčtování)")
                    }
                }
            })
        });
        kendo.bind(document.body, viewModel);
    });
</script>
