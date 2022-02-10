@Code
    ViewData("Title") = "Moje dměny"
    Layout = "~/Views/Shared/_Layout.vbhtml"

    Dim idu = 0
    Using db As New Data4995Entities
        idu = db.AG_tblUsers.FirstOrDefault(Function(e) e.UserLogin = User.Identity.Name).IDUser
    End Using
End Code

<style>
    .grid {
        display: grid;
        padding-top: 60px;
        height: 100%;
        grid-template-columns: 1fr;
        grid-template-rows: auto 1fr;
        gap: 0px 0px;
        grid-auto-flow: row;
        grid-template-areas: "." ".";
    }

    [data-role="splitter"] {
    height:100%;
    }
</style>

<div class="grid">
    <div class="topmenu">
        <ul data-role="menu">
            <li>
                <input data-role="dropdownlist" data-bind="source: uzivatele, value: iduzivatele, events: { change: uzivatelechange }" data-auto-width="true" data-value-primitive="true" data-value-field="value" data-text-field="text">
            </li>
            <li>
                <input data-role="dropdownlist" data-bind="source: nazvy, value: idnazvy, events: { change: nazvychange }" data-auto-width="true" data-value-primitive="true" data-value-field="value" data-text-field="text" data-option-label="-- vyber obdobi --">
            </li>
            <li>
                <input data-role="dropdownlist" data-bind="source: stavy, value: idstavy, events: { change: stavychange }" data-auto-width="true" data-value-primitive="true" data-value-field="value" data-text-field="text">
            </li>
        </ul>
    </div>
    <div data-role="splitter"
         data-panes="[
        { collapsible: true, scrollable: false, size: 400 },
        { collapsible: false, scrollable: false }
        ]"
         data-orientation="horizontal">
        <div data-role="grid"
             data-filterable="true"
             data-column-menu="false"
             data-pageable="true"
             data-editable="false"
             data-sortable="true"
             data-selectable="true"
             data-scrollable="true"
             data-resizable="true"
             data-reorderable="true"
             data-persist-selection="true"
             data-navigatable="true"
             data-auto-bind="true"
             data-toolbar="[{ template: '<h5>Měsíční vyúčtování</h5>' }]"
             data-bind="source: mesicnivyuctovani_source, events: { change: mesicnivyuctovani_change }"
             data-columns='[
                { field: "UserLastName", title: "Jméno", width: 150 },
                { field: "VyuctovaniNazev", title: "Označení" },
                { field: "rr_StavVyuctovaniText", title: "Stav" }
             ]'
             style="height:100%;">
        </div>
        <div data-role="grid"
             data-filterable="true"
             data-column-menu="false"
             data-pageable="false"
             data-editable="false"
             data-sortable="true"
             data-selectable="true"
             data-scrollable="true"
             data-resizable="true"
             data-reorderable="true"
             data-persist-selection="true"
             data-navigatable="true"
             data-auto-bind="true"
             data-toolbar="[{ template: '<h5>Vyúčtování pololožky <span id=odmenasum class=float-right>Odměna celkem: 0 Kč</span></h5>' }]"
             data-bind="source: vyuctovanitechpol_source"
             data-columns='[
                { field: "DatVzniku", attributes: { class: "text-center" }, format: "{0:d}", title: "Datum vzniku", width: 75 },
                { field: "IDVykazPrace", template: "#=cellIDVykazPrace(IDVykazPrace)#", title: " " },
                { field: "Nazev_firmy", title: "Firma" },
                { field: "rr_TypPolozkyPracakuHodnota", title: "Typ položky" },
                { field: "Hodin", attributes: { class: "text-right" }, title: "Hodin", width: 65 },
                { field: "PocetEMJ", attributes: { class: "text-right" }, title: "Počet", width: 65 },
                { field: "OdmenaTechnika", attributes: { class: "text-right" }, title: "Odměna technika", width: 65 },
                { field: "TextNaFakturu", title: "Text na fakturu" }
             ]'
             style="height:100%;">
        </div>
    </div>
</div>

<script>
    function cellIDVykazPrace(IDVykazPrace) {
        return `<a class="text-primary" href="@(Url.Action("Fakturace", "Home"))?i=` + IDVykazPrace + `" target="_blank">#` + IDVykazPrace + `</a>`;
    }

    $(function () {
        var viewModel = kendo.observable({
            uzivatele: new kendo.data.DataSource({
                schema: {
                    data: "data",
                    total: "total",
                    errors: "error"
                },
                transport: {
                    read: {
                        url: function () {
                            return '@Url.Action("AGsp_Get_UsersProFiltrVyuctovani", "Api/Service")';
                        }
                    }
                }
            }),
            iduzivatele: @idu,
            uzivatelechange: function (e) {
                this.mesicnivyuctovani_source.read();
                this.vyuctovanitechpol_source.read();
            },
            nazvy: new kendo.data.DataSource({
                schema: {
                    data: "data",
                    total: "total",
                    errors: "error"
                },
                transport: {
                    read: {
                        url: function () {
                            return '@Url.Action("AGsp_Get_VyuctovaniNazvyComboBox", "Api/Service")';
                        }
                    }
                }
            }),
            idnazvy: null,
            nazvychange: function (e) {
                this.mesicnivyuctovani_source.read();
                this.vyuctovanitechpol_source.read();
            },
            stavy: [
                { value: 0, text: "Všechny" },
                { value: 1, text: "Otevřené" },
                { value: 2, text: "Uzavřené" }
            ],
            idstavy: 0,
            stavychange: function (e) {
                this.mesicnivyuctovani_source.read();
                this.vyuctovanitechpol_source.read();
            },
            mesicnivyuctovani_source: new kendo.data.DataSource({
                serverSorting: true,
                serverFiltering: true,
                serverPaging: true,
                pageSize: 50,
                sort: { field: "VyuctovaniNazev", dir: "desc" },
                schema: {
                    data: "data",
                    total: "total",
                    errors: "error",
                    model: {
                        id: "IDVyuctovani",
                        fields: {
                            IDUser: { type: "number" },
                            VyuctovaniNazev: { type: "string" },
                            rr_StavVyuctovaniText: { type: "string" }
                        }
                    }
                },
                transport: {
                    read: {
                        url: function () {
                            return '@Url.Action("AGsp_Get_VyuctovaniSeznam", "Api/Service")';
                        }
                    },
                    parameterMap: function (data, type) {
                        var pm = kendo.data.transports.odata.parameterMap(data);
                        pm.iDUserFiltr = viewModel.get("iduzivatele");
                        pm.vyuctovaniNazev = viewModel.get("idnazvy");
                        pm.rr_StavVyuctovani = viewModel.get("idstavy");
                        return pm;
                    }
                },
                error: function (e) {
                    e.sender.cancelChanges();
                    if (e.errors) {
                        alert(e.errors);
                    } else {
                        alert("Systémová chyba aplikace. Kontaktujte vývojáře.")
                    }
                }
            }),
            mesicnivyuctovani_change: function (e) {
                var that = this;
                var item = e.sender.dataItem(e.sender.select());
                if (item) {
                    var f = {
                        iDUserFiltr: item.IDUser,
                        vyuctovaniNazev: item.VyuctovaniNazev,
                        rr_StavVyuctovani: item.rr_StavVyuctovani
                    }
                    that.vyuctovanitechpol_source.read(f);
                }
            },
            vyuctovanitechpol_source: new kendo.data.DataSource({
                serverSorting: true,
                serverFiltering: true,
                serverPaging: true,
                schema: {
                    data: "data",
                    total: "total",
                    errors: "error",
                    model: {
                        id: "IDVykazPracePol",
                        fields: {
                            IDVykazPrace: { type: "number" },
                            Nazev_firmy: { type: "string" },
                            IDVykazPracePol: { type: "number" },
                            DatVzniku: { type: "date" },
                            rr_TypPolozkyPracaku: { type: "number" },
                            rr_TypPolozkyPracakuHodnota: { type: "string" },
                            Hodin: { type: "number" },
                            PocetEMJ: { type: "number" },
                            ZTohoPocetEMJFree: { type: "number" },
                            TextNaFakturu: { type: "string" },
                            OdmenaTechnika: { type: "number" }
                        }
                    }
                },
                transport: {
                    read: {
                        url: function () {
                            return '@Url.Action("AGsp_Get_VyuctovaniTechnikaPol", "Api/Service")';
                        }
                    },
                    parameterMap: function (data, type) {
                        var pm = kendo.data.transports.odata.parameterMap(data);
                        return pm;
                    }
                },
                requestEnd: function (e) {
                    if (e.type == "read") {
                        $("#odmenasum").text("Odměna celkem: " + e.response.sum + " Kč")
                    }
                },
                error: function (e) {
                    e.sender.cancelChanges();
                    if (e.errors) {
                        alert(e.errors);
                    } else {
                        alert("Systémová chyba aplikace. Kontaktujte vývojáře.")
                    }
                }
            })
        });
        kendo.bind(document.body, viewModel);
    });
</script>


@*<div id="main" style="height: 100%;">
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
         data-columns="[]"
         data-bind="source: XXX }" style="height: 100%;"></div>
</div>

<script id="toolbar" type="text/x-kendo-template">
    <a class="k-button k-primary" id="btn-reload" href="\#"><span class="k-icon k-i-reload"></span> Aktualizovat/doplnit provize ze servisních smluv</a>
</script>*@

@*<script>
    $(function () {
        var viewModel = kendo.observable({

        })
        kendo.bind(document.body, viewModel);
        $(document).on('click', '#btn-reload', function (e) {

        })
    });
</script>*@