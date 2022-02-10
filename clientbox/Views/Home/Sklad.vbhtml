@Code
    ViewData("Title") = "Sklad"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<style scoped>
    .km-switch {
        width: 150px !important;
        margin-left: 6px;
    }

    .km-switch-label-on {
        left: -110px;
    }

    .k-switch-handle, .km-switch-handle {
        color: #212529;
    }

    .k-switch-container {
        padding: 0;
    }
</style>

<div id="main" style="height: calc(100% - 54px);">
    <nav class="navbar navbar-expand-sm bg-dark navbar-dark">
        <form class="form-inline my-2 my-lg-0" data-bind="events: { submit: hledatProdukt }">
            <div class="input-group input-group-sm">
                <input type="search" class="form-control" data-bind="value: hledanyProdukt" placeholder="Hledat..." aria-label="Hledat...">
                <div class="input-group-append">
                    <button class="btn btn-secondary" type="submit"><span class="k-icon k-i-zoom"></span></button>
                </div>
            </div>
        </form>
        <button class="btn btn-sm" style="margin-left:6px;margin-right:6px;" data-bind="events: { click: clearFilter }">
            <span class="k-icon k-i-filter-clear"></span>
        </button>
        <input data-role="switch"
               data-messages="{
                checked: 'Oper.',
                unchecked: 'Vše',
                }"
               data-bind="events: { change: switchChange }" title="Všechny/Operativní" />
        @*<input data-role="kendo.mobile.ui.Switch" type="checkbox" style="margin-left:6px;" data-bind="events: { change: switchChange }" data-on-label="Oper. zásoba " data-off-label="Všechny zásoby" />*@
        @*<button type="button" style="margin-left:6px;" class="btn btn-success btn-sm btn-add" data-toggle="modal" data-target="#modal_sklad"><span class="k-icon k-i-plus"></span> Naskladňování</button>*@
        <a style="margin-left:6px;" class="btn btn-default btn-sm" href="@Url.Action("InventurniStav", "Home")" target="_blank"><span class="k-icon k-i-print"></span> Tisk inventury</a>
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
         data-columns="[
         { 'field': 'Produkt', 'title': 'Produkt', template: '#=linkProdukt()#' },
         { 'field': 'Popis', 'title': 'Popis' },
         { 'field': 'Carovy_kod', 'title': 'Čárový kód' },
         { 'field': 'Dodavatel', 'title': 'Dodavatel' },
         { 'field': 'SkladovaZasoba', 'title': 'Skladová zásoba' },
         { 'field': 'OperativniZasoba', 'title': 'Operativní zásoba' },
         { 'field': 'PrumernaNakup', 'title': 'Průměr na nákup' },
         { 'field': 'PrumernaProdej', 'title': 'Průměr na prodej' },
         { 'field': 'NaposledyNaskladneno', 'format': '{0:d}', 'title': 'Naposledy naskladněno' }]"
         data-bind="source: AGsp_GetProduktSeznamHledaci, events: { dataBound: AGsp_GetProduktSeznamHledaci_dataBound, change: AGsp_GetProduktSeznamHledaci_change }" style="height: 100%;"></div>

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
                                <label class="text-muted">Čárový kód</label>
                                <input type="text" maxlength="30" name="Carovy_kod" placeholder="Čárový kód" data-bind="value: detail.Carovy_kod, events: { keyup: hledat_barcode }" class="form-control">
                            </div>
                            <div class="form-group col-md-7">
                                <label class="text-muted">Produkt</label>
                                <input data-role="multicolumncombobox"
                                       data-placeholder="Produkt"
                                       data-value-primitive="true"
                                       data-text-field="Popis"
                                       data-value-field="Produkt"
                                       data-filter="contains"
                                       data-filter-fields="['Produkt', 'Popis', 'Carovy_kod', 'Dodavatel']"
                                       data-columns="[ { 'field': 'Produkt', 'title': 'Produkt' },
                                                       { 'field': 'Popis', 'title': 'Popis' },
                                                       { 'field': 'Carovy_kod', 'title': 'Čárový kód' },
                                                       { 'field': 'Dodavatel', 'title': 'Dodavatel' } ]"
                                       data-bind="value: detail.Produkt, source: produkty, events: { change: hledat_produkt, keyup: hledat_produkt_key }"
                                       class="form-control" />
                            </div>
                        </div>
                    </form>

                    @*nalezeno*@
                    <form data-bind="visible: nalezeno">
                        <div class="form-row modal-body">
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
                                <label class="text-muted">Naskladněno</label>
                                <input data-format="0 \Ks" data-role="numerictextbox" type="number" data-min="1" min="1" required name="NaskladnenoEMJ" data-bind="value: detail.NaskladnenoEMJ" class="form-control">
                            </div>
                            <div class="form-group col-md-6">
                                <div class="row">
                                    <div class="col">
                                        <label class="text-muted">Nákup. cena</label>
                                        <input data-format="0.00 \Kč" data-step="0.10" name="Cena" data-role="numerictextbox" type="number" data-min="0" min="0" data-value-update="keyup" required data-bind="value: detail.CenaNakup, events: { change: calculateCena }" class="form-control">
                                    </div>
                                    <div class="col">
                                        <label class="text-muted">Prodej. cena navýšeno o <span data-bind="text: navyseni"></span></label>
                                        <input data-format="0.00 \Kč" data-step="0.10" name="CenaProdej" data-role="numerictextbox" type="number" data-min="0" min="0" data-value-update="keyup" required data-bind="value: detail.CenaProdej, events: { change: calculateCena }" class="form-control">
                                    </div>
                                </div>
                            </div>
                            <div class="col-md-1">

                            </div>
                            <div class="form-group col-md-5">
                                <label class="text-muted">Dodavatel</label>
                                <input data-role="combobox"
                                       data-value-primitive="true"
                                       data-text-field="Dodavatel"
                                       data-value-field="Dodavatel"
                                       data-bind="value: detail.Dodavatel, source: dodavatele" class="form-control" required />
                            </div>
                            <div class="form-group col-md-6">
                                <label class="text-muted">VS příjmového dokladu</label> <a class="btn btn-sm btn-light" style="float:right;" data-bind="events: { click: posledni_vs }">Použít poslední VS</a>
                                <input type="text" maxlength="12" required name="VSPrijmovehoDokladu" data-bind="value: detail.VSPrijmovehoDokladu, events: { change: vs_change }" class="form-control">
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="submit" class="btn btn-success" data-bind="events: { click: naskladnit }"><span class="k-icon k-i-cart"></span> Naskladnit</button>
                            <button type="submit" class="btn btn-info" data-bind="events: { click: rezervovat }"><span class="k-icon k-i-lock"></span> Naskladnit a zablokovat</button>
                            <button type="button" class="btn btn-default" data-dismiss="modal"><span class="k-icon k-i-cancel-outline"></span> Zrušit</button>
                        </div>
                    </form>
                    @*nenalezeno*@
                    <form data-bind="visible: nenalezeno">
                        <div class="form-row modal-body">
                            <div class="col-md-12">
                                <p style="color:#0094ff;border-bottom: 1px #0094ff solid">Produkt ještě neznáme, je to zřejmě novinka, založte ho vypněním dalších údajů</p>
                            </div>
                            <div class="col-md-1"></div>
                            <div class="form-group col-md-11">
                                <label class="text-muted">Popis</label>
                                <input type="text" maxlength="255" name="Popis" data-bind="value: detail.Popis" placeholder="Popis" class="form-control">
                            </div>

                            <div class="col-md-1"></div>
                            <div class="form-group col-md-5">
                                <label class="text-muted">Jednotky</label>
                                <input type="text" maxlength="30" required name="Jednotky" data-bind="value: detail.Jednotky" placeholder="Jednotky" class="form-control">
                            </div>
                            <div class="form-group col-md-6">
                                <label class="text-muted">Cena prodejní</label>
                                <input data-format="0.00 \Kč" data-step="0.10" name="Cena" data-role="numerictextbox" type="number" data-min="1" min="1" required data-bind="value: detail.CenaProdej" placeholder="Cena prodejní" class="form-control">
                            </div>

                            <div class="col-md-1"></div>
                            <div class="form-group col-md-5">
                                <label class="text-muted">Množství minimální</label>
                                <input data-format="0 \Ks" data-role="numerictextbox" type="number" data-min="1" min="1" required name="Mnozstvi_minimalni" data-bind="value: detail.Mnozstvi_minimalni" placeholder="Množství minimální" class="form-control">
                            </div>
                            <div class="form-group col-md-6">
                                <label class="text-muted">Internet</label>
                                <input type="url" maxlength="255" name="Internet" data-bind="value: detail.Internet" placeholder="Internet" class="form-control">
                            </div>

                            <div class="col-md-1"></div>
                            <div class="form-group col-md-5">
                                <label class="text-muted">Dodavatel</label>
                                <input data-role="combobox"
                                       data-value-primitive="true"
                                       data-text-field="Dodavatel"
                                       data-value-field="Dodavatel"
                                       data-bind="value: detail.Dodavatel, source: dodavatele" name="Dodavatel" class="form-control" required />
                            </div>
                            <div class="form-group col-md-6">
                                <label class="text-muted">Poznámka</label>
                                <input type="text" maxlength="255" name="Poznamka" data-bind="value: detail.Poznamka" placeholder="Poznámka" class="form-control">
                            </div>
                        </div>

                        <div class="modal-footer">
                            <button type="submit" class="btn btn-success" data-bind="events: { click: pridat }"><span class="k-icon k-i-plus"></span> Přidat nový produkt</button>
                            <button type="button" class="btn btn-default" data-dismiss="modal"><span class="k-icon k-i-cancel-outline"></span> Zrušit</button>
                        </div>
                    </form>
                </div>

                <div data-bind="visible: rezervovani">
                    <div class="modal-header">
                        <h4 class="modal-title">Zarezervovat</h4>
                    </div>

                    <div class="modal-body">
                        <div class="form-row">
                            <div class="form-group col-md-6">
                                <label class="text-muted">Počet kusů k zablokování</label>
                                <input data-format="0 \Ks" required data-role="numerictextbox" type="number" data-min="1" min="1" data-bind="value: rezervace.pocet" placeholder="Počet kusů" class="form-control">
                            </div>
                            <div class="form-group col-md-6">
                                <label class="text-muted">Cena prodejní</label><label class="text-muted" style="float:right;color:red;">(Průměrná nákupní cena: <span data-bind="text: detail.PrumernaNakup"></span>)</label>
                                <input data-format="0.00 \Kč" required data-role="numerictextbox" type="number" data-min="0" min="0" data-bind="value: rezervace.cena" placeholder="Cena" class="form-control">
                            </div>
                        </div>
                        <div class="form-row">
                            <div class="form-group col-md-6">
                                <label class="text-muted">Pro pracoviště klienta</label>
                                <input type="search" placeholder="Hledat..." data-bind="events: { keyup: rezervace.hledat_klienta }" class="form-control" style="border-bottom-left-radius:0;border-bottom-right-radius:0;border-bottom:0;">
                                <select data-role="listbox" data-text-field="Firma" data-value-field="Firma" data-bind="source: rezervace.klienti, events: { change: rezervace.select_klient }" data-template="tmpklient" class="form-control"></select>
                            </div>
                            <div class="form-group col-md-6">
                                <label class="text-muted">Seznam rozpracovaných pracovních listů</label>
                                <input type="search" placeholder="Hledat..." data-bind="events: { keyup: rezervace.hledat_pracak }" class="form-control" style="border-bottom-left-radius:0;border-bottom-right-radius:0;border-bottom:0;">
                                <select data-role="listbox" data-text-field="Firma" data-value-field="IDVykazPrace" data-bind="source: rezervace.pracaky, events: { change: rezervace.select_pracak }" data-template="tmppracak" class="form-control"></select>
                            </div>
                        </div>
                    </div>

                    <div class="modal-footer">
                        <button type="button" class="btn btn-success" data-bind="events: { click: rezervace.novy }"><span class="k-icon k-i-plus"></span> Na nový pracák</button>
                        <button type="button" class="btn btn-default" data-bind="events: { click: rezervace.zpet }"><span class="k-icon k-i-cancel-outline"></span> Zpět</button>
                    </div>
                </div>

            </div>
        </div>
    </div>
</div>

<div class="modal fade" id="modal_produkt_jednotlive">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">Produkt jednotlivě naskladněno</h4>
                <button type="button" class="close" data-dismiss="modal">&times;</button>
            </div>
            <div class="modal-body">
                <div data-role="grid"
                     data-auto-bind="false"
                     data-filterable="true"
                     data-column-menu="true"
                     data-pageable="false"
                     data-editable="false"
                     data-sortable="true"
                     data-selectable="false"
                     data-scrollable="true"
                     data-resizable="true"
                     data-no-records="{ template: '<h3 style=\'text-align:center;margin-top:16px;width:100%;\'>Žádné záznamy</h3>' }"
                     data-navigatable="false"
                     data-columns="[
                     { 'field': 'IDPrijemkaPol', 'template': '\\##=IDPrijemkaPol#', 'attributes': { class: 'text-center' }, 'title': 'Lepítko', 'width': 120 },
         { 'field': 'SkladovaZasoba', 'title': 'Skladová zásoba' },
         { 'field': 'OperativniZasoba', 'title': 'Operativní zásoba' },
         { 'field': 'DatumNaskladnil', 'format': '{0:d}', 'title': 'Naskladněno' },
         { 'field': 'NaskladnenoEMJ', 'title': 'Naskladněno' },
         { 'field': 'rr_DodavatelHodnota', 'title': 'Dodavatel' },
         { 'field': 'VSPrijmovehoDokladu', 'template': '#=linkPrijemka(VSPrijmovehoDokladu)#', 'title': 'Č. dokladu' },
         { 'field': 'CenaNakup', 'format': '{0:n2}', 'title': 'Cena nákup' },
         { 'field': 'ProdejniCenaPodleNakupu', 'format': '{0:n2}', 'title': 'Cena prodej' },
         { 'field': 'RezervovanoProFirmu', 'title': 'Rezervováno pro' },
         { 'template': '#=btnRezUvo(RezervovanoProFirmu, OperativniZasoba)#' }]"
                     data-bind="source: produkt_jednotlive" style="height: 500px;"></div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-danger" data-dismiss="modal"><span class="k-icon k-i-cancel-outline"></span> Zavřít</button>
            </div>
        </div>
    </div>
</div>

<div class="modal fade" id="modal_firmy_hledat">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">Hledat firmu</h4>
                <button type="button" class="close" data-dismiss="modal">&times;</button>
            </div>
            <div class="modal-body">
                <form class="form-inline my-2 my-lg-0" data-bind="events: { submit: hledatFirmu }">
                    <div class="input-group input-group-sm">
                        <input type="search" class="form-control" data-bind="value: hledatHodnotu" placeholder="Hledat..." aria-label="Hledat...">
                        <div class="input-group-append">
                            <button class="btn btn-secondary" type="submit"><span class="k-icon k-i-zoom"></span></button>
                        </div>
                    </div>
                </form>
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
         { 'field' : 'NazevPobocky' , 'title' : 'Pracoviště - vyhledávací název' , 'width' : 250 },
         { 'field' : 'rr_TypAdresy' , 'title' : 'Typ pobočky' , 'width' : 110 },
         { 'field' : 'Nazev_firmy' , 'title' : 'Firma - dle OR' },
         { 'field' : 'AdresaPracoviste' , 'title' : 'Adresa pracoviště' }]"
                     data-bind="source: AGsp_GetFirmaPracovisteSeznamHledej, events: { change: firma_select }" style="height: 500px;"></div>


                @*<div data-role="grid"
                    data-auto-bind="false"
                    data-filterable="true"
                    data-column-menu="true"
                    data-pageable="true"
                    data-editable="false"
                    data-sortable="true"
                    data-selectable="true"
                    data-scrollable="true"
                    data-resizable="true"
                    data-no-records="{ template: '<h3 style=\'text-align:center;margin-top:16px;width:100%;\'>Žádné záznamy</h3>' }"
                    data-navigatable="false"
                    data-columns="[
        { 'field': 'Nazev_firmy', 'title': 'Název firmy' },
        { 'field': 'ICO', 'title': 'IČO' },
        { 'field': 'DIC', 'title': 'DIČ' },
        { 'field': 'Ulice', 'title': 'Ulice' },
        { 'field': 'PSC', 'title': 'PSČ' },
        { 'field': 'Mesto', 'title': 'Město' }]"
                    data-bind="source: firmy, events: { change: firma_select }" style="height: 500px;"></div>*@
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-danger" data-dismiss="modal"><span class="k-icon k-i-cancel-outline"></span> Zavřít</button>
            </div>
        </div>
    </div>
</div>

<div class="modal fade" id="modal_rezerve">
    <div class="modal-dialog modal-sm">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">Rezervovat</h4>
                <button type="button" class="close" data-dismiss="modal">&times;</button>
            </div>
            <div class="modal-body">
                <div class="form-row">
                    <div class="form-group col-md-12">
                        <label class="text-muted">Rezervovat pro</label>
                        <input type="text" class="form-control" data-bind="value: model_rezerve_pro.NazevPobocky" readonly>
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group col-md-12">
                        <label class="text-muted">Množství</label>
                        <input required data-role="numerictextbox" data-format="n0" type="number" data-min="1" min="1" data-bind="value: model_rezerve_pro.EMJ" class="form-control">
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-success" data-bind="events: { click: modal_rezerve_save }"><span class="k-icon k-i-plus"></span> Rezervovat</button>
                <button type="button" class="btn btn-danger" data-dismiss="modal"><span class="k-icon k-i-cancel-outline"></span> Zavřít</button>
            </div>
        </div>
    </div>
</div>

<script id="tmpklient" type="text/html">
    <div>
        <div>#:NazevPobocky#</div>
        <small class="text-muted">#:AdresaPracoviste#</small>
    </div>
</script>

<script id="tmppracak" type="text/html">
    <div>
        <div>Pracák \\##:IDVykazPrace#, #:kendo.toString(new Date(data.DatVzniku), "dd.MM. yyyy HH:mm")#</div>
        <small class="text-muted">Založil: #:data.UserZalozil#, Upravil: #:data.UserUpravil#</small>
    </div>
</script>

<script>
    var produkt = '@Html.Raw(Request.QueryString("p"))';

    function btnzablokovat() {
        return '<button type="button" class="btn btn-primary btn-sm" data-toggle="modal" data-bind="events: { click: zablokovat }" data-toggle="modal" data-target="#modal_sklad"><span class="k-icon k-i-lock"></span> Zablokovat</button>';
    };

    function linkProdukt() {
        return `<a href="\#" class="text-primary" data-bind="text: Produkt, events: { click: show_produkt_jednotlive_naskladneno }"></a>`
    }

    function linkPrijemka(VSPrijmovehoDokladu) {
        return `<a href="@(Url.Action("Prijemky", "Home"))?vs=` + VSPrijmovehoDokladu + `" target="_blank" class="text-primary" data-bind="text: VSPrijmovehoDokladu"></a>`
    }

    function btnRezUvo(RezervovanoProFirmu, OperativniZasoba) {
        if (OperativniZasoba > 0) {
            if (RezervovanoProFirmu) {
                return `<div class="text-center"><a href="\#" class="text-primary" data-bind="events: { click: btn_uvolnit }">Uvolnit</a></div>`
            } else {
                return `<div class="text-center"><a href="\#" class="text-primary" data-bind="events: { click: btn_rezervovat }">Rezervovat</a></div>`
            }
        } else {
            return `<div class="text-center">Op. zásoba = 0</div>`
        }
    }

    $(function () {
        $("form").submit(function (e) {
            e.preventDefault();
        });

        $("#modal_sklad").on('shown.bs.modal', function () {
            viewModel.set("msg", { text: "", color: "red" });
            setDefault();
        });

        $("#modal_sklad").on('hidden.bs.modal', function () {
            viewModel.AGsp_GetProduktSeznamHledaci.read();
        });

        $("#modal_produkt_jednotlive").on('shown.bs.modal', function () {
            viewModel.produkt_jednotlive.read();
        });

        $("#modal_produkt_jednotlive").on('hidden.bs.modal', function () {
            //viewModel.AGsp_GetProduktSeznamHledaci.read();
        });

        $("#modal_firmy_hledat").on('shown.bs.modal', function () {
            viewModel.AGsp_GetFirmaPracovisteSeznamHledej.read();
        });

        $("input[name='Cena']").keyup(function (e) {
            var val = $(this).val().replace(",", ".");
            viewModel.detail.set("CenaNakup", parseFloat(val));
            viewModel.calculateCena();
        });

        $("input[name='CenaProdej']").keyup(function (e) {
            var val = $(this).val().replace(",", ".");
            viewModel.detail.set("CenaProdej", parseFloat(val));
            viewModel.calculateCena();
        });
        
        var rezervovat = false;
        function setDefault() {
            if (!rezervovat) {
                viewModel.set("detail", {});
                viewModel.set("naskladnovani", true);
                viewModel.set("rezervovani", false);
                viewModel.set("nalezeno", false);
                viewModel.set("nenalezeno", false);
                viewModel.rezervace.set("vybranaFirma", null);
            }
            $('input[name="Carovy_kod"]').focus();
            rezervovat = false;
        };

        var viewModel = kendo.observable({
            btn_uvolnit: function (e) {
                var that = this;
                $.get('@Url.Action("AGsp_Do_ProduktRezervace", "Api/Service")', { iDPrijemkaPol: e.data.IDPrijemkaPol, pocetEMJRezervovat: 0, rezervovat: false, firma_RezervovatProFirmu: "" }, function (result) {
                    if (result.error) {
                        alert(result.error);
                    } else {
                        that.produkt_jednotlive.read();
                    }
                })
            },
            btn_rezervovat: function (e) {
                this.set("model_rezerve_pro", {
                    IDPrijemkaPol: e.data.IDPrijemkaPol,
                    NazevPobocky: "",
                    Firma: "",
                    Nazev_firmy: "",
                    EMJ: e.data.OperativniZasoba
                });
                $("#modal_firmy_hledat").modal("show");
            },
            modal_rezerve_save: function (e) {
                var that = this;
                var model = this.get("model_rezerve_pro");
                $.get('@Url.Action("AGsp_Do_ProduktRezervace", "Api/Service")', { iDPrijemkaPol: model.IDPrijemkaPol, pocetEMJRezervovat: model.EMJ, rezervovat: true, firma_RezervovatProFirmu: model.Firma }, function (result) {
                    if (result.error) {
                        alert(result.error);
                    } else {
                        that.produkt_jednotlive.read();
                        $("#modal_rezerve").modal("hide");
                    }
                })
            },
            model_rezerve_pro: {
                IDPrijemkaPol: 0,
                NazevPobocky: "",
                Firma: "",
                Nazev_firmy: "",
                EMJ: 1
            },
            hledatHodnotu: "",
            hledatFirmu: function (e) {
                e.preventDefault();
                this.AGsp_GetFirmaPracovisteSeznamHledej.read();
            },
            AGsp_GetFirmaPracovisteSeznamHledej: new kendo.data.DataSource({
                schema: {
                    model: {
                        fields: {
                            ParametryValidni: { type: "number" },
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        var h = viewModel.get("hledatHodnotu");
                        $.get("@Url.Action("AGsp_GetFirmaPracovisteSeznamHledej", "Api/Service")", { hledej: h, hledatDleGPS: false, gPSLat: "", gPSLng: "" }, function (result) {
                            if (result.data) {
                                options.success(result.data);
                            }
                        });
                    },
                    parameterMap: function (options, operation) {
                        if (operation === "read") {
                            var pm = kendo.data.transports["odata-v4"].parameterMap(options);
                            return pm;
                        }
                    }
                },
                pageSize: 50
            }),
            firma_select: function (e) {
                var di = e.sender.dataItem(e.sender.select()).toJSON();
                this.model_rezerve_pro.set("NazevPobocky", di.NazevPobocky);
                this.model_rezerve_pro.set("Firma", di.Firma);
                this.model_rezerve_pro.set("Nazev_firmy", di.Nazev_firmy);
                $("#modal_firmy_hledat").modal("hide");
                $("#modal_rezerve").modal("show");
            },
            switchChange: function (e) {
                if (e.checked) {
                    this.AGsp_GetProduktSeznamHledaci.filter({ "field": "OperativniZasoba", "operator": "gte", "value": 1 })
                } else {
                    this.AGsp_GetProduktSeznamHledaci.filter({ })
                }
            },
            hledanyProdukt: produkt,
            hledatProdukt: function (e) {
                e.preventDefault();
                this.AGsp_GetProduktSeznamHledaci.read();
            },
            AGsp_GetProduktSeznamHledaci: new kendo.data.DataSource({
                schema: {
                    model: {
                        fields: {
                            NaposledyNaskladneno: { type: "date" }
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        var h = viewModel.get("hledanyProdukt");
                        $.get("@Url.Action("AGsp_GetProduktSeznamHledaci", "Api/Service")", { hledej: h }, function (result) {
                            options.success(result.data);
                        });
                    },
                    parameterMap: function (options, operation) {
                        if (operation === "read") {
                            var pm = kendo.data.transports["odata-v4"].parameterMap(options);
                            return pm;
                        }
                    }
                }
            }),
            clearFilter: function (e) {
                this.set("hledanyProdukt", "");
                this.AGsp_GetProduktSeznamHledaci.filter({});
                this.AGsp_GetProduktSeznamHledaci.read();
            },
            AGsp_GetProduktSeznamHledaci_change: function (e) {
                var di = e.sender.dataItem(e.sender.select());
                produkt = di.Produkt;
            },
            AGsp_GetProduktSeznamHledaci_dataBound: function (e) {
                var that = this;
                var row = null;
                var rows = e.sender.tbody.find("tr");
                rows.each(function () {
                    var di = e.sender.dataItem($(this));
                    $(this).attr("data-produkt", di.Produkt);
                })
                if (produkt) {   
                    row = e.sender.tbody.find('tr[data-produkt="' + produkt + '"]');
                    e.sender.select(row);
                } else {
                    row = e.sender.tbody.find('tr:first');
                    e.sender.select(row);
                }
            },
            zablokovat: function (e) {
                var d = e.data;
                rezervovat = true;
                this.set("detail", d);
                this.rezervovat();
            },
            calculateCena: function (e) {
                var cn = this.detail.get("CenaNakup"),
                    cp = this.detail.get("CenaProdej");
                if (cn > 0 && cp > 0) {
                    var v1 = parseFloat((cp / cn) - 1);
                    var v2 = parseFloat((v1 * 100));
                    this.set("navyseni", parseFloat(v2).toFixed(0) + " %");
                } else {
                    this.set("navyseni", "0 %");
                }
            },
            navyseni: "0 %",
            detail: {
                Produkt: "",
                VSPrijmovehoDokladu: null,
                CenaNakup: 0,
                CenaProdej: 0
            },
            produkt_jednotlive: new kendo.data.DataSource({
                schema: {
                    data: "data",
                    model: {
                        fields: {
                            DatumNaskladnil: { type: "date", editable: false },
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
                        url: "@Url.Action("AGsp_GetProduktJednotliveNaskladneno", "Api/Service")",
                        type: "GET"
                    },
                    parameterMap: function (options, operation) {
                        var produkt = viewModel.detail.get("Produkt");
                        return { produkt: produkt }
                    }
                }
            }),
            show_produkt_jednotlive_naskladneno: function (e) {
                var grid = $(e.currentTarget).closest(".k-grid").data("kendoGrid");
                var row = $(e.currentTarget).closest("tr");
                grid.select(row);

                this.detail.set("Produkt", e.data.Produkt);
                $('#modal_produkt_jednotlive').modal("show");
            },
            posledni_vs: function (e) {
                var vs = localStorage["vs"];
                if (vs) {
                    this.detail.set("VSPrijmovehoDokladu", vs)
                }
            },
            vs_change: function (e) {
                var vs = this.detail.get("VSPrijmovehoDokladu");
                if (vs) {
                    localStorage["vs"] = vs;
                }
            },
            msg: {
                color: "red",
                text: ""
            },
            naskladnovani: true,
            rezervovani: false,
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
            hledat_barcode: function (e) {
                var val = this.detail.get("Carovy_kod");
                if (val) {
                    if (e.keyCode === 13) {
                        this.hledat({
                            produkt: null,
                            barcode: val
                        });
                    }
                }
            },
            hledat_produkt_key: function (e) {
                var val = this.detail.get("Produkt");
                if (val) {
                    if (e.keyCode === 13) {
                        this.hledat({
                            produkt: val,
                            barcode: null
                        });
                    }
                }
            },
            hledat_produkt: function (e) {
                var val = e.sender.value();
                if (val) {
                    this.hledat({
                        produkt: val,
                        barcode: null
                    });
                }
            },
            produkty: new kendo.data.DataSource({
                schema: { data: "data" },
                transport: { read: "@Url.Action("ProduktSeznam_CBX", "Api/Service")" }
            }),
            hledat: function (params) {
                var that = this;
                //*****************************************
                //*HLEDAT PRODUKT PODLE NAZVU NEBO BARCODE*
                //*****************************************
                $.get("@Url.Action("AGsp_GetProduktDetail", "Api/Service")", params, function (result) {
                    that.set("naskladnovani", true);
                    that.set("rezervovani", false);
                    var dt = that.get("detail");
                    var d = {
                        Carovy_kod: (dt.Carovy_kod ? dt.Carovy_kod : ""),
                        Produkt: (dt.Produkt ? dt.Produkt : ""),
                        Popis: "",
                        Jednotky: "KS",
                        Cena: 0,
                        CenaNakup: 0,
                        CenaProdej: 0,
                        Mnozstvi_minimalni: 0,
                        Internet: "",
                        Dodavatel: "",
                        Poznamka: ""
                    };
                    that.set("msg", { text: "", color: "red" });
                    if (result.data.length > 0) {
                        //PRODUKT NALEZEN
                        d = result.data[0];
                        if (!d.Popis) {
                            d.Popis = (d.Produkt ? d.Produkt : "");
                        }
                        d.NaskladnenoEMJ = 1;
                        that.set("msg", { text: "", color: "green" });
                        that.set("detail", d);
                        that.set("nalezeno", true);
                        that.set("nenalezeno", false);
                        $('input[name="Cena"]').focus();
                    } else {
                        //PRODUKT NENALEZEN
                        that.set("msg", { text: "Produkt ještě neznáme, je to zřejmě novinka, založte ho", color: "red" });
                        that.set("detail", d);
                        that.set("nalezeno", false);
                        that.set("nenalezeno", true);
                        $('input[name="Produkt"]').focus();
                    }
                });
            },
            rezervovat: function (e) {
                var that = this;
                if (!rezervovat) {
                    var form = $(e.currentTarget).closest("form");
                    var valid = $(form).valid();
                    if (!valid) {
                        return false;
                    };
                }
                that.set("naskladnovani", false);
                that.set("rezervovani", true);
            },
            nalezeno: false,
            naskladnit: function (e) {
                var that = this;
                var form = $(e.currentTarget).closest("form");
                var valid = $(form).valid();
                if (!valid) {
                    return false;
                };
                var d = this.get("detail");
                d["IDMega"] = 0;
                //********************
                //*NASKLADNIT PRODUKT*
                //********************
                if (d.Produkt.length > 0) {
                    $.post("@Url.Action("AGsp_AddProduktNaskladnit", "Api/Service")", d.toJSON(), function (r) {
                        if (r.error) {
                            alert(r.error);
                        } else {
                            that.set("msg", { text: "Produkt " + d.Produkt + " naskladněn s lepítkem #" + r.data, color: "green" });
                            setDefault();
                        }
                    });
                } else {
                    that.set("msg", { text: "Vyplňte pole Produkt", color: "red" });
                }
            },
            nenalezeno: false,
            pridat: function (e) {
                var that = this;
                var form = $(e.currentTarget).closest("form");
                var valid = $(form).valid();
                if (!valid) {
                    return false;
                };
                var d = this.get("detail");
                //****************
                //*PRIDAT PRODUKT*
                //****************
                if (d.Produkt.length > 0) {
                    $.post("@Url.Action("AGsp_AddProduktZbozi", "Api/Service")", d.toJSON(), function (r) {
                        if (r.error) {
                            alert(r.error);
                        } else {
                            that.set("msg", { text: "", color: "" });
                            that.set("nalezeno", true);
                            that.set("nenalezeno", false);
                        }
                    });
                } else {
                    alert("Vyplňte pole Produkt");
                }
            },
            rezervace: {
                pocet: 1,
                cena: 0,
                vybranaFirma: null,
                klienti: new kendo.data.DataSource({
                    transport: {
                        read: function (options) {
                            var f = viewModel.rezervace.klienti.filter();
                            var h = "";
                            if (f) {
                                h = f.filters[0].value
                            }
                            $.get("@Url.Action("AGsp_GetFirmaPracovisteSeznamHledej", "Api/Service")", { hledej: h, hledatDleGPS: false, gPSLat: "", gPSLng: "" }, function (result) {
                                options.success(result.data);
                            });
                        }
                    },
                    serverPaging: true,
                    serverSorting: true,
                    serverFiltering: true,
                }),
                pracaky: new kendo.data.DataSource({
                    data: []
                }),
                hledat_klienta: function (e) {
                    var sarchString = $(e.currentTarget).val();
                    var filter = {
                        field: "Firma",
                        operator: "contains",
                        value: sarchString
                    };
                    viewModel.rezervace.klienti.filter(filter);
                    viewModel.rezervace.pracaky.data([]);
                },
                hledat_pracak: function (e) {
                    var sarchString = $(e.currentTarget).val();
                    var filters = {};
                    viewModel.rezervace.klienti.filter(filters);
                },
                select_klient: function (e) {
                    var element = e.sender.select();
                    var dataItem = e.sender.dataItem(element[0]);

                    var pobocka = dataItem.Pobocka;
                    viewModel.rezervace.set("vybranaFirma", pobocka)

                    var pm = {
                        "$inlinecount": "allpages",
                        "$format": "json",
                        "$filter": "IDPracoviste eq '" + pobocka + "'",
                        "stav": 0
                    }
                    $.get("@Url.Action("AGvw_FA_PracakySeznam", "Api/Service")", pm, function (result) {
                        viewModel.rezervace.pracaky.data(result.Data);
                    })
                },
                select_pracak: function (e) {
                    var element = e.sender.select();
                    var dataItem = e.sender.dataItem(element[0]);
                    var iDVykazPrace = dataItem.IDVykazPrace;
                    var firma = dataItem.PobockaNazev;
                    var pocet = viewModel.rezervace.pocet;
                    var cena = viewModel.rezervace.cena;
                    var d = viewModel.get("detail");
                    d["IDMega"] = 0;
                    if (parseFloat(cena) < parseFloat(d.PrumernaNakup)) {
                        var cnf1 = confirm("Cena prodejní je menší než nákupní. Je to v pořádku?");
                        if (!cnf1) {
                            return false;
                        }
                    }

                    var cnf = confirm("Rezervovat " + pocet + " KS pro " + firma + "?");
                    if (cnf) {
                        //var d = viewModel.get("detail");
                        //************
                        //*NASKALDNIT*
                        //************
                        $.post("@Url.Action("AGsp_AddProduktNaskladnit", "Api/Service")", d.toJSON(), function (r) {
                            if (r.error) {
                                alert(r.error);
                            } else {
                                //************
                                //*A BLOKOVAT*
                                //************

                                //iDVykazPrace As Integer,
                                //produkt As String,
                                //blokovatEMJ As Decimal,
                                //cenaEMJProdejni As Decimal,
                                //iDUserUpravil As Integer

                                $.get("@Url.Action("AGsp_AddNewPracakyPolozkaProduktZablokovat", "Api/Service")", { iDVykazPrace: iDVykazPrace, produkt: d.Produkt, blokovatEMJ: pocet, cenaEMJProdejni: cena, iDUserUpravil: 0, iDPrijemkaPol: 0 }, function (result) {
                                    if (result.error) {
                                        alert(result.error);
                                    } else {
                                        setDefault();
                                    }
                                })
                            }
                        });
                    }
                },
                novy: function (e) {
                    //***************************************************
                    //*NOVY PRACAK (VRATIT PRACAK), NASKLADNIT, BLOKOVAT*
                    //***************************************************
                    var firma = viewModel.rezervace.vybranaFirma;
                    var pocet = viewModel.rezervace.pocet;
                    var cena = viewModel.rezervace.cena;
                    var d = viewModel.get("detail");
                    $.post("@Url.Action("AGsp_AddProduktNaskladnit", "Api/Service")", d.toJSON(), function (r) {
                        if (r.error) {
                            alert(r.error);
                        } else {
                            if (firma) {
                                $.get("@Url.Action("AGsp_AddNewVykazPrace", "Api/Service")", { firma: firma, iDUser: 0 }, function (result) {
                                    if (r.error) {
                                        alert(r.error);
                                    } else {
                                        var iDVykazPrace = result.data;
                                        $.get("@Url.Action("AGsp_AddNewPracakyPolozkaProduktZablokovat", "Api/Service")", { iDVykazPrace: iDVykazPrace, produkt: d.Produkt, blokovatEMJ: pocet, cenaEMJProdejni: cena, iDUserUpravil: 0, iDPrijemkaPol: 0 }, function (result) {
                                            if (r.error) {
                                                alert(r.error);
                                            } else {
                                                setDefault();
                                            }
                                        })
                                    }
                                });
                            } else {
                                alert("Vyber klienta")
                            }
                        }
                    })
                },
                zpet: function () {
                    var that = viewModel;
                    that.set("naskladnovani", true);
                    that.set("rezervovani", false);
                }
            }
        })
        var original = viewModel.toJSON();
        kendo.bind(document.body, viewModel);
    })
</script>
