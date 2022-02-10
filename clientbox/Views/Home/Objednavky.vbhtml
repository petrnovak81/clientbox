@Code
    ViewData("Title") = "Objednavky"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div id="main" style="height: calc(100% - 54px);">
    <nav class="navbar navbar-expand-sm bg-dark navbar-dark">
        <form class="form-inline my-2 my-lg-0" data-bind="events: { submit: searchSubmit}">
            <div class="input-group input-group-sm">
                <input type="search" class="form-control" data-bind="value: searchValue" placeholder="Hledat..." aria-label="Hledat...">
                <div class="input-group-append">
                    <button class="btn btn-secondary" type="submit"><span class="k-icon k-i-zoom"></span></button>
                </div>
                <label title="zobrazit vše" style="margin-left:16px;">
                    <input data-role="switch" type="checkbox" data-bind="checked: zobrazitVse, events: { change: zobrazitChange }" />
                </label>
            </div>
        </form>
    </nav>
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
         data-columns="[
         { 'headerTemplate': thTmp(['Dodat do']), 'template': '#=tdTmp([{value: rr_DeadLineHodnota, type: \'string\', icon: \'k-i-clock\'}, {value: new Date(DeadLineDatum), type: \'date\', format: \'d\', icon: \'k-i-calendar\', attr: { style: \'color:\\#007bff;\' }}])#', 'width': 150 },
         { 'headerTemplate': thTmp(['Produkt', 'Čárový kód']), 'template': '#=tdTmp([{value: Produkt, type: \'string\', format: null}, {value: Carovy_kod, type: \'string\', format: null}])#', 'width': 170 },
         { 'headerTemplate': thTmp(['Popis', 'Poznámka']), 'template': '#=tdTmp([{value: Popis, type: \'string\', attr: { style: \'white-space: normal;font-weight:bold;\' }}, {value: Poznamka, type: \'string\', attr: { style: \'white-space:normal;\' }}])#' },
         { 'headerTemplate': thTmp(['Množství', 'Domluvena prod.cena']), 'template': '#=tdTmp([{value: ObjednanoEMJ, type: \'number\', format: \'0 ks\'}, {value: DomluvenaProdejniCena, type: \'number\', format: \'0.00 kč\', attr: { style: \'color:\\#28a745;\' }}])#', 'width': 85 },
         { 'headerTemplate': thTmp(['Nákupní cena', 'Prodejní cena']), 'template': '#=tdTmp([{value: Cena_nakupni, type: \'number\', format: \'0.00 kč\', attr: { style: \'color:\\#28a745;\' }}, {value: Cena_prodejni, type: \'number\', format: \'0.00 kč\', attr: { style: \'color:\\#28a745;\' }}])#', 'width': 120 },
         { 'headerTemplate': thTmp(['Pro firmu', 'Pro pracoviště']), 'template': '#=tdTmp([{value: Nazev_firmy, type: \'string\', format: null}, {value: NazevPracoviste, type: \'string\'}])#', 'width': 290 },
         { 'headerTemplate': thTmp(['Dodavatel', 'Stav objednávky']), 'template': '#=tdTmp([{value: Dodavatel, type: \'string\', format: null}, {value: rr_StavObjednavkyHodnota, type: \'link\', attr: { \'href\': \'\\#\', \'data-bind\': \'events:{click:zmenStav}\', style: \'color:\\#007bff;text-decoration:underline;\' }}])#', 'width': 120 },
         { 'headerTemplate': thTmp(['Kdy objednal', 'Kdo objednal']), 'template': '#=tdTmp([{value: new Date(DatumObjednano), type: \'date\', format: \'d\', icon: \'k-i-calendar\', attr: { style: \'color:\\#007bff;\' }}, {value: UserLastName, type: \'string\', icon: \'k-i-user\'}])#', 'width': 110 },
         { 'template': tdTmp([{value:'Naskladnit', type: 'button', attr: { 'class': 'btn btn-primary', 'data-bind': 'events:{click:naskladnit}' }, icon: 'k-i-cart'}]), 'width': 110 }]"
         data-bind="source: AGsp_GetObjednavkySeznam, events: { dataBound: AGsp_GetObjednavkySeznam_bound}" style="height: 100%;"></div>
</div>

<div class="modal fade" id="modal_sklad">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div data-bind="visible: naskladnovani">
                <div class="modal-header">
                    <h4 class="modal-title">Naskladňování nakoupeného zboží</h4>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <form>
                    <div class="form-row modal-body">
                        <div class="col-md-12">
                            <p style="color:#0094ff;border-bottom: 1px #0094ff solid">Načtěte čár. kód nebo vyhledejte dle názvu</p>
                        </div>
                        <div class="col-md-1">
                            <span class="k-icon k-i-greyscale" style="font-size: 32px;color:#0094ff"></span>
                        </div>
                        <div class="form-group col-md-4">
                            <label>Čárový kód</label>
                            <input type="text" maxlength="30" name="Carovy_kod" placeholder="Čárový kód" data-bind="value: detail.Carovy_kod" class="form-control">
                        </div>
                        <div class="form-group col-md-7">
                            <label>Produkt</label>
                            <input type="text" maxlength="30" name="Produkt" placeholder="Produkt" data-bind="value: detail.Produkt" class="form-control">
                        </div>
                        <div class="col-md-1">

                        </div>
                        <div class="form-group col-md-11">
                            <p><b><i data-bind="text: detail.Popis"></i></b></p>
                            <p><i data-bind="text: detail.Poznamka"></i></p>
                            <p>Minimální množství: <span data-bind="text: detail.Mnozstvi_minimalni"></span> <span data-bind="text: detail.Jednotky"></span></p>
                        </div>
                        <div class="col-md-12">
                            <p style="color:#0094ff;border-bottom: 1px #0094ff solid">Údaje o nákupu</p>
                        </div>
                        <div class="col-md-1">
                            <span class="k-icon k-i-cart" style="font-size: 32px;color:#0094ff"></span>
                        </div>
                        <div class="form-group col-md-5">
                            <label>Naskladněno</label>
                            <input type="number" required min="1" step="1" name="NaskladnenoEMJ" data-bind="value: detail.NaskladnenoEMJ" class="form-control">
                        </div>
                        <div class="form-group col-md-6">
                            <label>Nákup. cena</label>
                            <input type="number" required min="1" step="0.10" name="CenaNakup" data-bind="value: detail.CenaNakup" class="form-control">
                        </div>
                        <div class="col-md-1">

                        </div>
                        <div class="form-group col-md-5">
                            <label>Dodavatel</label>
                            <input data-role="combobox"
                                   data-value-primitive="true"
                                   data-text-field="Dodavatel"
                                   data-value-field="Dodavatel"
                                   data-bind="value: detail.Dodavatel, source: dodavatele" class="form-control" required />
                        </div>
                        <div class="form-group col-md-6">
                            <label>VS příjmového dokladu</label>
                            <input type="text" maxlength="12" required name="VSPrijmovehoDokladu" data-bind="value: detail.VSPrijmovehoDokladu" class="form-control">
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="submit" class="btn btn-success" data-bind="events: { click: btnnaskladnit }">Naskladnit</button>
                        <button type="submit" class="btn btn-info" data-bind="events: { click: btnrezervovat }">Naskladnit a zablokovat</button>
                        <button type="button" class="btn btn-warning" data-dismiss="modal">Zrušit</button>
                    </div>
                </form>
            </div>

            <div data-bind="visible: rezervovani">
                <div class="modal-header">
                    <h4 class="modal-title">Zarezervovat</h4>
                </div>

                <div class="modal-body">
                    <div class="form-row">
                        <div class="form-group col-md-4">
                            <label>Počet kusů k zablokování</label>
                            <input data-format="0 \Ks" data-role="numerictextbox" type="number" data-min="1" min="1" data-bind="value: detail.Pocet" placeholder="Počet kusů" class="form-control">
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-6">
                            <label>Pro klienta</label>
                            <input type="search" placeholder="Hledat..." data-bind="value: detail.Nazev_firmy, events: { keyup: hledat_klienta }" class="form-control" style="border-bottom-left-radius:0;border-bottom-right-radius:0;border-bottom:0;">
                            <select data-auto-bind="false" data-role="listbox" data-text-field="Firma" data-value-field="Firma" data-bind="source: klienti, events: { change: select_klient }" data-template="tmpklient" class="form-control"></select>
                        </div>
                        <div class="form-group col-md-6">
                            <label>Seznam rozpracovaných pracovních listů</label>
                            <input type="search" placeholder="Hledat..." data-bind="value: detail.Pracak, events: { keyup: hledat_pracak }" class="form-control" style="border-bottom-left-radius:0;border-bottom-right-radius:0;border-bottom:0;">
                            <select data-auto-bind="false" data-role="listbox" data-text-field="Firma" data-value-field="IDVykazPrace" data-bind="source: pracaky, events: { change: select_pracak }" data-template="tmppracak" class="form-control"></select>
                        </div>
                    </div>
                </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-success" data-bind="events: { click: btnnovy }">Na nový pracák</button>
                    <button type="button" class="btn btn-warning" data-bind="events: { click: btnzpet }">Zpět</button>
                </div>
            </div>
        </div>
    </div>
</div>

<script id="tmpklient" type="text/html">
    <div>
        <div>#:Firma#</div>
        <small class="text-muted">#:data.Ulice#, #:data.Mesto# #:data.PSC#</small>
    </div>
</script>

<script id="tmppracak" type="text/html">
    <div>
        <div>Pracák \\##:IDVykazPrace#,     #:kendo.toString(new Date(data.DatVzniku), "dd.MM. yyyy HH:mm")#</div>
        <small class="text-muted">Založil: #:data.UserZalozil#, Upravil: #:data.UserUpravil#</small>
    </div>
</script>

<div class="modal fade" id="dialogStavy">
    <div class="modal-dialog modal-sm">
        <div class="modal-content">
            <div class="modal-header">
                <h4>Změňte stav objednávky</h4>
                <button type="button" class="close" data-dismiss="modal">&times;</button>
            </div>
            <div class="modal-body">
                <div class="list-group" data-bind="source: AGsp_GetDialogObjednavkaTlacitka" data-auto-bind="false" data-template="tmpdialogstavy"></div>
                <script id="tmpdialogstavy" type="text/html">
                    <a href="\\#" class="list-group-item list-group-item-action" data-bind="events: { click: dialogStavySelect }">#:rr_StavObjednavkyHodnota#</a>
                </script>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-warning" data-dismiss="modal">Zrušit</button>
            </div>
        </div>
    </div>
</div>

<style>
    .k-grid td {
        white-space: normal;
        text-overflow: ellipsis;
        padding: 0;
        padding-left: 0;
    }
        .k-grid td > *:not(.cell-button) {
            padding-left: .75rem;
        }
    .k-grid th {
        white-space: normal;
        text-overflow: ellipsis;
        /*font-weight: bold;*/
        padding: 0;
        padding-left: 0;
    }
    .k-grid th > * {
        padding-left: .50rem;
    }
    .td-temp:first-child:not(.cell-button) {
        border-bottom: 1px dotted silver;
    }
        .td-temp:last-child:not(.cell-button) {
            border-bottom: 1px solid silver;
        }
    .th-temp:first-child {
        border-bottom: 1px dotted silver;
    }
    .cell-number {
    text-align:right;
    }
</style>

<script>
    function thTmp(o) {
        var header = "";
        $.each(o, function (index, value) {
            header += '<div title="' + value + '" class="th-temp">' + value + '</div>';
        });
        return header;
    }

    function tdTmp(o) {
        var cell = "";
        $.each(o, function (index, options) {
            var settings = $.extend({
                value: null,
                type: null,
                format: null,
                attr: null,
                icon: null
            }, options);
            var attr = "";
            if (settings.attr != null) {
                $.each(settings.attr, function (key, value) {
                    attr += key + '="' + value + '"';
                })
            }
            var t = settings.type;
            if (settings.value == null) {
                settings.value = "NULL";
                cell += '<div title="prázdný" class="td-temp cell-' + t + '"><i style="color:silver"'
                if (attr) {
                    cell += attr;
                }
                cell += '>' + settings.value + '</i></div>';
            } else {
                if (t === 'date') {
                    settings.value = kendo.toString(settings.value, (settings.format ? settings.format : 'd'));
                    cell += '<div title="' + settings.value + '" class="td-temp cell-' + t + '"><span '
                    if (attr) {
                        cell += attr;
                    }
                    cell += '>' + (settings.icon ? '<span class="k-icon ' + settings.icon + '"></span> ' : '') + settings.value + '</span></div>';
                }
                if (t === 'number') {
                    settings.value = kendo.toString(settings.value, (settings.format ? settings.format : 'n0'));
                    console.log(settings.value + " " + settings.format)
                    cell += '<div title="' + settings.value + '" class="td-temp cell-' + t + '"><span '
                    if (attr) {
                        cell += attr;
                    }
                    cell += '>' + (settings.icon ? '<span class="k-icon ' + settings.icon + '"></span> ' : '') + settings.value + '</span></div>';
                }
                if (t === 'link') {
                    cell += '<div title="' + settings.value + '" class="td-temp cell-' + t + '"><a '
                    if (attr) {
                        cell += attr;
                    }
                    cell += '>' + (settings.icon ? '<span class="k-icon ' + settings.icon + '"></span> ' : '') + settings.value + '</a></div>';
                }
                if (t === 'button') {
                    cell += '<div class="td-temp cell-' + t + '"><button '
                    if (attr) {
                        cell += attr;
                    }
                    cell += '>' + (settings.icon ? '<span class="k-icon ' + settings.icon + '"></span> ' : '') + settings.value + '</button></div>';
                }
                if (t === 'string') {
                    cell += '<div title="' + settings.value + '" class="td-temp cell-' + t + '"><span '
                    if (attr) {
                        cell += attr;
                    }
                    cell += '>' + (settings.icon ? '<span class="k-icon ' + settings.icon + '"></span> ' : '') + settings.value + '</span></div>';
                }
            }
        })
        return cell;
    };

    $(function () {
        $("form").submit(function (e) {
            e.preventDefault();
        });

        $("#modal_sklad").on('hidden.bs.modal', function () {
            viewModel.AGsp_GetObjednavkySeznam.read();
        });

        var viewModel = kendo.observable({
            detail: {},
            zobrazitVse: false,
            searchValue: "",
            searchSubmit: function (e) {
                this.AGsp_GetObjednavkySeznam.read();
            },
            zobrazitChange: function (e) {
                this.set("zobrazitVse", e.checked);
                this.AGsp_GetObjednavkySeznam.read();
            },
            zmenStav: function (e) {
                var d = {
                    IDObjednavkyPol: e.data.IDObjednavkyPol,
                    Carovy_kod: e.data.Carovy_kod,
                    Produkt: e.data.Produkt,
                    Mnozstvi_minimalni: 0,
                    Jednotky: "Ks",
                    Popis: e.data.Popis,
                    Poznamka: e.data.Poznamka,
                    NaskladnenoEMJ: e.data.ObjednanoEMJ,
                    CenaNakup: e.data.Cena_nakupni,
                    Cena: e.data.Cena_prodejni,
                    Dodavatel: e.data.Dodavatel,
                    VSPrijmovehoDokladu: "",
                    Pocet: 1,
                    Nazev_firmy: e.data.Nazev_firmy,
                    Pracak: ""
                };
                this.set("detail", d);
                this.AGsp_GetDialogObjednavkaTlacitka.read({ IDObjednavkyPol: e.data.IDObjednavkyPol })
            },
            hledat_klienta: function () {
                var filter = {
                    field: "Firma",
                    operator: "eq",
                    value: this.detail.get("Nazev_firmy")
                };
                this.klienti.filter(filter);
                this.klienti.read();
                this.pracaky.read();
            },
            hledat_pracak: function () {
                var filter = {
                    field: "Firma",
                    operator: "contains",
                    value: this.detail.get("Pracak")
                };
                this.pracaky.filter(filter);
                this.pracaky.read();
            },
            panelsVisible: false,
            naskladnovani: true,
            rezervovani: false,
            btnnaskladnit: function (e) {
                var that = this;
                var form = $(e.currentTarget).closest("form");
                var valid = $(form).valid();
                if (!valid) {
                    return false;
                };
                var d = this.get("detail");
                localStorage.setItem("VSPrijmovehoDokladu", d.VSPrijmovehoDokladu)
                if (d.Produkt.length > 0) {
                    $.post("@Url.Action("AGsp_AddProduktNaskladnit", "Api/Service")", d.toJSON(), function (r) {
                        if (r.error) {
                            alert(r.error);
                        } else {
                            that.set("naskladnovani", true);
                            that.set("rezervovani", false);
                            $("#modal_sklad").modal("hide");
                        }
                    });
                } else {
                    alert("Vyplňte pole Produkt");
                }
            },
            btnrezervovat: function (e) {
                var form = $(e.currentTarget).closest("form");
                var valid = $(form).valid();
                console.log(valid)
                if (!valid) {
                    return false;
                };
                this.set("naskladnovani", false);
                this.set("rezervovani", true);
                this.hledat_klienta();
            },
            btnzpet: function (e) {
                this.set("naskladnovani", true);
                this.set("rezervovani", false);
            },
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
            dialogStavySelect: function (e) {
                var id = e.data.IDObjednavkyPol;
                var stav = e.data.rr_StavObjednavky
                var that = this;
                switch (stav) {
                    case 0:
                        $("#dialogStavy").modal("hide");
                        break;
                    case 1:
                        $("#dialogStavy").modal("hide");
                        break;
                    case 2:
                        $("#dialogStavy").modal("hide");
                        break;
                    case 3:
                        $.get("@Url.Action("AGsp_Run_Objednavka0or1to3", "Api/Service")", { iDObjednavkyPol: id }, function (result) {
                            if (result.error) {
                                alert(result.error)
                            } else {
                                that.AGsp_GetObjednavkySeznam.read();
                                $("#dialogStavy").modal("hide");
                            }
                        });
                        break;
                    case 4:
                        $("#dialogStavy").modal("hide");
                        $("#modal_sklad").modal("show");
                        break;
                    case 5:
                        $.get("@Url.Action("AGsp_Run_Objednavka0or1or3to5", "Api/Service")", { iDObjednavkyPol: id }, function (result) {
                            if (result.error) {
                                alert(result.error)
                            } else {
                                that.AGsp_GetObjednavkySeznam.read();
                                $("#dialogStavy").modal("hide");
                            }
                        });
                        break;
                }
            },
            AGsp_GetDialogObjednavkaTlacitka: new kendo.data.DataSource({
                schema: {
                    model: {
                        id: "rr_StavObjednavky"
                    }
                },
                transport: {
                    read: function (options) {
                        var id = options.data.IDObjednavkyPol;
                        $.get("@Url.Action("AGsp_GetDialogObjednavkaTlacitka", "Api/Service")", { iDObjednavkyPol: id }, function (result) {
                            var d = result.data;
                            $.each(d, function (index, object) {
                                object["IDObjednavkyPol"] = id;
                            });
                            options.success(d);
                            $("#dialogStavy").modal("show");
                        });
                    }
                }
            }),
            AGsp_GetObjednavkySeznam: new kendo.data.DataSource({
                pageSize: 50,
                schema: {
                    model: {
                        id: "IDObjednavkyPol"
                    }
                },
                transport: {
                    read: function (options) {
                        var h = viewModel.get("searchValue");
                        var v = viewModel.get("zobrazitVse");
                        $.get("@Url.Action("AGsp_GetObjednavkySeznam", "Api/Service")", { hledej: h, zobrazitVse: v }, function (result) {
                            options.success(result.data);
                        });
                    }
                }
            }),
            AGsp_GetObjednavkySeznam_bound: function (e) {
                var grid = e.sender;
                var rows = $(e.sender.element).find("tbody tr");
                $.each(rows, function (i, r) {
                    var item = grid.dataItem(r);
                    $(r).find("td:first").css({ 'background': item.Barva });
                })
                var row = e.sender.tbody.find('tr:first');
                e.sender.select(row);
            },
            klienti: new kendo.data.DataSource({
                transport: {
                    read: function (options) {
                        var h = viewModel.detail.Nazev_firmy;
                        $.get("@Url.Action("AGsp_GetHledejGlobalFullText", "Api/Service")", { hledej: h }, function (result) {
                            options.success(result.data);
                        });
                    }
                },
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true
            }),
            pracaky: new kendo.data.DataSource({
                transport: {
                    read: function (options) {
                        var pm = {
                            "$inlinecount": "allpages",
                            "$format": "json",
                            "$filter": "contains(Firma, '" + viewModel.detail.Nazev_firmy + "')",
                            "stav": 0
                        }
                        $.get("@Url.Action("AGvw_FA_PracakySeznam", "Api/Service")", pm, function (result) {
                            options.success(result.Data);
                        });
                    }
                },
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true
            }),
            select_klient: function (e) {
                var element = e.sender.select();
                var dataItem = e.sender.dataItem(element[0]);
                var firma = dataItem.Firma;
                this.detail.set("Nazev_firmy", firma);
                this.pracaky.read();
            },
            select_pracak: function (e) {
                var element = e.sender.select();
                var dataItem = e.sender.dataItem(element[0]);
                var iDVykazPrace = dataItem.IDVykazPrace;
                var firma = dataItem.Firma;
                var pocet = this.detail.Pocet;
                var cnf = confirm("Rezervovat " + pocet + " KS pro " + firma + "?");
                if (cnf) {
                    var that = this;
                    var d = this.get("detail");
                    localStorage.setItem("VSPrijmovehoDokladu", d.VSPrijmovehoDokladu);
                    $.post("@Url.Action("AGsp_AddProduktNaskladnit", "Api/Service")", d.toJSON(), function (r) {
                        if (r.error) {
                            alert(r.error);
                        } else {
                            $.get("@Url.Action("AGsp_AddNewPracakyPolozkaProduktZablokovat", "Api/Service")", { iDVykazPrace: iDVykazPrace, produkt: d.Produkt, blokovatEMJ: pocet, cenaEMJProdejni: d.Cena, iDUserUpravil: 0, iDPrijemkaPol: 0 }, function (result) {
                                if (r.error) {
                                    alert(r.error);
                                } else {
                                    that.set("naskladnovani", true);
                                    that.set("rezervovani", false);
                                    $("#modal_sklad").modal("hide");
                                }
                            })
                        }
                    });
                };
            },
            btnnovy: function (e) {
                var that = this;
                var d = this.get("detail");
                if (d.Nazev_firmy) {
                    $.post("@Url.Action("AGsp_AddProduktNaskladnit", "Api/Service")", d.toJSON(), function (r) {
                        if (r.error) {
                            alert(r.error);
                        } else {
                            $.get("@Url.Action("AGsp_AddNewVykazPrace", "Api/Service")", { firma: d.Nazev_firmy, iDUser: 0 }, function (result) {
                                if (r.error) {
                                    alert(r.error);
                                } else {
                                    var iDVykazPrace = result.data;
                                    $.get("@Url.Action("AGsp_AddNewPracakyPolozkaProduktZablokovat", "Api/Service")", { iDVykazPrace: iDVykazPrace, produkt: d.Produkt, blokovatEMJ: d.Pocet, cenaEMJProdejni: d.Cena, iDUserUpravil: 0, iDPrijemkaPol: 0 }, function (result) {
                                        if (r.error) {
                                            alert(r.error);
                                        } else {
                                            that.set("naskladnovani", true);
                                            that.set("rezervovani", false);
                                            $("#modal_sklad").modal("hide");
                                        }
                                    })
                                }
                            });
                        };
                    });
                } else {
                    alert("Vyber klienta");
                };
            },
            naskladnit: function (e) {
                var d = {
                    IDObjednavkyPol: e.data.IDObjednavkyPol,
                    Carovy_kod: e.data.Carovy_kod,
                    Produkt: e.data.Produkt,
                    Mnozstvi_minimalni: 0,
                    Jednotky: "Ks",
                    Popis: e.data.Popis,
                    Poznamka: e.data.Poznamka,
                    NaskladnenoEMJ: e.data.ObjednanoEMJ,
                    CenaNakup: e.data.Cena_nakupni,
                    Cena: e.data.Cena_prodejni,
                    Dodavatel: e.data.Dodavatel,
                    VSPrijmovehoDokladu: "",
                    Pocet: 1,
                    Nazev_firmy: e.data.Nazev_firmy,
                    Pracak: ""
                };
                this.set("detail", d);
                $("#modal_sklad").modal("show");
            },
            akce: function (e) {
                console.log(e)
            }
        });
        kendo.bind(document.body, viewModel);
    })
</script>
