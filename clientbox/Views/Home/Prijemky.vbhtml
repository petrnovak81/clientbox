@Code
    ViewData("Title") = "Prijemky"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<style>
    .k-grid-delete {
        border-color: #dc3545;
        background: #dc3545;
        color: white !important;
    }

    .k-grid .k-grid-edit-row td:not(.k-hierarchy-cell), .k-grid .k-command-cell, .k-grid .k-edit-cell {
        padding: 0;
    }

    .k-grid td:not(:last-child) {
        padding-left: .75rem;
    }

    .k-grid .k-link {
        color: #007bff;
        text-decoration: underline;
    }
</style>

<div id="main" style="height: 100%;">
    <div data-role="splitter"
         data-panes="[
        { collapsible: false, scrollable: false, size: '40%' },
        { collapsible: false, scrollable: false }
        ]"
         data-orientation="horizontal"
         style="height:100%;width:100%;">
        <div data-role="grid"
             data-autobind="true"
             data-scrollable="true"
             data-filterable="true"
             data-selectable="true"
             data-pageable="false"
             data-toolbar="[{ template: '<h3>Příjemky<a role=\'button\' class=\'k-button k-button-icontext doklad-add float-right\' href=\'\\#\'><span class=\'k-icon k-i-plus\'></span>Nový doklad</a></h3>' }]"
             data-bind="source: prijemky_source, events: { dataBound: prijemky_source_databound, change: prijemky_change }"
             data-columns="[{ field: 'IDDokladu', template: '\\#${IDDokladu}', title: ' ', width: 50, editable: false }, { field: 'VSDokladu', title: 'VS dokladu', width: 120 }, { field: 'FirmaDodavatel', title: 'Dodavatel' }, { field: 'Vytvoril', title: 'Vytvořil' }, { template: '<a data-role=\'button\' data-bind=\'events: { click: doklad_edit }\'><span class=\'k-icon k-i-edit\'></span></a>', title: '&nbsp;', width: 50 }]"></div>
        <div data-role="grid"
             data-autobind="false"
             data-scrollable="true"
             data-selectable="true"
             data-pageable="false"
             data-toolbar="[{ template: '<h3>Položky příjemky<a role=\'button\' class=\'k-button k-button-icontext polozka-add float-right\' href=\'\\#\'><span class=\'k-icon k-i-plus\'></span>Nová položka</a></h3>' }]"
             data-bind="source: polozky_source"
             data-columns="[{ field: 'IDPrijemkaPol', template: '\\#${IDPrijemkaPol}', title: ' ', width: 50 }, { field: 'Produkt', title: 'Produkt', template: '#=linkProdukt(Produkt)#' }, { field: 'Popis', title: 'Popis' }, { field: 'NaskladnenoEMJ', title: 'Naskladněno' }, { field: 'SkladovaZasoba', title: 'Skladová zásoba' }, { field: 'OperativniZasoba', title: 'Operativní zásoba' }, { field: 'CenaNakup', title: 'Cena nákup' }, { field: 'ProdejniCenaPodleNakupu', title: 'Cena prodej' }, { field: 'RezervovanoProFirmu', title: 'Rezervováno pro' }, { field: 'rr_DruhNakladuText', title: 'Druh nákladu' }, { template: '<a data-role=\'button\' data-bind=\'events: { click: polozka_delete }\'><span class=\'k-icon k-i-close\'></span></a>', title: '&nbsp;', width: 50 }]"></div>
    </div>
</div>

<div class="modal fade" id="modal_doklad">
    <div class="modal-dialog modal-md">
        <form class="modal-content" data-bind="events: { submit: modal_doklad_save }">
            <div class="modal-header">
                <h4 class="modal-title">Příjemka</h4>
                <button type="button" class="close" data-dismiss="modal">&times;</button>
            </div>
            <div class="modal-body">
                <div class="form-row">
                    <div class="form-group col">
                        <label class="text-muted">VS dokladu</label>
                        <input type="text" class="form-control input_modal_doklad" data-bind="value: prijemka.VSDokladu" required name="Datum příjmu" />
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group col">
                        <label class="text-muted">Dodavatel</label>
                        <input data-role="combobox"
                               data-value-primitive="true"
                               data-text-field="Dodavatel"
                               data-value-field="Dodavatel"
                               data-bind="value: prijemka.FirmaDodavatel, source: dodavatele" name="Dodavatel" class="form-control" />
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <button type="submit" class="btn btn-success"><span class="k-icon k-i-save"></span> Uložit</button>
                <button type="button" class="btn btn-default" data-dismiss="modal"><span class="k-icon k-i-cancel-outline"></span> Zrušit</button>
            </div>
        </form>
    </div>
</div>

<div class="modal fade" id="modal_polozka">
    <div class="modal-dialog modal-md">
        <form class="modal-content" data-bind="events: { submit: modal_polozka_save }">
            <div class="modal-header">
                <h4 class="modal-title">Položka příjemky</h4>
                <button type="button" class="close" data-dismiss="modal">&times;</button>
            </div>
            <div class="modal-body">
                <div class="form-row">
                    <div class="form-group col">
                        <label class="text-muted">Produkt</label>
                        <div class="input-group">
                            <input data-role="multicolumncombobox"
                                   data-placeholder="Produkt"
                                   data-value-primitive="true"
                                   data-text-field="Produkt"
                                   data-value-field="Produkt"
                                   data-filter="contains"
                                   data-filter-fields="['Produkt']"
                                   data-columns="[ { 'field': 'Produkt', 'title': 'Produkt' },
                                               { 'field': 'Popis', 'title': 'Popis' },
                                               { 'field': 'Carovy_kod', 'title': 'Čárový kód' },
                                               { 'field': 'Dodavatel', 'title': 'Dodavatel' } ]"
                                   data-bind="value: produkt.Produkt, source: produkty_source, events: { change: hledat_produkt }"
                                   class="form-control input_modal_polozka" name="Produkt" />
                            <div class="input-group-append">
                                <button class="btn btn-outline-secondary" data-bind="events: { click: novy_produkt }"><span class="k-icon k-i-plus"></span></button>
                            </div>
                        </div>
                    </div>
                    <div class="form-group col">
                        <label class="text-muted">Datum příjmu</label>
                        <input data-role="datepicker" class="form-control" data-bind="value: produkt.DatumPrijmu" required name="DatumPrijmu" />
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group col">
                        <label class="text-muted">Popis</label>
                        <div data-bind="text: produkt.Popis"></div>
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group col">
                        <label class="text-muted">Naskladnit</label>
                        <input data-role="numerictextbox" type="number" required data-min="0" data-bind="value: produkt.Naskladnit" class="form-control" name="Naskladnit" />
                    </div>
                    <div class="form-group col">
                        <label class="text-muted">Nákladová skupina</label>
                        <select data-role="dropdownlist" class="form-control border" data-value-primitive="true" data-value-field="value" data-text-field="text" data-bind="source: skupiny, value: produkt.Skupina" required name="Skupina"></select>
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group col">
                        <label class="text-muted">Cena nákup</label>
                        <input data-role="numerictextbox" type="number" required data-min="0" data-bind="value: produkt.CenaNakup" class="form-control cenanakup" name="CenaNakup" />
                    </div>
                    <div class="form-group col">
                        <label class="text-muted">Cena prodej navýšeno o <span data-bind="text: navyseni"></span></label>
                        <input data-role="numerictextbox" type="number" required data-min="0" data-bind="value: produkt.CenaProdej" class="form-control cenaprodej" name="CenaProdej" />
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <button type="submit" class="btn btn-success"><span class="k-icon k-i-plus"></span> Naskladnit</button>
                <button type="button" class="btn btn-default" data-dismiss="modal"><span class="k-icon k-i-cancel-outline"></span> Zrušit</button>
            </div>
        </form>
    </div>
</div>

<div class="modal fade" id="modal_novy_produkt">
    <div class="modal-dialog modal-md">
        <form class="modal-content" data-bind="events: { submit: modal_novy_produkt_save }">
            <div class="modal-header">
                <h4 class="modal-title">Nový produkt</h4>
                <button type="button" class="close" data-dismiss="modal">&times;</button>
            </div>
            <div class="modal-body">
                <div class="form-row">
                    <div class="form-group col">
                        <label class="text-muted">Kód/Produkt</label>
                        <input type="text" required data-bind="value: produkt.Produkt" class="form-control input_modal_novy_produkt" name="Produkt" />
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group col">
                        <label class="text-muted">Popis</label>
                        <textarea cols="3" maxlength="255" data-bind="value: produkt.Popis" class="form-control" name="Popis" required></textarea>
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group col">
                        <label class="text-muted">Jednotky</label>
                        <input type="text" required data-bind="value: produkt.Jednotky" class="form-control" name="Jednotky" />
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <button type="submit" class="btn btn-success"><span class="k-icon k-i-plus"></span> Přidat nový produkt</button>
                <button type="button" class="btn btn-default" data-dismiss="modal"><span class="k-icon k-i-cancel-outline"></span> Zrušit</button>
            </div>
        </form>
    </div>
</div>

<script>
    var vs = '@Html.Raw(Request.QueryString("vs"))';

    function linkProdukt(Produkt) {
       return `<a href="@(Url.Action("Sklad", "Home"))?p=` + Produkt + `" target="_blank" class="text-primary" data-bind="text: Produkt"></a>`
    }

    $(function () {
        $(".cenanakup").keyup(function (e) {
            var val = $(this).val().replace(",", ".");
            if (!val) {
                val = 0;
            }
            var float = parseFloat(val);
            var navyseni = (float / 100) * 10;
            viewModel.produkt.set("CenaNakup", float);
            viewModel.produkt.set("CenaProdej", float + navyseni);
            viewModel.calc_navyseni();
        });

        $(".cenaprodej").keyup(function (e) {
            var val = $(this).val().replace(",", ".");
            if (!val) {
                val = 0;
            }
            viewModel.produkt.set("CenaProdej", parseFloat(val));
            viewModel.calc_navyseni();
        });

        $('#modal_doklad').on('shown.bs.modal', function () {
            $(".input_modal_doklad").focus();
        });

        $('#modal_polozka').on('shown.bs.modal', function () {
            $(".input_modal_polozka").focus();
        });

        $('#modal_novy_produkt').on('shown.bs.modal', function () {
            $(".input_modal_novy_produkt").focus();
        });

        $('#modal_novy_produkt').on('hide.bs.modal', function () {
            $(".input_modal_polozka").focus();
        });

        var viewModel = kendo.observable({
            prijemka: null,
            produkt: null,
            skupiny: [],
            navyseni: "0 %",
            calc_navyseni: function (e) {
                var cn = this.produkt.get("CenaNakup"),
                    cp = this.produkt.get("CenaProdej");
                if (cn > 0 && cp > 0) {
                    var v1 = parseFloat((cp / cn) - 1);
                    var v2 = parseFloat((v1 * 100));
                    this.set("navyseni", parseFloat(v2).toFixed(0) + " %");
                } else {
                    this.set("navyseni", "0 %");
                }
            },
            skupiny: new kendo.data.DataSource({
                schema: { data: "data" },
                transport: { read: "@Url.Action("RegRest", "Api/Service")?Register=rr_DruhNakladu" }
            }),
            prijemky_source: new kendo.data.DataSource({
                schema: { data: "data", total: "total", errors: "error" },
                transport: {
                    read: "@Url.Action("AGsp_Get_DokladHLSeznam", "Api/Service")",
                    parameterMap: function (options, operation) {
                        if (vs) {
                            return { filter: vs };
                        }
                        return { filter: null };
                    }
                }
            }),
            prijemky_source_databound: function (e) {
                var row = e.sender.tbody.find('tr:first');
                if (row) {
                    e.sender.select(row);
                }
            },
            polozky_source: new kendo.data.DataSource({
                schema: { data: "data", total: "total", errors: "error" },
                transport: {
                    read: "@Url.Action("AGsp_Get_PrijemkaPolSeznam", "Api/Service")",
                    parameterMap: function (options, operation) {
                        var params = { IDDokladu: 0 };
                        var prijemka = viewModel.get("prijemka");
                        if (prijemka) {
                            params.IDDokladu = prijemka.IDDokladu;
                        }
                        return params;
                    }
                }
            }),
            dodavatele: new kendo.data.DataSource({
                schema: {
                    data: "data"
                },
                transport: {
                    read: {
                        url: "@Url.Action("AGvw_FirmyDodavatele", "Api/Service")",
                        type: "GET"
                    }
                }
            }),
            produkty_source: new kendo.data.DataSource({
                serverFiltering: true,
                schema: { data: "data" },
                transport: {
                    read: "@Url.Action("ProduktSeznam_CBX", "Api/Service")",
                    parameterMap: function (options, operation) {
                        var filter = viewModel.produkty_source.filter();
                        if (filter) {
                            filter = filter.filters[0];
                            if (filter) {
                                filter = filter.filters[0];
                                if (filter) {
                                    return { filter: filter.value }
                                }
                            }
                        }
                        return { filter: "" }
                    }
                }
            }),
            prijemky_change: function (e) {
                var di = e.sender.dataItem(e.sender.select());
                this.set("prijemka", di.toJSON());
                this.polozky_source.read();
            },
            doklad_edit: function (e) {
                this.set("prijemka", e.data);
                $("#modal_doklad").modal("show");
            },
            modal_doklad_save: function (e) {
                e.preventDefault();
                var that = this;
                var model = this.get("prijemka").toJSON();
                $.post("@Url.Action("AGsp_Do_IU_Doklad", "Api/Service")", model, function (result) {
                    if (result.error) {
                        alert(r.error);
                    } else {
                        that.prijemky_source.read();
                        $("#modal_doklad").modal("hide");
                    }
                });
            },
            polozka_delete: function (e) {
                var that = this;
                $.get("@Url.Action("AGsp_Do_DelPrijemkaProdukt", "Api/Service")", { iDPrijemkaPol: e.data.IDPrijemkaPol }, function (result) {
                    if (result.error) {
                        alert(r.error);
                    } else {
                        that.polozky_source.read();
                    }
                });
            },
            modal_polozka_save: function (e) {
                e.preventDefault();
                var that = this;
                var p = this.get("produkt");
                var d = this.get("prijemka");
                var model = {
                    iDDokladu: d.IDDokladu,
                    rr_DruhNakladu: p.Skupina,
                    produkt: p.Produkt,
                    datumPrijmu: kendo.toString(new Date(p.DatumPrijmu), "yyyy-MM-dd HH:mm:ss"),
                    naskladnenoEMJ: p.Naskladnit,
                    cenaNakup: p.CenaNakup,
                    cenaProdejni: p.CenaProdej,
                    vSPrijmovehoDokladu: d.VSDokladu
                }
                kendo.ui.progress($("#modal_polozka"), true);
                $.get("@Url.Action("AGsp_Do_IPrijemkaProduktNaskladnit", "Api/Service")", model, function (result) {
                    kendo.ui.progress($("#modal_polozka"), false);
                    if (result.error) {
                        alert(result.error);
                    } else {
                        that.set("produkt", {
                            Produkt: "",
                            Jednotky: "Ks",
                            Popis: "",
                            DatumPrijmu: new Date(),
                            Naskladnit: 1,
                            Skupina: 1,
                            CenaNakup: null,
                            CenaProdej: null
                        });
                        that.polozky_source.read();
                        alert("Produkt " + model.produkt + " naskladněn s lepítkem #" + result.data);
                    }
                });
            },
            novy_produkt: function (e) {
                $("#modal_novy_produkt").modal("show");
            },
            modal_novy_produkt_save: function (e) {
                e.preventDefault();
                var p = this.get("produkt");
                var that = this;
                var model = {
                    Produkt: p.Produkt,
                    Popis: p.Popis,
                    Jednotky: p.Jednotky
                }
                kendo.ui.progress($("#modal_novy_produkt"), true);
                $.post("@Url.Action("AGsp_AddProduktZbozi", "Api/Service")", model, function (result) {
                    kendo.ui.progress($("#modal_novy_produkt"), false);
                    if (result.error) {
                        alert(result.error);
                    } else {
                        $("#modal_novy_produkt").modal("hide");
                    }
                });
            },
            hledat_produkt: function (e) {
                var val = this.produkt.get("Produkt");
                var that = this;
                if (val) {
                    $.get("@Url.Action("AGsp_GetProduktDetail", "Api/Service")", {
                        produkt: val,
                        barcode: null
                    }, function (result) {
                        var data = result.data[0];
                        that.set("btnenabled", true);
                        if (data) {
                            that.produkt.set("Popis", data.Popis);
                        }
                    })
                } else {
                    alert("zadej produkt");
                }
            }
        });
        kendo.bind(document.body, viewModel);
        $(document).on("click", ".doklad-add", function (e) {
            viewModel.set("prijemka", {
                IDDokladu: 0,
                VSDokladu: "",
                FirmaDodavatel: "",
                FirmaDodavatelNazev: "",
                IDUserVytvoril: 0,
                Vytvoril: ""
            });
            $("#modal_doklad").modal("show");
        });
        $(document).on("click", ".polozka-add", function (e) {
            var prijemka = viewModel.get("prijemka");
            if (prijemka.IDDokladu > 0) {
                viewModel.set("produkt", {
                    Produkt: "",
                    Jednotky: "Ks",
                    Popis: "",
                    DatumPrijmu: new Date(),
                    Naskladnit: 1,
                    Skupina: 1,
                    CenaNakup: null,
                    CenaProdej: null
                });
                $("#modal_polozka").modal("show");
            } else {
                alert("Vyber příjemku v levém seznamu")
            }
        });
    })
</script>
