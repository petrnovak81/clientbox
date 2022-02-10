@Code
    ViewData("Title") = "Adresář"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<style>
    .container-grid {
        display: grid;
        height: 100%;
        grid-template-columns: 1fr 50px;
        grid-template-rows: auto 1fr;
        grid-template-areas: "a a" "b c";
    }

    .b {
        grid-area: b;
        height: 100%;
    }

    .a {
        grid-area: a;
    }

    .c {
        grid-area: c;
    }

    .k-grid td {
    padding-left:3px;
    }
</style>

<div id="main" class="container-grid">
    <div class="a">
        <ul data-role="menu">
            <li>
                <form class="form-inline" data-bind="events: { submit: search }">
                    <div class="input-group input-group-sm">
                        <input type="search" class="form-control" data-bind="value: searchvalue" placeholder="Hledat..." aria-label="Hledat...">
                        <div class="input-group-append">
                            <button class="btn btn-secondary" type="submit"><span class="k-icon k-i-zoom"></span></button>
                            <button class="btn btn-secondary" type="button" data-bind="events: { click: clearfilter }"><span class="k-icon k-i-filter-clear"></span></button>
                        </div>
                    </div>
                </form>
            </li>
        </ul>
    </div>
    <div class="b" 
         data-role="grid"
         data-resizable="true"
         data-sortable="true"
         data-filterable="true"
         data-editable="false"
         data-scrollable="true"
         data-selectable="true"
         data-pageable="true"
         data-persist-selection="true"
         data-bind="source: source, events: { change: change, dataBound: dataBound }"
         data-columns="[
                { field: 'FirmaNazev', title: 'Název firmy', width: 150 },
                { field: 'Jednatel', title: 'Jednatel', width: 150 },
                { field: 'KontaktTelefon', title: 'Telefon', width: 100 },
                { field: 'EmailProFakturaci', title: 'Email', width: 100 },
                { field: 'PSC', title: 'PSČ', width: 70 },
                { field: 'Mesto', title: 'Město', width: 100 },
                { field: 'ICO', title: 'IČO', width: 90 },
                { field: 'DIC', title: 'DIČ', width: 105 },
                { field: 'ICP', title: 'PČP', width: 105 },
                { field: 'ICPOsoba', title: 'PČP Osoba', width: 105 },
                { field: 'ICPAdresa', title: 'PČP Adresa', width: 105 },
                { field: 'ICPNazev1', title: 'PČP Název 1', width: 105 },
                { field: 'ICPNazev2', title: 'PČP Název 2', width: 105 },
                { field: 'ICPOdbornost', title: 'PČP Odbornost', width: 105 },
                { field: 'UzNepouzivat', title: 'Nepoužívat', template: '#=cellBoolean(UzNepouzivat)#', width: 95 }]"></div>
    <div class="c bg-light">
        <button class="btn btn-light rounded-0  btn-block text-center" title="Nová firma" data-cmd="1" data-bind="events: { click: command }"><span class="fa fa-plus"></span></button>
        <button class="btn btn-light rounded-0  btn-block text-center" title="Upravit firmu" data-cmd="2" data-bind="enabled: enabled, events: { click: command }"><span class="fa fa-pencil"></span></button>
        <button class="btn btn-light rounded-0  btn-block text-center" title="Smazat firmu" data-cmd="3" data-bind="enabled: enabled, events: { click: command }"><span class="fa fa-trash"></span></button>
    </div>
</div>

<div id="win-1"
     data-role="window"
     data-width="450"
     data-height="auto"
     data-modal="true"
     data-auto-focus="true"
     data-title="Nová firma"
     data-actions="['close']" data-bind="events: { open: open }" style="display:none;">
    <form data-bind="events: { submit: save }">
        <div class="container" data-bind="visible: windowpagevisible(1)">
            <div class="form-group row">
                <label class="col-3 col-form-label text-muted">IČP</label>
                <div class="col-9">
                    <div class="input-group input-group">
                        <input type="text" class="form-control form-control-sm" data-bind="value: selected.ICP" maxlength="8">
                        <span class="input-group-append" style="width:100px;">
                            <button class="btn btn-outline-primary btn-sm border w-100" type="button">HLEDAT</button>
                        </span>
                    </div>
                </div>
            </div>
            <div class="form-group row">
                <label class="col-3 col-form-label text-muted">Osoba</label>
                <div class="col-9">
                    <input type="text" class="form-control form-control-sm" data-bind="value: selected.ICPOsoba" disabled>
                </div>
            </div>
            <div class="form-group row">
                <label class="col-3 col-form-label text-muted">Adresa</label>
                <div class="col-9">
                    <input type="text" class="form-control form-control-sm" data-bind="value: selected.ICPAdresa" disabled>
                </div>
            </div>
            <div class="form-group row">
                <label class="col-3 col-form-label text-muted">Název 1</label>
                <div class="col-9">
                    <input type="text" class="form-control form-control-sm" data-bind="value: selected.ICPNazev1" disabled>
                </div>
            </div>
            <div class="form-group row">
                <label class="col-3 col-form-label text-muted">Název 2</label>
                <div class="col-9">
                    <input type="text" class="form-control form-control-sm" data-bind="value: selected.ICPNazev2" disabled>
                </div>
            </div>
            <div class="form-group row">
                <label class="col-3 col-form-label text-muted">Odbornost</label>
                <div class="col-9">
                    <input type="text" class="form-control form-control-sm" data-bind="value: selected.ICPOdbornost" disabled>
                </div>
            </div>
            <div class="text-right">
                <button type="button" role="button" class="k-button k-button-icontext" data-bind="events: { click: windownext }"><span class="k-icon k-i-arrow-chevron-right"></span>Pokračovat</button>
                <button type="button" role="button" class="k-button k-button-icontext" data-bind="events: { click: storno }"><span class="k-icon k-i-cancel"></span>Storno</button>
            </div>
        </div>
        <div class="container" data-bind="visible: windowpagevisible(2)">
            <div class="form-group row">
                <label class="col-3 col-form-label text-muted">IČ/IČO</label>
                <div class="col-9">
                    <div class="input-group input-group">
                        <input type="text" class="form-control form-control-sm" data-bind="value: selected.ICO" maxlength="8">
                        <span class="input-group-append" style="width:100px;">
                            <button class="btn btn-outline-primary btn-sm border w-100" type="button" data-bind="events: { click: ares }">HLEDAT</button>
                        </span>
                    </div>
                </div>
            </div>
            <div class="form-group row">
                <label class="col-3 col-form-label text-muted">Jednatel</label>
                <div class="col-9">
                    <input type="text" class="form-control form-control-sm" data-bind="value: selected.Jednatel">
                </div>
            </div>
            <div class="form-group row">
                <label class="col-3 col-form-label text-muted">Telefon</label>
                <div class="col-9">
                    <input type="tel" class="form-control form-control-sm" data-bind="value: selected.KontaktTelefon">
                </div>
            </div>
            <div class="form-group row">
                <label class="col-3 col-form-label text-muted">Email</label>
                <div class="col-9">
                    <input type="email" class="form-control form-control-sm" data-bind="value: selected.EmailProFakturaci">
                </div>
            </div>
            <div class="form-group row">
                <label class="col-3 col-form-label text-muted">DIČ</label>
                <div class="col-9">
                    <input type="text" class="form-control form-control-sm" data-bind="value: selected.DIC" disabled>
                </div>
            </div>
            <div class="form-group row">
                <label class="col-3 col-form-label text-muted">Název</label>
                <div class="col-9">
                    <input type="text" class="form-control form-control-sm" data-bind="value: selected.FirmaNazev" disabled>
                </div>
            </div>
            <div class="form-group row">
                <label class="col-3 col-form-label text-muted">Ulice</label>
                <div class="col-9">
                    <input type="text" class="form-control form-control-sm" data-bind="value: selected.Ulice" disabled>
                </div>
            </div>
            <div class="form-group row">
                <label class="col-3 col-form-label text-muted">Město</label>
                <div class="col-9">
                    <input type="text" class="form-control form-control-sm" data-bind="value: selected.Mesto" disabled>
                </div>
            </div>
            <div class="form-group row">
                <label class="col-3 col-form-label text-muted">PSČ</label>
                <div class="col-9">
                    <input type="text" class="form-control form-control-sm" data-bind="value: selected.PSC" disabled>
                </div>
            </div>
            <div class="form-group row">
                <label class="col-3 col-form-label text-muted">Zápis OR</label>
                <div class="col-9">
                    <input type="text" class="form-control form-control-sm" data-bind="value: selected.ZapisOR" disabled>
                </div>
            </div>
            <div class="text-right">
                <button type="button" role="button" class="k-button k-button-icontext" data-bind="events: { click: windowback }"><span class="k-icon k-i-arrow-chevron-left"></span>Zpět</button>
                <button type="submit" role="button" class="k-button k-button-icontext"><span class="k-icon k-i-save"></span>Uložit</button>
                <button type="button" role="button" class="k-button k-button-icontext" data-bind="events: { click: storno }"><span class="k-icon k-i-cancel"></span>Storno</button>
            </div>
        </div>
    </form>
</div>

<script>
    var idfirmy = '@Html.Raw(Request.QueryString("i"))';

    function cellBoolean(value) {
        if (value) {
            return `<div class="text-center"><span class="k-icon k-i-checkbox-checked"></span></div>`;
        }
        return `<div class="text-center"><span class="k-icon k-i-checkbox"></span></div>`;
    }

    (function (global) {

        'use strict';

        var app = global.app = global.app || {};
        var win = null;

        app.viewModel = kendo.observable({
            searchvalue: "",
            search: function (e) {
                e.preventDefault();
                this.source.read();
            },
            windowpage: 1,
            windowpagevisible: function (i) {
                var page = this.get("windowpage");
                if (page == i) {
                    return true;
                }
                return false;
            },
            windownext: function (e) {
                var page = this.get("windowpage");
                page += 1;
                this.set("windowpage", page);
            },
            windowback: function (e) {
                var page = this.get("windowpage");
                page -= 1;
                this.set("windowpage", page);
            },
            clearfilter: function (e) {
                this.set("searchvalue", null);
                this.source.filter({});
            },
            enabled: function (e) {
                var selected = this.get("selected");
                if (selected) {
                    return true;
                }
                return false;
            },
            command: function (e) {
                var cmd = $(e.currentTarget).data("cmd");
                if (cmd == 1 || cmd == 2) {
                    win = $("#win-1").data("kendoWindow");
                    this.set("windowpage", 1);
                    if (cmd == 1) {
                        this.set("selected", {
                            IDKnihy: 2,
                            FirmaNazev: "",
                            Ulice: "",
                            PSC: "",
                            Mesto: "",
                            ICO: "",
                            DIC: "",
                            EmailProObjednavky: "",
                            EmailProFakturaci: "",
                            UzNepouzivat: false,
                            Odberatel: true,
                            Dodavatel: false,
                            Kategorie: "",
                            Pozn: "",
                            IDUserUpravil: 0,
                            CasVytvoril: new Date(),
                            CasUpravil: new Date(),
                            ZapisOR: "",
                            ICP: "",
                            ICPOsoba: "",
                            ICPAdresa: "",
                            ICPNazev1: "",
                            ICPNazev2: "",
                            ICPOdbornost: "",
                            Jednatel: "",
                            KontaktTelefon: "",
                            IDFirmy: 0
                        });
                    }
                    win.open().center().element.closest(".k-window").css({
                        top: 100
                    });
                }
                if (cmd == 3) {
                    var that = this;
                    var model = this.get("selected");
                    $.post("@Url.Action("AGsp_Do_DelFirma", "Api/Service")", { iDFirmy: model.IDFirmy }, function (result) {
                        if (result.error) { alert(result.error) } else {
                            that.source.read();
                        }
                    });
                }
            },
            source: new kendo.data.DataSource({
                serverSorting: true,
                serverFiltering: true,
                serverPaging: true,
                schema: {
                    data: "data",
                    total: "total",
                    errors: "error",
                    model: {
                        id: "IDFirmy",
                        fields: {
                            IDKnihy: { type: "number" },
                            FirmaNazev: { type: "string" },
                            Ulice: { type: "string" },
                            PSC: { type: "string" },
                            Mesto: { type: "string" },
                            ICO: { type: "string" },
                            DIC: { type: "string" },
                            EmailProObjednavky: { type: "string" },
                            EmailProFakturaci: { type: "string" },
                            UzNepouzivat: { type: "boolean" },
                            Odberatel: { type: "boolean" },
                            Dodavatel: { type: "boolean" },
                            Kategorie: { type: "string" },
                            Pozn: { type: "string" },
                            IDUserUpravil: { type: "number" },
                            CasVytvoril: { type: "date" },
                            CasUpravil: { type: "date" },
                            ZapisOR: { type: "string" },
                            ICP: { type: "string" },
                            ICPOsoba: { type: "string" },
                            ICPAdresa: { type: "string" },
                            ICPNazev1: { type: "string" },
                            ICPNazev2: { type: "string" },
                            ICPOdbornost: { type: "string" },
                            Jednatel: { type: "string" },
                            KontaktTelefon: { type: "string" },
                            IDFirmy: { type: "Integer" }
                        }
                    }
                },
                filter: (idfirmy ? { field: "IDFirmy", operator: "eq", value: parseInt(idfirmy) } : {}),
                pageSize: 50,
                transport: {
                    read: {
                        url: function () {
                            return '@Url.Action("AGsp_Get_FirmySeznam", "Api/Service")';
                        }
                    },
                    parameterMap: function (data, type) {
                        var pm = kendo.data.transports.odata.parameterMap(data);
                        pm.hledej = app.viewModel.get("searchvalue");
                        pm.iDKnihy = 2;
                        return pm;
                    }
                },
                error: function (e) {
                    e.sender.cancelChanges();
                    if (e.errors) {
                        agAlert("Hlášení", e.errors)
                    } else {
                        agAlert("Hlášení" + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.")
                    }
                }
            }),
            selected: null,
            change: function (e) {
                var grid = e.sender;
                var selected = grid.select();
                var item = grid.dataItem(selected);
                if (item) {
                    this.set("selected", item);
                }
            },
            dataBound: function (e) {
                var grid = e.sender;
                var items = grid.items();
                this.set("selected", null);
                $.each(items, function () {
                    var di = grid.dataItem($(this));
                    $(this).attr("data-id", di.IDFirmy);
                    if (idfirmy == di.IDFirmy) {
                        grid.select($(this));
                    }
                });
            },
            ares: function (e) {
                var that = this,
                    ico = that.selected.get("ICO");
                if (ico) {
                    kendo.ui.progress($("#win-1"), true);
                    $.get("@Url.Action("Ares", "Api/Service")", { ico: ico }, function (result) {
                        kendo.ui.progress($("#win-1"), false);
                        that.selected.set("FirmaNazev", result.firma);
                        that.selected.set("Ulice", result.ulice + " " + result.cp);
                        that.selected.set("PSC", result.psc);
                        that.selected.set("Mesto", result.mesto);
                        that.selected.set("DIC", result.dic);
                        that.selected.set("ZapisOR", result.or);
                    });
                } else {
                    alert("Zadejte IČ")
                }
            },
            save: function (e) {
                var that = this;
                var model = this.get("selected");
                e.preventDefault();
                $.post("@Url.Action("AGsp_Do_IUFirma", "Api/Service")", model.toJSON(), function (result) {
                    if (result.error) { alert(result.error) } else {
                        idfirmy = result.data;
                        win.close();
                        that.source.read();
                    }
                })
            },
            open: function (e) {

            },
            storno: function (e) {
                win.close();
            },
        });

        kendo.bind($('body'), app.viewModel);
    })(window)
</script>