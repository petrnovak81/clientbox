@Code
    ViewData("Title") = "Klienti"
    Layout = "~/Views/Shared/_Layout.vbhtml"

    Dim db As New Data4995Entities
    Dim _user = db.AG_tblUsers.FirstOrDefault(Function(e) e.UserLogin = User.Identity.Name)
End Code

<div id="main" style="height: calc(100% - 54px);">
    <nav class="navbar navbar-expand-sm bg-dark navbar-dark">
        <form class="form-inline my-2 my-lg-0" data-bind="events: { submit: hledat }">
            <div class="input-group input-group-sm">
                <input type="search" class="form-control" data-bind="value: hledatHodnotu" placeholder="Hledat..." aria-label="Hledat...">
                <div class="input-group-append">
                    <button class="btn btn-secondary" type="submit"><span class="k-icon k-i-zoom"></span></button>
                </div>
            </div>
        </form>
        <button class="btn btn-sm" style="margin-left:6px;" data-bind="events: { click: clearFilter }">
            <span class="k-icon k-i-filter-clear"></span>
        </button>
        <button type="button" style="margin-left:6px;" class="btn btn-success btn-sm btn-add" data-bind="events: { click: klientAdd }"><span class="k-icon k-i-plus"></span> Nový klient</button>
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
         { 'template' : '#=btnDetail()#' , 'title' : ' ' , 'width' : 80 },
         { 'template' : '#=btnTiket()#' , 'title' : ' ' , 'width' : 80 },
         { 'template' : '#=btnHistory()#' , 'title' : ' ' , 'width' : 80 },
         { 'field' : 'NazevPobocky' , 'title' : 'Pracoviště - vyhledávací název' , 'width' : 250 },
         { 'field' : 'rr_TypAdresy' , 'title' : 'Typ pobočky' , 'width' : 110 },
         { 'field' : 'Nazev_firmy' , 'title' : 'Firma - dle OR' },
         { 'field' : 'AdresaPracoviste' , 'title' : 'Adresa pracoviště' },
         { 'field' : 'GPSValid' , 'template': '#=tmpTrueFalse(GPSValid)#', 'title': 'GPS validní', 'width' : 32 },
         { 'field' : 'ParametryValidni' , 'template': '#=tmpTrueFalse(ParametryValidni)#', 'title': 'Parametry zkontrolovány', 'width' : 32 }]"
         data-bind="source: AGsp_GetFirmaPracovisteSeznamHledej, events: { change: select, dataBound: seznamDataBound }" style="height: 100%;"></div>
</div>

<div class="modal fade" id="modal_detail_klient">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <nav class="navbar fixed-top bg-secondary">
                    <div class="d-flex flex-row" style="width:calc(100% - 60px);">
                        <button type="button" class="btn btn-primary btn-sm" data-bind="events: { click: novyPracak }"><span class="k-icon k-i-plus"></span> Nový pracák</button>
                        <button type="button" class="ml-1 btn btn-primary btn-sm" data-bind="events: { click: pracaky }"><span class="k-icon k-i-zoom"></span> Pracáky</button>
                        <button type="button" class="ml-1 btn btn-primary btn-sm" data-bind="events: { click: klientEdit }"><span class="k-icon k-i-edit"></span> Klient</button>
                        <button type="button" class="ml-1 btn btn-primary btn-sm" data-bind="events: { click: rezervovat }">Rezervované zboží</button>
                        <button type="button" class="ml-1 btn btn-secondary btn-sm" data-bind="events: { click: previous }" title="Předchozí záznam"><span class="k-icon k-i-arrow-60-up"></span></button>
                        <button type="button" class="ml-1 btn btn-secondary btn-sm" data-bind="events: { click: next }" title="Následující záznam"><span class="k-icon k-i-arrow-60-down"></span></button>
                        <div class="text-white" style="margin-left:6px;width:400px;">
                            <span>Parametry validní:</span>
                            <input data-role="switch"
                                   data-messages="{
                            checked: '✔',
                            unchecked: '✖',
                            }" data-bind="checked: selectedItem.ParametryValidni, events: { change: parametryValidniChange }" />
                            @*<input data-role="kendo.mobile.ui.Switch" data-on-label="Ano" data-off-label="Ne" data-bind="checked: selectedItem.ParametryValidni, events: { change: parametryValidniChange }" />*@
                        </div>
                        <div class="text-white" style="margin-right:1%;width:400px;">
                            <span>Nezobrazovat v seznamu firem:</span>
                            <input data-role="switch"
                            data-messages="{
                            checked: '✔',
                            unchecked: '✖',
                            }"
                            data-bind="checked: vypnout, events: { change: vypnoutChange }" />
                            @*<input data-role="kendo.mobile.ui.Switch" data-on-label="Ano" data-off-label="Ne" data-bind="checked: vypnout, events: { change: vypnoutChange }" />*@
                        </div>
                    </div>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </nav>
            </div>
            <div class="modal-body">
                <div class="row">
                    <div class="col">
                        <div class="form-group">
                            <label class="text-muted">Detail:</label><br />
                            <i data-bind="text: detail.Detail1"></i><br />
                            <i data-bind="text: detail.Detail2"></i><br />
                            <i data-bind="text: detail.Detail3"></i><br />
                            <i data-bind="text: detail.Detail4"></i>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col">
                        <label class="text-muted">Kontakty:</label>
                        <div class="hr-2"></div>
                        <div data-role="grid"
                             data-filterable="false"
                             data-pageable="false"
                             data-editable="inline"
                             data-sortable="false"
                             data-selectable="false"
                             data-scrollable="false"
                             data-resizable="false"
                             data-toolbar="['create']"
                             data-no-records="{ template: '<h3 style=\'text-align:center;margin-top:16px;\'>Žádné položky</h3>' }"
                             data-navigatable="false"
                             data-columns="[
            { 'field': 'Typ_KU', 'type': 'foreign', 'template': typ_KU, 'editor': typykontaktnichUdaju, 'title': 'Typ', 'width': 120 },
            { 'field': 'Nazev_KU', 'title': 'Název' },
            { 'field': 'Hodnota_KU', 'title': 'Hodnota' },
            { command: ['edit'], title: '&nbsp;', 'width': 120}]"
                             data-bind="source: AGsp_GetFirmaPracovisteDetailKontakty, events: { dataBound: AGsp_GetFirmaPracovisteDetailKontakty_DataBound } }">
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col">
                        <label class="text-muted">Inventář:</label>
                        <div class="hr-2"></div>
                        <div data-role="grid"
                             data-filterable="false"
                             data-pageable="false"
                             data-editable="inline"
                             data-sortable="false"
                             data-selectable="false"
                             data-scrollable="true"
                             data-resizable="true"
                             data-toolbar="['create']"
                             data-no-records="{ template: '<h3 style=\'text-align:center;margin-top:16px;\'>Žádné položky</h3>' }"
                             data-navigatable="false"
                             data-columns="[
            { 'field': 'InventarProdukt', 'title': 'Produkt' },
            { 'field': 'DatumNaposledyZakoupeno', 'title': 'Naposledy zakoupeno', 'format': '{0:d}' },
            { 'field': 'InventarPopis', 'title': 'Popis' },
            { 'template': btnObjednat, title:' ', width: 100 },
            { 'command': ['destroy', 'edit'], title:'&nbsp;', width: 120 }]"
                             data-bind="source: inventar, events: { dataBound: inventarDataBound } }">
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col">
                        <label class="text-muted">Parametry firmy:</label>
                        <div class="hr-2"></div>
                        <div data-role="grid"
                             data-filterable="false"
                             data-pageable="false"
                             data-editable="inline"
                             data-sortable="false"
                             data-selectable="false"
                             data-scrollable="true"
                             data-resizable="true"
                             data-toolbar='[{ name: "custom", text: "Přidat nový záznam", iconClass: "k-icon k-i-plus" }]'
                             data-no-records="{ template: '<h3 style=\'text-align:center;margin-top:16px;\'>Žádné položky</h3>' }"
                             data-navigatable="false"
                             data-columns="[
                 { 'field': 'PlatnostOd', 'format': '{0:d}', 'title': 'Platí od' },
            { 'field': 'HodinovaSazba', 'format': '{0:0 \Kč}', 'title': 'Sazba/hod' },
            { 'field': 'SazbaKm', 'title': 'Jízdné/km' },
            { 'field': 'rr_TypServisniSmlouvy', 'template': rr_TypServisniSmlouvy, 'editor': rr_TypServisniSmlouvyEdit, 'title': 'Typ servisní smlouvy' },
            { 'field': 'ServiskaCenaMesicne', 'format': '{0:0 \Kč}', 'title': 'Serviska cena/měs.' },
            { 'field': 'ServiskaNaposledyVyuctovana', 'format': '{0:d}', 'title': 'Serviska naposledy vyúčtována' },
            { 'field': 'ServiskaIntervalObnoveni', 'title': 'Interval účtování servisky' },
            { 'field': 'ServiskaVolneHodiny', 'title': 'Volné hodiny servisky' },
                 { 'field': 'SazbaMalyZasah', 'title': 'Sazba malý zásah' },
                 { 'field': 'SazbaVelkyZasah', 'title': 'Sazba velký zásah' },
                 { 'field': 'UserLastName', 'title': 'Upravil' },
                 { command: ['edit'], title: '&nbsp;', 'width': 100}]"
                             data-bind="source: parametry, events: { dataBound: parametryDataBound } }">
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

<div class="modal fade" id="modal_pracak">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">Nový pracák</h4>
                <button type="button" class="close" data-dismiss="modal">&times;</button>
            </div>
            <div class="modal-body">
                <div class="form-row">
                    <div class="form-group col-md-12">
                        <label class="text-muted">Datum založení</label>
                        <input data-role="datepicker" type="date" data-bind="value: pracak.DatVzniku" placeholder="Datum založení" class="form-control" style="width:100%;">
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group col-md-12">
                        <label class="text-muted">Poznámka</label>
                        <textarea type="text" rows="3" data-bind="value: pracak.Poznamka" placeholder="Poznámka" class="form-control" style="width:100%;"></textarea>
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group col-md-12">
                        <label class="text-muted">Stav pracáku</label>
                        <select data-role="dropdownlist" data-value-primitive="true" data-value-field="value" required data-text-field="text" data-bind="source: stavy, value: pracak.rr_StavPracaku" class="form-control" style="width:100%;"></select>
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group col-md-12">
                        <label class="text-muted">Vyúčtováno fakturou</label>
                        <input type="text" data-bind="value: pracak.CisloFaktury" placeholder="Vyúčtováno fakturou" class="form-control" style="width:100%;">
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-success" data-bind="events: { click: pracakSave }"><span class="k-icon k-i-save"></span> Uložit</button>
                <button type="button" class="btn btn-default" data-dismiss="modal"><span class="k-icon k-i-cancel-outline"></span> Zavřít</button>
            </div>
        </div>
    </div>
</div>

<div class="modal fade" id="modal_inventar">
    <div class="modal-dialog">
        <form class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">Inventář</h4>
                <button type="button" class="close" data-dismiss="modal">&times;</button>
            </div>
            <div class="modal-body">
                <div class="form-row">
                    <div class="form-group col-md-12">
                        <a class="btn btn-outline-primary" style="width:100%;" data-bind="events: { click: dejProdukt }">
                            Produkt
                            <input type="text" readonly required data-bind="value: polozka.InventarProdukt" class="form-control" />
                        </a>
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group col-md-12">
                        <label class="text-muted">Popis</label>
                        <textarea type="text" data-bind="value: polozka.InventarPopis" class="form-control"></textarea>
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group col-md-12">
                        <label class="text-muted">Zakoupeno</label>
                        <input data-role="datepicker" type="date" data-bind="value: polozka.DatumNaposledyZakoupeno" class="form-control">
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group col-md-12">
                        <label class="text-muted">Hlídat datum expirace</label>
                        <input type="checkbox" data-role="switch" data-bind="checked: datExp" class="form-control" />
                    </div>
                </div>
                <div class="form-row" data-bind="visible: datExp">
                    <div class="form-group col-md-12">
                        <label class="text-muted">Datum expirace</label>
                        <input data-role="datepicker" type="date" data-bind="value: polozka.DatumExpirace" class="form-control">
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <button type="submit" class="btn btn-success" data-bind="events: { click: inventarPolSave }"><span class="k-icon k-i-save"></span> Uložit</button>
                <button type="button" class="btn btn-default" data-dismiss="modal"><span class="k-icon k-i-cancel-outline"></span> Zavřít</button>
            </div>
        </form>
    </div>
</div>

<div class="modal fade" id="modal_detail">
    <div class="modal-dialog modal-lg">
        <form class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">Klient edit</h4>
                <button type="button" class="close" data-dismiss="modal">&times;</button>
            </div>
            <div class="modal-body">
                <div class="form-row">
                    <div class="form-group col-md-4">
                        <label class="text-muted">Firma</label>
                        <input type="text" data-bind="value: detail.Firma" class="form-control" readonly>
                    </div>
                    <div class="form-group col-md-4">
                        <label class="text-muted">Vyhledávací název pobočky firmy</label>
                        <input type="text" data-bind="value: detail.Nazev_firmy" class="form-control" required>
                    </div>
                    <div class="form-group col-md-4">
                        <label class="text-muted">IČZ</label>
                        <input type="text" maxlength="100" data-bind="value: detail.ICZ" class="form-control" required>
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group col-md-6">
                        <label class="text-muted">Vzdálenost</label>
                        <input data-role="numerictextbox" data-format="0.0 \Km" data-min="0" type="number" data-bind="value: detail.Vzdalenost" class="form-control">
                    </div>
                    <div class="form-group col-md-6">
                        <label class="text-muted">Obor činnosti</label>
                        <input type="text" data-bind="value: detail.Obor_cinnosti" class="form-control">
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group col-md-12">
                        <label class="text-muted">Poznámky</label>
                        <textarea rows="3" data-bind="value: detail.Poznamky" class="form-control"></textarea>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <button type="submit" class="btn btn-success" data-bind="events: { click: klientSave }"><span class="k-icon k-i-save"></span> Uložit</button>
                <button type="button" class="btn btn-default" data-dismiss="modal"><span class="k-icon k-i-cancel-outline"></span> Zavřít</button>
            </div>
        </form>
    </div>
</div>

<div class="modal fade" id="modal_produkty">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">Produkty</h4>
                <button type="button" class="close" data-dismiss="modal">&times;</button>
            </div>
            <div class="modal-body">
                <div data-role="grid"
                     data-auto-bind="false"
                     data-filterable="true"
                     data-pageable="true"
                     data-editable="false"
                     data-sortable="true"
                     data-selectable="true"
                     data-scrollable="true"
                     data-resizable="true"
                     data-column-menu="true"
                     data-no-records="{ template: '<h3 style=\'text-align:center;margin-top:16px;\'>Žádné položky</h3>' }"
                     data-bind="source: produkty, events: { change: produktySelect }"></div>
                @*<select data-auto-bind="false" data-role="listbox" data-text-field="Produkt" data-value-field="Produkt" data-bind="source: produkty, events: { change: produktySelect }" class="form-control"></select>*@
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal"><span class="k-icon k-i-cancel-outline"></span> Zavřít</button>
            </div>
        </div>
    </div>
</div>

<div class="modal fade" id="klientAddModal">
    <div class="modal-dialog modal-lg">
        <form class="modal-content" data-bind="events: { submit: klientAddSave }">
            <div class="modal-header">
                <h4 class="modal-title" id="exampleModalLabel">Nová firma pro účetnictví</h4>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <div class="form-row">
                    <div class="form-group col-md-4">
                        <label class="text-muted required" for="ico">
                            IČO
                        </label>
                        <div class="input-group">
                            <div class="input-group-append">
                                <button class="btn btn-secondary" type="button" data-bind="events: { click: ares }">Ares</button>
                            </div>
                            <input class="form-control" type="text" required maxlength="20" id="ico" data-bind="value: newKlient.ICO" />
                        </div>
                    </div>
                    <div class="form-group col-md-4">
                        <label class="text-muted" for="icz">
                            IČZ
                        </label>
                        <input class="form-control" type="text" required maxlength="100" id="icz" data-bind="value: newKlient.ICZ" />
                    </div>
                    <div class="form-group col-md-4">
                        <label class="text-muted" for="nazev">
                            Název firmy dle obchodního rejstříku
                        </label>
                        <input class="form-control" type="text" maxlength="255" id="nazev" data-bind="value: newKlient.Nazev_firmy" />
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group col-md-6">
                        <label class="text-muted" for="email">
                            Email, na který mají chodit faktury
                        </label>
                        <input class="form-control" type="email" required maxlength="40" id="email" data-bind="value: newKlient.E_mail" />
                    </div>
                    <div class="form-group col-md-6">
                        <label class="text-muted" for="firma">
                            Unikátní index firmy (použij název firmy)
                        </label>
                        <input class="form-control" type="text" required maxlength="30" id="firma" data-bind="value: newKlient.Firma, events: { change: firmaExist }" />
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group col-md-6">
                        <label class="text-muted" for="phone1">
                            Telefon 1
                        </label>
                        <input class="form-control" type="tel" required maxlength="40" id="phone1" data-bind="value: newKlient.Telefon_1" />
                    </div>
                    <div class="form-group col-md-6">
                        <label class="text-muted" for="phone2">
                            Telefon 2
                        </label>
                        <input class="form-control" type="tel" maxlength="40" id="phone2" data-bind="value: newKlient.Telefon_2" />
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group col-md-6">
                        <label class="text-muted" for="dic">
                            DIČ
                        </label>
                        <input class="form-control" type="text" maxlength="20" id="dic" data-bind="value: newKlient.DIC" />
                    </div>
                    <div class="form-group col-md-6">
                        <label class="text-muted" for="obor">
                            Obor  činnosti
                        </label>
                        <input class="form-control" type="text" required maxlength="30" id="obor" data-bind="value: newKlient.Obor_cinnosti" />
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group col-md-6">
                        <label class="text-muted" for="poznamky">
                            Poznámky
                        </label>
                        <textarea class="form-control" type="text" rows="7" id="poznamky" data-bind="value: newKlient.Poznamky"></textarea>
                    </div>
                    <div class="form-group col-md-6">
                        <label class="text-muted" for="prijmeni">
                            Příjmení odpovědné osoby
                        </label>
                        <input class="form-control" type="text" required maxlength="30" id="prijmeni" data-bind="value: newKlient.Prijmeni" />
                        <label class="text-muted" for="krestni">
                            Jméno
                        </label>
                        <input class="form-control" type="text" maxlength="30" id="krestni" data-bind="value: newKlient.Krestni" />
                        <label class="text-muted" for="titul">
                            Titul
                        </label>
                        <input class="form-control" type="text" maxlength="20" id="titul" data-bind="value: newKlient.Titul" />
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <small class="text-muted">Po založení nového klienta do účetnictví se automaticky vytvoří nové centrální pracoviště s identiskými údaji. Další pracoviště klienta založte v mobilu.</small>
                <button type="submit" class="btn btn-success"><span class="k-icon k-i-save"></span> Uložit</button>
                <button type="button" class="btn btn-default" data-dismiss="modal"><span class="k-icon k-i-cancel-outline"></span> Zavřít</button>
            </div>
        </form>
    </div>
</div>

<div class="modal fade" id="modal_history">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">Historie</h4>
                <button type="button" class="close" data-dismiss="modal">&times;</button>
            </div>
            <div class="modal-body">
                <input class="k-textbox" style="width:100%;margin-bottom:6px;" data-bind="value: historysource.RecordCommentType" placeholder="Předmět" />
                <textarea class="k-textbox" style="width:100%;height:100px;margin-bottom:6px;" data-bind="value: historysource.RecordCommentTxt" placeholder="Komentář"></textarea>
                <div style="margin-bottom:6px;">
                    <button type="button" class="btn btn-default" data-bind="enabled: btnhistoryenabled, events: { click: historyadd }"><span class="k-icon k-i-plus"></span> Přidat</button>
                    <button type="button" class="btn btn-default" data-bind="enabled: btnhistoryenabled, events: { click: historysave }"><span class="k-icon k-i-save"></span> Uložit</button>
                </div>
                <div data-role="grid"
                     data-auto-bind="false"
                     data-filterable="false"
                     data-pageable="false"
                     data-editable="false"
                     data-sortable="false"
                     data-selectable="true"
                     data-scrollable="true"
                     data-resizable="true"
                     data-no-records="{ template: '<h3 style=\'text-align:center;margin-top:16px;\'>Žádné položky</h3>' }"
                     data-columns="[
            { 'field': 'RecordDate', 'format': '{0:d}', 'title': 'Datum' },
            { 'field': 'RecordCommentType', 'title': 'Typ' },
            { 'field': 'RecordCommentTxt', 'title': 'Komentář' }]"
                     data-bind="source: history, events: { change: historychange } }" style="height:400px;">
                </div>
            </div>
            <div class="modal-footer">
                <button type="button" class="btn btn-default" data-dismiss="modal"><span class="k-icon k-i-cancel-outline"></span> Zavřít</button>
            </div>
        </div>
    </div>
</div>

<script>
    var firma = '@Html.Raw(Request.QueryString("f"))';

    function btnDetail(e) {
        return '<button class="btn btn-info btn-sm" data-bind="events: { click: btndetail }"><span class="k-icon k-i-zoom"></span> Detail</button>';
    };

    function btnTiket(e) {
        return '<button class="btn btn-success btn-sm" data-bind="events: { click: btntiket }"><span class="k-icon k-i-plus"></span> Tiket</button>';
    }

    function btnHistory(e) {
        return '<button class="btn btn-sm" data-bind="events: { click: btnhistory }"><span class="k-icon k-i-list-unordered"></span> Historie</button>';
    };

    function tmpTrueFalse(state) {
        if (state) {
            return '<div style="color:#28a745;text-align:center;"><span class="k-icon k-i-check-circle"></span></div>'
        } else {
            return '<div style="color:#dc3545;text-align:center;"><span class="k-icon k-i-close-circle"></span></div>'
        }
    }

    function rr_TypServisniSmlouvyEdit(container, options) {
        $('<input required name="' + options.field + '"/>')
            .appendTo(container)
            .kendoDropDownList({
                autoBind: false,
                dataTextField: "text",
                dataValueField: "value",
                dataSource: {
                    schema: { data: "data" },
                    transport: { read: "@Url.Action("RegRest", "Api/Service")?Register=rr_TypServisniSmlouvy" }
                }
            });
    };

    function typykontaktnichUdaju(container, options) {
        $('<input required name="' + options.field + '"/>')
            .appendTo(container)
            .kendoDropDownList({
                autoBind: true,
                dataTextField: "text",
                dataValueField: "value",
                dataSource: {
                    schema: {
                        data: "data"
                    },
                    transport: {
                        read: "@Url.Action("AGvwrr_TypykontaktnichUdaju", "Api/Service")"
                    }
                }
            });
    };

    function rr_TypServisniSmlouvy() {
        return '<input data-role="dropdownlist" data-text-field="text" data-value-field="value" data-bind="source: typyServSmlouvy, value: rr_TypServisniSmlouvy" />';
    }

    function btnObjednat(data) {
        return '<button type="button" class="btn btn-primary" data-bind="events: { click: inventarObjednat }"><span class="k-icon k-i-cart"></span> Objednat</button>';
    }

    function typ_KU() {
        return '<input data-role="dropdownlist" data-text-field="text" data-value-field="value" data-bind="source: typyKontaktu, value: Typ_KU" />';
    }

    $(function () {

        $("form").submit(function (e) {
            e.preventDefault();
        });

        $('#modal_pracak').on('shown.bs.modal', function () {
            var that = viewModel;
            $.get('@Url.Action("AGsp_GetVykazPraceDetail", "Api/Service")', { iDVykazPrace: that.iDVykazPrace }, function (res) {
                var d = res.data;
                if (d) {
                    d["DatVzniku"] = new Date(d["DatVzniku"]);
                } else {
                    var min60 = new Date(new Date().getTime() + 60 * 60000);
                    d = {
                        IDVykazPracePol: null,
                        IDVykazPrace: 0,
                        IDUserUpravil: IDUser,
                        rr_TypPolozkyPracaku: 3,
                        DatVzniku: new Date(),
                        CasOd: kendo.toString(new Date(), "HH:mm"),
                        CasDo: kendo.toString(min60, "HH:mm"),
                        Hodin: 1,
                        IDTechnika: IDUser,
                        Produkt: null,
                        TextNaFakturu: null,
                        TextInterniDoMailu: "",
                        PocetEMJ: 1,
                        CenaEMJ: 0,
                        Zasah: false,
                        Vzdalenka: false,
                        Zdarma: false
                    }
                }
                that.set("pracak", d);
            });
            $.get('@Url.Action("RegRest", "Api/Service")', { Register: "rr_StavPracaku" }, function (res) {
                that.set("stavy", res.data);
            });
        })

        $('#modal_rezervace').on('shown.bs.modal', function () {
            viewModel.AGsp_GetProduktRezervovaneProduktyFirmy.read();
        });

        var viewModel = kendo.observable({
            vypnout: false,
            vypnoutChange: function (e) {
                if (e.checked) {
                    var that = this;
                    var r = confirm("Opravdu si přejete dále firmu nanabízet/nezobrazovat?");
                    if (r == true) {
                        var selected = this.get("selectedItem");
                        $.get('@Url.Action("AGsp_Do_UFirmaZneviditelnitFirmu", "Api/Service")', { zneviditelnit: true, firma: selected.Pobocka }, function (res) {
                            that.set("vypnout", false);
                            if (res.error) {
                                alert(res.error);
                            } else {
                                that.AGsp_GetFirmaPracovisteSeznamHledej.read();
                                $('#modal_detail_klient').modal("hide");
                            }
                        });
                    } else {
                        this.set("vypnout", false);
                    }
                }
            },
            hledatHodnotu: "",
            btndetail: function (e) {
                var grid = $(e.currentTarget).closest(".k-grid").data("kendoGrid");
                var row = $(e.currentTarget).closest("tr");
                grid.select(row);
                $('#modal_detail_klient').modal("show");
            },
            ticket: {
                IDTicket: 0,
                Firma: null,
                Nazev_firmy: null,
                Pobocka: null,
                Nazev_pobocka: null,
                PobockaUlice: null,
                PobockaPSC: null,
                PobockaMesto: null,
                ICO: null,
                CasVytvoreni: new Date(),
                IDUserVytvoril: @_user.IDUser,
                UserVytvoril: "@Html.Raw(_user.UserLastName)",
                IDUserResitel:@_user.IDUser,
                UserResitel: "@Html.Raw(_user.UserLastName)",
                CasResitelPrevzal: null,
                DomluvenyTerminCas: new Date(),
                DomluvenyTerminCasDo: plusHours(new Date(), 1),
                rr_DeadLine: 0,
                rr_DeadLineHodnota: "Někdy příště",
                DatumDeadLine: plusDays(new Date(), 1),
                Predmet: null,
                Telo: null,
                InterniPoznamka: null,
                rr_TicketStav: 1,
                TicketStav: "Nový",
                rr_TicketPriorita: 1,
                TicketPriorita: "Nízká - ",
                IDMailu: null,
                EmailKomuPoslanMail: null,
                UdalostVGoogleCalend: true,
                IDGoogleCaledar: null,
                IDVykazPraceSparovany: 0,
                rr_LokalitaBarva: 0,
                rr_LokalitaBarvaText: "Určí google kalendář",
                Barva: null,
                rr_TypZasahu: 2,
                rr_TypZasahuText: "Osobně",
                Zkratka: null,
                OdesilatKlientoviEmaily: true,
                NaposledyOdeslanMailKlientovi: null,
                rr_FakturovatNaFirmu: 0,
                rr_FakturovatNaFirmuHodnota: "?"
            },
            btntiket: function (e) {
                var grid = $(e.currentTarget).closest(".k-grid").data("kendoGrid");
                var row = $(e.currentTarget).closest("tr");
                var di = grid.dataItem(row);
                grid.select(row);

                this.ticket.set("Firma", di.Firma);
                this.ticket.set("Nazev_firmy", di.Nazev_firmy);
                this.ticket.set("Pobocka", di.Pobocka);
                this.ticket.set("Nazev_pobocka", di.NazevPobocky);

                this.pobocky.read();
                this.lokality.read({ iDUser: @_user.IDUser });

                $('#modal_ticket').slideWindow("show");
            },
            ispopis: function () {
                var pop = this.ticket.get("Telo");
                return (pop ? true : false);
            },
            lokality_change: function (e) {
                var data = this.lokality.data();
                var item = data.find(x => x.value == e.sender.value());
                this.ticket.set("Barva", item.color);
                this.ticket.set("rr_LokalitaBarvaText", item.text);
            },
            user_change: function (e) {
                var idu = this.ticket.get("IDUserResitel");
                this.lokality.read({ iDUser: idu });
            },
            pobocky_bound: function (e) {
                var items = e.sender.items();
                if (items.length > 0) {
                    e.sender.select(items[0]);
                }
            },
            typyzasahu_change: function (e) {
                this.ticket.set("rr_TypZasahuText", e.sender.text());
            },
            pobocky: new kendo.data.DataSource({
                schema: {
                    data: "data",
                    errors: "error"
                },
                transport: {
                    read: {
                        url: "@Url.Action("PobockyFirmy", "Api/Service")",
                        type: "GET"
                    },
                    parameterMap: function (options, operation) {
                        var firma = viewModel.ticket.get("Firma")
                        return { firma: firma };
                    }
                }
            }),
            rr_FakturovatNaFirmu: new kendo.data.DataSource({
                schema: { data: "data", total: "total" },
                transport: { read: "@Url.Action("RegRest", "Api/Service")?Register=rr_FakturovatNaFirmu" }
            }),
            typyzasahu: new kendo.data.DataSource({
                schema: { data: "data", total: "total" },
                transport: { read: "@Url.Action("RegRest", "Api/Service")?Register=rr_TypZasahu" }
            }),
            lokality: new kendo.data.DataSource({
                schema: { data: "data", total: "total" },
                transport: { read: "@Url.Action("AGsp_Get_LokalitaBarvaProGoogleKal", "Api/Service")" }
            }),
            priority: new kendo.data.DataSource({
                schema: { data: "data", total: "total" },
                transport: { read: "@Url.Action("RegRest", "Api/Service")?Register=rr_TicketPriorita" }
            }),
            terminy: new kendo.data.DataSource({
                schema: { data: "data", total: "total" },
                transport: { read: "@Url.Action("RegRest", "Api/Service")?Register=rr_DeadLine" }
            }),
            resitele: new kendo.data.DataSource({
                schema: { data: "data", total: "total" },
                transport: { read: "@Url.Action("Uzivatele", "Api/Service")" }
            }),
            ticket_leftbtn_visible: false,
            ticket_date_change: function (e) {
                var ticket = this.get("ticket");
                if (ticket) {
                    var d1 = new Date(ticket.get("DomluvenyTerminCas"));
                    var d2 = new Date(ticket.get("DomluvenyTerminCasDo"));
                    if (d1 >= d2) {
                        var dt = plusHours(d1, 1);
                        ticket.set("DomluvenyTerminCasDo", dt);
                    }
                }
            },
            ticket_email: null,
            ticket_save: function (e) {
                e.preventDefault();
                var that = this;
                var savetype = $(e.originalEvent.submitter).attr("value");
                var data = this.get("ticket").toJSON();
                if (data.CasVytvoreni) {
                    data.CasVytvoreni = kendo.toString(new Date(data.CasVytvoreni), "yyyy-MM-dd HH:mm:ss");
                }
                if (data.DomluvenyTerminCas) {
                    data.DomluvenyTerminCas = kendo.toString(new Date(data.DomluvenyTerminCas), "yyyy-MM-dd HH:mm:ss");
                }
                if (data.DomluvenyTerminCasDo) {
                    data.DomluvenyTerminCasDo = kendo.toString(new Date(data.DomluvenyTerminCasDo), "yyyy-MM-dd HH:mm:ss");
                }
                if (data.DatumDeadLine) {
                    data.DatumDeadLine = kendo.toString(new Date(data.DatumDeadLine), "yyyy-MM-dd HH:mm:ss");
                }
                data["savetype"] = savetype;
                kendo.ui.progress($("#modal_ticket"), true);
                $.post("@Url.Action("AGsp_Do_IUTicketRucne", "Api/Service")", data, function (result) {
                    kendo.ui.progress($("#modal_ticket"), false);
                    if (result.error) {
                        alert(result.error);
                    } else {
                        $('#modal_ticket').slideWindow("hide");
                        if (result.action == 1) {
                            window.open(
                                '@Url.Action("Fakturace", "Home")' + "?i=" + result.data,
                                '_blank'
                            );
                        }
                        if (result.action == 3) {
                            if (!result.data) {
                                kendoNotification("Klient nemá vyplněný email", "info", 5000);
                            } else {
                                if (data.rr_TypZasahu != 3) {
                                    that.set("ticket_email", result.data);
                                    $('#modal_email').modal("show");
                                }
                            }
                        }
                    }
                })
            },
            email_post: function (e) {
                e.preventDefault();
                var email = this.get("ticket_email");
                $.post("@Url.Action("SendEmailTicket", "Home")", { EmailTo: email.emailTo, Subject: email.emailSubject, Body: email.emailBody }, function (result) {
                    if (result.error) {
                        alert(result.error);
                    } else {
                        $('#modal_email').modal("hide");
                    }
                });
            },
            close_email: function (e) {

            },
            firma: null,
            historysource: {
                IDFirmyRecordHistory: 0,
                IDFirmy: null,
                RecordDate: new Date(),
                RecordCommentType: null,
                RecordCommentTxt: null,
                IDUser: 0
            },
            btnhistoryenabled: function (e) {
                var RecordCommentType = this.historysource.get("RecordCommentType"),
                    RecordCommentTxt = this.historysource.get("RecordCommentTxt")
                if (RecordCommentType && RecordCommentTxt) {
                    return true;
                }
                return false;
            },
            btnhistory: function (e) {
                var grid = $(e.currentTarget).closest(".k-grid").data("kendoGrid");
                var row = $(e.currentTarget).closest("tr");
                var di = grid.dataItem(row);
                this.set("firma", di);
                grid.select(row);
                $('#modal_history').modal("show");
                this.history.read();
            },
            historyadd: function (e) {
                var di = this.get("firma");
                var that = this;
                var model = {
                    IDFirmyRecordHistory: 0,
                    IDFirmy: di.NazevPobocky,
                    RecordDate: new Date(),
                    RecordCommentType: this.historysource.get("RecordCommentType"),
                    RecordCommentTxt: this.historysource.get("RecordCommentTxt"),
                    IDUser: 0
                }
                $.get('@Url.Action("AGsp_AddOrEditFirmyRecordHistory", "Api/Service")', model, function (result) {
                    that.history.read();
                });
            },
            historysave: function (e) {
                var di = this.get("firma");
                var that = this;
                var model = this.get("historysource").toJSON();
                $.get('@Url.Action("AGsp_AddOrEditFirmyRecordHistory", "Api/Service")', model, function (result) {
                    that.history.read();
                });
            },
            history: new kendo.data.DataSource({
                schema: {
                    model: {
                        id: "IDFirmyRecordHistory",
                        fields: {
                            RecordDate: { type: "date" }
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        var di = viewModel.get("firma");
                        $.get('@Url.Action("AGsp_GetFirmyRecordHistorySeznam", "Api/Service")', { iDFirmy: di.NazevPobocky }, function (result) {
                            options.success(result.data);
                        });
                    }
                },
            }),
            historychange: function (e) {
                var s = e.sender.select();
                var d = e.sender.dataItem(s);
                this.set("historysource", d)
            },
            hledat: function (e) {
                e.preventDefault();
                this.AGsp_GetFirmaPracovisteSeznamHledej.read();
            },
            typyKontaktu: new kendo.data.DataSource({
                schema: {
                    data: "data"
                },
                transport: {
                    read: "@Url.Action("AGvwrr_TypykontaktnichUdaju", "Api/Service")"
                }
            }),
            produktyHledej: "",
            produkty: new kendo.data.DataSource({
                transport: {
                    read: function (options) {
                        var h = viewModel.get("produktyHledej");
                        $.get('@Url.Action("AGsp_HledejProduktFullText", "Api/Service")', { hledej: h }, function (result) {
                            options.success(result.data);
                        });
                    }
                }
            }),
            datExp: false,
            polozka: {},
            dejProdukt: function (e) {
                this.produkty.read();
                $('#modal_produkty').modal("show");
            },
            produktySelect: function (e) {
                var element = e.sender.select();
                var dataItem = e.sender.dataItem(element[0]);

                this.polozka.set("InventarProdukt", dataItem.Produkt);
                if (!this.polozka.InventarPopis) {
                    this.polozka.set("InventarPopis", dataItem.Popis);
                }

                $('#modal_produkty').modal("hide");
            },
            inventarPolSave: function (e) {
                var form = $(e.currentTarget).closest("form");
                var valid = $(form).valid();
                if (!valid) {
                    return false;
                };

                //console.log(this.polozka)

                //inventar.IDInventare,
                //    inventar.Firma,
                //    inventar.InventarPopis,
                //    inventar.InventarProdukt,
                //    inventar.DatumNaposledyZakoupeno,
                //    inventar.rr_TypInventare,
                //    inventar.DatumExpirace
            },
            iDVykazPrace: 0,
            pracak: {},
            stavy: [],
            novyPracak: function (e) {
                //Nový pracák založí nový pracák a otevře ho na nové záložce.
                var that = this;
                if (that.f2) {
                    $.get('@Url.Action("AGsp_AddNewVykazPrace", "Api/Service")', { firma: that.f2 }, function (res) {
                        var id = parseInt(res.data);
                        if (id) {
                            that.set("iDVykazPrace", id);
                            $("#modal_pracak").modal("show");
                        };
                    });
                } else { alert("Vyber klienta") }
            },
            pracaky: function (e) {
                //Pracáky - tlačítko přepne na záložku Fakturace, postaví se na patřičný pracák a zobrazí vlevo jeho detail
                if (this.f1) {
                    window.location.href = '@Url.Action("Fakturace", "Home")' + "?f=" + this.f1;
                } else { alert("Vyber klienta") }
            },
            klientEdit: function (e) {
                if (this.f1) {
                    $("#modal_detail").modal("show");
                } else { alert("Vyber klienta") }
            },
            rezervovat_selected: {
                Firma: "",
                IDPrijemkaPol: 0,
                OperativniZasoba: 0
            },
            modal_mnozstvi_rezervovat_save: function (e) {
                var that = this,
                    model = that.get("rezervovat_selected");
                $.get('@Url.Action("AGsp_Do_ProduktRezervace", "Api/Service")', { iDPrijemkaPol: model.IDPrijemkaPol, pocetEMJRezervovat: model.OperativniZasoba, rezervovat: true, firma_RezervovatProFirmu: model.Firma }, function (result) {
                    if (result.error) {
                        alert(result.error);
                    } else {
                        $("#modal_mnozstvi_rezervovat").modal("hide");
                        that.AGsp_GetProduktRezervovaneProduktyFirmy.read();
                    }
                }); 
            },
            btn_rezervovat: function (e) {
                var firma = this.get("f1");
                this.set("rezervovat_selected", {
                    Firma: firma,
                    IDPrijemkaPol: e.data.IDPrijemkaPol,
                    OperativniZasoba: e.data.OperativniZasoba
                });
                $("#modal_mnozstvi_rezervovat").modal("show");
            },
            rezervovat: function (e) {
                $("#modal_rezervace").modal("show");
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
                        var firma = viewModel.get("f1");
                        return { firma: firma }
                    }
                }
            }),
            klientSave: function (e) {
                var that = this;
                var d = {
                    firma: this.detail.Firma,
                    nazev_firmy: this.detail.Nazev_firmy,
                    vzdalenost: this.detail.Vzdalenost,
                    obor_cinnosti: this.detail.Obor_cinnosti,
                    poznamky: this.detail.Poznamky,
                    icz: this.detail.ICZ
                };
                $.get("@Url.Action("AGsp_EditFirmaDetailPodrobne", "Api/Service")", d, function (res) {
                    if (res.error) {
                        alert(res.error)
                    } else {
                        $.get("@Url.Action("AGsp_GetFirmaPracovisteDetail", "Api/Service")", { firma: that.f1, pracoviste: that.f2 }, function (result) {
                            var d = result.data[0];
                            that.set("detail", d);
                            that.AGsp_GetFirmaPracovisteSeznamHledej.read();
                        });
                        $("#modal_detail").modal("hide");
                    }
                })
            },
            newKlient: {},
            klientAdd: function (e) {
                this.set("newKlient", {
                    Firma: "",
                    Nazev_firmy: "",
                    Telefon_1: "",
                    Telefon_2: "",
                    E_mail: "",
                    ICO: "",
                    DIC: "",
                    Obor_cinnosti: "",
                    Poznamky: "",
                    Titul: "",
                    Krestni: "",
                    Prijmeni: "",
                    Funkce: "",
                    Zasilat: ""
                })
                $("#klientAddModal").modal("show");
            },
            klientAddSave: function (e) {
                e.preventDefault();
                var d = this.newKlient.toJSON();
                var that = this;
                try {
                    $.post("@Url.Action("AGsp_AddNewFirma", "Api/Service")", d, function (res) {
                        if (res.error) {
                            alert(res.error)
                        } else {
                            $("#klientAddModal").modal("hide");
                            that.AGsp_GetFirmaPracovisteSeznamHledej.read();
                        }
                    });
                } catch (ex) {
                    alert(ex.message);
                }
            },
            firmaExist: function (e) {
                var that = this;
                $.get("@Url.Action("AGsp_FirmaExist", "Api/Service")", { firma: that.newKlient.Firma }, function (res) {
                    if (res.data) {
                        alert("Firma již existuje")
                        that.newKlient.set("Firma", "");
                    }
                })
            },
            ares: function (e) {
                var that = this;
                if (that.newKlient.ICO) {
                    $.get("@Url.Action("AresBasicInfo", "Api/Service")", { ico: that.newKlient.ICO }, function (res) {
                        if (res) {
                            that.newKlient.set("Firma", res.firma);
                            that.newKlient.set("Nazev_firmy", res.firma);
                            that.newKlient.set("DIC", res.dic);
                        }
                    })
                } else {
                    alert("Zadejte IČO");
                }
            },
            pracakSave: function (e) {
                //var stav = this.pracak.rr_StavPracaku;
                //if (stav === 0) {
                //    var r = confirm("pracák ke kontrole a fakturaci ? ");
                //    if (r == true) {
                //        stav = 1;
                //    }
                //}
                var d = {
                    iDVykazPrace: this.pracak.IDVykazPrace,
                    datVzniku: kendo.toString(new Date(this.pracak.DatVzniku), "yyyy-MM-dd"),
                    iDUserUpravil: 0,
                    poznamka: this.pracak.Poznamka,
                    rr_StavPracaku: 0
                };
                $.post('@Url.Action("AGsp_EditVykazPrace", "Api/Service")', d, function (res) {
                    if (res.error) {
                        alert(res.error)
                    } else {
                        window.location.href = '@Url.Action("Fakturace", "Home")' + "?i=" + d.iDVykazPrace;
                        //$("#modal_pracak").modal("hide");
                    }
                });
            },
            validGpsBtnStyle: "btn btn-outline-danger btn-sm",
            gpsSave: function (e) {
                var that = this;
                var d = e.data.detail;
                $.get("@Url.Action("AGsp_EditFirmaGPS", "Api/Service")", { firma: d.Firma, gPSLat: d.GPSLat, gPSLng: d.GPSLng, gPSValid: d.GPSValid }, function (result) {
                    if (result.error) {
                        alert(result.error)
                    } else {
                        that.AGsp_GetFirmaPracovisteSeznamHledej.read();
                    };
                });
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
            clearFilter: function (e) {
                //this.set("produktyHledej", "");
                //this.AGsp_GetFirmaPracovisteSeznamHledej.filter({});
                //this.AGsp_GetFirmaPracovisteSeznamHledej.read();
                window.location.replace('@Url.Action("Klienti", "Home")');
            },
            next: function (e) {
                var di = this.get("selectedItem");
                var cur = $('tr[data-uid="' + di.uid + '"]')
                var nex = cur.next();
                nex.click();
            },
            previous: function (e) {
                var di = this.get("selectedItem");
                var cur = $('tr[data-uid="' + di.uid + '"]')
                var pre = cur.prev();
                pre.click();
            },
            seznamDataBound: function (e) {
                var row = e.sender.tbody.find('tr:first');
                if (!this.selectedItem) {
                    if (row.length > 0) {
                        e.sender.select(row);
                    }
                } else {
                    var uid = this.selectedItem.uid;
                    row = e.sender.tbody.find('tr[data-uid="' + uid + '"]');
                    if (row.length > 0) {
                        e.sender.select(row);
                    } else {
                        row = e.sender.tbody.find('tr:first');
                        if (row.length > 0) {
                            e.sender.select(row);
                        }
                    }
                }
            },
            AGsp_GetFirmaPracovisteDetailKontakty: new kendo.data.DataSource({
                schema: {
                    model: {
                        id: "Kontaktni_udaj",
                        fields: {
                            Typ_KU: { type: "string" },
                            Nazev_KU: { type: "string", defaultValue: "mobil" },
                            Hodnota_KU: { type: "string", defaultValue: "123456789" }
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        var that = viewModel;
                        $.get("@Url.Action("AGsp_GetFirmaPracovisteDetailKontakty", "Api/Service")", { firma: that.f1, pracoviste: that.f2 }, function (result) {
                            if (result.error) {
                                alert(result.error);
                            } else {
                                options.success(result.data);
                            }
                        });
                    },
                    update: function (options) {
                        var that = viewModel;
                        var d = options.data;
                        d.Kontakt = that.f2;
                        $.post("@Url.Action("AGsp_AddOrEditFirmyKontaktniudaje", "Api/Service")", d, function (result) {
                            if (result.error) {
                                alert(result.error);
                            };
                            viewModel.AGsp_GetFirmaPracovisteDetailKontakty.read();
                        });
                    },
                    create: function (options) {
                        var that = viewModel;
                        var d = options.data;
                        d.Kontakt = that.f2;
                        $.post("@Url.Action("AGsp_AddOrEditFirmyKontaktniudaje", "Api/Service")", d, function (result) {
                            if (result.error) {
                                alert(result.error);
                            };
                            viewModel.AGsp_GetFirmaPracovisteDetailKontakty.read();
                        });
                    }
                }
            }),
            AGsp_GetFirmaPracovisteDetailKontakty_DataBound: function (e) {
                var btns = $(e.sender.element).find('.k-button');
                if (!viewModel.f1) {
                    btns.attr("disabled", "disabled");
                } else {
                    btns.removeAttr("disabled");
                }
            },
            detail: {},
            f1: 0,
            f2: 0,
            inventar: new kendo.data.DataSource({
                schema: {
                    model: {
                        id: "IDInventare",
                        fields: {
                            InventarProdukt: { type: "string" },
                            DatumNaposledyZakoupeno: { type: "date" },
                            InventarPopis: { type: "string" },
                            Pracoviste: { type: "string" },
                            DatumExpirace: { type: "date" }
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        var that = viewModel;
                        $.get("@Url.Action("AGsp_GetFirmaInventarSeznam", "Api/Service")", { firma: that.f1, pracoviste: that.f2 }, function (result) {
                            if (result.data) {
                                options.success(result.data);
                            }
                        });
                    },
                    update: function (options) {
                        var that = viewModel;
                        var d = options.data;
                        if (!d.rr_TypInventare) {
                            d.rr_TypInventare = 0;
                        }
                        d["Firma"] = that.f2;
                        $.post("@Url.Action("AGsp_AddOrEditFirmyInventar", "Api/Service")", d, function (result) {
                            if (result.error) {
                                alert(result.error);
                            };
                            options.success();
                        });
                    },
                    create: function (options) {
                        var that = viewModel;
                        var d = options.data;
                        if (!d.rr_TypInventare) {
                            d.rr_TypInventare = 0;
                        }
                        d["Firma"] = that.f2;
                        d.IDInventare = 0
                        console.log(d)
                        $.post("@Url.Action("AGsp_AddOrEditFirmyInventar", "Api/Service")", d, function (result) {
                            if (result.error) {
                                alert(result.error);
                            };
                            options.success();
                        });
                    }
                },
                pageSize: 50
            }),
            inventarDataBound: function (e) {
                var btns = $(e.sender.element).find('.k-button');
                if (!viewModel.f1) {
                    btns.attr("disabled", "disabled");
                } else {
                    btns.removeAttr("disabled");
                }
            },
            inventarObjednat: function (e) {
                this.set("polozka", e.data);
                $("#modal_inventar").modal("show");
            },
            parametry: new kendo.data.DataSource({
                schema: {
                    model: {
                        id: "Firma",
                        fields: {
                            IDFirmaParametr: { type: "number" },
                            HodinovaSazba: { type: "number" },
                            SazbaKm: { type: "number" },
                            rr_TypServisniSmlouvy: { type: "number" },
                            ServiskaCenaMesicne: { type: "number" },
                            ServiskaNaposledyVyuctovana: { type: "date" },
                            ServiskaIntervalObnoveni: { type: "number" },
                            ServiskaVolneHodiny: { type: "number" },
                            PlatnostOd: { type: "date" },
                            UserLastName: { type: "string", editable: false }
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        var that = viewModel;
                        $.get("@Url.Action("AGsp_GetFirmaParametry", "Api/Service")", { firma: that.f1 }, function (result) {
                            options.success(result.data);
                        });
                    },
                    update: function (options) {
                        var d = options.data;
                        d.PlatnostOd = kendo.toString(new Date(d.PlatnostOd), "yyyy-MM-dd HH:mm:ss")
                        $.post("@Url.Action("AGsp_EditFirmaParametry", "Api/Service")", d, function (result) {
                            if (result.error) {
                                alert(result.error);
                            };
                            viewModel.parametry.read();
                        });
                    }
                },
                pageSize: 50
            }),
            typyServSmlouvy: new kendo.data.DataSource({
                schema: { data: "data" },
                transport: { read: "@Url.Action("RegRest", "Api/Service")?Register=rr_TypServisniSmlouvy" }
            }),
            parametryDataBound: function (e) {
                var btns = $(e.sender.element).find('.k-button');
                if (!viewModel.f1) {
                    btns.attr("disabled", "disabled");
                } else {
                    btns.removeAttr("disabled");
                }
                @*var rows = e.sender.tbody.children();
                for (var j = 0; j < rows.length; j++) {
                    var row = $(rows[j]);
                    var dataItem = e.sender.dataItem(row);
                    var ddt = $(row).find('.dropDownTemplate');
                    $(ddt).kendoDropDownList({
                        value: dataItem.rr_TypServisniSmlouvy,
                        dataSource: new kendo.data.DataSource({
                            schema: {
                                data: "data"
                            },
                            transport: {
                                read: "@Url.Action("RegRest", "Api/Service")?Register=rr_TypServisniSmlouvy"
                            }
                        }),
                        dataTextField: "text",
                        dataValueField: "value"
                    });
                }*@
            },
            selectedItem: {},
            parametryValidniChange: function (e) {
                var that = this;
                var i = e.checked ? 1 : 0;
                $.get("@Url.Action("AGsp_Do_FirmaParametryValidni", "Api/Service")", { firma: that.f1, stavValidity: i }, function (result) {
                    if (result.error) {
                        alert(result.error);
                    } else {
                        that.AGsp_GetFirmaPracovisteSeznamHledej.read();
                    }
                })
            },
            select: function (e) {
                var that = this;
                var grid = e.sender;
                var item = $.map(grid.select(), function (a, b) {
                    return grid.dataItem(a);
                })[0];
                that.f1 = item.Firma;
                that.f2 = item.Pobocka;

                item.ParametryValidni = (item.ParametryValidni > 0 ? true : false)

                that.set("selectedItem", item);
                $.get("@Url.Action("AGsp_GetFirmaPracovisteDetail", "Api/Service")", { firma: that.f1, pracoviste: that.f2 }, function (result) {
                    var d = result.data[0];

                    if (d.GPSValid) {
                        that.set("validGpsBtnStyle", "btn btn-outline-success btn-sm");
                    } else {
                        that.set("validGpsBtnStyle", "btn btn-outline-danger btn-sm");
                    }

                    that.set("detail", d);
                });
                that.inventar.read();
                that.parametry.read();
                that.AGsp_GetFirmaPracovisteDetailKontakty.read();
                $(".k-grid-toolbar").delegate(".k-grid-custom", "click", function (e) {
                    e.preventDefault();
                    $.get("@Url.Action("AGsp_AddNewFirmaParametry", "Api/Service")", { firma: that.f1, iDUser: 0 }, function(result) {
                        that.parametry.read();
                    });
                });
            }
        });
        var original = viewModel.toJSON();
        kendo.bind(document.body, viewModel);

        if (firma) {
            viewModel.set("hledatHodnotu", firma);
            viewModel.AGsp_GetFirmaPracovisteSeznamHledej.read();
        }
    })
</script>

@*get, put, post, delete*@
