@Code
    ViewData("Title") = "Pohyby"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div id="main" style="height: 100%;">
    @Code
        '<Authorize(Roles:="hanzl@agilo.cz, fakturace@agilo.cz, novak@agilo.cz")>
        If User.IsInRole("hanzl@agilo.cz") Or User.IsInRole("fakturace@agilo.cz") Or User.IsInRole("novak@agilo.cz") Then
            @<div data-role="grid"
                  data-scrollable="true"
                  data-selectable="true"
                  data-bind="source: source, toolbar: toolbar"
                  data-columns="[{ field: 'NazevUctu', attributes: { 'class': 'pl-3' }, title: 'Název účtu', width: 150 },
                        { field: 'Pohledavky', attributes: { 'class': 'pl-3 font-weight-bold text-success' }, format: '{0:n2} Kč', title: 'Pohledávky' }]"
                  style="height:50%;"></div>
            @<div data-role="grid"
                  data-scrollable="true"
                  data-selectable="true"
                  data-sortable="true"
                  data-bind="source: source2"
                  data-columns="[{ field: 'Variabilni_symbol', attributes: { 'class': 'pl-3' }, title: 'Var. symbol' },
                  { field: 'Zbyva_uhradit', attributes: { 'class': 'pl-3' }, format: '{0:n2} Kč', title: 'Zbývá uhradit' },
                  { field: 'PoSplatnosti', attributes: { 'class': 'pl-3' }, format: '{0:n2} Kč', title: 'Po splatnosti' },
                  { field: 'Rok', attributes: { 'class': 'pl-3' }, format: '{0:n0}', title: 'Rok' },
                  { field: 'Datum_splatnosti', attributes: { 'class': 'pl-3' }, format: '{0:d}', title: 'Datum splatnosti' },
                  { field: 'Nazev_firmy', attributes: { 'class': 'pl-3' }, title: 'Název firmy' },
                  ]"
                  style="height:50%;"></div>
        ElseIf User.IsInRole("hanzl@agilo.cz") Or User.IsInRole("fakturace@agilo.cz") Or User.IsInRole("novak@agilo.cz") Or User.IsInRole("frolikova@agilo.cz") Then
            @<div data-role="grid"
                  data-scrollable="true"
                  data-selectable="true"
                  data-sortable="true"
                  data-bind="source: source2"
                  data-columns="[{ field: 'Variabilni_symbol', attributes: { 'class': 'pl-3' }, title: 'Var. symbol' },
                  { field: 'Zbyva_uhradit', attributes: { 'class': 'pl-3' }, format: '{0:n2} Kč', title: 'Zbývá uhradit' },
                  { field: 'PoSplatnosti', attributes: { 'class': 'pl-3' }, format: '{0:n2} Kč', title: 'Po splatnosti' },
                  { field: 'Rok', attributes: { 'class': 'pl-3' }, format: '{0:n0}', title: 'Rok' },
                  { field: 'Datum_splatnosti', attributes: { 'class': 'pl-3' }, format: '{0:d}', title: 'Datum splatnosti' },
                  { field: 'Nazev_firmy', attributes: { 'class': 'pl-3' }, title: 'Název firmy' },
                  ]"
                  style="height:100%;"></div>
        End If
    End Code
</div>

<script type="text/x-kendo-tmpl" id="toolbar-template">
    <button class="btn btn-default" data-bind="events: { click: gettransitions }">Aktualizovat</button>
</script>

<script>
    $(function () {
        kendo.data.binders.widget.toolbar = kendo.data.Binder.extend({
            init: function (widget, bindings, options) {
                kendo.data.Binder.fn.init.call(this, widget.element[0], bindings, options);
            },
            refresh: function () {
                var model = this.bindings["toolbar"].get();
                var element = $(this.element);
                element.data("kendoGrid").setOptions({
                    toolbar: model.template
                });
                kendo.bind(element.find(".k-grid-toolbar"), model.model);
            }
        });

        var viewModel = kendo.observable({
            toolbar: {
                template: kendo.template($('#toolbar-template').html()),
                model: kendo.observable({
                    gettransitions: function (e) {
                        kendo.ui.progress($("body"), true);
                        $.get('@Url.Action("GetFioTransactions", "Api/Service")', {}, function (result) {
                            kendo.ui.progress($("body"), false);
                            if (result) {
                                alert(result);
                            }
                            viewModel.source.read();
                        })
                    }
                })
            },
            source: new kendo.data.DataSource({
                schema: {
                    data: "data",
                    total: "total",
                    errors: "error",
                    model: {
                        fields: {
                            Pohledavky: { type: "number" }
                        }
                    }
                },
                transport: {
                    read: {
                        url: "@Url.Action("AGsp_Get_DashboardUcty", "Api/Service")"
                    },
                    parameterMap: function (options, operation) {
                        return { id: 0 };
                    }
                }
            }),
            source2: new kendo.data.DataSource({
                serverSorting: true,
                schema: {
                    data: "data",
                    total: "total",
                    errors: "error",
                    model: {
                        fields: {
                            Variabilni_symbol: { type: "string" },
                            Zbyva_uhradit: { type: "number" },
                            PoSplatnosti: { type: "number" },
                            Rok: { type: "number" },
                            Datum_splatnosti: { type: "date" },
                            Nazev_firmy: { type: "string" }
                        }
                    }
                },
                transport: {
                    read: {
                        url: "@Url.Action("AGsp_Get_DashboardPohledavkyPodrobne", "Api/Service")"
                    },
                    parameterMap: function (options, operation) {
                        var pm = kendo.data.transports.odata.parameterMap(options);
                        return pm;
                    }
                }
            })
        })

        kendo.bind(document.body, viewModel);
    })
</script>
