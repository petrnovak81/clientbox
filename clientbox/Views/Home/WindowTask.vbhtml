@ModelType Integer


@Code
    Layout = Nothing
End Code

<!DOCTYPE html>

<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>Vytvořit Task</title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.0/css/bootstrap.min.css">
    <link rel="stylesheet" href="https://kendo.cdn.telerik.com/2020.2.617/styles/kendo.bootstrap-v4.min.css" />

    <script src="https://kendo.cdn.telerik.com/2020.2.617/js/jquery.min.js"></script>

    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.0/js/bootstrap.min.js"></script>

    <script src="https://kendo.cdn.telerik.com/2020.2.617/js/kendo.all.min.js"></script>
    <script src="https://kendo.cdn.telerik.com/2020.2.617/js/cultures/kendo.culture.cs-CZ.min.js"></script>
    <script src="https://kendo.cdn.telerik.com/2020.2.617/js/messages/kendo.messages.cs-CZ.min.js"></script>

    <style>
        body {
            background-color: #f2f6ff;
        }

        * {
            font-size: 12px;
        }

        .container-fluid {
            height: 100vh;
        }

            .container-fluid form {
                justify-content: space-between;
                flex-direction: column;
                height: 100vh;
                display: flex;
            }

            .container-fluid .btn-holder {
                justify-content: flex-end;
                display: flex;
            }

        .k-grid td, .k-grid th {
            padding: 2px;
        }
    </style>
</head>
<body class="container-fluid">
    <form data-bind="events: { submit: ulozittask }">
        <table>
            <tbody>
                <tr>
                    <td width="250"><a href="#" data-bind="events: { click: pracovisteclick }">Pracoviště zákazníka <span class="k-icon k-i-plus"></span></a></td>
                    <td class="text-muted">Firma</td>
                    <td width="250"></td>
                    <td class="text-muted text-center border-left">Ticket ID</td>
                </tr>
                <tr>
                    <td class="border-bottom"><a href="#" data-bind="text: source.Nazev_firmy, events: { click: firmalink }"></a></td>
                    <td class="border-bottom" colspan="2"><span data-bind="text: source.Nadrizena_firma"></span>, <span data-bind="text: source.Ulice"></span>, <span data-bind="text: source.PSC"></span> <span data-bind="text: source.Mesto"></span></td>
                    <td class="text-center border-left border-bottom"><h1 data-bind="text: source.IDTicket"></h1></td>
                </tr>
                <tr>
                    <td class="p-2" colspan="4"></td>
                </tr>
                <tr>
                    <td class="text-muted">Řešitel</td>
                    <td>
                        <select data-role="dropdownlist" data-value-primitive="true" data-value-field="value" style="width:100%;" data-text-field="text" data-bind="value: source.IDUserResitel, source: users"></select>
                    </td>
                    <td class="text-muted pl-2">Vytvořil</td>
                    <td>
                        <select data-role="dropdownlist" data-value-primitive="true" data-value-field="value" style="width:100%;" data-text-field="text" data-bind="value: source.IDUserVytvoril, source: users"></select>
                    </td>
                </tr>
                <tr>
                    <td class="text-muted">Řešitel převzal</td>
                    <td><input data-bind="datetime: source.CasResitelPrevzal" class="form-control" readonly style="width:100%;" /></td>
                    <td class="text-muted pl-2">Vytvořeno [<span data-bind="text: source.CasVytvoreniDoba"></span>]</td>
                    <td><input data-bind="datetime: source.CasVytvoreni" class="form-control" readonly style="width:100%;" /></td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td class="text-muted pl-2">Dead Line [<span data-bind="text: source.DatumDeadlineDoba"></span>]</td>
                    <td><input data-role="datepicker" data-bind="value: source.DatumDeadLine" style="width:100%;" /></td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td class="text-muted pl-2">Priorita</td>
                    <td>
                        <select data-role="dropdownlist" data-value-primitive="true" data-value-field="value" style="width:100%;" data-text-field="text" data-bind="value: source.rr_TicketPriorita, source: priority"></select>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td class="text-muted pl-2">Stav</td>
                    <td>
                        <select data-role="dropdownlist" data-value-primitive="true" data-value-field="value" style="width:100%;" data-text-field="text" data-bind="value: source.rr_TicketStav, source: stavy"></select>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" class="text-muted">Předmět</td>
                </tr>
                <tr>
                    <td colspan="4"><input type="text" class="form-control" placeholder="" data-bind="value: source.Predmet" /></td>
                </tr>
                <tr>
                    <td colspan="4" class="text-muted">Tělo</td>
                </tr>
                <tr>
                    <td colspan="4"><textarea class="form-control" rows="6" placeholder="" data-bind="value: source.Telo"></textarea></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <span class="text-muted">Historie</span>
                        <div data-role="grid"
                             data-toolbar="#=toolbarTmp()#"
                             data-columns="[
                             { field: 'Cas', title: 'Čas', format: '{0:dd.MM.yyyy HH:mm}', width: 150 },
                             { field: 'Komentar', title: 'Komentář' }
                             ]" data-bind="source: historie, events: { dataBound: databound }" style="height:150px;"></div>
                    </td>
                </tr>
            </tbody>
        </table>
        <div class="btn-holder mb-3">
            <button type="submit" class="btn btn-primary">Uložit</button>
            <a class="btn btn-light ml-2" onclick="javascript:window.close('','_parent','');">Zavřít</a>
        </div>
    </form>

    <div class="modal fade" id="modalkomentar">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">

                <div class="modal-header">
                    <h4 class="modal-title">Komentář</h4>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>

                <div class="modal-body">
                    <textarea class="form-control" data-bind="value: selecteditem.Komentar" rows="4"></textarea>
                </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-bind="events: { click: modalsave }">Uložit</button>
                    <button type="button" class="btn btn-secondary" data-bind="events: { click: modalcancel }">Zrušit</button>
                </div>

            </div>
        </div>
    </div>

    <div class="modal fade" id="modalpracoviste">
        <div class="modal-dialog modal-dialog-centered modal-xl modal-dialog-scrollable" style="max-width: 100%;margin: 1.75rem auto;">
            <div class="modal-content">

                <div class="modal-header">
                    <h4 class="modal-title">Pracoviště zákazníka</h4>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>

                <div class="modal-body">
                    <form data-bind="events: { submit: hledat }" style="height:40px;">
                        <div class="input-group">
                            <input type="search" class="form-control" data-bind="value: hledatvalue" placeholder="Hledat..." aria-label="Hledat...">
                            <div class="input-group-append">
                                <button class="btn btn-secondary" type="submit"><span class="k-icon k-i-zoom"></span></button>
                            </div>
                        </div>
                    </form>
                    <div data-role="grid"
                         data-selectable="true"
                         data-columns="[
                             { 'field' : 'NazevPobocky' , 'title' : 'Pracoviště' , 'width' : 250 },
                             { 'field' : 'rr_TypAdresy' , 'title' : 'Typ pobočky' , 'width' : 110 },
                             { 'field' : 'Nazev_firmy' , 'title' : 'Firma' },
                             { 'field' : 'AdresaPracoviste' , 'title' : 'Adresa pracoviště' }
                             ]" data-bind="source: firmy, events: { change: pracovistechange }" style="height:100%;width:100%;"></div>
                </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-bind="events: { click: modalpracovisteselect }">Vybrat</button>
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Zrušit</button>
                </div>

            </div>
        </div>
    </div>

    <span id="knotification"></span>

    <script>
        kendo.culture("cs-CZ");

        var idticket = '@Model';

        function toolbarTmp() {
            return '<button type="button" class="k-button" data-bind="events: { click: novykom }"><span class="k-icon k-i-plus"></span>Přidat komentář</button>';
        };

        var popupNotification = null;
        function kendoNotification(msg, type, ms) {
            if (type === undefined) { type = "info" };
            if (ms === undefined) { ms = 5000 };
            var badgeid = 'badge_' + kendo.guid();
            var s = ms / 1000;
            msg = '<span class="badge badge-pill badge-light" id="' + badgeid + '">' + s + '</span> ' + msg;
            popupNotification.setOptions({ autoHideAfter: ms });
            popupNotification.show(msg, type);
            var int = setInterval(function (e) {
                s = s - 1;
                $("#" + badgeid).text(s);
                if (s < 1) {
                    clearInterval(int);
                }
            }, 1000);
        };

        (function (global) {

            popupNotification = $("#knotification").kendoNotification({
                autoHideAfter: 0,
                show: function (e) { },
                position: {
                    pinned: false,
                    top: 20,
                    right: 20,
                },
                stacking: "bottom"
            }).data("kendoNotification");

            kendo.data.binders.datetime = kendo.data.Binder.extend({
                init: function (element, bindings, options) {
                    kendo.data.Binder.fn.init.call(this, element, bindings, options);
                },
                refresh: function () {
                    var data = this.bindings["datetime"].get();
                    if (data) {
                        var obj = new Date(data);
                        var d = kendo.toString(obj, "dd.MM.yyyy HH:mm");
                        if ($(this.element).is("input")) {
                            $(this.element).val(d);
                        } else {
                            $(this.element).text(d);
                        }
                    } else {
                        if ($(this.element).is("input")) {
                            $(this.element).val("");
                        } else {
                            $(this.element).text("");
                        }
                    }
                }
            });

            var viewModel = kendo.observable({
                source: {},
                users: [
                    { value: 10, text: "Hanzl" },
                    { value: 6, text: "Novák" },
                    { value: 8, text: "Nezbeda" },
                    { value: 7, text: "Pecher" },
                    { value: 12, text: "Frolíková" }
                ],
                priority: [
                    { value: 1, text: "Nízká - " },
                    { value: 2, text: "Normální !" },
                    { value: 3, text: "Vysoká !!" }
                ],
                stavy: [
                    { value: 1, text: "Nový" },
                    { value: 2, text: "Makám na tom" },
                    { value: 3, text: "Odložil jsem" },
                    { value: 4, text: "Ukončen" },
                    { value: 5, text: "Stornován" }
                ],
                pracovisteclick: function (e) {
                    this.set("pracovisteselected", null);
                    $("#modalpracoviste").modal("show");
                },
                firmalink: function () {
                    var pobocka = this.source.get("Firma");
                    return "http://clientbox.cz:40086/Home/Klienti?f=" + pobocka;
                },
                modalpracovisteselect: function (e) {
                    var selected = this.get("pracovisteselected");
                    if (selected) {
                        this.source.set("Firma", selected.Pobocka);
                        $("#modalpracoviste").modal("hide");
                        this.ulozittask(e);
                    } else {
                        alert("Vyberte pracoviště");
                    }
                },
                pracovisteselected: null,
                pracovistechange: function (e) {
                    var di = e.sender.dataItem(e.sender.select());
                    this.set("pracovisteselected", di.toJSON());
                },
                hledatvalue: "",
                hledat: function (e) {
                    e.preventDefault();
                    this.set("pracovisteselected", null);
                    this.firmy.read();
                },
                firmy: new kendo.data.DataSource({
                schema: {
                    model: {
                        fields: {
                            ParametryValidni: { type: "number" },
                        }
                    }
                },
                transport: {
                    read: function (options) {
                        var h = viewModel.get("hledatvalue");
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
                historie: new kendo.data.DataSource({
                    schema: {
                        data: "data",
                        total: "total",
                        errors: "error",
                        model: {
                            id: "IDTicketHistorie",
                            fields: {
                                Cas: { type: "date" },
                                Komentar: { type: "string" }
                            }
                        }
                    },
                    pageSize: 100,
                    transport: {
                        read: {
                            url: function () {
                                return '@Url.Action("AGsp_Get_TicketHistorie", "Api/Service")' + "?iDTicket=" + idticket;
                            }
                        },
                        parameterMap: function (data, type) {
                            var pm = kendo.data.transports.odata.parameterMap(data);
                            return pm;
                        }
                    },
                    error: function (e) {
                        e.sender.cancelChanges();
                        if (e.errors) {
                            alert(e.errors);
                        } else {
                            alert("Systémová chyba aplikace. Kontaktujte vývojáře.");
                        }
                    }
                }),
                selecteditem: null,
                modalcancel: function (e) {
                    this.historie.cancelChanges();
                    $("#modalkomentar").modal("hide");
                },
                modalsave: function (e) {
                    var that = this;
                    var komentar = this.selecteditem.get("Komentar");
                    $.get('@Url.Action("AGsp_Do_ITicketZapisDoHistorie", "Api/Service")', { iDTicket: idticket, komentar: komentar }, function (result) {
                        if (result.error) { alert(result.error) } else {
                            that.historie.read();
                            $("#modalkomentar").modal("hide");
                        }
                    });
                },
                databound: function (e) {
                    var that = this;
                    var toolbar = kendo.observable({
                        novykom: function (e) {
                            var item = that.historie.insert(0, { Cas: new Date(), Komentar: "" });
                            that.set("selecteditem", item);
                            $("#modalkomentar").modal("show");
                        }
                    });
                    kendo.bind($(e.sender.element).find(".k-grid-toolbar"), toolbar);
                },
                ulozittask: function (e) {
                    e.preventDefault();
                    var selected = this.get("source").toJSON();
                    if (selected.DatumDeadLine) {
                        selected.DatumDeadLine = kendo.toString(selected.DatumDeadLine, "yyyy-MM-ddTHH:mm:ss");
                    }
                    $.post('@Url.Action("AGsp_Do_IUTicket", "Api/Service")', selected, function (result) {
                        if (result.error) { alert(result.error) } else {
                            getSource();
                            kendoNotification("Task uložen", "success", 5000);
                        }
                    });
                }
            });

            kendo.bind($("body"), viewModel);

            function getSource() {
                $.get('@Url.Action("AGsp_Get_TicketDetail", "Api/Service")', { iDTicket: idticket }, function (result) {
                    if (result.error) { alert(result.error) } else {
                        viewModel.set("source", result.data);
                    }
                });
            }

            getSource();
        })(window);
    </script>
</body>
</html>
