@Code
    ViewData("Title") = "Fakturace"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div id="main" style="height: calc(100% - 54px);">
    <nav class="navbar navbar-expand-sm bg-dark navbar-dark">
        <ul class="navbar-nav">
            <li class="nav-item dropdown">
                <select data-sql-source="AGvwrr_StavPracaku" data-role="dropdownlist" data-text-field="text" data-value-field="value" data-value-primitive="true" data-bind="source: AGvwrr_StavPracaku, value: filtrStav, events: { change: filtrStavChange }"></select>
            </li>
            <li>
                <button class="btn btn-sm" style="margin-left:6px;" data-bind="events: { click: clearFilter }">
                    <span class="k-icon k-i-filter-clear"></span>
                </button>
            </li>
        </ul>
    </nav>

    <div data-role="grid"
         data-filterable="true"
         data-column-menu="true"
         data-pageable="false"
         data-editable="false"
         data-sortable="true"
         data-selectable="true"
         data-scrollable="true"
         data-resizable="true"
         data-no-records="{ template: '<h3 style=\'text-align:center;margin-top:16px;\'>Žádné záznamy</h3>' }"
         data-navigatable="true"
         data-sql-source="AGvw_FA_PracakySeznam"
         data-columns="[
         { 'template': '#=btnDetail()#', 'title': ' ', 'width': 80 },
         { 'template': '#=btnDetailPrac()#', 'title': ' ', 'width': 120 },
         { 'field': 'IDVykazPrace', template: '\\##:IDVykazPrace#', title: 'Pracák', width: 50 },
         { 'field': 'rr_StavPracakuHodnota', 'template': '#=stavPracaku(rr_StavPracaku, rr_StavPracakuHodnota)#', 'title': 'Stav pracáku', 'width': 100 },
         { 'field': 'DatVzniku', 'template': '#=dateTmp(DatVzniku)#', 'title': 'Datum vzniku', width: 100 },
         { 'field': 'PobockaNazev', 'title': 'Pobočka' },
         { 'field': 'rr_TypServisniSmlouvyHodnota', 'title': 'Typ servisní smlouvy' },
         { 'field': 'CisloFaktury', 'title': 'Číslo faktury' },
         { 'field': 'UserZalozil', 'title': 'Založil' },
         { 'field': 'UserUpravil', 'title': 'Upravil' },
         { 'field': 'rr_FakturovatNaFirmuHodnota', 'title': 'Fakturovat na', width: 110 },
         { 'title': ' ', 'template': btnpolozky, width: 80 },
         { 'title': ' ', 'template': btnstorno, width: 80 }]"
         data-bind="source: AGvw_FA_PracakySeznam, events: { dataBound: AGvw_FA_PracakySeznam_DataBound, change: AGvw_FA_PracakySeznam_Change }" style="height: 100%;"></div>
</div>

<div class="modal fade" id="modal_detail">
    <div class="modal-dialog modal-max">
        <div class="modal-content">
            <div class="modal-header">
                <nav class="navbar fixed-top bg-secondary">
                    <div class="d-flex flex-row">
                        <button type="button" class="ml-1 btn btn-success btn-sm" data-bind="enabled: btn1visible, events: { click: btn00to10 }" title="Předat k validaci"><span class="k-icon k-i-track-changes-accept"></span> K Validaci</button>
                        <button type="button" class="ml-1 btn btn-success btn-sm" data-bind="enabled: btn2visible, events: { click: btn10to20 }" title="Předat k fakturaci"><span class="k-icon k-i-table-align-top-left"></span> K Fakturaci</button>
                        <button type="button" class="ml-1 btn btn-success btn-sm" data-bind="enabled: btn3visible, events: { click: btn20to30 }" title="Odeslat do Altus Vario"><span class="k-icon k-i-upload"></span> Altus vario</button>
                        <button type="button" class="ml-1 btn btn-primary btn-sm" data-bind="enabled: btn4visible, events: { click: editPolozky }" title="Položky pracáku"><span class="k-icon k-i-paste-plain-text"></span> Položky</button>
                        <button type="button" class="ml-1 btn btn-secondary btn-sm" data-bind="events: { click: previous }" title="Předchozí záznam"><span class="k-icon k-i-arrow-60-up"></span></button>
                        <button type="button" class="ml-1 btn btn-secondary btn-sm" data-bind="events: { click: next }" title="Následující záznam"><span class="k-icon k-i-arrow-60-down"></span></button>
                    </div>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </nav>
            </div>
            <div class="modal-body">
                <div data-bind="html: detail.hlavicka.Detail1"></div>
                <div class="row">
                    <div class="col">
                        <div class="form-group">
                            <label class="text-muted">Zákazník:</label>
                            <input disabled type="text" class="form-control" data-bind="value: detail.hlavicka.Nazev_firmy">
                        </div>
                        <div class="form-group">
                            <label class="text-muted">Úkon proveden na pracovišti:</label>
                            <input disabled type="text" class="form-control" data-bind="value: detail.hlavicka.NazevPobocky">
                        </div>
                        <div class="form-group">
                            <label class="text-muted">Datum zahájení úkonu:</label>
                            <input data-role="datepicker" class="form-control" type="date" data-bind="value: detail.hlavicka.DatVzniku, enabled: btn4visible, events: { change: changeDetail }">
                        </div>
                        <div class="form-group">
                            <label class="text-muted" data-bind="visible: btn4visible">Fakturovat na firmu:</label>
                            <select data-value-primitive="true" data-value-field="value" data-text-field="text" data-bind="enabled: btn4visible, source: rr_FakturovatNaFirmu, value: detail.hlavicka.rr_FakturovatNaFirmu, events: { change: changeDetail }" class="form-control"></select>
                        </div>
                    </div>
                    <div class="col">
                        <div class="form-group">
                            <label class="text-muted">IČO:</label>
                            <input disabled type="text" class="form-control" data-bind="value: detail.hlavicka.ICO">
                        </div>
                        <div class="form-group">
                            <label class="text-muted">Vzdálenost:</label>
                            <input disabled data-format="0 \Km" data-role="numerictextbox" data-bind="value: detail.hlavicka.Vzdalenost" class="form-control" />
                        </div>
                        <div class="form-group">
                            <label class="text-muted">Typ servisky:</label>
                            <input disabled type="text" class="form-control" data-bind="value: detail.hlavicka.rr_TypServisniSmlouvyHodnota">
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col">
                        <label class="text-muted">Položky:</label>
                        <div data-role="grid"
                             data-filterable="false"
                             data-pageable="false"
                             data-editable="false"
                             data-sortable="false"
                             data-selectable="false"
                             data-scrollable="true"
                             data-resizable="true"
                             data-no-records="{ template: '<h3 style=\'text-align:center;margin-top:16px;\'>Žádné položky</h3>' }"
                             data-navigatable="false"
                             data-columns="[
         { 'field': 'rr_TypPolozkyPracakuHodnota', 'title': 'Typ položky', 'width': 170 },
         { 'field': 'Vzdal', 'title': 'Způsob', 'width': 75 },
         { 'field': 'TextNaFakturu', 'title': 'Text na fakturu', 'width': 150 },
         { 'field': 'PocetEMJ', 'template': '#=btnpuvod(PocetEMJ)#', 'title': 'Množství', 'width': 85 },
         { 'field': 'CenaEMJ', 'template': '#=CellMoney(CenaEMJ)#', 'title': 'Cena', 'width': 85 },
         { 'field': 'CelkemBezDPH', 'template': '#=CellMoney(CelkemBezDPH)#', 'title': 'Cena celkem bez DPH', 'width': 110 },
         { 'field': 'Zdrm', 'title': 'Účtovat', 'width': 75 },
         { 'field': 'Technik', 'title': 'Technik', 'width': 90 },
         { 'field': 'Upravil', 'title': 'Upravil', 'width': 90 },
         { 'title': ' ', 'template': btnpolozkydelete, width: 100 }]"
                             data-bind="source: detail.polozky }"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<div class="modal fade" id="modal_pracak_detail">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">Detail pracáku #0</h4>
                <button type="button" class="close" data-dismiss="modal">&times;</button>
            </div>
            <div class="modal-body">
                <div data-auto-bind="false" data-bind="source: AGsp_GetPracakPolozkyNahled" data-template="tmp-pracak-polozky-nahled"></div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal"><span class="k-icon k-i-cancel-outline"></span> Zavřít</button>
            </div>
        </div>
    </div>
</div>

<div class="modal fade" id="modal_pracak_email" style="overflow-y:auto;">
    <div class="modal-dialog modal-lg">
        <form class="modal-content" method="post" data-bind="events: { submit: pracak_email_post }">
            <div class="modal-header">
                <h4 class="modal-title">Email</h4>
                <button type="button" class="close" data-dismiss="modal">&times;</button>
            </div>
            <div class="modal-body">
                <div class="form-row">
                    <div class="form-group col-md-12">
                        <div class="input-group">
                            <div class="input-group-prepend">
                                <span class="input-group-text">Komu</span>
                            </div>
                            <input type="email" class="form-control" data-bind="value: pracakemail.emailTo" required>
                        </div>
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group col-md-12">
                        <div class="input-group">
                            <div class="input-group-prepend">
                                <span class="input-group-text">Předmět</span>
                            </div>
                            <input type="text" class="form-control" data-bind="value: pracakemail.emailSubject" required>
                        </div>
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group col-md-12">
                        <textarea data-role="editor"
                                  data-tools="[]"
                                  rows="5"
                                  class="form-control"
                                  data-bind="value: pracakemail.emailBody"
                                  style="height:600px;"
                                  required></textarea>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <button type="submit" class="btn btn-success"><span class="k-icon k-i-email"></span> Odeslat</button>
                <button type="button" class="btn btn-default" data-dismiss="modal" data-bind="events: { click: pracak_email_close }"><span class="k-icon k-i-cancel-outline"></span> Neodesílat</button>
            </div>
        </form>
    </div>
</div>

<script id="tmp-pracak-polozky-nahled" type="text/html">
    <a class="row pracak-detail-polozka" href="\\#" data-bind="events: { click: puvod }">
        <div class="col-md-12" title="#=TextNaFakturu#" onclick="toggleWrap(this)">#=TextNaFakturu#</div>
        <div class="col-md-4" title="Počet: #=PocetEMJ#"><small class="text-muted">Počet:</small> #=PocetEMJ#</div>
        <div class="col-md-4" title="Cena bez DPH: #=CenaEMJBezDPHNaFakturu# kč"><small class="text-muted">Cena bez DPH:</small> #=CenaEMJBezDPHNaFakturu# kč</div>
        <div class="col-md-4" title="Cena celkem: #=Celkem# kč"><small class="text-muted">Cena celkem:</small> #=Celkem# kč</div>
        <div class="col-md-12" title="#=rr_VypocetKomentHodnota#" onclick="toggleWrap(this)"><small class="text-primary">#=rr_VypocetKomentHodnota#</small></div>
    </a>
</script>

<div class="modal fade" id="modal_puvod">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">Původ položky (<span data-bind="text: puvodData.IDPrijemkaPol"></span>)</h4>
                <button type="button" class="close" data-dismiss="modal">&times;</button>
            </div>
            <div class="modal-body">
                <div class="grid-container-modal_puvod">
                    <div class="Produkt"><label class="text-muted">Produkt: </label> <span data-bind="text: puvodData.Produkt"></span></div>
                    <div class="BlokaceEMJ"><label class="text-muted">Blokace: </label> <span data-bind="text: puvodData.BlokaceEMJ"></span></div>
                    <div class="NaskladnenoEMJ"><label class="text-muted">Naskladněno: </label> <span data-bind="text: puvodData.NaskladnenoEMJ"></span></div>
                    <div class="CenaNakup"><label class="text-muted">Cena nákup: </label> <span data-bind="text: puvodData.CenaNakup"></span></div>
                    <div class="VSPrijmovehoDokladu"><label class="text-muted">VS příjmového dokladu: </label> <span data-bind="text: puvodData.VSPrijmovehoDokladu"></span></div>
                    <div class="SkladovaZasoba"><label class="text-muted">Skladová zásoba: </label> <span data-bind="text: puvodData.SkladovaZasoba"></span></div>
                    <div class="OperativniZasoba"><label class="text-muted">Operativní zásoba: </label> <span data-bind="text: puvodData.OperativniZasoba"></span></div>
                    <div class="Dodavatel"><label class="text-muted">Dodavatel: </label> <span data-bind="text: puvodData.Dodavatel"></span></div>
                    <div class="DatumObjednal"><label class="text-muted">Datum objednání: </label> <span data-bind="text: puvodData.DatumObjednal"></span></div>
                    <div class="KdoObjednal"><label class="text-muted">Objednal: </label> <span data-bind="text: puvodData.KdoObjednal"></span></div>
                    <div class="DatumNaskladnil"><label class="text-muted">Datum naskladnění: </label> <span data-bind="text: puvodData.DatumNaskladnil"></span></div>
                    <div class="KdoNaskladnil"><label class="text-muted">Naskladnil: </label> <span data-bind="text: puvodData.KdoNaskladnil"></span></div>
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal"><span class="k-icon k-i-cancel-outline"></span> Zavřít</button>
            </div>
        </div>
    </div>
</div>

<div class="modal fade" id="modal_fakturace">
    <div class="modal-dialog modal-md">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">Více faktur od jednoho klienta</h4>
                <button type="button" class="close" data-dismiss="modal">&times;</button>
            </div>
            <div class="modal-body">
                Tomuto klientovi se má fakturovat ještě <span data-bind="text: modal_fakturace.pocetDEalsichFaktur"></span> pracáků.
            </div>
            <div class="modal-footer">
                @*1 pokracuju dal*@
                <button type="button" class="btn btn-default" data-act="1" data-bind="events: { click: avisfakturovat }">Tento pracák fakturovat samostatně</button>
                @*2 procedura na slouceni*@
                <button type="button" class="btn btn-default" data-act="2" data-bind="events: { click: avisfakturovat }">Sloučit všechny na jednu fakturu</button>
                @*3 zavru okno a odfiltruju pracaky podle id firmy*@
                <button type="button" class="btn btn-default" data-act="3" data-bind="events: { click: avisfakturovat }"><span class="k-icon k-i-cancel-outline"></span> Nefakturova, nejdřív se na faktury podívá</button>
            </div>
        </div>
    </div>
</div>

<style>
    .pracak-detail-polozka {
        padding-left: 1rem;
    }

        .pracak-detail-polozka:not(:first-child) {
            padding-top: 1rem;
        }

        .pracak-detail-polozka:not(:last-child) {
            border-bottom: 1px solid #e9ecef;
        }

        .pracak-detail-polozka .col-md-12 {
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
        }

    .grid-container-modal_puvod {
        display: grid;
        grid-template-columns: 1fr 1fr;
        grid-template-rows: auto auto auto auto auto auto;
        grid-template-areas: "BlokaceEMJ Produkt" "NaskladnenoEMJ CenaNakup" "VSPrijmovehoDokladu SkladovaZasoba" "OperativniZasoba Dodavatel" "DatumObjednal KdoObjednal" "DatumNaskladnil KdoNaskladnil";
    }

    .BlokaceEMJ {
        grid-area: BlokaceEMJ;
    }

    .Produkt {
        grid-area: Produkt;
    }

    .NaskladnenoEMJ {
        grid-area: NaskladnenoEMJ;
    }

    .CenaNakup {
        grid-area: CenaNakup;
    }

    .VSPrijmovehoDokladu {
        grid-area: VSPrijmovehoDokladu;
    }

    .SkladovaZasoba {
        grid-area: SkladovaZasoba;
    }

    .OperativniZasoba {
        grid-area: OperativniZasoba;
    }

    .Dodavatel {
        grid-area: Dodavatel;
    }

    .DatumObjednal {
        grid-area: DatumObjednal;
    }

    .KdoObjednal {
        grid-area: KdoObjednal;
    }

    .DatumNaskladnil {
        grid-area: DatumNaskladnil;
    }

    .KdoNaskladnil {
        grid-area: KdoNaskladnil;
    }

    .progress-0 {
        height: 100%;
        width: 100%;
        background-image: linear-gradient(45deg, rgba(0, 123, 255, 0.50) 25%, white 25%);
    }

    .progress-10 {
        height: 100%;
        width: 100%;
        background-image: linear-gradient(45deg, rgba(0, 123, 255, 0.50) 50%, white 50%);
    }

    .progress-20 {
        height: 100%;
        width: 100%;
        background-image: linear-gradient(45deg, rgba(0, 123, 255, 0.50) 75%, white 75%);
    }

    .progress-30 {
        height: 100%;
        width: 100%;
        background: rgba(0, 123, 255, 0.50);
    }

    .progress-40 {
        height: 100%;
        width: 100%;
        background: rgba(40, 167, 69, 0.50);
    }

    .progress-50 {
        height: 100%;
        width: 100%;
        background: rgba(220, 53, 69, 0.50);
    }

    .progress-55 {
        height: 100%;
        width: 100%;
        background: rgba(0, 123, 255, 0.50);
    }

    .progress-60 {
        height: 100%;
        width: 100%;
        background: rgba(128, 128, 128, 0.50);
    }
</style>

<script>
    //IDVykazPracePol

    //kendo.ui.progress($("body"), true)

    var idfirma = '@Html.Raw(Request.QueryString("f"))';
    var idprac = '@Html.Raw(Request.QueryString("i"))';
    var idtick = '@Html.Raw(Request.QueryString("t"))';

    function stavPracaku(rr_StavPracaku, rr_StavPracakuHodnota) {
        return `<div class="progress-` + rr_StavPracaku + ` text-center">` + rr_StavPracakuHodnota + `</div>`;
    }

    function btnDetail(e) {
        return '<button class="btn btn-info btn-sm" data-bind="events: { click: btndetail }"><span class="k-icon k-i-zoom"></span>Detail</button>';
    };

    function btnDetailPrac(e) {
        return '<button class="btn btn-success btn-sm" data-bind="events: { click: btnpracakdetail }"><span class="k-icon k-i-zoom"></span>Detail pracáku</button>';
    };

    function CellMoney(val) {
        return '<div style="text-align:right;">' + kendo.toString(val, "c") + '</div>'
    }

    function CellNum(val, format) {
        return '<div style="text-align:right;">' + kendo.toString(val, "n1") + '</div>'
    }

    function dateTmp(value) {
        return kendo.toString(new Date(value), "d");
    };

    function rr_FakturovatNaFirmu(e) {
        if (e.rr_StavPracaku > 10) {
            return '<input disabled class="dropDownTemplate"/>';
        } else {
            return '<input class="dropDownTemplate"/>';
        }
    };

    function btnpolozky(e) {
        if (e.rr_StavPracaku > 10) {
            return '<button disabled class="btn btn-primary btn-sm" data-bind="events: { click: btnpolozky }"><span class="k-icon k-i-paste-plain-text"></span> Položky</button>';
        } else {
            return '<button class="btn btn-primary btn-sm" data-bind="events: { click: btnpolozky }"><span class="k-icon k-i-paste-plain-text"></span> Položky</button>';
        }
    };

    function btnstorno(e) {
        if (e.rr_StavPracaku > 10) {
            return '<button disabled class="btn btn-danger btn-sm" data-bind="events: { click: btn00to50 }"><span class="k-icon k-i-cancel-outline"></span> Storno</button>';
        } else {
            return '<button class="btn btn-danger btn-sm" data-bind="events: { click: btn00to50 }"><span class="k-icon k-i-cancel-outline"></span> Storno</button>';
        }
    };

    function btnpolozkydelete(e) {
        return '<a role="button" class="k-button k-button-icontext" href="#" data-bind="events: { click: btnpolozkydelete }"><span class="k-icon k-i-close"></span>Smazat</a>';
    };

    function btnpuvod(value) {
        return '<a role="button" class="k-button k-button-icontext" title="Původ položky" href="#" data-bind="events: { click: puvod }">' + value + '</a>';
    };

    function cellIDVykazPrace(IDVykazPrace) {
        return `<a class="text-primary" href="@(Url.Action("Fakturace", "Home"))?i=` + IDVykazPrace + `" target="_blank">#` + IDVykazPrace + `</a>`;
    }

    $(function () {
        $('#modal_polozka').on('shown.bs.modal', function () {
            var that = viewModel;
            $.get('@Url.Action("RegRest", "Api/Service")', { Register: "rr_TypPolozkyPracaku" }, function (res) {
                that.set("typy", res.data);
            });
            $.get('@Url.Action("Uzivatele", "Api/Service")', null, function (res) {
                that.set("technici", res.data);
            });
        });

        $("#modal_polozka").on('hidden.bs.modal', function () {
            viewModel.polozky.read();
            viewModel.detail.polozky.read();
        });

        $("#modal_polozky").on('hidden.bs.modal', function () {
            viewModel.AGvw_FA_PracakySeznam.read();
        });

        $('#modal_polozky').on('shown.bs.modal', function () {
            viewModel.AGsp_GetProduktRezervovaneProduktyFirmy.read();
        });

        var viewModel = kendo.observable({
            pracakemail: {
                emailTo: "",
                emailSubject: "",
                emailBody: ""
            },
            pracak_email_close: function (e) {
                var that = this;
                var email = this.get("pracakemail");
                $.post("@Url.Action("SendEmailTicket", "Home")", { EmailTo: "hanzl@@agilo.cz", Subject: email.emailSubject, Body: email.emailBody }, function (result) {
                    that.AGvw_FA_PracakySeznam.read();
                });
            },
            pracak_email_post: function (e) {
                e.preventDefault();
                var that = this;
                var email = this.get("pracakemail");
                $.post("@Url.Action("SendEmailTicket", "Home")", { EmailTo: email.emailTo, Subject: email.emailSubject, Body: email.emailBody }, function (result) {
                    if (result.error) {
                        alert(result.error);
                    } else {
                        $('#modal_pracak_email').modal("hide");
                        that.AGvw_FA_PracakySeznam.read();
                    }
                });
            },
            vyskladnit_do_pracaku_selected: null,
            modal_mnozstvi_save: function (e) {
                var that = this;
                var d = this.get("vyskladnit_do_pracaku_selected");
                var iDVykazPrace = this.get("iDVykazPrace");
                var q = {
                    iDVykazPrace: iDVykazPrace,
                    produkt: d.Produkt,
                    blokovatEMJ: d.OperativniZasoba,
                    cenaEMJProdejni: d.ProdejniCenaPodleNakupu,
                    iDUserUpravil: 0,
                    iDPrijemkaPol: d.IDPrijemkaPol
                }
                $("#modal_mnozstvi").modal("hide");
                $.get("@Url.Action("AGsp_AddNewPracakyPolozkaProduktZablokovat", "Api/Service")", q, function (result) {
                    if (result.error) {
                        alert(result.error);
                    } else {
                        that.polozky.read();
                        that.AGsp_GetProduktRezervovaneProduktyFirmy.read();
                    }
                });
            },
            btn_vyskladnit_do_pracaku: function (e) {
                var d = e.data.toJSON();
                this.set("vyskladnit_do_pracaku_selected", d);
                $("#modal_mnozstvi").modal("show");
            },
            AGsp_GetProduktRezervovaneProduktyFirmy: new kendo.data.DataSource({
                schema: {
                    data: "data",
                    model: {
                        fields: {
                            DatumNaskladnil: { type: "date" },
                            SkladovaZasoba: { type: "number" },
                            OperativniZasoba: { type: "number" },
                            NaskladnenoEMJ: { type: "number" },
                            CenaNakup: { type: "number" },
                            ProdejniCenaPodleNakupu: { type: "number" }
                        }
                    }
                },
                transport: {
                    read: {
                        url: "@Url.Action("AGsp_GetProduktRezervovaneProduktyFirmy", "Api/Service")",
                        type: "GET"
                    },
                    parameterMap: function (options, operation) {
                        var firma = viewModel.get("iDFirma");
                        return { firma: firma }
                    }
                }
            }),
            AGsp_GetPracakPolozkyNahled: new kendo.data.DataSource({
                transport: {
                    read: function (options) {
                        $.get('@Url.Action("AGsp_GetPracakPolozkyNahled", "Api/Service")', { iDVykazPrace: options.data.IDVykazPrace }, function (result) {
                            var data = result.data || [];
                            options.success(result.data);
                            if (result.error) {
                                alert(result.error)
                            } else {
                                if (data.length > 0) {
                                    $('#modal_pracak_detail').modal('show');
                                } else {
                                    alert("Žádné položky k zobrazení.")
                                }
                            }
                        });
                    }
                }
            }),
            puvodData: {},
            puvod: function (e) {
                var that = this;
                var iDVykazPracePol = e.data.IDVykazPracePol;
                $.get('@Url.Action("AGsp_GetPuvodPolozkyNaPracaku", "Api/Service")', { iDVykazPracePol: iDVykazPracePol }, function (result) {
                    if (result.error) {
                        alert(result.error)
                    } else {
                        var data = result.data || {
                            "BlokaceEMJ": 0,
                            "Produkt": "",
                            "NaskladnenoEMJ": 0,
                            "CenaNakup": 0,
                            "VSPrijmovehoDokladu": "",
                            "SkladovaZasoba": 0,
                            "OperativniZasoba": 0,
                            "Dodavatel": null,
                            "DatumObjednal": null,
                            "KdoObjednal": null,
                            "DatumNaskladnil": "",
                            "KdoNaskladnil": "",
                            "IDPrijemkaPol": 0
                        };
                        if (data.DatumObjednal) {
                            data.DatumObjednal = kendo.toString(new Date(data.DatumObjednal), "d");
                        }
                        if (data.DatumNaskladnil) {
                            data.DatumNaskladnil = kendo.toString(new Date(data.DatumNaskladnil), "d");
                        }
                        that.set("puvodData", data);
                        $('#modal_puvod').modal("show");
                    }
                })
            },
            btnpracakdetail: function (e) {
                var grid = $(e.currentTarget).closest(".k-grid").data("kendoGrid");
                var row = $(e.currentTarget).closest("tr");
                var id = e.data.IDVykazPrace;
                grid.select(row);
                $('#modal_pracak_detail .modal-header h4').text('Detail pracáku #' + id);
                this.AGsp_GetPracakPolozkyNahled.read({ IDVykazPrace: id });
            },
            btndetail: function (e) {
                var grid = $(e.currentTarget).closest(".k-grid").data("kendoGrid");
                var row = $(e.currentTarget).closest("tr");
                grid.select(row);
                $('#modal_detail').modal("show");
            },
            message: "",
            btn1visible: false,
            btn2visible: false,
            btn3visible: false,
            btn4visible: false,
            btn00to10: function (e) {
                var that = this;
                var iDVykazPrace = this.get("iDVykazPrace");
                $.get('@Url.Action("AGsp_Run_Pracak00to10", "Api/Service")', { iDVykazPrace: iDVykazPrace }, function (result) {
                    var d = result.data;
                    if (result.error) {
                        alert(result.error)
                    } else {
                        $('#modal_detail').modal("hide");
                        that.set("selectedRow", null);
                        that.AGvw_FA_PracakySeznam.read();
                        $.get('@Url.Action("EmailBodyPoValidaci", "Api/Service")', { iDVykazPrace: iDVykazPrace }, function (result) {
                            if (result.error) { alert(result.error) } else {
                                that.set("pracakemail", result.data);
                                $('#modal_pracak_email').modal("show");
                            }
                        });
                    }
                })
            },
            btn10to20: function (e) {
                var that = this;
                var iDVykazPrace = this.get("iDVykazPrace");
                $.get('@Url.Action("AGsp_Run_Pracak10to20", "Api/Service")', { iDVykazPrace: iDVykazPrace }, function (result) {
                    if (result.error) {
                        alert(result.error);
                        if (result.ll == 120) {
                            $('#modal_detail').modal("hide");
                            that.AGvw_FA_PracakySeznam.read();
                        }
                    } else {
                        $('#modal_detail').modal("hide");
                        that.AGvw_FA_PracakySeznam.read();
                    }
                })
            },
            modal_fakturace: {
                iDFirmy: "",
                iDVykazPrace: 0,
                rr_FakturovatNaFirmu: 0,
                pocetDEalsichFaktur: 0
            },
            btn20to30: function (e) {
                var hl = this.detail.hlavicka;
                $.get("@Url.Action("CilovaAVISFirma", "Api/AVISService")", { iDVykazPrace: hl.IDVykazPrace }, function (result) {
                    if (result.error) {
                        alert(result.error)
                    } else {
                        viewModel.set("modal_fakturace", result.data);
                        if (result.data.pocetDEalsichFaktur > 0) {
                            $('#modal_fakturace').modal("show");
                        } else {
                            viewModel.avisfakturovat(1);
                        }
                    }
                });
            },
            avisfakturovat: function (e) {
                var d = this.get("modal_fakturace"),
                    that = viewModel,
                    typ = $(e.currentTarget).data("act"),
                    typ = (!typ ? 1 : typ);
                if (typ < 3) {
                    $.get("@Url.Action("AVISFakturace", "Api/AVISService")", { typ: typ, iDVykazPrace: d.iDVykazPrace, rr_FakturovatNaFirmu: d.rr_FakturovatNaFirmu }, function (result) {
                        if (result.error) {
                            alert("Došlo k chybě. Informuj Marka nebo Petra. Popis chyby:\n" + result.error);
                        } else {
                            //if (result.data) {
                            //    alert("Byla vygenerována faktura s číslem: " & result.data);
                            //}
                        }
                        $('#modal_fakturace').modal("hide");
                        $('#modal_detail').modal("hide");
                        that.AGvw_FA_PracakySeznam.read();
                    });
                }
                if (typ == 3) {
                    $('#modal_fakturace').modal("hide");
                    $('#modal_detail').modal("hide");
                    that.AGvw_FA_PracakySeznam.filter({ field: "IDFirmy", operator: "eq", value: d.iDFirmy });
                }
            },
            btn00to50: function (e) {
                var that = this;
                if (e.data.rr_StavPracaku < 21) {
                    $.get('@Url.Action("AGsp_Run_Pracak00to50", "Api/Service")', { iDVykazPrace: e.data.IDVykazPrace }, function (result) {
                        if (result.error) {
                            alert(result.error);
                        } else {
                            that.AGvw_FA_PracakySeznam.read();
                        }
                    });
                } else {
                    alert("Pracák je ve stavu kdy již nejde stornovat.");
                }
            },
            AGvwrr_StavPracaku: new kendo.data.DataSource({
                transport: {
                    read: function (options) {
                        $.get('@Url.Action("AGvwrr_StavPracaku", "Api/Service")', {}, function (result) {
                            options.success(result.data);
                        });
                    }
                }
            }),
            produktyHledej: "",
            sluzbyHledej: "",
            produktySelect: function (e) {
                var element = e.sender.select();
                var dataItem = e.sender.dataItem(element[0]);

                this.polozka.set("Produkt", dataItem.Produkt);
                if (!this.polozka.TextNaFakturu) {
                    this.polozka.set("TextNaFakturu", dataItem.Popis);
                }
                if (!this.polozka.TextInterniDoMailu) {
                    this.polozka.set("TextInterniDoMailu", dataItem.Popis);
                }
                if (!this.polozka.PocetEMJ) {
                    this.polozka.set("PocetEMJ", 1);
                }
                if (!dataItem.Cena) {
                    this.polozka.set("CenaEMJ", 0);
                } else {
                    this.polozka.set("CenaEMJ", dataItem.Cena);
                }

                $('#modal_produkty').modal("hide");
                $('#modal_sluzby').modal("hide");
            },
            hledatprodukt: function (e) {
                e.preventDefault();
                this.produkty.read();
            },
            hledatsluzbu: function (e) {
                e.preventDefault();
                this.sluzby.read();
            },
            produkty: new kendo.data.DataSource({
                pageSize: 25,
                transport: {
                    read: function (options) {
                        var h = viewModel.get("produktyHledej");
                        $.get('@Url.Action("AGsp_GetProduktSeznamHledaci", "Api/Service")', { hledej: h }, function (result) {
                            options.success(result.data);
                        });
                    }
                }
            }),
            sluzby: new kendo.data.DataSource({
                pageSize: 25,
                transport: {
                    read: function (options) {
                        var h = viewModel.get("sluzbyHledej");
                        $.get('@Url.Action("AGsp_HledejSluzbaFullText", "Api/Service")', { hledej: h }, function (result) {
                            options.success(result.data);
                        });
                    }
                }
            }),
            dejProdukt: function (e) {
                this.produkty.read();
                $('#modal_produkty').modal("show");
            },
            dejSluzbu: function (e) {
                this.sluzby.read();
                $('#modal_sluzby').modal("show");
                $('#modal_sluzby').on('shown.bs.modal', function (e) {
                    $('#hledatsluzbu').focus();
                })
            },
            typy: [],
            typyVisible: function (e) {
                var tp = this.polozka.get("rr_TypPolozkyPracaku");
                $("#typ" + tp).show(500);
            },
            typChange: function (e) {
                var that = this;
                var tp = e.sender.value();
                if (tp == 5 || tp == 6 || tp == 7) {
                    $.get('@Url.Action("AGsp_GetPracakHodnotyDopravne", "Api/Service")', { iDVykazPrace: that.get("iDVykazPrace"), rr_TypPolozkyPracaku: tp }, function (res) {
                        var d = res.data;
                        that.polozka.set("TextNaFakturu", d.text);
                        that.polozka.set("TextInterniDoMailu", d.text);
                        that.polozka.set("PocetEMJ", d.km);
                        that.polozka.set("NajetoKM", d.najeto);
                    });
                };
            },
            technici: [],
            polozka: {
                IDVykazPracePol: 0,
                IDVykazPrace: 0,
                IDUserUpravil: 0,
                rr_TypPolozkyPracaku: 4,
                CasOd: 0,
                CasDo: 0,
                Hodin: 1,
                IDTechnika: 0,
                Produkt: "",
                TextNaFakturu: "",
                TextInterniDoMailu: "",
                PocetEMJ: 0,
                NajetoKM: 0,
                CenaEMJ: 0,
                Zasah: false,
                Vzdalenka: false,
                Zdarma: false,
                NavzdoryServisceUctovat: false
            },
            tnfkeyup: function (e) {
                var val = $(e.currentTarget).val();
                this.polozka.set("TextNaFakturu", val);
                this.polozka.set("TextInterniDoMailu", val);
            },
            text_nf_ticket: function (e) {
                var that = this,
                    pol = this.get("polozka");
                $.get('@Url.Action("AGsp_Get_TicketTextTicketu", "Api/Service")', { iDVykazPrace: that.get("iDVykazPrace") }, function (res) {
                    if (res.error) {
                        alert(res.error)
                    } else {
                        pol.set("TextNaFakturu", res.data);
                        if (!pol.TextInterniDoMailu) {
                            pol.set("TextInterniDoMailu", res.data);
                        }
                    }
                });
            },
            polozky: new kendo.data.DataSource({
                schema: {
                    model: {
                        id: "IDVykazPracePol"
                    }
                },
                transport: {
                    read: function (options) {
                        var id = viewModel.get("iDVykazPrace");
                        $.get('@Url.Action("AGsp_GetVykazPracePolSeznam", "Api/Service")', { iDVykazPrace: id }, function (result) {
                            options.success(result.data);
                        });
                    }
                }
            }),
            polozkyDelete: function (e) {
                var that = this;
                if (that.rr_StavPracaku < 11) {
                    $.get('@Url.Action("AGsp_DelVykazPracePol", "Api/Service")', { iDVykazPracePol: e.data.IDVykazPracePol }, function (result) {
                        if (result.error) {
                            alert(result.error);
                        } else {
                            that.polozky.read();
                            that.AGsp_GetProduktRezervovaneProduktyFirmy.read();
                        }
                    });
                } else {
                    alert("Pracák musí být ve stavu Pořizován nebo validovat");
                }
            },
            polozkaSave: function (e) {
                var d = this.get("polozka").toJSON();
                var that = this;

                d.CasOd = kendo.toString(d.CasOd, "HH:mm:ss");
                d.CasDo = kendo.toString(d.CasDo, "HH:mm:ss");

                $.post('@Url.Action("AGsp_AddOrEditVykazPracePol", "Api/Service")', d, function (res) {
                    if (res.error) {
                        alert(res.error)
                    } else {
                        if (res.data > 0) {
                            var tp = res.data;
                            that.polozka.set("rr_TypPolozkyPracaku", tp);
                            if (tp == 5 || tp == 6 || tp == 7) {
                                $.get('@Url.Action("AGsp_GetPracakHodnotyDopravne", "Api/Service")', { iDVykazPrace: that.get("iDVykazPrace"), rr_TypPolozkyPracaku: tp }, function (res) {
                                    var d = res.data;
                                    that.polozka.set("TextNaFakturu", d.text);
                                    that.polozka.set("TextInterniDoMailu", d.text);
                                    that.polozka.set("PocetEMJ", d.km);
                                    that.polozka.set("NajetoKM", d.najeto);
                                });
                            };
                        } else {
                            $('#modal_polozka').modal("hide");
                        }
                    }
                });
            },
            iDFirma: 0,
            iDVykazPrace: 0,
            rr_StavPracaku: 0,
            iDVykazPracePol: 0,
            btnpolozky: function (e) {
                this.set("iDFirma", e.data.IDFirma);
                this.set("iDVykazPrace", e.data.IDVykazPrace);
                this.set("rr_StavPracaku", e.data.rr_StavPracaku);
                this.polozky.read();
                $('#modal_polozky').modal("show");
            },
            btnpolozkydelete: function (e) {
                var that = this;
                if (that.rr_StavPracaku < 11) {
                    $.get('@Url.Action("AGsp_DelVykazPracePol", "Api/Service")', { iDVykazPracePol: e.data.IDVykazPracePol }, function (result) {
                        if (result.error) {
                            alert(result.error);
                        } else {
                            that.detail.polozky.read();
                        }
                    });
                } else {
                    alert("Pracák musí být ve stavu Pořizován nebo validovat");
                }
            },
            seznamhodin: new kendo.data.DataSource({
                schema: {
                    data: "data",
                    total: "total",
                    errors: "error",
                    model: {
                        id: "IDVykazPracePol",
                        fields: {
                            DatVzniku: { type: "date" },
                            IDTechnika: { type: "number" },
                            UserLastName: { type: "string" },
                            CerpanoVolnychHodin: { type: "number" },
                            IDVykazPrace: { type: "number" },
                            rr_HodinoveUctovaniText: { type: "string" },
                            TextNaFakturu: { type: "string" }
                        }
                    }
                },
                transport: {
                    read: {
                        url: function () {
                            return '@Url.Action("AGsp_Get_VykazPraceCerpaneHodinyVMesici", "Api/Service")';
                        }
                    },
                    parameterMap: function (data, type) {
                        var pm = kendo.data.transports.odata.parameterMap(data);
                        pm.iDVykazPrace = viewModel.get("iDVykazPrace");
                        return pm;
                    }
                },
                requestEnd: function (e) {
                    if (e.type == "read") {
                        var v = e.response.volnychHod;
                        var z = e.response.zbyvaHod;
                        $(".zbyvaHod").text("Volných hod. na smlouvě/zbývá: " + v + "h/" + z + "h");
                    }
                },
                error: function (e) {
                    e.sender.cancelChanges();
                    if (e.error) {
                        alert(e.error);
                    } else {
                        alert("Systémová chyba aplikace. Kontaktujte vývojáře.")
                    }
                }
            }),
            seznamjizd: new kendo.data.DataSource({
                schema: {
                    data: "data",
                    total: "total",
                    errors: "error",
                    model: {
                        id: "IDVykazPracePol",
                        fields: {
                            DatVzniku: { type: "date" },
                            IDTechnika: { type: "number" },
                            UserLastName: { type: "string" },
                            PrvniJizdaVMesici: { type: "boolean" },
                            PocetEMJ: { type: "number" },
                            NajetoKM: { type: "number" },
                            IDVykazPrace: { type: "number" }
                        }
                    }
                },
                transport: {
                    read: {
                        url: function () {
                            return '@Url.Action("AGsp_Get_VykazPraceSeznamJizdVMesici", "Api/Service")';
                        }
                    },
                    parameterMap: function (data, type) {
                        var pm = kendo.data.transports.odata.parameterMap(data);
                        pm.iDVykazPrace = viewModel.get("iDVykazPrace");
                        return pm;
                    }
                },
                error: function (e) {
                    e.sender.cancelChanges();
                    if (e.error) {
                        alert(e.error);
                    } else {
                        alert("Systémová chyba aplikace. Kontaktujte vývojáře.")
                    }
                }
            }),
            editPolozky: function (e) {
                var iDVykazPrace = this.get("iDVykazPrace");
                var rr_StavPracaku = this.get("rr_StavPracaku");
                if (iDVykazPrace === 0) {
                    alert("Vyber pracák");
                    return false;
                }
                if (rr_StavPracaku > 10) {
                    alert("Pracák musí být ve stavu Pořizován nebo validovat");
                    return false;
                }
                this.polozky.read();
                $('#modal_polozky').modal("show");
            },
            mvvisible: false,
            polozkySelect: function (e) {
                var that = this;
                var rr_StavPracaku = this.get("rr_StavPracaku");
                var element = e.sender.select();
                var dataItem = e.sender.dataItem(element[0]);
                that.set("iDVykazPracePol", dataItem.IDVykazPracePol);
                if (rr_StavPracaku > 10) {
                    alert("Pracák musí být ve stavu Pořizován nebo validovat");
                    return false;
                }
                that.set("polozka", {});
                $.get('@Url.Action("AGsp_GetVykazPracePolDetail", "Api/Service")', { iDVykazPracePol: that.get("iDVykazPracePol") }, function (res) {
                    var d = res.data[0];
                    var z = parseInt(d["VelkyZasah"]);

                    d["VelkyZasah"] = (z === 1 ? true : false);

                    $(".typy").hide();
                    that.set("polozka", d);
                    that.seznamhodin.read();
                    that.seznamjizd.read();

                    setTimeout(function () {
                        $('#modal_polozka').modal("show");
                    }, 500);
                });
            },
            rr_HodinoveUctovani_change: function (e) {
                var id = $(e.currentTarget).attr("id");
                var typ = this.polozka.get("rr_TypPolozkyPracaku");
                switch (id) {
                    case "rr_HodinoveUctovani1":

                        break;
                    case "rr_HodinoveUctovani11":

                        break;
                    case "rr_HodinoveUctovani2":

                        break;
                    case "rr_HodinoveUctovani22":

                        break;
                    case "rr_HodinoveUctovani3":
                        var min15 = new Date(new Date().getTime() + 15 * 60000);
                        this.polozka.set("TextNaFakturu", "Malý zásah do 15 minut");
                        this.polozka.set("Hodin", 0.25);
                        this.polozka.set("CasOd", kendo.toString(new Date(), "HH:mm"));
                        this.polozka.set("CasDo", kendo.toString(min15, "HH:mm"));
                        break;
                    case "rr_HodinoveUctovani33":
                        var min15 = new Date(new Date().getTime() + 15 * 60000);
                        this.polozka.set("TextNaFakturu", "Malý zásah do 15 minut");
                        this.polozka.set("Hodin", 0.25);
                        this.polozka.set("CasOd", kendo.toString(new Date(), "HH:mm"));
                        this.polozka.set("CasDo", kendo.toString(min15, "HH:mm"));
                        break;
                    case "rr_HodinoveUctovani4":
                        var min30 = new Date(new Date().getTime() + 30 * 60000);
                        this.polozka.set("TextNaFakturu", "Malý zásah do 30 minut");
                        this.polozka.set("Hodin", 0.5);
                        this.polozka.set("CasOd", kendo.toString(new Date(), "HH:mm"));
                        this.polozka.set("CasDo", kendo.toString(min30, "HH:mm"));
                        break;
                    case "rr_HodinoveUctovani44":
                        var min30 = new Date(new Date().getTime() + 30 * 60000);
                        this.polozka.set("TextNaFakturu", "Malý zásah do 30 minut");
                        this.polozka.set("Hodin", 0.5);
                        this.polozka.set("CasOd", kendo.toString(new Date(), "HH:mm"));
                        this.polozka.set("CasDo", kendo.toString(min30, "HH:mm"));
                        break;
                    case "rr_HodinoveUctovani5":
                        var min60 = new Date(new Date().getTime() + 60 * 60000);
                        this.polozka.set("TextNaFakturu", "Velký zásah do 60 minut");
                        this.polozka.set("Hodin", 1);
                        this.polozka.set("CasOd", kendo.toString(new Date(), "HH:mm"));
                        this.polozka.set("CasDo", kendo.toString(min60, "HH:mm"));
                        break;
                    case "rr_HodinoveUctovani55":
                        var min60 = new Date(new Date().getTime() + 60 * 60000);
                        this.polozka.set("TextNaFakturu", "Velký zásah do 60 minut");
                        this.polozka.set("Hodin", 1);
                        this.polozka.set("CasOd", kendo.toString(new Date(), "HH:mm"));
                        this.polozka.set("CasDo", kendo.toString(min60, "HH:mm"));
                        break;
                }
                $("#tnf" + typ).focus();
            },
            polozkyNew: function (e) {
                var that = this;
                var rr_StavPracaku = this.get("rr_StavPracaku");
                if (rr_StavPracaku > 10) {
                    alert("Pracák musí být ve stavu Pořizován nebo validovat");
                    return false;
                }
                var min60 = new Date(new Date().getTime() + 60 * 60000);
                var ds = {
                    IDVykazPracePol: 0,
                    IDVykazPrace: that.get("iDVykazPrace"),
                    IDUserUpravil: dataUser.IDUser,
                    rr_TypPolozkyPracaku: 4,
                    CasOd: kendo.toString(new Date(), "HH:mm"),
                    CasDo: kendo.toString(min60, "HH:mm"),
                    Hodin: 1,
                    IDTechnika: dataUser.IDUser,
                    Produkt: "",
                    TextNaFakturu: "",
                    TextInterniDoMailu: "",
                    PocetEMJ: 1,
                    NajetoKM: 0,
                    CenaEMJ: 0,
                    Zasah: false,
                    Vzdalenka: false,
                    Zdarma: false,
                    NavzdoryServisceUctovat: false,
                    rr_HodinoveUctovani: 2
                };
                that.set("polozka", ds);
                $('#modal_polozka').modal("show");
                that.seznamhodin.read();
                that.seznamjizd.read();
            },
            zaschange: function (e) {
                var typ = this.polozka.get("rr_TypPolozkyPracaku");
                var min30 = new Date(new Date().getTime() + 30 * 60000);
                var min60 = new Date(new Date().getTime() + 60 * 60000);
                if (e.checked) {
                    this.polozka.set("TextNaFakturu", "Velký zásah do 60 minut");
                    this.polozka.set("Hodin", 1);
                    this.polozka.set("CasOd", kendo.toString(new Date(), "HH:mm"));
                    this.polozka.set("CasDo", kendo.toString(min60, "HH:mm"));
                } else {
                    this.polozka.set("TextNaFakturu", "Malý zásah do 30 minut");
                    this.polozka.set("Hodin", 0.5);
                    this.polozka.set("CasOd", kendo.toString(new Date(), "HH:mm"));
                    this.polozka.set("CasDo", kendo.toString(min30, "HH:mm"));
                }
                $("#tnf" + typ).focus();
            },
            changeDetail: function (e) {
                var that = this;
                var d = this.detail.hlavicka;
                if (!d.IDVykazPrace) {
                    alert("Vyber pracák");
                    return false;
                }
                $.get("@Url.Action("AGsp_Do_FA_FakturovatNa", "Api/Service")", { iDVykazPrace: d.IDVykazPrace, rr_FakturovatNaFirmu: d.rr_FakturovatNaFirmu, datumZasahu: kendo.toString(d.DatVzniku, "yyyy-MM-dd HH:mm:ss") }, function (result) {
                    that.AGvw_FA_PracakySeznam.read();
                });
            },
            detail: {
                hlavicka: {},
                polozky: new kendo.data.DataSource({
                    schema: {
                        model: {
                            id: "IDVykazPracePol"
                        }
                    },
                    transport: {
                        read: function (options) {
                            var id = viewModel.get("iDVykazPrace");
                            $.get('@Url.Action("AGsp_FA_PracakyDetailPol", "Api/Service")', { iDVykazPrace: id }, function (result) {
                                options.success(result.data);
                            });
                        }
                    }
                })
            },
            filtrStav: (idfirma || idprac ? 60 : dataUser.MenuFaktVychoziFiltr),
            filtrStavChange: function (e) {
                var stav = e.sender.value();
                this.set("filtrStav", stav);
                this.AGvw_FA_PracakySeznam.filter({});
            },
            AGvw_FA_PracakySeznam: new kendo.data.DataSource({
                serverPaging: true,
                serverSorting: true,
                serverFiltering: true,
                sort: { field: "IDVykazPrace", dir: "desc" },
                filter: (idfirma ? { field: "IDFirma", operator: "contains", value: idfirma } : {}),
                schema: {
                    total: "Total",
                    data: "Data",
                    model: {
                        id: "IDVykazPrace",
                        fields: {
                            DatVzniku: { type: 'date' },
                            rr_StavPracakuHodnota: { type: 'string' },
                            CisloFaktury: { type: 'number' },
                            rr_FakturovatNaFirmu: { type: 'enums' }
                        }
                    }
                },
                transport: {
                    read: {
                        url: "@Url.Action("AGvw_FA_PracakySeznam", "Api/Service")",
                        type: "GET"
                    },
                    parameterMap: function (options, operation) {
                        if (operation === "read") {
                            var pm = kendo.data.transports["odata-v4"].parameterMap(options);
                            if (pm.$filter) {
                                pm.$filter = pm.$filter.replace(/rr_FakturovatNaFirmu/g, "rr_FakturovatNaFirmuHodnota");
                            }
                            pm.stav = viewModel.get("filtrStav");
                            return pm;
                        } else {
                            return options.models[0];
                        }
                    }
                }
            }),
            next: function (e) {
                var id = this.get("iDVykazPrace");
                var cur = $('tr[data-id="' + id + '"]')
                var nex = cur.next();
                nex.click();
            },
            previous: function (e) {
                var id = this.get("iDVykazPrace");
                var cur = $('tr[data-id="' + id + '"]')
                var pre = cur.prev();
                pre.click();
            },
            clearFilter: function (e) {
                window.location.replace('@Url.Action("Fakturace", "Home")');
                //this.AGvw_FA_PracakySeznam.filter({});
            },
            rr_FakturovatNaFirmu: new kendo.data.DataSource({
                schema: {
                    data: "data"
                },
                transport: {
                    read: "@Url.Action("RegRest", "Api/Service")?Register=rr_FakturovatNaFirmu"
                }
            }),
            AGvw_FA_PracakySeznam_DataBound: function (e) {
                var that = this;
                var rows = e.sender.tbody.find("tr");

                rows.each(function () {
                    var di = e.sender.dataItem($(this));
                    $(this).attr("data-idfirma", di.IDFirma);
                    $(this).attr("data-id", di.IDVykazPrace);
                })

                var row = null;
                var di = null;
                var selected = that.get("selectedRow");
                if (idfirma) {
                    row = e.sender.tbody.find('tr[data-idfirma="' + idfirma + '"]');
                    e.sender.select(row);
                    idfirma = null;
                } else if (idprac) {
                    row = e.sender.tbody.find('tr[data-id="' + idprac + '"]');
                    di = e.sender.dataItem(row);
                    if (di) {
                        this.set("iDVykazPrace", di.IDVykazPrace);
                        this.set("rr_StavPracaku", di.rr_StavPracaku);
                    }
                    this.polozky.read();
                    $('#modal_polozky').modal("show");
                    e.sender.select(row);
                    idprac = null;
                } else if (selected) {
                    row = e.sender.tbody.find('tr[data-id="' + selected.IDVykazPrace + '"]');
                    e.sender.select(row);
                } else {
                    row = e.sender.tbody.find('tr:first');
                    e.sender.select(row);
                }
            },
            selectedRow: null,
            AGvw_FA_PracakySeznam_Change: function (e) {
                var grid = e.sender;
                var item = $.map(grid.select(), function (a, b) {
                    return grid.dataItem(a);
                })[0];
                var id = item.IDVykazPrace;
                var stav = item.rr_StavPracaku;

                this.set("iDFirma", item.IDFirma);
                this.set("iDVykazPrace", id);
                this.set("rr_StavPracaku", stav);
                this.set("btn1visible", false);
                this.set("btn2visible", false);
                this.set("btn3visible", false);

                if (stav === 0) {
                    this.set("btn1visible", true);
                } else if (stav === 10) {
                    this.set("btn2visible", true);
                } else if (stav === 20) {
                    this.set("btn3visible", true);
                }

                if (stav > 10) {
                    this.set("btn4visible", false);
                } else {
                    this.set("btn4visible", true);
                }

                viewModel.set("selectedRow", item);

                $.get("@Url.Action("AGsp_GetVykazPraceDetail", "Api/Service")", { iDVykazPrace: id }, function (result) {
                    if (result.error) {
                        alert(result.error);
                    } else {
                        var d = result.data;
                        if (d) {
                            viewModel.detail.set("hlavicka", d);
                        } else {
                            viewModel.detail.set("hlavicka", {});
                        }
                    }
                    viewModel.detail.polozky.read();
                });
            }
        });

        var original = viewModel.toJSON();
        kendo.bind(document.body, viewModel);

        viewModel.bind("change", function (e) {
            if (e.field === "polozka" || e.field === "polozka.Vzdalenka") {
                var d = viewModel.get("polozka");
                viewModel.set("mvvisible", (d.Vzdalenka ? true : false))
            }
        })

        $.each($('[data-sql-source]'), function () {
            var element = $(this);
            var source = element.attr("data-sql-source");
            if (source) {
                $('<div role="tooltip" style="display: none; z-index: 99;" class="data-sql-tooltip k-widget k-tooltip k-tooltip-closable k-popup k-group k-reset k-state-border-up" data-role="popup" style="position: absolute; display: flex; opacity: 1; z-index: 99;"><div class="k-tooltip-content">Zdroj: ' + source + '</div></div>').insertBefore(element);
            }
        });
        var bindtooltip = false;
        $(window).keydown(function (e) {
            if (e.ctrlKey && e.keyCode == 13) {
                if (bindtooltip) {
                    $(".data-sql-tooltip").hide();
                    bindtooltip = false;
                } else {
                    $(".data-sql-tooltip").show();
                    bindtooltip = true;
                }
            }
        });
    });
</script>
