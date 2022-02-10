@Code
    ViewData("Title") = "100 Mega"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div id="main" style="height:100%;">
    <div data-role="grid"
         data-filterable="true"
         data-column-menu="true"
         data-pageable="false"
         data-editable="false"
         data-sortable="true"
         data-selectable="false"
         data-scrollable="true"
         data-resizable="true"
         data-auto-bind="false"
         data-no-records="{ template: '<h3 style=\'text-align:center;margin-top:16px;\'>Žádné záznamy</h3>' }"
         data-navigatable="true"
         data-columns="[
         { 'field': 'rr_MegaStavHodnota', 'title': 'Stav', width: 100 },
         { 'field': 'CasVzniku', 'title': 'Čas vzniku', width: 100 },
         { 'field': 'Objednavka', 'title': 'Objednávka', width: 100 },
         { 'field': 'MegaCodeObjednaci', 'title': 'Kód zboží', width: 100 },
         { 'field': 'MegaName', 'title': 'Název zboží' },
         { 'field': 'MegaCelkovaCena', 'template': editcelkcena, 'title': 'Cena celkem', 'width': 80 },
         { 'field': 'MegaQtyPocetEMJ', 'template': readonlynumber, 'title': 'Počet', 'width': 80 },
         { 'field': 'DohodnutaProdejniCenaEMJ', 'template': editcena, 'title': 'Prodejní cena', 'width': 80 },
         { 'field': 'Poznamka', 'template': editpozn, 'title': 'Poznámka' },
         { 'title': ' ', 'template': btnnaskladnit, width: 80 },
         { 'title': ' ', 'template': btnnaskladnitablokovat, width: 160 },
         { 'title': ' ', 'template': btndelete, width: 30 }]"
         data-bind="source: AGsp_GetMegaPolSeznam" style="height: 100%;"></div>
</div>

<script>
    function readonlynumber(e) {
        return '<div style="text-align:right;">' + e.MegaQtyPocetEMJ + '</div>'
    }

    function editcelkcena(e) {
        return '<input data-bind="value: MegaCelkovaCena, events: { change: update }" type="number" step="0.01" min="0" style="text-align:right;border:0;border-bottom:1px dotted blue;width:100%;background:transparent;" value="' + (e.MegaCelkovaCena ? e.MegaCelkovaCena : '') + '" />';
    }

    function editcena(e) {
        return '<input data-bind="value: DohodnutaProdejniCenaEMJ, events: { change: update }" type="number" step="0.01" min="0" style="text-align:right;border:0;border-bottom:1px dotted blue;width:100%;background:transparent;" value="' + (e.DohodnutaProdejniCenaEMJ ? e.DohodnutaProdejniCenaEMJ : '') + '" />';
    }

    function editpozn(e) {
        return '<input data-bind="value: Poznamka, events: { change: update }" type="text" style="border:0;border-bottom:1px dotted blue;width:100%;background:transparent;" value="' + (e.Poznamka ? e.Poznamka : '') + '" />';
    }

    function btndelete(e) {
        return '<button type="button" class="btn btn-danger btn-sm" data-bind="events: { click: delete }" title="Smazat">&#10006;</button>';
    };

    function btnnaskladnit(e) {
        return '<button type="button" class="btn btn-success btn-sm" data-bind="events: { click: naskladnit }" title="Naskladnění">Naskladnit</button>';
    };

    function btnnaskladnitablokovat(e) {
        return '<button type="button" class="btn btn-info btn-sm" data-bind="events: { click: zablokovat }" title="Naskladnění a zablokování">Naskladnit a zablokovat</button>';
    };

    $(function () {
        //objednavky 100mega
        $.get('@Url.Action("sto_mega_getresult", "Api/AVISService")', { resultType: "Order" }, function (e) {
            $.get('@Url.Action("sto_mega_getresult", "Api/AVISService")', { resultType: "DocTrInv" }, function (e) {
                viewModel.AGsp_GetMegaPolSeznam.read();
            })
        })
        

        var viewModel = kendo.observable({
            AGsp_GetMegaPolSeznam: new kendo.data.DataSource({
                schema: {
                    model: {
                        id: "IDMega",
                        fields: {
                            rr_MegaStavHodnota: { type: "string" },
                            CasVzniku: { type: 'date' },
                            Objednavka: { type: 'string' },
                            MegaCodeObjednaci: { type: 'string' },
                            MegaName: { type: 'string' },
                            MegaCelkovaCena: { type: 'number' },
                            MegaQtyPocetEMJ: { type: 'number' },
                            DohodnutaProdejniCenaEMJ: { type: 'number' },
                            Poznamka: { type: 'string' }
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        $.get('@Url.Action("AGsp_GetMegaPolSeznam", "Api/Service")', { iDMega: 0 }, function (result) {
                            options.success(result.data);
                        });
                    }
                }
            }),
            delete: function (e) {
                $.get('@Url.Action("AGsp_DelMegaPolozku", "Api/Service")', { iDMega: e.data.IDMega }, function (result) {
                    viewModel.AGsp_GetMegaPolSeznam.read();
                });
            },
            update: function (e) {
                var d = e.data.toJSON();
                $.post('@Url.Action("AGsp_EditMegaPol", "Api/Service")', d, function (result) {
                    viewModel.AGsp_GetMegaPolSeznam.read();
                });
            },
            naskladnit: function (e) {
                var d = {
                    IDMega: e.data.IDMega,
                    IDObjednavkyPol: 0,
                    Produkt: e.data.MegaCodeObjednaci,
                    NaskladnenoEMJ: e.data.MegaQtyPocetEMJ,
                    CenaNakup: e.data.MegaCelkovaCena,
                    Dodavatel: "100Mega",
                    VSPrijmovehoDokladu: ""
                };
                $.post("@Url.Action("AGsp_AddProduktNaskladnit", "Api/Service")", d, function (r) {
                    if (r.error) {
                        alert(r.error);
                    } else {
                        $(e.currentTarget).css("display", "none");
                    }
                });
            },
            zablokovat: function (e) {
                $.get('@Url.Action("sto_mega_getresultbycode", "Api/AVISService")', { resultType: "StoItemEAN", code: e.data.MegaCodeObjednaci }, function (result) {
                    if (result.error) {
                        alert(result.error);
                    } else {
                        var carovy_kod = result.data.Result.StoItem['@@EAN'];
                        var d = {
                            Produkt: e.data.MegaCodeObjednaci,
                            Popis: e.data.MegaName,
                            Carovy_kod: carovy_kod,
                            Jednotky: "KS",
                            Cena: 0,
                            Dodavatel: "100Mega",
                            Mnozstvi_minimalni: 0,
                            Internet: "",
                            Poznamka: "",
                            Datum_aktualizace: new Date()
                        };
                        $.post('@Url.Action("AGsp_AddProduktZbozi", "Api/Service")', d, function (result) {
                            if (result.error) {
                                alert(result.error);
                            } else {
                                d = {
                                    IDMega: e.data.IDMega,
                                    IDObjednavkyPol: 0,
                                    Produkt: e.data.MegaCodeObjednaci,
                                    NaskladnenoEMJ: e.data.MegaQtyPocetEMJ,
                                    CenaNakup: e.data.MegaCelkovaCena,
                                    Dodavatel: "100Mega",
                                    VSPrijmovehoDokladu: ""
                                };
                                $.post("@Url.Action("AGsp_AddProduktNaskladnit", "Api/Service")", d, function (result) {
                                    if (result.error) {
                                        alert(result.error);
                                    } else {
                                        window.location.href = '@Url.Action("Sklad", "Home")' + "?produkt=" + d.Produkt;
                                    }
                                });
                            }
                        });
                    };
                });
            }
        });
        kendo.bind(document.body, viewModel);
    });
</script>