@Code
    Layout = Nothing

    Dim db As New Data4995Entities
    Dim _user = db.AG_tblUsers.FirstOrDefault(Function(e) e.UserLogin = User.Identity.Name)
    Dim _iduser = 0
    If _user IsNot Nothing Then
        _iduser = _user.IDUser
    End If
    End Code

<!DOCTYPE html>

<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <meta name="theme-color" content="#28a745">
    <meta name="msapplication-navbutton-color" content="#28a745">
    <meta name="apple-mobile-web-app-status-bar-style" content="#28a745">
    <uses-permission android:name="android.permission.RECORD_AUDIO" />
    <style>
        html {
            font-size: 14px;
            font-family: Arial, Helvetica, sans-serif;
        }
    </style>
    <title></title>
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>

    <link rel="stylesheet" href="https://kendo.cdn.telerik.com/2020.3.1118/styles/kendo.bootstrap-v4.min.css" />
    <link rel="stylesheet" href="https://kendo.cdn.telerik.com/2020.3.1118/styles/kendo.mobile.all.min.css" />
    <link href="~/Content/mobile.css" rel="stylesheet" />

    <script src="https://kendo.cdn.telerik.com/2020.3.1118/js/jquery.min.js"></script>
    <script src="https://kendo.cdn.telerik.com/2020.3.1118/js/kendo.all.min.js"></script>
    <script src="https://kendo.cdn.telerik.com/2020.3.1118/js/cultures/kendo.culture.cs-CZ.min.js"></script>
    <script src="https://kendo.cdn.telerik.com/2020.3.1118/js/messages/kendo.messages.cs-CZ.min.js"></script>

    <script>
        var speech = window['SpeechRecognition'] || window['webkitSpeechRecognition'];
        var recognition = new speech();
        recognition.continuous = true;
        recognition.lang = 'cs-CZ';
        recognition.interimResults = true;
        recognition.onerror = function (event) {
            console.log('onerror', event);
        }
        recognition.onresult = function (event) {
            for (var i = event.resultIndex; i < event.results.length; ++i) {
                if (event.results[i].isFinal) {
                    console.log('onresult', event.results[i][0].transcript);
                }
            }
        }
        recognition.start();

        //speechRecognition.onresult = console.log;
        //speechRecognition.start();

        //if (!('webkitSpeechRecognition' in window)) {
        //    console.log("nepovoleno");
        //} else {
        //    console.log("povoleno");
        //}

        //var recognition = new webkitSpeechRecognition();
        //recognition.continuous = true;
        //recognition.lang = 'cs-CZ';
        //recognition.interimResults = true;

        //recognition.onresult = function (event) {
        //    console.log(event);
        //}

        //recognition.onspeechend = function () {
        //    recognition.stop();
        //}

        //recognition.onnomatch = function (event) {
        //    console.log("I didn't recognise that color.");
        //}

        //recognition.onerror = function (event) {
        //    console.log('Error occurred in recognition: ' + event.error);
        //}

        //recognition.start();
    </script>
</head>
<body>
    <div id="main" data-role="view" data-title="Pracovní listy" data-model="app.main" data-show="app.main.show">
        <div data-role="header">
            <div data-role="navbar">
                <span data-role="view-title"></span>
                <a data-role="button" data-align="right" data-click="record">🎤</a>
                <a href="#pracak-komu" data-role="button" data-align="right" data-icon="add"></a>
            </div>
        </div>
        <ul data-role="listview"
            data-auto-bind="false"
            data-pull-to-refresh="false"
            data-bind="source: praclist"
            data-filterable="{field: 'Firma', operator: 'contains', placeholder: 'Hledat...'}"
            data-template="tmp_prac_list"></ul>
    </div>

    <div id="pracak-komu" data-role="view" data-title="Komu" data-model="app.pracak" data-show="app.pracak.showkomu">
        <div data-role="header">
            <div data-role="navbar">
                <a data-role="backbutton" data-align="left">Back</a>
                <span data-role="view-title"></span>
            </div>
        </div>
        <ul data-role="listview"
            data-auto-bind="false"
            data-pull-to-refresh="false"
            data-bind="source: klienti"
            data-filterable="{field: 'Firma', operator: 'contains', placeholder: 'Hledat...'}"
            data-template="tmp_klient"></ul>
    </div>

    <div id="pracak" data-role="view" data-title="Pracak" data-model="app.pracak" data-show="app.pracak.showdetail">
        <div data-role="header">
            <div data-role="navbar">
                <a data-role="backbutton" href="#main" data-align="left">Back</a>
                <span data-role="view-title"></span>
            </div>
        </div>
        <form class="form-content p-3" data-bind="events: { submit: pracak_save }">
            <label class="km-required text-muted">Datum výkonu</label>
            <input type="date" class="km-justified k-textbox" data-bind="value: detail.DatVzniku" required />
            <label class="mt-3 text-muted">Pobočka</label>
            <input type="text" class="km-justified k-textbox" data-bind="value: detail.NazevPobocky" readonly />
            <label class="mt-3 text-muted">Firma</label>
            <input type="text" class="km-justified k-textbox" data-bind="value: detail.Nazev_firmy" readonly />
            <label class="mt-3 text-muted">Vyúčtováno fakturou</label>
            <input type="text" class="km-justified k-textbox" data-bind="value: detail.CisloFaktury" readonly />
            <label class="mt-3 text-muted">Interní poznámka</label>
            <textarea rows="3" maxlength="255" class="km-justified k-textbox" data-bind="value: detail.Poznamka"></textarea>
            <a data-bind="events: { click: btnpolozky }" class="km-justified km-button mt-3">Popložky</a>
            <button type="submit" class="km-justified btn btn-success text-white mt-1">Uložit</button>
        </form>
    </div>

    <div id="pracak-polozky" data-role="view" data-title="Výkaz práce - položky" data-model="app.pracak" data-show="app.pracak.showpolozky">
        <div data-role="header">
            <div data-role="navbar">
                <a data-role="backbutton" data-align="left">Back</a>
                <span data-role="view-title"></span>
                <a data-bind="events: { click: add_polozka }" data-role="button" data-align="right" data-icon="add"></a>
            </div>
        </div>
        <ul data-role="listview"
            data-auto-bind="false"
            data-pull-to-refresh="false"
            data-bind="source: polozky"
            data-template="tmp_polozka"></ul>
    </div>

    <div id="pracak-polozka" data-role="view" data-title="Položky" data-model="app.pracak" data-show="app.pracak.showpolozka">
        <div data-role="header">
            <div data-role="navbar">
                <a data-role="backbutton" data-align="left">Back</a>
                <span data-role="view-title"></span>
            </div>
        </div>
        <div class="form-content p-3">
            <label class="text-muted">Typ položky</label>
            <input data-role="dropdownlist" data-value-primitive="true" data-value-field="value" data-text-field="text" data-bind="source: typy_source, value: polozka_source.rr_TypPolozkyPracaku, events: { change: typ_change }" class="km-justified k-textbox">
        </div>
        <form class="form-content p-3" data-bind="visible: polozka_visible(1), events: { submit: polozka_save }">
            <label class="text-muted km-required">Produkt</label>
            <input data-role="dropdownlist" data-filter="true" data-value-primitive="true" data-value-field="Produkt" data-text-field="Popis" data-bind="source: produkty, value: polozka_source.Produkt" class="km-justified k-textbox">
            <label class="mt-3 text-muted">Text na fakturu</label>
            <textarea rows="3" maxlength="250" class="km-justified k-textbox" data-bind="value: polozka_source.TextNaFakturu"></textarea>
            <label class="mt-3 text-muted">Text do emailu</label>
            <textarea rows="3" maxlength="2000" class="km-justified k-textbox" data-bind="value: polozka_source.TextInterniDoMailu"></textarea>
            <label class="mt-3 text-muted">Počet EMJ</label>
            <input type="number" class="km-justified k-textbox" data-bind="value: polozka_source.PocetEMJ" />
            <label class="mt-3 text-muted">Prodejní cena</label>
            <input type="number" class="km-justified k-textbox" data-bind="value: polozka_source.CenaEMJ" />
            <button type="submit" class="km-justified btn btn-success text-white mt-3">Uložit</button>
        </form>
        <form class="form-content p-3" data-bind="visible: polozka_visible(2), events: { submit: polozka_save }">
            <label class="text-muted km-required">Služba</label>
            <input data-role="dropdownlist" data-filter="true" data-value-primitive="true" data-value-field="Produkt" data-text-field="Popis" data-bind="source: sluzby, value: polozka_source.Produkt" class="km-justified k-textbox">
            <label class="mt-3 text-muted">Text na fakturu</label>
            <textarea rows="3" maxlength="250" class="km-justified k-textbox" data-bind="value: polozka_source.TextNaFakturu"></textarea>
            <label class="mt-3 text-muted">Text do emailu</label>
            <textarea rows="3" maxlength="2000" class="km-justified k-textbox" data-bind="value: polozka_source.TextInterniDoMailu"></textarea>
            <label class="mt-3 text-muted">Počet EMJ</label>
            <input type="number" class="km-justified k-textbox" data-bind="value: polozka_source.PocetEMJ" />
            <label class="mt-3 text-muted">Prodejní cena</label>
            <input type="number" class="km-justified k-textbox" data-bind="value: polozka_source.CenaEMJ" />
            <button type="submit" class="km-justified btn btn-success text-white mt-3">Uložit</button>
        </form>
        <form class="form-content p-3" data-bind="visible: polozka_visible(3), events: { submit: polozka_save }">
            <div class="text-right">
                <label class="mt-2 text-muted float-left">Vydálenka</label>
                <input type="checkbox" data-role="switch" data-bind="checked: polozka_source.Vzdalenka" />
            </div>
            <div class="mt-3 text-right" data-bind="visible: polozka_source.Vzdalenka">
                <label class="mt-2 text-muted float-left">Malý/Velký zásah</label>
                <input type="checkbox" data-role="switch" data-bind="checked: polozka_source.Zasah, events: { change: zasah_change }" />
            </div>
            <label class="mt-3 text-muted km-required">Od</label>
            <input type="time" data-bind="value: polozka_source.CasOd" class="km-justified k-textbox" required />
            <label class="mt-3 text-muted km-required">Do</label>
            <input type="time" data-bind="value: polozka_source.CasDo" class="km-justified k-textbox" required />
            <label class="mt-3 text-muted km-required">Text na fakturu</label>
            <textarea rows="3" maxlength="250" class="km-justified k-textbox" data-bind="value: polozka_source.TextNaFakturu"></textarea>
            <label class="mt-3 text-muted">Text do emailu</label>
            <textarea rows="3" maxlength="2000" class="km-justified k-textbox" data-bind="value: polozka_source.TextInterniDoMailu"></textarea>
            <div class="mt-3 text-right">
                <label class="mt-2 text-muted float-left">Zdarma</label>
                <input type="checkbox" data-role="switch" data-bind="checked: polozka_source.Zdarma" />
            </div>
            <button type="submit" class="km-justified btn btn-success text-white mt-3">Uložit</button>
        </form>
        <form class="form-content p-3" data-bind="visible: polozka_visible(4), events: { submit: polozka_save }">
            <div class="text-right">
                <label class="mt-2 text-muted float-left">Vydálenka</label>
                <input type="checkbox" data-role="switch" data-bind="checked: polozka_source.Vzdalenka" />
            </div>
            <div class="mt-3 text-right" data-bind="visible: polozka_source.Vzdalenka">
                <label class="mt-2 text-muted float-left">Malý/Velký zásah</label>
                <input type="checkbox" data-role="switch" data-bind="checked: polozka_source.Zasah, events: { change: zasah_change }" />
            </div>
            <label class="mt-3 text-muted km-required">Hodin</label>
            <input type="number" data-bind="value: polozka_source.Hodin" class="km-justified k-textbox" required />
            <label class="mt-3 text-muted">Text na fakturu</label>
            <textarea rows="3" maxlength="250" class="km-justified k-textbox" data-bind="value: polozka_source.TextNaFakturu"></textarea>
            <label class="mt-3 text-muted">Text do emailu</label>
            <textarea rows="3" maxlength="2000" class="km-justified k-textbox" data-bind="value: polozka_source.TextInterniDoMailu"></textarea>
            <div class="mt-3 text-right">
                <label class="mt-2 text-muted float-left">Zdarma</label>
                <input type="checkbox" data-role="switch" data-bind="checked: polozka_source.Zdarma" />
            </div>
            <button type="submit" class="km-justified btn btn-success text-white mt-3">Uložit</button>
        </form>
        <form class="form-content p-3" data-bind="visible: polozka_visible(5), events: { submit: polozka_save }">
            <label class="mt-3 text-muted">Text na fakturu</label>
            <textarea rows="3" maxlength="250" class="km-justified k-textbox" data-bind="value: polozka_source.TextNaFakturu"></textarea>
            <label class="mt-3 text-muted km-required">Účtovaných KM</label>
            <input type="number" class="km-justified k-textbox" data-bind="value: polozka_source.PocetEMJ" />
            <button type="submit" class="km-justified btn btn-success text-white mt-3">Uložit</button>
        </form>
        <form class="form-content p-3" data-bind="visible: polozka_visible(6), events: { submit: polozka_save }">
            <label class="mt-3 text-muted">Text na fakturu</label>
            <textarea rows="5" maxlength="250" class="km-justified k-textbox" data-bind="value: polozka_source.TextNaFakturu"></textarea>
            <label class="mt-3 text-muted km-required">Účtovaných KM</label>
            <input type="number" class="km-justified k-textbox" data-bind="value: polozka_source.PocetEMJ" required />
            <button type="submit" class="km-justified btn btn-success text-white mt-3">Uložit</button>
        </form>
        <form class="form-content p-3" data-bind="visible: polozka_visible(7), events: { submit: polozka_save }">
            <label class="mt-3 text-muted">Text na fakturu</label>
            <textarea rows="5" maxlength="250" class="km-justified k-textbox" data-bind="value: polozka_source.TextNaFakturu"></textarea>
            <label class="mt-3 text-muted km-required">Účtovaných KM</label>
            <input type="number" class="km-justified k-textbox" data-bind="value: polozka_source.PocetEMJ" required />
            <button type="submit" class="km-justified btn btn-success text-white mt-3">Uložit</button>
        </form>
    </div>

    <div id="calendar" data-role="view" data-title="Kalendář" data-model="app.calendar">
        <div data-role="scheduler"
             data-editable="false"
             data-footer="false"
             data-mobile="phone"
             data-views="[{ type: 'day', eventTemplate: $('#event-template').html() }]"
             data-bind="source: source, events: { dataBound: databound, navigate: navigate }"></div>
    </div>

    @*sablony*@
    <script type="text/x-kendo-template" id="tmp_prac_list">
        # if (rr_StavPracaku < 20) { #
        <a class="km-listview-link" href="\\#pracak?i=${IDVykazPrace}">
            <div class="km-thumbnail pracak-#:rr_StavPracaku#"></div>
            <div class="list-subtitle">
                <span class="badge badge-info">\#${IDVykazPrace}</span>
                <span class="badge state-${rr_StavPracaku}">#=rr_StavPracakuHodnota#</span>
                <span class="badge badge-light float-right">#=kendo.toString(new Date(DatVzniku), "dd.MM.yyyy")#</span>
            </div>
            <div class="list-title text-truncate">${Firma}</div>
        </a>
        <a data-role="detailbutton" data-i="${IDVykazPrace}" data-bind="events: { click: storno }" data-icon="trash"></a>
        # } else { #
        <a class="km-listview-link" onclick="kendoAlert('Pracák', 'Nelze upravovat pracák, který není ve stavu pořizován nebo validovat.');">
            <div class="km-thumbnail pracak-#:rr_StavPracaku#"></div>
            <div class="list-subtitle">
                <span class="badge badge-info">\#${IDVykazPrace}</span>
                <span class="badge state-${rr_StavPracaku}">#=rr_StavPracakuHodnota#</span>
                <span class="badge badge-light float-right">#=kendo.toString(new Date(DatVzniku), "dd.MM.yyyy")#</span>
            </div>
            <div class="list-title text-truncate">${Firma}</div>
        </a>
        # } #
    </script>
    <script type="text/x-kendo-template" id="tmp_klient">
        <a data-bind="click: klient_select" class="km-listview-link">
            <div class="km-thumbnail contact"></div>
            <div class="list-title">${Nazev_firmy}</div>
            <div class="list-subtitle">${Firma}</div>
        </a>
    </script>
    <script type="text/x-kendo-template" id="tmp_polozka">
        <a class="km-listview-link" href="\\#pracak-polozka?i=${IDVykazPrace}&ip=${IDVykazPracePol}">
            <div class="km-thumbnail polozka p${rr_TypPolozkyPracaku}"></div>
            <div class="list-title text-truncate">${rr_TypPolozkyPracakuHodnota}</div>
            <div class="list-subtitle text-truncate">${TextNaFakturu}, Počet: ${PocetEMJ}</div>
        </a>
        <a data-role="detailbutton" data-i="${IDVykazPrace}" data-ip="${IDVykazPracePol}" data-bind="events: { click: deletepolozka }" data-icon="trash"></a>
    </script>
    <script id="event-template" type="text/x-kendo-template">
        <a href="\#" class="text-white" title="${title} 🕓 #=kendo.toString(new Date(start), 'HH:mm')# - #=kendo.toString(new Date(end), 'HH:mm')# ${(location ? location : '')}" style="line-height:16px" data-bind="events: { click: event_click }">
            # if(iDT) { #
            <span class="badge badge-light text-dark">\#${iDT}</span>
            # } #
            <span>${title}</span>
            <div><small>🕓 #=kendo.toString(new Date(start), "HH:mm")# - #=kendo.toString(new Date(end), "HH:mm")# ${(location ? location : '')}</small></div>
        </a>
    </script>
    @*sablony end*@

    <script>
        var google_colors = [
            "#039be5",
            "#7986cb",
            "#33b679",
            "#8e24aa",
            "#e67c73",
            "#f6c026",
            "#f5511d",
            "#039be5",
            "#616161",
            "#3f51b5",
            "#0b8043",
            "#d60000"
        ];

        (function (global) {
            "use strict";

            var app = global.app = global.app || {};

            kendo.culture("cs-CZ");

            function kendoAlert(title, body) {
                $("<div></div>").kendoAlert({
                    width: "95%",
                    title: title,
                    content: body
                }).data("kendoAlert").open();
            }

            function kendoDialog(title, body, buttons, callback) {
                var actions = [];
                if (buttons) {
                    $.each(buttons, function (a, b) {
                        b.action = function () { return callback(a) }
                        actions.push(b);
                    });
                } else {
                    actions.push({ text: 'Ano', action: function () { return callback(1) } });
                    actions.push({ text: 'Ne', action: function () { return callback(2) } });
                }
                $("<div></div>").kendoAlert({
                    width: "95%",
                    title: title,
                    content: body,
                    actions: actions,
                }).data("kendoAlert").open();
            }

            app.kMobile = new kendo.mobile.Application(document.body, {
                skin: "nova",
                hideAddressBar: true,
                transition: "slide",
                initial: '#main',
                statusBarStyle: "#8abf50"
            });

            app.main = kendo.observable({
                praclist: new kendo.data.DataSource({
                    serverPaging: true,
                    serverSorting: true,
                    serverFiltering: true,
                    schema: {
                        data: "data",
                        total: "total",
                        errors: "error",
                        model: {
                            id: "IDVykazPrace"
                        }
                    },
                    transport: {
                        read: {
                            url: "@Url.Action("AGsp_GetVykazPraceNedokoncene", "Api/Service")",
                            type: "GET"
                        },
                        parameterMap: function (options, operation) {
                            var filter = app.main.praclist.filter();
                            var params = { hledej: "" }
                            if (filter) {
                                params.hledej = filter.filters[0].value;
                            }
                            return params;
                        }
                    },
                    error: function (e) {
                        if (e.status == "customerror") {
                            kendoAlert('Hlášení', e.errors)
                        }
                    }
                }),
                show: function (e) {
                    app.main.praclist.read();
                },
                storno: function (e) {
                    var that = this;
                    var i = $(e.target).data("i");
                    if (e.data.rr_StavPracaku < 21) {
                        kendoDialog("Storno", "Opravdu si přejete pracák stornovat?", null, function (a) {
                            if (a == 1) {
                                $.get('@Url.Action("AGsp_Run_Pracak00to50", "Api/Service")', { iDVykazPrace: i }, function (result) {
                                    if (result.error) {
                                        kendoAlert('Hlášení', result.error);
                                    } else {
                                        that.praclist.read();
                                    }
                                });
                            }
                        });
                    } else {
                        kendoAlert('Hlášení', "Pracák je ve stavu kdy již nelze stornovat.");
                    }
                }
            });

            app.calendar = kendo.observable({
                weekdate: new Date(),
                calendars: [
                    { id: 1, name: "Pavel Pecher", calendarColor: "#7986CB", calendarId: "podpora@agilo.cz" },
                    { id: 2, name: "Tomáš Nezbeda", calendarColor: "#EF6C00", calendarId: "nezbeda@doctorum.cz" },
                    { id: 3, name: "Petr Novák", calendarColor: "#8e24aa", calendarId: "700nr6qvjl5a88nee07sl7oen8@group.calendar.google.com" },
                    { id: 4, name: "Marek Hanzl", calendarColor: "#039be5", calendarId: "0pq584pdtsp0i8ge18q6g9jb1s@group.calendar.google.com" }
                ],
                source: new kendo.data.SchedulerDataSource({
                    transport: {
                        read: {
                            url: "@Url.Action("getGoogleEvents", "Api/Service")",
                            dataType: "json",
                            type: "POST"
                        },
                        parameterMap: function (options, operation) {
                            var wdt = app.calendar.get("weekdate");
                            return { "": ["podpora@agilo.cz"], weekdate: kendo.toString(wdt, "yyyy-MM-dd HH:mm:ss") };
                        }
                    },
                    error: function (e) {
                        if (e.status == "customerror") {
                            alert(e.errors)
                        }
                    },
                    schema: {
                        data: "data",
                        total: "total",
                        errors: "error",
                        model: {
                            id: "id",
                            fields: {
                                id: { from: "Id", type: "number" },
                                iDT: { from: "IDT" },
                                title: { from: "Title" },
                                start: { type: "date", from: "Start" },
                                end: { type: "date", from: "End" },
                                startTimezone: { from: "StartTimezone" },
                                endTimezone: { from: "EndTimezone" },
                                description: { from: "Description" },
                                recurrenceId: { from: "RecurrenceId" },
                                recurrenceRule: { from: "RecurrenceRule" },
                                recurrenceException: { from: "RecurrenceException" },
                                isAllDay: { type: "boolean", from: "IsAllDay" },
                                location: { from: "Location" },
                                colorId: { from: "ColorId" },
                                calendarId: { from: "CalendarId" },
                                location: { from: "Location" }
                            }
                        }
                    }
                }),
                navigate: function (e) {
                    this.set("weekdate", e.date);
                    this.source.read();
                },
                databound: function (e) {
                    var that = this;
                    var items = e.sender.items();
                    var calendars = that.get("calendars");
                    var events = e.sender.dataItems();
                    var d1 = new Date();
                    $.each(items, function (a, b) {
                        var uid = $(b).data("uid");
                        var event = events.find(x => x.uid == uid);
                        if (event) {
                            var calendar = calendars.find(x => x.calendarId == event.calendarId);
                            if (calendar) {
                                var d2 = new Date(event.end);
                                var color = calendar.calendarColor;
                                if (event.colorId) {
                                    color = google_colors[event.colorId];
                                }
                                if (d1 > d2) {
                                    $(b).css("opacity", 0.3);
                                }
                                $(b).css("border-color", "white");
                                $(b).css("background-color", color);
                                $(b).css("padding", "6px");
                                $(b).css("border-radius", "4px");
                            }
                        }
                    });
                },
                event_click: function (e) {
                    e.preventDefault();
                }
            });

            app.pracak = kendo.observable({
                detail: null,
                klienti: new kendo.data.DataSource({
                    serverPaging: true,
                    serverSorting: true,
                    serverFiltering: true,
                    schema: {
                        data: "data",
                        total: "total",
                        errors: "error",
                        model: {}
                    },
                    transport: {
                        read: {
                            url: "@Url.Action("AGsp_GetFirmaPracovisteSeznamHledej", "Api/Service")",
                            type: "GET"
                        },
                        parameterMap: function (options, operation) {
                            var filter = app.pracak.klienti.filter();
                            var params = { hledej: "", hledatDleGPS: false, gPSLat: "", gPSLng: "" }
                            if (filter) {
                                params.hledej = filter.filters[0].value;
                            }
                            return params;
                        }
                    },
                    error: function (e) {
                        if (e.status == "customerror") {
                            kendoAlert('Hlášení', e.errors)
                        }
                    }
                }),
                polozky: new kendo.data.DataSource({
                    serverPaging: true,
                    serverSorting: true,
                    serverFiltering: true,
                    schema: {
                        data: "data",
                        total: "total",
                        errors: "error",
                        model: {
                            id: "IDVykazPracePol"
                        }
                    },
                    transport: {
                        read: {
                            url: "@Url.Action("AGsp_GetVykazPracePolSeznam", "Api/Service")",
                            type: "GET"
                        }
                    },
                    error: function (e) {
                        if (e.status == "customerror") {
                            kendoAlert('Hlášení', e.errors)
                        }
                    }
                }),
                produkty: new kendo.data.DataSource({
                    serverPaging: true,
                    serverSorting: true,
                    serverFiltering: true,
                    schema: {
                        data: "data",
                        errors: "error",
                        model: {}
                    },
                    transport: {
                        read: {
                            url: "@Url.Action("AGsp_HledejProduktFullText", "Api/Service")",
                            type: "GET"
                        },
                        parameterMap: function (options, operation) {
                            var filter = app.pracak.produkty.filter();
                            var params = { hledej: "" }
                            if (filter) {
                                if (filter.filters.length > 0) {
                                    params.hledej = filter.filters[0].value;
                                }
                            }
                            return params;
                        }
                    },
                    error: function (e) {
                        if (e.status == "customerror") {
                            kendoAlert('Hlášení', e.errors)
                        }
                    }
                }),
                sluzby: new kendo.data.DataSource({
                    serverPaging: true,
                    serverSorting: true,
                    serverFiltering: true,
                    schema: {
                        data: "data",
                        errors: "error",
                        model: {}
                    },
                    transport: {
                        read: {
                            url: "@Url.Action("AGsp_HledejSluzbaFullText", "Api/Service")",
                            type: "GET"
                        },
                        parameterMap: function (options, operation) {
                            var filter = app.pracak.sluzby.filter();
                            var params = { hledej: "" }
                            if (filter) {
                                if (filter.filters.length > 0) {
                                    params.hledej = filter.filters[0].value;
                                }
                            }
                            return params;
                        }
                    },
                    error: function (e) {
                        if (e.status == "customerror") {
                            kendoAlert('Hlášení', e.errors)
                        }
                    }
                }),
                klient_select: function (e) {
                    kendo.ui.progress($("body"), true);
                    $.get("@Url.Action("AGsp_AddNewVykazPrace", "Api/Service")", { firma: e.data.Pobocka }, function (result) {
                        kendo.ui.progress($("body"), false);
                        app.kMobile.navigate("#pracak?i=" + result.data);
                    });
                },
                btnpolozky: function (e) {
                    var d = app.pracak.get("detail");
                    if (d.IDVykazPrace) {
                        app.kMobile.navigate("#pracak-polozky?i=" + d.IDVykazPrace);
                    }
                },
                pracak_save: function (e) {
                    e.preventDefault();
                    var d = this.get("detail").toJSON();
                    kendo.ui.progress($("body"), true);
                    $.post("@Url.Action("AGsp_EditVykazPrace", "Api/Service")", {
                        iDVykazPrace: d.IDVykazPrace,
                        datVzniku: kendo.toString(new Date(d.DatVzniku), "yyyy-MM-dd HH:mm:ss"),
                        iDUserUpravil: 0,
                        poznamka: d.Poznamka,
                        rr_StavPracaku: d.rr_StavPracaku
                    }, function (result) {
                        kendo.ui.progress($("body"), false);
                        if (result.error) {
                            kendoAlert('Hlášení', result.error)
                        } else {
                            if (d.rr_StavPracaku === 0) {
                                kendoDialog("Validace", "Předat pracák k validaci?", null, function (a) {
                                    if (a == 1) {
                                        kendo.ui.progress($("body"), true);
                                        $.get(app.action.url("AGsp_Run_Pracak00to10", "Api/Service"), { iDVykazPrace: d.IDVykazPrace }, function (result) {
                                            kendo.ui.progress($("body"), false);
                                            if (result.error) {
                                                kendoAlert('Hlášení', result.error);
                                            } else {
                                                app.kMobile.navigate("#main");
                                            }
                                        });
                                    } else {
                                        app.kMobile.navigate("#main");
                                    }
                                });
                            }
                        }
                    });
                },
                deletepolozka: function (e) {
                    var i = $(e.target).data("i");
                    var ip = $(e.target).data("ip");
                    $.get("@Url.Action("AGsp_DelVykazPracePol", "Api/Service")", { iDVykazPracePol: ip }, function (result) {
                        if (result.error) {
                            kendoAlert('Hlášení', result.error)
                        } else {
                            app.pracak.polozky.read({ iDVykazPrace: i });
                        }
                    });
                },
                add_polozka: function (e) {
                    var d = this.get("detail");
                    if (d) {
                        app.kMobile.navigate("#pracak-polozka?i=" + d.IDVykazPrace + "&ip=0");
                    }
                },
                showkomu: function (e) {
                    app.pracak.klienti.read();
                },
                showpolozky: function (e) {
                    var params = e.view.params;
                    app.pracak.polozky.read({ iDVykazPrace: params.i });
                },
                showdetail: function (e) {
                    var params = e.view.params;
                    e.view.options.title = "Výkaz práce #" + params.i;
                    $.get("@Url.Action("AGsp_GetVykazPraceDetail", "Api/Service")", { iDVykazPrace: params.i }, function (result) {
                        var d = result.data;
                        if (d) {
                            d["DatVzniku"] = new Date(d["DatVzniku"]);
                        }
                        app.pracak.set("detail", d);
                    });
                },
                typy_source: new kendo.data.DataSource({
                    schema: { data: "data" },
                    transport: { read: "@Url.Action("RegRest", "Api/Service")?Register=rr_TypPolozkyPracaku" }
                }),
                typ_change: function (e) {
                    var that = this;
                    var iDVykazPrace = this.polozka_source.get("IDVykazPrace");
                    var typ = e.sender.value();
                    if (typ == 5 || typ == 6 || typ == 7) {
                        $.get('@Url.Action("AGsp_GetPracakHodnotyDopravne", "Api/Service")', { iDVykazPrace: iDVykazPrace, rr_TypPolozkyPracaku: typ }, function (result) {
                            var d = result.data;
                            that.polozka_source.set("TextNaFakturu", d.text);
                            that.polozka_source.set("TextInterniDoMailu", d.text);
                            that.polozka_source.set("PocetEMJ", d.km);
                        });
                    };
                },
                polozka_source: {
                    IDVykazPrace: 0
                },
                polozka_visible: function (e) {
                    var d = this.get("polozka_source");
                    if (d) {
                        if (d.get("rr_TypPolozkyPracaku") == e) {
                            return true;
                        }
                    }
                    return false;
                },
                showpolozka: function (e) {
                    var params = e.view.params;
                    var iDVykazPrace = parseInt(params.i);
                    var iDVykazPracePol = parseInt(params.ip);
                    if (iDVykazPracePol == 0) {
                        var min30 = new Date(new Date().getTime() + 30 * 60000);
                        var d = {
                            IDVykazPrace: iDVykazPrace,
                            IDVykazPracePol: 0,
                            IDUserUpravil: 0,
                            rr_TypPolozkyPracaku: 4,
                            CasOd: kendo.toString(new Date(), "HH:mm"),
                            CasDo: kendo.toString(min30, "HH:mm"),
                            Hodin: 0.5,
                            IDTechnika: @_iduser,
                            Produkt: "",
                            TextNaFakturu: "",
                            TextInterniDoMailu: "",
                            PocetEMJ: 1,
                            CenaEMJ: 0,
                            Zasah: false,
                            Vzdalenka: false,
                            Zdarma: false,
                            NavzdoryServisceUctovat: false
                        };
                        app.pracak.set("polozka_source", d);
                    } else {
                        $.get('@Url.Action("AGsp_GetVykazPracePolDetail", "Api/Service")', { iDVykazPracePol: iDVykazPracePol }, function (result) {
                            var d = result.data[0];
                            var z = parseInt(d["VelkyZasah"]);
                            d["VelkyZasah"] = (z === 1 ? true : false);
                            app.pracak.set("polozka_source", d);
                        });
                    }
                },
                zasah_change: function (e) {
                    var min30 = new Date(new Date().getTime() + 30 * 60000);
                    var min60 = new Date(new Date().getTime() + 60 * 60000);
                    if (e.checked) {
                        this.polozka_source.set("Hodin", 1);
                        this.polozka_source.set("CasOd", kendo.toString(new Date(), "HH:mm"));
                        this.polozka_source.set("CasDo", kendo.toString(min60, "HH:mm"));
                    } else {
                        this.polozka_source.set("Hodin", 0.5);
                        this.polozka_source.set("CasOd", kendo.toString(new Date(), "HH:mm"));
                        this.polozka_source.set("CasDo", kendo.toString(min30, "HH:mm"));
                    }
                },
                polozka_save: function (e) {
                    e.preventDefault();
                    var d = this.get("polozka_source").toJSON();
                    d.CasOd = kendo.toString(d.CasOd, "HH:mm:ss");
                    d.CasDo = kendo.toString(d.CasDo, "HH:mm:ss");
                    kendo.ui.progress($("body"), true);
                    $.post('@Url.Action("AGsp_AddOrEditVykazPracePol", "Api/Service")', d, function (result) {
                        kendo.ui.progress($("body"), false);
                        if (result.error) {
                            kendoAlert('Hlášení', result.error)
                        } else {
                            app.kMobile.navigate("#:back");
                        }
                    });
                }
            });
        })(window);
    </script>
</body>
</html>
