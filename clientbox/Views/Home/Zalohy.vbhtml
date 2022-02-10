@Code
    ViewData("Title") = "Zálohy klientů"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<style>
    html, body {
        overflow: hidden;
    }

    .historie .k-grid th {
        padding: 0;
        padding-left: 3px;
    }

    .noborder td {
        border-width: 0;
    }
</style>

<div id="main" style="height: 100%;">
    <div data-role="splitter"
         data-panes="[
        { collapsible: true, scrollable: false },
        { collapsible: false, scrollable: false, size: '50%' }
        ]"
         data-orientation="horizontal"
         style="height:100%;width:100%;">
        <div data-role="grid"
             data-resizable="true"
             data-sortable="true"
             data-filterable="true"
             data-editable="true"
             data-scrollable="true"
             data-selectable="true"
             data-pageable="true"
             data-persist-selection="true"
             data-bind="source: AGsp_Get_DoctorumBackupProfile, toolbar: toolbar, events: { change: klient_change }"
             data-columns="[
             { field: 'IDKlienta', attributes: { class: 'text-center bg-light text-secondary' }, title: 'ID', width: 80 },
             { field: 'KdoToJe', title: 'Kdo to je', width: 150 },
             { template: '#=clientProgress(IDBackupProfile)#', title: 'Průběh zálohování', width: 150 },
             { field: 'rr_UcetniStavSBText', attributes: { class: 'pl-2' }, title: 'Účetní stav', width: 100 },
             { field: 'Firma', attributes: { class: 'pl-2' }, title: 'Název firmy', width: 180 },
             { field: 'PCOnLine', attributes: { class: 'text-center' }, template: '#=pCOnLine(PCOnLine, IDBackupProfile)#', title: 'PC OnLine', width: 40 },
             { field: 'PopisZarizeni', attributes: { class: 'pl-2' }, title: 'Popis zařízení', width: 150 },
             { field: 'VerzeSluzby', attributes: { class: 'pl-2' }, title: 'Verze', width: 100 },
             { field: 'DatumZalozeni', attributes: { class: 'pl-2' }, format: '{0:yyyy-MM-dd HH:mm}', title: 'Datum založení', width: 120 },
             { field: 'DatumExpirace', attributes: { class: 'pl-2' }, template: '#=datumExpirace(ExpiraceCervene, DatumExpirace)#', title: 'Datum expirace', width: 120 },
             { field: 'rr_StavBackupProfile', template: '#=rr_StavBackupProfileHodnota#', attributes: { class: 'pl-2' }, title: 'Stav', width: 80},
             { field: 'CasPosledniZalohy', attributes: { class: 'pl-2' }, template: '#=casPosledniZalohy(BarvaPosledniZalohy, CasPosledniZalohy)#', title: 'Poslední záloha', width: 120 },
             { field: 'DovolenaDo', editor: dovolenaEditor, attributes: { class: 'pl-2' }, format: '{0:yyyy-MM-dd HH:mm}', title: 'Dovolená do', width: 120 },
             { field: 'TextPosledniZalohy', attributes: { class: 'pl-2' }, title: 'Poslední záloha', width: 80 },
             { field: 'Hodin', attributes: { class: 'pl-2' }, format: '{0:n0}', title: 'Hodin', width: 80 }]"></div>
        <div data-role="grid"
             data-resizable="true"
             data-auto-bind="false"
             data-scrollable="true"
             data-selectable="true"
             data-pageable="true"
             data-bind="source: zalohy, toolbar: bctoolbar"
             data-columns="[{ field: 'BackupDate', attributes: { class: 'pl-2 bg-light text-secondary' }, format: '{0:yyyy-MM-dd HH:mm}', title: 'Datum zálohy', width: 150 },
             { field: 'BackupFileName', attributes: { class: 'pl-2' }, template: '#=downloadFile(BackupFileName, rr_BackupUspesnost)#', title: 'Soubor' },
             { field: 'rr_BackupUspesnostHodnota', attributes: { class: 'pl-2' }, template: '#=xmlConfig(rr_BackupUspesnostHodnota)#', title: 'Stav', width: 150 }]"></div>
    </div>
</div>

<div id="dialogClienTimes"
     data-role="window"
     data-width="400"
     data-height="auto"
     data-modal="true"
     data-visible="false"
     data-title="Časy záloh"
     data-actions="['close']" style="display:none;">
    <div data-role="grid"
         data-editable="inline"
         data-resizable="true"
         data-auto-bind="false"
         data-scrollable="true"
         data-selectable="true"
         data-toolbar="['create', 'cancel']"
         data-bind="source: clientTimes"
         data-columns="[{ field: 'dayint', editor: dayEdit, attributes: { class: 'pl-2' }, template: '#=day#', title: 'Den zálohy', width: 150 },
                        { field: 'time', format: '{0:HH:mm}', editor: timeEdit, attributes: { class: 'pl-2' }, title: 'Čas zálohy' },
                        { template: '#=cellRemove()#', title: ' ', width: 40 }]"></div>
    <div class="text-right mt-2">
        <button type="button" role="button" class="k-button k-button-icontext" data-bind="events: { click: close }"><span class="k-icon k-i-close"></span>Zavřít</button>
    </div>
</div>

<div id="clienMessage"
     data-role="window"
     data-width="500"
     data-height="auto"
     data-modal="true"
     data-visible="false"
     data-title="Zpráva klientovi"
     data-actions="['close']" style="display:none;">
    <table class="w-100">
        <tr><td>Titulek/Předmět</td></tr>
        <tr><td><input type="text" class="k-textbox w-100" data-bind="value: clienMessageTitle" /></td></tr>
        <tr><td>Zpráva</td></tr>
        <tr><td><textarea style="height:4cm;" class="k-textbox w-100" data-bind="value: clienMessage"></textarea></td></tr>
    </table>
    <div class="text-right mt-2">
        <button type="button" role="button" class="k-button k-button-icontext" data-bind="events: { click: clienMessageSendPopup }"><span class="fa fa-comment mr-2"></span> Poslat do SafeBerry</button>
        <button type="button" role="button" class="k-button k-button-icontext" data-bind="events: { click: clienMessageSendEmail }"><span class="fa fa-envelope-o mr-2"></span>Poslat emailem</button>
        <button type="button" role="button" class="k-button k-button-icontext" data-bind="events: { click: clienMessageClose }"><span class="k-icon k-i-close"></span>Zavřít</button>
    </div>
</div>

<div id="pridatZaznamHistorie"
     data-role="window"
     data-width="500"
     data-height="auto"
     data-modal="true"
     data-visible="false"
     data-title="Přidat do historie"
     data-actions="['close']" style="display:none;">
    <table class="w-100">
        <tr><td>Předmět</td></tr>
        <tr><td><input type="text" class="k-textbox w-100" data-bind="value: historiePredmet" /></td></tr>
        <tr><td>Zpráva</td></tr>
        <tr><td><textarea style="height:4cm;" class="k-textbox w-100" data-bind="value: historieText"></textarea></td></tr>
    </table>
    <div class="text-right mt-2">
        <button type="button" role="button" class="k-button k-button-icontext" data-bind="events: { click: historieSave }"><span class="fa fa-save mr-2"></span>Uložit</button>
        <button type="button" role="button" class="k-button k-button-icontext" data-bind="events: { click: historieClose }"><span class="k-icon k-i-close mr-2"></span>Zavřít</button>
    </div>
</div>

<script type="text/x-kendo-tmpl" id="kient-toolbar-template">
    <h3>Klienti</h3>
    <form class="form-inline" data-bind="events: { submit: filter_change }">
        <div class="input-group w-100">
            <input type="search" class="form-control" data-bind="value: hledej" placeholder="Hledat..." aria-label="Hledat...">
            <div class="input-group-append">
                <button class="btn btn-secondary" type="submit"><span class="k-icon k-i-zoom"></span></button>
            </div>
        </div>
    </form>
    <div class="radio-filters" data-bind="source: filters" data-template="kient-toolbar-radio"></div>
</script>

<script type="text/x-kendo-tmpl" id="kient-toolbar-radio">
    <span data-uid="${uid}" class="mr-2 border-right">
        <input type="radio" name="rbfilter" value="${index}" id="rb${index}" class="k-radio" data-bind="checked: filter, events: { change: filter_change }">
        <label class="k-radio-label" for="rb${index}">${value}</label>
    </span>
</script>

<script type="text/x-kendo-tmpl" id="historie-toolbar-template">
    <button data-role="button" data-bind="events: { click: create }"><i class="fa fa-plus mr-2" aria-hidden="true"></i> Přidat záznam</button>
</script>

<script type="text/x-kendo-tmpl" id="backups-toolbar-template">
    <h3><i class="fa fa-circle" data-bind="css: { text-success: selected.PCOnLine }"></i> Zálohy klienta</h3>
    <div class="row" data-bind="visible: visible" style="overflow-x:auto;">
        <div class="col-md-2">
            <div data-role="grid"
                 data-editable="false"
                 data-resizable="true"
                 data-auto-bind="false"
                 data-scrollable="true"
                 data-selectable="false"
                 data-toolbar="<span class='text-muted'>Časy záloh</span>"
                 data-bind="source: backuptimes"
                 data-columns="[{ field: 'day', attributes: { class: 'pl-2' }, title: 'Den zálohy', width: 60 },
                                { field: 'time', attributes: { class: 'pl-2' }, title: 'Čas zálohy' }]" class="h-100"></div>
        </div>
        <div class="col-md-10">
            <table border="0" class="noborder">
                <tr>
                    <td class="text-muted pl-2 pr-2">Popis zařízení</td>
                    <td><input type="text" class="k-textbox w-100" data-bind="value: selected.PopisZarizeni" readonly /></td>
                    <td class="text-muted  pl-2 pr-2">Stav</td>
                    <td><input data-role="dropdownlist" class="w-100" data-auto-width="true" data-value-primitive="true" data-value-field="value" data-text-field="text" id="rr_StavBackupProfile" data data-bind="source: stavy, value: selected.rr_StavBackupProfile, events: { change: save }" /></td>
                </tr>
                <tr>
                    <td class="text-muted  pl-2 pr-2">Název firmy</td>
                    <td><input data-role="dropdownlist" class="w-100" data-auto-width="true" data-value-primitive="true" data-value-field="firma" data-text-field="firma" data data-bind="source: firmy, value: selected.Firma, events: { change: save }" /></td>
                    <td class="text-muted pl-2 pr-2">Kdy zaplaceno</td>
                    <td><input class="w-100" data-role="datepicker" data-bind="value: selected.DoKdyZaplaceno, events: { change: save }" /></td>
                </tr>
                <tr>
                    <td class="text-muted  pl-2 pr-2">Ambulantní SW</td>
                    <td><input data-role="dropdownlist" class="w-100" data-auto-width="true" data-value-primitive="true" data-value-field="value" data-text-field="text" data data-bind="source: ambsw, value: selected.rr_AmbulSW, events: { change: save }" /></td>
                    <td class="text-muted  pl-2 pr-2">Fakturovat na IČO</td>
                    <td><input type="text" class="k-textbox w-100" data-bind="value: selected.FakturovatNaICO, events: { change: save }" /></td>
                </tr>
                <tr>
                    <td class="text-muted pl-2 pr-2">Kdo to je</td>
                    <td><input type="text" class="k-textbox w-100" id="KdoToJe" data-bind="value: selected.KdoToJe, events: { change: save }" /></td>
                    <td class="text-muted pl-2 pr-2">Poslední faktura</td>
                    <td><input type="text" class="k-textbox w-100" data-bind="value: selected.PosledniFaktura, events: { change: save }" /></td>
                </tr>
                <tr>
                    <td class="text-muted pl-2 pr-2">Telefon</td>
                    <td><input type="number" class="k-textbox w-100" data-bind="value: selected.Telefon, events: { change: save }" /></td>
                    <td class="text-muted pl-2 pr-2">Do kdy zaplaceno předplatné</td>
                    <td><input class="w-100" data-role="datepicker" data-bind="value: selected.DatumExpirace" disabled /></td>
                </tr>
                <tr>
                    <td class="text-muted pl-2 pr-2">Email</td>
                    <td><input type="email" class="k-textbox w-100" data-bind="value: selected.Email, events: { change: save }" /></td>
                    <td class="text-muted pl-2 pr-2">Smluvená částka bez DPH</td>
                    <td><input type="number" class="k-textbox w-100" data-bind="value: selected.SmluvniCenaMaintenance, events: { change: save }" /></td>
                </tr>
                <tr>
                    <td class="text-muted pl-2 pr-2">Poslední pokus</td>
                    <td><input class="w-100" data-role="datepicker" data-bind="value: selected.CasPoslednihoPokusuOZalohu" readonly /></td>
                    <td class="text-muted pl-2 pr-2">Účetní stav</td>
                    <td>
                        <input data-role="dropdownlist" class="w-100" data-auto-width="true" data-value-primitive="true" data-value-field="value" data-text-field="text" data data-bind="source: ucetnistavy, value: selected.rr_UcetniStavSB, events: { change: save }" />
                    </td>
                </tr>
                <tr>
                    <td class="text-muted pl-2 pr-2"></td>
                    <td></td>
                    <td class="text-muted pl-2 pr-2">Popisek aktuálního balíčku <span data-bind="text: selected.PopisekAktualnihoBalicku"></span></td>
                    <td>
                        <input type="checkbox" id="checkboxserver" class="k-checkbox" data-bind="checked: selected.SBvRezimuServer, events: { change: save }">
                        <label class="k-checkbox-label" for="checkboxserver">Serverová verze</label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="col-md-4">
            <span class="text-muted ml-2 mr-2">Poznámka</span><br />
            <textarea style="height:3cm;" class="k-textbox w-100" data-bind="value: selected.Poznamka, events: { change: save }"></textarea>
        </div>
        <div class="col-md-8 historie">
            <div data-role="grid"
                 data-editable="false"
                 data-resizable="true"
                 data-auto-bind="false"
                 data-scrollable="true"
                 data-selectable="false"
                 data-bind="source: historie, toolbar: historietoolbar"
                 data-columns="[{ field: 'CasVzniku', format: '{0:dd.MM.yyyy HH:mm}', attributes: { class: 'pl-2' }, title: 'Čas', width: 120 },
                                { field: 'Predmet', attributes: { class: 'pl-2' }, title: 'Předmět' },
                                { field: 'Txt', attributes: { class: 'pl-2' }, title: 'Text' },
                                { field: 'UserLastName', attributes: { class: 'pl-2' }, title: 'Uživatel' }]" class="h-100 mt-2"></div>
        </div>
        <div class="col-md-12 mt-2">
            <button data-role="button" class="action" data-bind="events: { click: mesicniReport }"><i class="fa fa-envelope mr-2" aria-hidden="true"></i> Měsíční report</button>
            <button data-role="button" class="action" data-bind="events: { click: detailRemoteUpdate }"><i class="fa fa-refresh mr-2" aria-hidden="true"></i> Vzdalena aktualizace</button>
            <button data-role="button" class="action" data-bind="events: { click: detailSendMessage }"><i class="fa fa-comment mr-2" aria-hidden="true"></i> Poslat zprávu</button>
            <a href="\\#" target="_blank" data-role="button" class="action" data-bind="events: { click: detailDostupnost }"><i class="fa fa-bar-chart mr-2" aria-hidden="true"></i> Graf dostupnosti</a>
            <button data-role="button" class="action" data-bind="events: { click: detailGetConfig }"><i class="fa fa-file-code-o mr-2" aria-hidden="true"></i> Vzdálená konfigurace</button>
            <button data-role="button" class="action" data-bind="events: { click: detailRemoteBackup }"><i class="fa fa-cloud-download mr-2" aria-hidden="true"></i> Manuální zálohování</button>
            <button data-role="button" class="action" data-bind="events: { click: detailGetTimes }"><i class="fa fa-clock-o mr-2" aria-hidden="true"></i> Nastavení časů</button>
            <select data-role="dropdownlist" class="action" style="width:250px" data-bind="value: nahoditRestartovatValue, events: { change: nahoditRestartovat }" data-option-label="-- Nahodit/Restartovat --">
                <option value="0">V klidu</option>
                <option value="1">Restart SignalR</option>
                <option value="2">Restar Safeberry</option>
            </select>
        </div>
    </div>
</script>

<div class="modal fade" id="modal_mesicni_email" style="overflow-y:auto;">
    <div class="modal-dialog modal-lg">
        <form class="modal-content" method="post" data-bind="events: { submit: mesicniEmailPost }">
            <div class="modal-header">
                <h4 class="modal-title">Email</h4>
                <button type="button" class="close" data-dismiss="modal">&times;</button>
            </div>
            <div class="modal-body">
                <div class="form-row">
                    <div class="form-group col-md-12">
                        <div class="input-group">
                            <div class="input-group-prepend">
                                <span class="input-group-text">K datu</span>
                            </div>
                            <input type="date" class="form-control" data-bind="value: mesicniReportData.MDate" required>
                        </div>
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group col-md-12">
                        <div class="input-group">
                            <div class="input-group-prepend">
                                <span class="input-group-text">Komu</span>
                            </div>
                            <input type="email" class="form-control" data-bind="value: mesicniReportData.emailTo" required>
                        </div>
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group col-md-12">
                        <div class="input-group">
                            <div class="input-group-prepend">
                                <span class="input-group-text">Předmět</span>
                            </div>
                            <input type="text" class="form-control" data-bind="value: mesicniReportData.emailSubject" required>
                        </div>
                    </div>
                </div>
                <div class="form-row">
                    <div class="form-group col-md-12">
                        <textarea data-role="editor"
                                  data-tools="[]"
                                  rows="5"
                                  class="form-control"
                                  data-bind="value: mesicniReportData.emailBody"
                                  style="height:600px;"
                                  required></textarea>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <button type="submit" class="btn btn-success"><span class="k-icon k-i-email"></span> Odeslat</button>
                <button type="button" class="btn btn-default" data-dismiss="modal"><span class="k-icon k-i-cancel-outline"></span> Neodesílat</button>
            </div>
        </form>
    </div>
</div>

<script>
    function titleCell(value) {
        return '<span title="' + value + '">' + value + '</span>';
    }

        function cellRemove() {
            return '<div class="text-center"><a role="button" class="text-danger k-grid-delete" href="#" title="Smazat záznam"><span class="k-icon k-i-delete"></span></a></div>';
        }

        function pCOnLine(PCOnLine, IDBackupProfile) {
            if (PCOnLine == 0) {
                return `<a href="@Url.Action("GrafDostupnosti", "Home")?IDBackupProfile=` + IDBackupProfile + `" target="_blank"><i class="fa fa-desktop text-muted" aria-hidden="true"></i></a>`
            }
            return `<a href="@Url.Action("GrafDostupnosti", "Home")?IDBackupProfile=` + IDBackupProfile + `" target="_blank"><i class="fa fa-desktop text-primary" aria-hidden="true"></i></a>`
        }

        function xmlConfig(rr_BackupUspesnostHodnota) {
            return '<a href="#" class="text-primary" data-bind="events: { click: openxml }">' + rr_BackupUspesnostHodnota + '</a>'
        }

        function getConfig(PCOnLine) {
            if (PCOnLine) {
                return '<a href="#" class="text-primary" data-bind="events: { click: getconfig }">XML</a>'
            } else {
                return '<span>XML</span>'
            }
        }

        function remoteBackup(PCOnLine) {
            if (PCOnLine) {
                return '<a href="#" class="text-primary" title="Manuální zálohování" data-bind="events: { click: remotebackup }"><i class="fa fa-cloud-download" aria-hidden="true"></i></a>'
            } else {
                return '<i class="fa fa-cloud-download text-muted" aria-hidden="true"></i>'
            }
        }

        function getTimes(PCOnLine) {
            if (PCOnLine) {
                return '<a href="#" class="text-primary" title="Nastavení časů" data-bind="events: { click: getTimes }"><i class="fa fa-clock-o" aria-hidden="true"></i></a>'
            } else {
                return '<i class="fa fa-clock-o text-muted" aria-hidden="true"></i>'
            }
        }

        function clientProgress(IDBackupProfile) {
            return `<div class="progress" style="height:100%;">
  <div id="progress` + IDBackupProfile + `" class="progress-bar progress-bar-striped active" role="progressbar" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100" style="width:0%;height:100%;">
    Průběh zálohy ...
  </div>
</div>`;
        }

        function dayEdit(container, options) {
            $('<input name="' + options.field + '" data-bind="value: ' + options.field + '" />')
                .appendTo(container)
                .kendoDropDownList({
                    dataTextField: "text",
                    dataValueField: "value",
                    dataSource: {
                        data: [
                            { value: 1, text: "pondělí" },
                            { value: 2, text: "úterý" },
                            { value: 3, text: "středa" },
                            { value: 4, text: "čtvrtek" },
                            { value: 5, text: "pátek" },
                            { value: 6, text: "sobota" },
                            { value: 7, text: "neděle" }
                        ]
                    },
                    change: function (e) {
                        options.model.set("day", e.sender.text());
                    }
                });
        }

        function timeEdit(container, options) {
            $('<input name="' + options.field + '" data-bind="value: ' + options.field + '" />')
                .appendTo(container)
                .kendoTimePicker();
        }

        function datumExpirace(e, d) {
            var v = kendo.toString(new Date(d), "yyyy-MM-dd HH:mm");
            if (e == 1) {
                return `<span style="color: red">` + v + `</span>`
            } else {
                return `<span>` + v + `</span>`
            }
        }

        function casPosledniZalohy(i, c) {
            switch (i) {
                case 1:
                    return `<span style="color: green">` + kendo.toString(new Date(c), "yyyy-MM-dd HH:mm") + `</span>`
                    break;
                case 2:
                    return `<span style="color: blue">` + kendo.toString(new Date(c), "yyyy-MM-dd HH:mm") + `</span>`
                    break;
                case 3:
                    return `<span style="color: orange">` + kendo.toString(new Date(c), "yyyy-MM-dd HH:mm") + `</span>`
                    break;
                case 4:
                    return `<span style="color: purple">` + kendo.toString(new Date(c), "yyyy-MM-dd HH:mm") + `</span>`
                    break;
            }
        }

        function downloadFile(BackupFileName, rr_BackupUspesnost) {
        if (rr_BackupUspesnost == 4 || rr_BackupUspesnost == 2) {
            return '<span class="text-secondary">' + BackupFileName.replace("~/", "") + '</span>'
        } else {
            return '<a href="@(Url.Action("DownloadBackupZip", "Home"))' + "?path=" + BackupFileName + '" target="_blank" class="text-primary">' + BackupFileName.replace("~/", "") + '</a>'
        }
        }

        function dovolenaEditor(container, options) {
            $('<input name="' + options.field + '" data-bind="value: ' + options.field + '" />')
                .appendTo(container)
                .kendoDatePicker({
                    change: function (e) {
                        viewModel.AGsp_Get_DoctorumBackupProfile.sync();
                    }
                });
        }

        function popisEditor(container, options) {
            var tx = $('<input class="k-textbox" name="' + options.field + '" data-bind="value: ' + options.field + '" />').appendTo(container);
            tx.change(function (e) {
                options.model.set("PopisZarizeni", $(this).val());
                viewModel.AGsp_Get_DoctorumBackupProfile.sync();
            })
        }

        var viewModel = null;
        var wintimes = null;
    var winmsg = null;
    var winhistorie = null;
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

            viewModel = kendo.observable({
                ucetnistavy: [
                    { value: 1, text: "Demo" },
                    { value: 2, text: "Objednáno" },
                    { value: 3, text: "Fa vystavena" },
                    { value: 4, text: "Zaplaceno" },
                    { value: 5, text: "Předplatné vypršelo" }
                ],
                toolbar: {
                    template: kendo.template($('#kient-toolbar-template').html()),
                    model: kendo.observable({
                        filter: 1,
                        hledej: "",
                        filters: new kendo.data.DataSource({
                            schema: { data: "data", total: "total" },
                            transport: { read: "@Url.Action("AGvwrr_BackupFilterProfile", "Api/Service")" }
                        }),
                        filter_change: function (e) {
                            e.preventDefault();
                            viewModel.AGsp_Get_DoctorumBackupProfile.page(1);
                            viewModel.AGsp_Get_DoctorumBackupProfile.read();
                        }
                    })
                },
                bctoolbar: {
                    template: kendo.template($('#backups-toolbar-template').html()),
                    model: kendo.observable({
                        selected: null,
                        visible: function () {
                            var d = this.get("selected");
                            if (d) {
                                return true;
                            }
                            return false;
                        },
                        mesicniReport: function (e) {
                            var selected = this.get("selected");
                            $.get('@Url.Action("EmailBodyMesicniReport", "Api/SafeBerry")', { iDBackupProfile: selected.IDBackupProfile, vaseID: selected.IDKlienta, emailTo: selected.Email }, function (result) {
                                if (result.error) { alert(result.error) } else {
                                    result.data.MDate = new Date();
                                    viewModel.set("mesicniReportData", result.data);
                                    $('#modal_mesicni_email').modal("show");
                                }
                            });
                        },
                        detailSendMessage: function (e) {
                            winmsg = $("#clienMessage").data("kendoWindow");
                            winmsg.open().center();
                        },
                        detailDostupnost: function (e) {
                            e.preventDefault();
                            var selected = this.get("selected");
                            window.open('@Url.Action("GrafDostupnosti", "Home")?IDBackupProfile=' + selected.IDBackupProfile, "_blank");
                        },
                        detailGetConfig: function (e) {
                            var selected = this.get("selected");
                            $.get("@Url.Action("GetConfig", "Api/Service")", { id: selected.IDBackupProfile, caller: "@User.Identity.Name.ToLower" }, null);
                        },
                        detailRemoteBackup: function (e) {
                            var selected = this.get("selected");
                            $.get("@Url.Action("RemoteBackup", "Api/Service")", { id: selected.IDBackupProfile }, null);
                        },
                        detailRemoteUpdate: function (e) {
                            var selected = this.get("selected");
                            $.get("@Url.Action("RemoteUpdate", "Api/SafeBerry")", { id: selected.IDBackupProfile }, null);
                        },
                        detailGetTimes: function (e) {
                            var selected = this.get("selected");
                            $.get("@Url.Action("GetTimes", "Api/SafeBerry")", { id: selected.IDBackupProfile, caller: "@User.Identity.Name.ToLower" }, null);
                        },
                        nahoditRestartovatValue: null,
                        nahoditRestartovat: function (e) {
                            var selected = this.get("selected");
                            var that = this;
                            $.get("@Url.Action("NahoditRestartovat", "Api/SafeBerry")", { id: selected.IDBackupProfile, stav: e.sender.value() }, function (e) {

                            });
                        },
                        firmy: new kendo.data.DataSource({
                            sort: { field: "firma", dir: "asc" },
                            schema: {
                                data: "data",
                                total: "total",
                                errors: "error"
                            },
                            transport: {
                                read: "@Url.Action("AGvw_FirmyAPobocky_CBX", "Api/Service")",
                                type: "GET"
                            }
                        }),
                        stavy: [
                            { value: 1, text: "Aktivní" },
                            { value: 2, text: "Demo" },
                            { value: 3, text: "Vpnutý" },
                            { value: 4, text: "Přeinstalováno" },
                            { value: 5, text: "Nově založený" },
                            { value: 9, text: "Testovací" }
                        ],
                        ambsw: [
                            { value: 1, text: "Medicus" },
                            { value: 2, text: "PC Doktor" },
                            { value: 3, text: "PC Dent" },
                            { value: 4, text: "SmartMedix" },
                            { value: 5, text: "Amicus" },
                            { value: 6, text: "xx" }
                        ],
                        save: function (e) {
                            var d = this.get("selected").toJSON();
                            if (e.sender) {
                                var id = $(e.sender.element).attr("id");
                                if (id == "rr_StavBackupProfile") {
                                    if (d.rr_StavBackupProfile == 1 || d.rr_StavBackupProfile == 2) {
                                        if (!d.KdoToJe.length > 0) {
                                            alert("Nejdříve zadej kdo to je");
                                            if (d.rr_StavBackupProfile == 2) {
                                                this.selected.set("rr_StavBackupProfile", 1);
                                            } else {
                                                this.selected.set("rr_StavBackupProfile", 2);
                                            }
                                            $("#KdoToJe").focus();
                                            return false;
                                        }
                                    }
                                }
                            }

                            d.DovolenaDo = kendo.toString(d.DovolenaDo, "yyyy-MM-dd HH:mm:ss");
                            d.DoKdyZaplaceno = kendo.toString(d.DoKdyZaplaceno, "yyyy-MM-dd HH:mm:ss");
                            d.DatumExpirace = kendo.toString(d.DatumExpirace, "yyyy-MM-dd HH:mm:ss");
                            $.post("@Url.Action("AGsp_Do_UDoctorumBackupProfile", "Api/Service")", d, function (result) {
                                if (result.error) {
                                    alert(result.error)
                                } else {
                                    viewModel.AGsp_Get_DoctorumBackupProfile.read();
                                }
                            });
                        }
                    })
                },
                clienMessageTitle: "SafeBerry - dispečink",
                clienMessage: "",
                mesicniReportData: {
                    emailTo: "",
                    emailSubject: "",
                    emailBody: "",
                    MDate: new Date()
                },
                mesicniEmailPost: function (e) {
                    e.preventDefault();
                    var that = this;
                    var selected = this.get("klientSelected");
                    var email = this.get("mesicniReportData");
                    $.post("@Url.Action("SendReportEmail", "Api/SafeBerry")", { iDBackupProfile: selected.IDBackupProfile, EmailTo: email.emailTo, Subject: email.emailSubject, Body: email.emailBody, MDate: kendo.toString(email.MDate, "yyyy-MM-ddTHH:mm:ss") }, function (result) {
                        if (result.error) {
                            alert(result.error);
                        } else {
                            $('#modal_mesicni_email').modal("hide");
                            that.historie.read();
                        }
                    });
                },
                clienMessageSendPopup: function (e) {
                    if (this.get("clienMessageTitle") && this.get("clienMessage")) {
                        var selected = this.get("klientSelected");
                        $.get("@Url.Action("ClientMessage", "Api/SafeBerry")", {
                            caller: "@User.Identity.Name.ToLower",
                            id: selected.IDBackupProfile,
                            title: this.get("clienMessageTitle"),
                            message: this.get("clienMessage"),
                            btn: 0,
                            icon: 3
                        }, function (result) {
                            winmsg.close();
                        });
                    } else {
                        alert("Vyplˇnte potřebné údaje")
                    }
                },
                clienMessageSendEmail: function (e) {
                    if (this.get("clienMessageTitle") && this.get("clienMessage")) {
                        var selected = this.get("klientSelected");
                        if (selected.Email) {
                            $.get("@Url.Action("ClientEmail", "Api/SafeBerry")", {
                                email: selected.Email,
                                subject: this.get("clienMessageTitle"),
                                body: this.get("clienMessage")
                            }, function (result) {
                                if (result) {
                                    alert(result);
                                } else {
                                    winmsg.close();
                                }
                            });
                        } else {
                            alert("Klient nemá vyplněn email");
                        }
                    } else {
                        alert("Vyplňte potřebné údaje")
                    }
                },
                clienMessageClose: function (e) {
                    winmsg.close();
                },
                close: function (e) {
                    wintimes.close();
                },
                openxml: function (e) {
                    let blob = new Blob([e.data.MetadataXML], { type: 'text/xml' });
                    let url = URL.createObjectURL(blob);
                    window.open(url);
                    URL.revokeObjectURL(url);
                },
                zmenaStavu: function (e) {
                    var that = viewModel;
                    switch (e.data.rr_StavBackupProfile) {
                        case 1:
                            e.data.set("rr_StavBackupProfile", 3);
                            break;
                        case 2:
                            e.data.set("rr_StavBackupProfile", 1);
                            break;
                        case 3:
                            e.data.set("rr_StavBackupProfile", 2);
                            break;
                    }
                    var d = e.data.toJSON();
                    d.DovolenaDo = kendo.toString(d.DovolenaDo, "yyyy-MM-dd HH:mm:ss");
                    d.DoKdyZaplaceno = kendo.toString(d.DoKdyZaplaceno, "yyyy-MM-dd HH:mm:ss");
                    $.post("@Url.Action("AGsp_Do_UDoctorumBackupProfile", "Api/Service")", d, function (result) {
                        if (result.error) {
                            alert(result.error)
                        } else {
                            that.klienti.read();
                        }
                    });
                },
                pobocky: new kendo.data.DataSource({
                    schema: {
                        data: "data",
                        total: "total",
                        errors: "error"
                    },
                    transport: {
                        read: {
                            url: "@Url.Action("AGvw_FirmyAPobocky_CBX", "Api/Service")",
                            type: "GET"
                        }
                    }
                }),
                AGsp_Get_DoctorumBackupProfile: new kendo.data.DataSource({
                    schema: {
                        model: {
                            id: "IDBackupProfile",
                            fields: {
                                IDKlienta: { type: "number", editable: false },
                                IDBackupProfile: { type: "number", editable: false },
                                Firma: { type: "string", editable: false },
                                PopisZarizeni: { type: "string", editable: false },
                                rr_StavBackupProfile: { type: "number", editable: false },
                                rr_StavBackupProfileHodnota: { type: "string", editable: false },
                                VerzeSluzby: { type: "string", editable: false },
                                DatumZalozeni: { type: "date", editable: false },
                                DatumExpirace: { type: "date", editable: false },
                                ExpiraceCervene: { type: "number", editable: false },
                                CasPosledniZalohy: { type: "date", editable: false },
                                DovolenaDo: { type: "date", editable: false },
                                BarvaPosledniZalohy: { type: "number", editable: false },
                                TextPosledniZalohy: { type: "string", editable: false },
                                PCOnLine: { type: "number", editable: false },
                                CasMax: { type: "date", editable: false },
                                Pocet: { type: "number", editable: false },
                                Prodleni: { type: "number", editable: false },
                                Poznamka: { type: "string", editable: false },
                                DoKdyZaplaceno: { type: "date", editable: false },
                                PosledniFaktura: { type: "string", editable: false },
                                rr_AmbulSW: { type: "number", editable: false },
                                PopisekAktualnihoBalicku: { type: "string", editable: false },
                                KdoToJe: { type: "string", editable: false },
                                rr_UcetniStavSB: { type: "number", editable: false },
                                rr_UcetniStavSBText: { type: "string", editable: false }
                            }
                        }
                    },
                    pageSize: 50,
                    transport: {
                        read: function (options) {
                            var filter = viewModel.toolbar.model.get("filter");
                            var hledej = viewModel.toolbar.model.get("hledej");
                            $.get("@Url.Action("AGsp_Get_DoctorumBackupProfile_G2", "Api/Service")", { rr_BackupFilterProfile: filter, hledej: hledej }, function (result) {
                                options.success(result.data);
                            });
                        }
                    }
                }),
                klientSelected: null,
                klient_change: function (e) {
                    var item = e.sender.dataItem(e.sender.select());
                    var d = item.toJSON();
                    if (item) {
                        this.set("klientSelected", item);
                        this.bctoolbar.model.set("selected", d);
                        if (item.Nazev_firmy) {
                            $("#klientident").text(" - " + item.Nazev_firmy + ", " + item.PopisZarizeni);
                        } else {
                            $("#klientident").text(" - " + item.PopisZarizeni);
                        }
                        this.historie.read();
                        this.zalohy.read();
                    }
                },
                historiePredmet: null,
                historieText: null,
                historieSave: function (e) {
                    var that = this;
                    if (this.get("historiePredmet") && this.get("historieText")) {
                        var selected = this.get("klientSelected");
                        $.post("@Url.Action("AGsp_Do_IHistory", "Api/Service")", {
                            iDBackupProfile: selected.IDBackupProfile,
                            predmet: this.get("historiePredmet"),
                            text: this.get("historieText"),
                        }, function (result) {
                            if (result.error) {
                                alert(result.error);
                            } else {
                                that.historie.read();
                                winhistorie.close();
                            }
                        });
                    } else {
                        alert("Vyplňte potřebné údaje")
                    }
                },
                historieClose: function (e) {
                    winhistorie.close()
                },
                historietoolbar: {
                    template: kendo.template($('#historie-toolbar-template').html()),
                    model: kendo.observable({
                        create: function (e) {
                            winhistorie = $("#pridatZaznamHistorie").data("kendoWindow");
                            winhistorie.open().center();
                        }
                    })
                },
                historie: new kendo.data.DataSource({
                    schema: {
                        data: "data",
                        errors: "error",
                        model: {
                            id: "IDHistory",
                            fields: {
                                CasVzniku: { type: "date" },
                                Predmet: { type: "string" },
                                Txt: { type: "string" },
                                UserLastName: { type: "string" }
                            }
                        }
                    },
                     transport: {
                        read: {
                            url: "@Url.Action("AGsp_Get_Historie", "Api/Service")",
                            type: "GET"
                        },
                        parameterMap: function (options, operation) {
                            var id = viewModel.klientSelected.get("IDBackupProfile")
                            return { iDBackupProfile: id };
                        }
                    }
                }),
                backuptimes: new kendo.data.DataSource({
                    schema: {
                        model: {
                            fields: {
                                day: { type: "string" },
                                time: { type: "string" }
                            }
                        }
                    }
                }),
                zalohy: new kendo.data.DataSource({
                    schema: {
                        data: "data",
                        errors: "error",
                        model: {
                            id: "IDBackup",
                            fields: {
                                BackupDate: { type: "date" }
                            }
                        }
                    },
                    transport: {
                        read: {
                            url: "@Url.Action("AGsp_Get_BackupZalohyJednohoProfilu", "Api/Service")",
                            type: "GET"
                        },
                        parameterMap: function (options, operation) {
                            var id = viewModel.klientSelected.get("IDBackupProfile")
                            return { iDBackupProfile: id };
                        }
                    },
                    requestEnd: function (e) {
                        viewModel.backuptimes.data(e.response.backuptimes);
                    }
                }),
                clientTimes: new kendo.data.DataSource({
                    schema: {
                        model: {
                            id: "id",
                            fields: {
                                dayint: { type: "number" },
                                day: { type: "string" },
                                time: { type: "date" }
                            }
                        }
                    },
                    transport: {
                        read: function (e) {
                            e.success([]);
                        },
                        create: function (e) {
                            var view = viewModel.clientTimes.view();
                            iudTime(e, view)
                        },
                        update: function (e) {
                            var view = viewModel.clientTimes.view();
                            iudTime(e, view)
                        },
                        destroy: function (e) {
                            var view = viewModel.clientTimes.view();
                            iudTime(e, view)
                        }
                    }
                })
            });
            kendo.bind(document.body, viewModel);

            @*$.get('@Url.Action("ReportEmail", "Api/SafeBerry")', { iDBackupProfile: 202, email: "novak@agilo.cz" }, function (result) {
                if (result.message) { alert(result.message) }
            });*@

            function iudTime(e, view) {
                var selected = viewModel.get("klientSelected");
                var iDBackupProfile = selected.IDBackupProfile;
                var xml = "<backuptimes>";
                $.each(view, function (i, d) {
                    xml += "<backuptime>";
                    xml += "<dayint>" + d.dayint + "</dayint>";
                    xml += "<day>" + d.day + "</day>";
                    xml += "<time>" + kendo.toString(d.time, "yyyy-MM-ddTHH:mm:ss") + "</time>";
                    xml += "</backuptime>";
                });
                xml += "</backuptimes>";
                $.get("@Url.Action("SetTimes", "Api/SafeBerry")", { id: iDBackupProfile, times: xml }, function (result) {
                    e.success();
                });
            }

            $(document).keyup(function (e) {
                if (e.key === "Escape") {
                    $.each($('[data-role="grid"]'), function () {
                        var grid = $(this).data("kendoGrid");
                        if (grid) {
                            if (grid.options) {
                                if (grid.options.editable) {
                                    grid.cancelChanges();
                                }
                            }
                        }
                    })
                }
            });

            $('[data-role="grid"]:not(.grid-batch)').delegate("tbody>tr", "keypress", function (e) {
                if (e.which == 13) {
                    var element = $(this).closest('[data-role="grid"]');
                    var grid = element.data("kendoGrid");
                    var opts = grid.getOptions();
                    if (opts.editable) {
                        $(this).find("input").trigger("change");
                        e.preventDefault();
                        grid.saveChanges();
                    }
                }
            });

            $('[data-role="grid"]').delegate("tbody>tr", "dblclick", function (e) {
                var element = $(this).closest('[data-role="grid"]');
                var grid = element.data("kendoGrid");
                var opts = grid.getOptions();
                if (opts.editable) {
                    var isedit = $(this).hasClass("k-grid-edit-row");
                    if (!isedit) {
                        grid.editRow($(this));
                    }
                }
            });

            var hub = $.connection.AgiloHub;
            hub.client.doctorumBackupChange = function () {
                viewModel.klienti.read();
            };
            hub.client.uploadChenge = function () {
                viewModel.zalohy.read();
            };
            hub.client.getDeviceInformationCallback = function (caller, infos) {
                if ('@User.Identity.Name' == caller) {
                    console.log(infos);
                }
            };
            hub.client.uploadConfig = function (xml, caller) {
                kendo.ui.progress($(document.body), false);
                if ('@User.Identity.Name' == caller) {
                    if (xml) {
                        let blob = new Blob([xml], { type: 'text/xml' });
                        let url = URL.createObjectURL(blob);
                        window.open(url);
                        URL.revokeObjectURL(url);
                    }
                }
            };
            hub.client.clientConnected = function (iDBackupProfile, isOnline) {
                var selected = viewModel.bctoolbar.model.get("selected");
                if (iDBackupProfile == selected.IDBackupProfile) {
                    selected.set("PCOnLine", isOnline);
                }
            };
            hub.client.clientProgress = function (iDBackupProfile, text, value, isIndeterminate) {
                var prg = $('#progress' + iDBackupProfile);
                if (prg.length > 0) {
                    prg.css("width", value + "%");
                    prg.text(text);
                    if (isIndeterminate) {
                        prg.addClass("progress-bar-striped");
                    } else {
                        prg.removeClass("progress-bar-striped");
                    }
                }
            };
            hub.client.getTimesCallback = function (caller, xml, iDBackupProfile) {
                if ('@User.Identity.Name' == caller) {
                    if (xml) {
                        var xmlDoc = $.parseXML(xml),
                            $xml = $(xmlDoc),
                            $backuptime = $xml.find("backuptime"),
                            obj = [];
                        $.each($backuptime, function (a, b) {
                            var $dayint = $(b).find("dayint")[0].textContent;
                            var $day = $(b).find("day")[0].textContent;
                            var $time = $(b).find("time")[0].textContent;
                            obj.push({ id: a, dayint: parseInt($dayint), day: $day, time: new Date($time) });
                        });
                        wintimes = $("#dialogClienTimes").data("kendoWindow");
                        wintimes.open().center();
                        viewModel.clientTimes.data(obj);
                    }
                }
            };
            hub.client.sendRemoteDesktop = function (base64) {
                $("#remotedesktop").attr("src", base64)
            };

            $.connection.hub.start({ jsonp: true }).done(function () {
                $.get("@Url.Action("GetDeviceInformation", "Api/SafeBerry")", { id: 175, caller: "@User.Identity.Name.ToLower", win32: "Win32_BIOS" }, null);
            });

            $.connection.hub.disconnected(function () {
                setTimeout(function () {
                    $.connection.hub.start({ jsonp: true });
                }, 5000); // Restart connection after 5 seconds.
            });
        });
</script>

@*Win32_1394Controller
Win32_1394ControllerDevice
Win32_AccountSID
Win32_ActionCheck
Win32_ActiveRoute
Win32_AllocatedResource
Win32_ApplicationCommandLine
Win32_ApplicationService
Win32_AssociatedBattery
Win32_AssociatedProcessorMemory
Win32_AutochkSetting
Win32_BaseBoard
Win32_Battery
Win32_Binary
Win32_BindImageAction
Win32_BIOS
Win32_BootConfiguration
Win32_Bus Win32_CacheMemory
Win32_CDROMDrive
Win32_CheckCheck
Win32_CIMLogicalDeviceCIMDataFile
Win32_ClassicCOMApplicationClasses
Win32_ClassicCOMClass
Win32_ClassicCOMClassSetting
Win32_ClassicCOMClassSettings
Win32_ClassInforAction
Win32_ClientApplicationSetting
Win32_CodecFile
Win32_COMApplicationSettings
Win32_COMClassAutoEmulator
Win32_ComClassEmulator
Win32_CommandLineAccess
Win32_ComponentCategory
Win32_ComputerSystem
Win32_ComputerSystemProcessor
Win32_ComputerSystemProduct
Win32_ComputerSystemWindowsProductActivationSetting
Win32_Condition
Win32_ConnectionShare
Win32_ControllerHastHub
Win32_CreateFolderAction
Win32_CurrentProbe
Win32_DCOMApplication
Win32_DCOMApplicationAccessAllowedSetting
Win32_DCOMApplicationLaunchAllowedSetting
Win32_DCOMApplicationSetting
Win32_DependentService
Win32_Desktop
Win32_DesktopMonitor
Win32_DeviceBus
Win32_DeviceMemoryAddress
Win32_Directory
Win32_DirectorySpecification
Win32_DiskDrive
Win32_DiskDrivePhysicalMedia
Win32_DiskDriveToDiskPartition
Win32_DiskPartition
Win32_DiskQuota
Win32_DisplayConfiguration
Win32_DisplayControllerConfiguration
Win32_DMAChanner
Win32_DriverForDevice
Win32_DriverVXD
Win32_DuplicateFileAction
Win32_Environment
Win32_EnvironmentSpecification
Win32_ExtensionInfoAction
Win32_Fan
Win32_FileSpecification
Win32_FloppyController
Win32_FloppyDrive
Win32_FontInfoAction
Win32_Group
Win32_GroupDomain
Win32_GroupUser
Win32_HeatPipe
Win32_IDEController
Win32_IDEControllerDevice
Win32_ImplementedCategory
Win32_InfraredDevice
Win32_IniFileSpecification
Win32_InstalledSoftwareElement
Win32_IP4PersistedRouteTable
Win32_IP4RouteTable
Win32_IRQResource
Win32_Keyboard
Win32_LaunchCondition
Win32_LoadOrderGroup
Win32_LoadOrderGroupServiceDependencies
Win32_LoadOrderGroupServiceMembers
Win32_LocalTime
Win32_LoggedOnUser
Win32_LogicalDisk
Win32_LogicalDiskRootDirectory
Win32_LogicalDiskToPartition
Win32_LogicalFileAccess
Win32_LogicalFileAuditing
Win32_LogicalFileGroup
Win32_LogicalFileOwner
Win32_LogicalFileSecuritySetting
Win32_LogicalMemoryConfiguration
Win32_LogicalProgramGroup
Win32_LogicalProgramGroupDirectory
Win32_LogicalProgramGroupItem
Win32_LogicalProgramGroupItemDataFile
Win32_LogicalShareAccess
Win32_LogicalShareAuditing
Win32_LogicalShareSecuritySetting
Win32_LogonSession
Win32_LogonSessionMappedDisk
Win32_MappedLogicalDisk
Win32_MemoryArray
Win32_MemoryArrayLocation
Win32_MemoryDevice
Win32_MemoryDeviceArray
Win32_MemoryDeviceLocation
Win32_MIMEInfoAction
Win32_MotherboardDevice
Win32_MoveFileAction
Win32_NamedJobObject
Win32_NamedJobObjectActgInfo
Win32_NamedJobObjectLimit
Win32_NamedJobObjectLimitSetting
Win32_NamedJobObjectProcess
Win32_NamedJobObjectSecLimit
Win32_NamedJobObjectSecLimitSetting
Win32_NamedJobObjectStatistics
Win32_NetworkAdapter
Win32_NetworkAdapterConfiguration
Win32_NetworkAdapterSetting
Win32_NetworkClient
Win32_NetworkConnection
Win32_NetworkLoginProfile
Win32_NetworkProtocol
Win32_NTDomain
Win32_NTEventlogFile
Win32_NTLogEvent
Win32_NTLogEventComputer
Win32_NTLogEvnetLog
Win32_NTLogEventUser
Win32_ODBCAttribute
Win32_ODBCDataSourceAttribute
Win32_ODBCDataSourceSpecification
Win32_ODBCDriverAttribute
Win32_ODBCDriverSoftwareElement
Win32_ODBCDriverSpecification
Win32_ODBCSourceAttribute
Win32_ODBCTranslatorSpecification
Win32_OnBoardDevice
Win32_OperatingSystem
Win32_OperatingSystemAutochkSetting
Win32_OperatingSystemQFE
Win32_OSRecoveryConfiguración
Win32_PageFile
Win32_PageFileElementSetting
Win32_PageFileSetting
Win32_PageFileUsage
Win32_ParallelPort
Win32_Patch
Win32_PatchFile
Win32_PatchPackage
Win32_PCMCIAControler
Win32_PerfFormattedData_ASP_ActiveServerPages
Win32_PerfFormattedData_ASPNET_114322_ASPNETAppsv114322
Win32_PerfFormattedData_ASPNET_114322_ASPNETv114322
Win32_PerfFormattedData_ASPNET_2040607_ASPNETAppsv2040607
Win32_PerfFormattedData_ASPNET_2040607_ASPNETv2040607
Win32_PerfFormattedData_ASPNET_ASPNET
Win32_PerfFormattedData_ASPNET_ASPNETApplications
Win32_PerfFormattedData_aspnet_state_ASPNETStateService
Win32_PerfFormattedData_ContentFilter_IndexingServiceFilter
Win32_PerfFormattedData_ContentIndex_IndexingService
Win32_PerfFormattedData_DTSPipeline_SQLServerDTSPipeline
Win32_PerfFormattedData_Fax_FaxServices
Win32_PerfFormattedData_InetInfo_InternetInformationServicesGlobal
Win32_PerfFormattedData_ISAPISearch_HttpIndexingService
Win32_PerfFormattedData_MSDTC_DistributedTransactionCoordinator
Win32_PerfFormattedData_NETCLRData_NETCLRData
Win32_PerfFormattedData_NETCLRNetworking_NETCLRNetworking
Win32_PerfFormattedData_NETDataProviderforOracle_NETCLRData
Win32_PerfFormattedData_NETDataProviderforSqlServer_NETDataProviderforSqlServer
Win32_PerfFormattedData_NETFramework_NETCLRExceptions
Win32_PerfFormattedData_NETFramework_NETCLRInterop
Win32_PerfFormattedData_NETFramework_NETCLRJit
Win32_PerfFormattedData_NETFramework_NETCLRLoading
Win32_PerfFormattedData_NETFramework_NETCLRLocksAndThreads
Win32_PerfFormattedData_NETFramework_NETCLRMemory
Win32_PerfFormattedData_NETFramework_NETCLRRemoting
Win32_PerfFormattedData_NETFramework_NETCLRSecurity
Win32_PerfFormattedData_NTFSDRV_ControladordealmacenamientoNTFSdeSMTP
Win32_PerfFormattedData_Outlook_Outlook
Win32_PerfFormattedData_PerfDisk_LogicalDisk
Win32_PerfFormattedData_PerfDisk_PhysicalDisk
Win32_PerfFormattedData_PerfNet_Browser
Win32_PerfFormattedData_PerfNet_Redirector
Win32_PerfFormattedData_PerfNet_Server
Win32_PerfFormattedData_PerfNet_ServerWorkQueues
Win32_PerfFormattedData_PerfOS_Cache
Win32_PerfFormattedData_PerfOS_Memory
Win32_PerfFormattedData_PerfOS_Objects
Win32_PerfFormattedData_PerfOS_PagingFile
Win32_PerfFormattedData_PerfOS_Processor
Win32_PerfFormattedData_PerfOS_System
Win32_PerfFormattedData_PerfProc_FullImage_Costly
Win32_PerfFormattedData_PerfProc_Image_Costly
Win32_PerfFormattedData_PerfProc_JobObject
Win32_PerfFormattedData_PerfProc_JobObjectDetails
Win32_PerfFormattedData_PerfProc_Process
Win32_PerfFormattedData_PerfProc_ProcessAddressSpace_Costly
Win32_PerfFormattedData_PerfProc_Thread
Win32_PerfFormattedData_PerfProc_ThreadDetails_Costly
Win32_PerfFormattedData_RemoteAccess_RASPort
Win32_PerfFormattedData_RemoteAccess_RASTotal
Win32_PerfFormattedData_RSVP_RSVPInterfaces
Win32_PerfFormattedData_RSVP_RSVPService
Win32_PerfFormattedData_Spooler_PrintQueue
Win32_PerfFormattedData_TapiSrv_Telephony
Win32_PerfFormattedData_Tcpip_ICMP
Win32_PerfFormattedData_Tcpip_IP
Win32_PerfFormattedData_Tcpip_NBTConnection
Win32_PerfFormattedData_Tcpip_NetworkInterface
Win32_PerfFormattedData_Tcpip_TCP
Win32_PerfFormattedData_Tcpip_UDP
Win32_PerfFormattedData_TermService_TerminalServices
Win32_PerfFormattedData_TermService_TerminalServicesSession
Win32_PerfFormattedData_W3SVC_WebService
Win32_PerfRawData_ASP_ActiveServerPages
Win32_PerfRawData_ASPNET_114322_ASPNETAppsv114322
Win32_PerfRawData_ASPNET_114322_ASPNETv114322
Win32_PerfRawData_ASPNET_2040607_ASPNETAppsv2040607
Win32_PerfRawData_ASPNET_2040607_ASPNETv2040607
Win32_PerfRawData_ASPNET_ASPNET
Win32_PerfRawData_ASPNET_ASPNETApplications
Win32_PerfRawData_aspnet_state_ASPNETStateService
Win32_PerfRawData_ContentFilter_IndexingServiceFilter
Win32_PerfRawData_ContentIndex_IndexingService
Win32_PerfRawData_DTSPipeline_SQLServerDTSPipeline
Win32_PerfRawData_Fax_FaxServices
Win32_PerfRawData_InetInfo_InternetInformationServicesGlobal
Win32_PerfRawData_ISAPISearch_HttpIndexingService
Win32_PerfRawData_MSDTC_DistributedTransactionCoordinator
Win32_PerfRawData_NETCLRData_NETCLRData
Win32_PerfRawData_NETCLRNetworking_NETCLRNetworking
Win32_PerfRawData_NETDataProviderforOracle_NETCLRData
Win32_PerfRawData_NETDataProviderforSqlServer_NETDataProviderforSqlServer
Win32_PerfRawData_NETFramework_NETCLRExceptions
Win32_PerfRawData_NETFramework_NETCLRInterop
Win32_PerfRawData_NETFramework_NETCLRJit
Win32_PerfRawData_NETFramework_NETCLRLoading
Win32_PerfRawData_NETFramework_NETCLRLocksAndThreads
Win32_PerfRawData_NETFramework_NETCLRMemory
Win32_PerfRawData_NETFramework_NETCLRRemoting
Win32_PerfRawData_NETFramework_NETCLRSecurity
Win32_PerfRawData_NTFSDRV_ControladordealmacenamientoNTFSdeSMTP
Win32_PerfRawData_Outlook_Outlook
Win32_PerfRawData_PerfDisk_LogicalDisk
Win32_PerfRawData_PerfDisk_PhysicalDisk
Win32_PerfRawData_PerfNet_Browser
Win32_PerfRawData_PerfNet_Redirector
Win32_PerfRawData_PerfNet_Server
Win32_PerfRawData_PerfNet_ServerWorkQueues
Win32_PerfRawData_PerfOS_Cache
Win32_PerfRawData_PerfOS_Memory
Win32_PerfRawData_PerfOS_Objects
Win32_PerfRawData_PerfOS_PagingFile
Win32_PerfRawData_PerfOS_Processor
Win32_PerfRawData_PerfOS_System
Win32_PerfRawData_PerfProc_FullImage_Costly
Win32_PerfRawData_PerfProc_Image_Costly
Win32_PerfRawData_PerfProc_JobObject
Win32_PerfRawData_PerfProc_JobObjectDetails
Win32_PerfRawData_PerfProc_Process
Win32_PerfRawData_PerfProc_ProcessAddressSpace_Costly
Win32_PerfRawData_PerfProc_Thread
Win32_PerfRawData_PerfProc_ThreadDetails_Costly
Win32_PerfRawData_RemoteAccess_RASPort
Win32_PerfRawData_RemoteAccess_RASTotal
Win32_PerfRawData_RSVP_RSVPInterfaces
Win32_PerfRawData_RSVP_RSVPService
Win32_PerfRawData_Spooler_PrintQueue
Win32_PerfRawData_TapiSrv_Telephony
Win32_PerfRawData_Tcpip_ICMP
Win32_PerfRawData_Tcpip_IP
Win32_PerfRawData_Tcpip_NBTConnection
Win32_PerfRawData_Tcpip_NetworkInterface
Win32_PerfRawData_Tcpip_TCP
Win32_PerfRawData_Tcpip_UDP
Win32_PerfRawData_TermService_TerminalServices
Win32_PerfRawData_TermService_TerminalServicesSession
Win32_PerfRawData_W3SVC_WebService
Win32_PhysicalMedia
Win32_PhysicalMemory
Win32_PhysicalMemoryArray
Win32_PhysicalMemoryLocation
Win32_PingStatus
Win32_PNPAllocatedResource
Win32_PnPDevice
Win32_PnPEntity
Win32_PnPSignedDriver
Win32_PnPSignedDriverCIMDataFile
Win32_PointingDevice
Win32_PortableBattery
Win32_PortConnector
Win32_PortResource
Win32_POTSModem
Win32_POTSModemToSerialPort
Win32_Printer
Win32_PrinterConfiguration
Win32_PrinterController
Win32_PrinterDriver
Win32_PrinterDriverDll
Win32_PrinterSetting
Win32_PrinterShare
Win32_PrintJob
Win32_Process
Win32_Processor
Win32_Product
Win32_ProductCheck
Win32_ProductResource
Win32_ProductSoftwareFeatures
Win32_ProgIDSpecification
Win32_ProgramGroup
Win32_ProgramGroupContents
Win32_Property
Win32_ProtocolBinding
Win32_Proxy
Win32_PublishComponentAction
Win32_QuickFixEngineering
Win32_QuotaSetting
Win32_Refrigeration
Win32_Registry
Win32_RegistryAction
Win32_RemoveFileAction
Win32_RemoveIniAction
Win32_ReserveCost
Win32_ScheduledJob
Win32_SCSIController
Win32_SCSIControllerDevice
Win32_SecuritySettingOfLogicalFile
Win32_SecuritySettingOfLogicalShare
Win32_SelfRegModuleAction
Win32_SerialPort
Win32_SerialPortConfiguration
Win32_SerialPortSetting
Win32_ServerConnection
Win32_ServerSession
Win32_Service
Win32_ServiceControl
Win32_ServiceSpecification
Win32_ServiceSpecificationService
Win32_SessionConnection
Win32_SessionProcess
Win32_Share
Win32_ShareToDirectory
Win32_ShortcutAction
Win32_ShortcutFile
Win32_ShortcutSAP
Win32_SID
Win32_SoftwareElement
Win32_SoftwareElementAction
Win32_SoftwareElementCheck
Win32_SoftwareElementCondition
Win32_SoftwareElementResource
Win32_SoftwareFeature
Win32_SoftwareFeatureAction
Win32_SoftwareFeatureCheck
Win32_SoftwareFeatureParent
Win32_SoftwareFeatureSoftwareElements
Win32_SoundDevice
Win32_StartupCommand
Win32_SubDirectory
Win32_SystemAccount
Win32_SystemBIOS
Win32_SystemBootConfiguration
Win32_SystemDesktop
Win32_SystemDevices
Win32_SystemDriver
Win32_SystemDriverPNPEntity
Win32_SystemEnclosure
Win32_SystemLoadOrderGroups
Win32_SystemLogicalMemoryConfiguration
Win32_SystemNetworkConnections
Win32_SystemOperatingSystem
Win32_SystemPartitions
Win32_SystemProcesses
Win32_SystemProgramGroups
Win32_SystemResources
Win32_SystemServices
Win32_SystemSlot
Win32_SystemSystemDriver
Win32_SystemTimeZone
Win32_SystemUsers
Win32_TapeDrive
Win32_TCPIPPrinterPort
Win32_TemperatureProbe
Win32_Terminal
Win32_TerminalService
Win32_TerminalServiceSetting
Win32_TerminalServiceToSetting
Win32_TerminalTerminalSetting
Win32_Thread
Win32_TimeZone
Win32_TSAccount
Win32_TSClientSetting
Win32_TSEnvironmentSetting
Win32_TSGeneralSetting
Win32_TSLogonSetting
Win32_TSNetworkAdapterListSetting
Win32_TSNetworkAdapterSetting
Win32_TSPermissionsSetting
Win32_TSRemoteControlSetting
Win32_TSSessionDirectory
Win32_TSSessionDirectorySetting
Win32_TSSessionSetting
Win32_TypeLibraryAction
Win32_UninterruptiblePowerSupply
Win32_USBController
Win32_USBControllerDevice
Win32_USBHub
Win32_UserAccount
Win32_UserDesktop
Win32_UserInDomain
Win32_UTCTime
Win32_VideoController
Win32_VideoSettings
Win32_VoltageProbe
Win32_VolumeQuotaSetting
Win32_WindowsProductActivation
Win32_WMIElementSetting
Win32_WMISetting*@