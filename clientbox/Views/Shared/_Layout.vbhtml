<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width" />
    <title>CBox | @ViewBag.Title</title>
    <link rel="icon" type="image/png" href="~/Images/crm.png" />

    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/font-awesome/4.7.0/css/font-awesome.min.css" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.1.0/css/bootstrap.min.css">
    <link rel="stylesheet" href="https://kendo.cdn.telerik.com/2020.3.1118/styles/kendo.bootstrap-v4.min.css" />
    <link href="~/Content/app.css" rel="stylesheet" />

    <link href="//fonts.googleapis.com/css?family=Roboto+Condensed:300,300i,400,400i,700,700i&subset=cyrillic,cyrillic-ext,greek,greek-ext,latin-ext,vietnamese" rel="stylesheet">
    <link href="//fonts.googleapis.com/css?family=Open+Sans" rel="stylesheet">
    <link href="//fonts.googleapis.com/css?family=Philosopher" rel="stylesheet">

    <script src="https://kendo.cdn.telerik.com/2020.3.1118/js/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.1.0/js/bootstrap.min.js"></script>
    <script src="https://kendo.cdn.telerik.com/2020.3.1118/js/kendo.all.min.js"></script>
    <script src="https://kendo.cdn.telerik.com/2020.3.1118/js/cultures/kendo.culture.cs-CZ.min.js"></script>
    <script src="https://kendo.cdn.telerik.com/2020.3.1118/js/messages/kendo.messages.cs-CZ.min.js"></script>
    <script src="https://cdn.jsdelivr.net/jquery.validation/1.16.0/jquery.validate.min.js"></script>

    @*<script src="https://api.trello.com/1/client.js?key=d049ef7c912e99555af2965786b1e6b4&token=ea81572132b4d376fffee2e1c34aec13b471d854e315fc6e9f5b2a76de0baac7"></script>*@

    <style>
        .k-widget {
            font-size: 14px;
        }
    </style>

    <script>
        kendo.culture("cs-CZ");

        jQuery.extend(jQuery.validator.messages, {
            required: "Toto pole je povinné.",
            remote: "Opravte toto pole.",
            email: "Zadejte prosím platnou e-mailovou adresu.",
            url: "Zadejte prosím platnou adresu URL.",
            datum: "Zadejte prosím platné datum.",
            dateISO: "Zadejte prosím platný datum (ISO).",
            number: "Zadejte prosím platné číslo.",
            digits: "Zadejte prosím pouze číslice.",
            creditcard: "Zadejte prosím platné číslo kreditní karty.",
            equalTo: "Zadejte znovu stejnou hodnotu.",
            accept: "Zadejte hodnotu s platnou příponou.",
            maxlength: jQuery.validator.format("Zadejte prosím ne více než {0} znaků"),
            minlength: jQuery.validator.format("Zadejte alespoň {0} znaků"),
            rangelength: jQuery.validator.format("Zadejte prosím hodnotu mezi {0} a {1} dlouhými znaky."),
            range: jQuery.validator.format("Zadejte prosím hodnotu mezi {0} a {1}."),
            max: jQuery.validator.format("Zadejte prosím hodnotu menší nebo rovnou {0}."),
            min: jQuery.validator.format("Zadejte prosím hodnotu větší nebo rovnou {0}.")
        });

        kendo.data.binders.required = kendo.data.Binder.extend({
            refresh: function () {
                var required = this.bindings.required.get();
                if (required) {
                    this.element.setAttribute("required", "required");
                } else {
                    this.element.removeAttribute("required");
                }
            }
        });

        var ud = '@(Html.Raw((CType((Context.User.Identity), FormsIdentity)).Ticket.UserData))';
        var dataUser = JSON.parse(ud.replace(/&quot;/g, '"'));
        var usersActivity = [];
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

        function plusDays(date, days) {
            return new Date(date.setDate(date.getDate() + days));
        }

        function plusHours(date, hours) {
            return new Date(date.setHours(date.getHours() + hours));
        }

        $(function () {
            kendo.data.binders.widget.minDateTime = kendo.data.Binder.extend({
                init: function (widget, bindings, options) {
                    kendo.data.Binder.fn.init.call(this, widget.element[0], bindings, options);
                },
                refresh: function () {
                    var that = this,
                        value = that.bindings["minDateTime"].get();
                    $(that.element).data("kendoDateTimePicker").min(value);
                }
            });

            function getCards(q, callback) {
                $.get('https://api.trello.com/1/members/novak@agilo.cz/boards?fields=id,name,url&key=' + q.key + '&token=' + q.token, function (res) {
                    var board = $.grep(res, function (a) {
                        return a.name === q.boardName;
                    })[0];
                    if (board) {
                        $.get('https://api.trello.com/1/boards/' + board.id + '/lists?fields=id,name,url&key=' + q.key + '&token=' + q.token, function (res) {
                            var list = $.grep(res, function (a) {
                                return a.name === q.listName;
                            })[0];
                            if (list) {
                                $.get('https://api.trello.com/1/boards/' + board.id + '/cards?fields=id,idList,name,dateLastActivity,due,dueComplete,labels,url&key=' + q.key + '&token=' + q.token, function (res) {
                                    var cards = $.grep(res, function (a) {
                                        var hotovo = $.grep(a.labels, function (b) {
                                            return b.name === "Hotovo";
                                        });
                                        if (hotovo.length == 0) {
                                            return a.idList === list.id;
                                        }
                                    });
                                    if (cards.length > 0) {
                                        callback(cards)
                                    } else {
                                        callback({ error: "Žádné úkoly v listu " + q.listName + "." });
                                    }
                                })
                            } else {
                                callback({ error: "List " + q.listName + " nenalezen." });
                            }
                        })
                    } else {
                        callback({ error: "Nástěnka " + q.boardName + " nenalezena." });
                    }
                })

                var string = kendo.ui.FilterMenu.prototype.options.operators.string;
                var number = kendo.ui.FilterMenu.prototype.options.operators.number;
                var date = kendo.ui.FilterMenu.prototype.options.operators.number;
                var enums = kendo.ui.FilterMenu.prototype.options.operators.enums;
                var messages = {
                    "string": {
                        "contains": string.contains,
                        "eq": string.eq,
                        "neq": string.neq,
                        "startswith": string.startswith,
                        "doesnotcontain": string.doesnotcontain,
                        "endswith": string.endswith,
                        "isempty": "Je prázdné",
                        "isnotempty": "Není prázdné"
                    },
                    "number": {
                        "eq": number.eq,
                        "neq": number.neq,
                        "gte": number.gte,
                        "gt": number.gt,
                        "lte": number.lte,
                        "lt": number.lt,
                        "isnull": "Je prázdné",
                        "isnotnull": "Není prázdné"
                    },
                    "date": {
                        "eq": date.eq,
                        "neq": date.neq,
                        "gte": date.gte,
                        "gt": date.gt,
                        "lte": date.lte,
                        "lt": date.lt,
                        "isnull": "Je prázdné",
                        "isnotnull": "Není prázdné"
                    },
                    "enums": {
                        "eq": enums.eq,
                        "neq": enums.eq,
                        "isnull": "Je prázdné",
                        "isnotnull": "Není prázdné"
                    }
                };
                kendo.ui.FilterMenu.prototype.options.operators = messages;
            }

            getCards({
                userName: dataUser.TrelloUID,
                key: dataUser.TrelloKey,
                token: dataUser.TrelloToken,
                boardName: "Úkoly CBOX",
                listName: dataUser.TrelloSloupec
            }, function (res) {
                var c = 0;
                if (res.error) {
                    $("#trellobadge").text(0);
                } else {
                    $.each(res, function (index, value) {
                        if (value.labels.length > 0) {
                            var lblh = $.grep(value.labels, function (a) {
                                return a.name === "Hotovo";
                            });
                            if (lblh.length === 0) {
                                c += 1;
                            }
                        } else {
                            c += 1;
                        }
                    });
                    $("#trellobadge").text(c);
                };
            });


            $.ajaxSetup({
                cache: false,
                timeout: 0,
                beforeSend: function (xhr) {
                    //kendo.ui.progress($(document.body), true);
                    //console.log(xhr)
                },
                complete: function (xhr, stat) {
                    //kendo.ui.progress($(document.body), false);
                    //console.log(xhr)
                },
                error: function (jqXHR, exception) {
                    //kendo.ui.progress($(document.body), false);
                    var msg = '';
                    if (jqXHR.status === 0) {
                        msg = 'Not connect.\n Verify Network. [0]';
                    } else if (jqXHR.status == 401) {
                        msg = 'Neautorizovaný uživatel 401';
                    } else if (jqXHR.status == 404) {
                        msg = 'Requested page not found. [404]';
                    } else if (jqXHR.status == 500) {
                        msg = 'Internal Server Error [500].';
                    } else if (exception === 'parsererror') {
                        msg = 'Requested JSON parse failed.';
                    } else if (exception === 'timeout') {
                        msg = 'Time out error.';
                    } else if (exception === 'abort') {
                        msg = 'Ajax request aborted.';
                    } else {
                        msg = 'Uncaught Error.\n' + jqXHR.responseText;
                    }
                    alert(msg);
                    return;
                }
            });

            window.onerror = function (msg, url, line, col, error) {
                kendo.ui.progress($(document.body), false);
                //alert("Error: " + msg + "\nurl: " + url);
                console.log(msg)
                console.log(url)
                console.log(line)
                console.log(col)
                console.log(error)
            };
        })
    </script>
</head>
<body style="font-family: 'Roboto Condensed', sans-serif;">
    <nav class="navbar navbar-expand-lg fixed-top bg-success navbar-dark">
        @*<a class="navbar-brand" href="#" title="Detail" onclick="openLeft()" data-bind="visible: panelsVisible">&#9776;</a>*@

        <a class="navbar-brand" href="#">
            <img src="~/Images/crm.png" alt="Client box" /><span style="margin-left:10px;">&Copf;&Bopf;&oopf;&xopf;</span>
        </a>

        <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navitems" aria-controls="navitems" aria-expanded="false" aria-label="Toggle navigation">
            <span class="navbar-toggler-icon"></span>
        </button>
        <div id="navitems" class="navbar-collapse collapse">
            <ul class="navbar-nav mr-auto">
                <li class="@(If(Html.CurAction = "Klienti", "nav-item active", "nav-item"))">
                    <a class="nav-link" href="@Url.Action("Klienti", "Home")">Klienti</a>
                </li>
                <li class="@(If(Html.CurAction = "Zalohy", "nav-item active", "nav-item"))">
                    <a class="nav-link" href="@Url.Action("Zalohy", "Home")">Zálohy</a>
                </li>
                <li class="@(If(Html.CurAction = "Tikety", "nav-item active", "nav-item"))">
                    <a class="nav-link" href="@Url.Action("Tikety", "Home")">Tikety</a>
                </li>
                <li class="@(If(Html.CurAction = "Objednavky", "nav-item active", "nav-item"))">
                    <a class="nav-link" href="@Url.Action("Objednavky", "Home")">Objednávky</a>
                </li>
                <li class="@(If(Html.CurAction = "Sklad", "nav-item active", "nav-item"))">
                    <a class="nav-link" href="@Url.Action("Sklad", "Home")">Sklad</a>
                </li>
                <li class="@(If(Html.CurAction = "Prijemky", "nav-item active", "nav-item"))">
                    <a class="nav-link" href="@Url.Action("Prijemky", "Home")">Příjemky</a>
                </li>
                <li class="@(If(Html.CurAction = "StoMega", "nav-item active", "nav-item"))">
                    <a class="nav-link" href="@Url.Action("StoMega", "Home")">100Mega</a>
                </li>
                <li class="@(If(Html.CurAction = "Fakturace", "nav-item active", "nav-item"))">
                    <a class="nav-link" href="@Url.Action("Fakturace", "Home")">Fakturace</a>
                </li>
                <li class="@(If(Html.CurAction = "Mapa techniků", "nav-item active", "nav-item"))">
                    <a class="nav-link" href="@Url.Action("Mapa", "Home")">Mapa</a>
                </li>
                <li class="@(If(Html.CurAction = "Ciselniky", "nav-item active", "nav-item"))">
                    <a class="nav-link" href="@Url.Action("Ciselniky", "Home")">Číselníky</a>
                </li>
                <li class="@(If(Html.CurAction = "Uzivatele", "nav-item active", "nav-item"))">
                    <a class="nav-link" href="@Url.Action("Uzivatele", "Home")">Správa uživatelů</a>
                </li>
                <li>
                    <a id="trellolink" class="nav-link" href="https://trello.com/b/LrqtpFYT/%C3%BAkoly-cbox" target="_blank">Úkoly <span class="badge badge-danger" id="trellobadge">0</span></a>
                </li>
                <li class="@(If(Html.CurAction = "Adresar", "nav-item active", "nav-item"))">
                    <a class="nav-link" href="@Url.Action("Adresar", "Home")">Adresář</a>
                </li>
                <li class="@(If(Html.CurAction = "Weby", "nav-item active", "nav-item"))">
                    <a class="nav-link" href="@Url.Action("Weby", "Home")">Naše weby</a>
                </li>
            </ul>
            <ul class="navbar-nav">
                @Code
                    If User.Identity.IsAuthenticated Then
                        @<li><a class="nav-link" href="@Url.Action("LogOut", "Account")"><span class="k-button-icontext k-icon k-i-user"></span> Odhlásit @User.Identity.Name</a></li>
                    Else
                        @<li><a class="nav-link" href="@Url.Action("Login", "Account")"><span class="k-button-icontext k-icon k-i-user"></span> Přihlásit</a></li>
                    End If
                End Code
                <li class="nav-item dropdown">
                    <a class="nav-link" href="#" id="navbarOptsDropdown" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                        <span class="k-icon k-i-more-vertical"></span>
                    </a>
                    <div class="dropdown-menu dropdown-menu-right" aria-labelledby="navbarOptsDropdown">
                        @Code
                            If User.IsInRole("hanzl@agilo.cz") Or User.IsInRole("fakturace@agilo.cz") Or User.IsInRole("novak@agilo.cz") Or User.IsInRole("frolikova@agilo.cz") Then
                                @<a class="dropdown-item" href="@Url.Action("ServiskySeznam", "Home")">Seznam servisních smluv</a>
                            End If
                        End Code
                        <a class="dropdown-item" href="@Url.Action("MojeServisky", "Home")">Seznam servisních smluv technika - moje servisky</a>
                        @Code
                            If User.IsInRole("hanzl@agilo.cz") Or User.IsInRole("fakturace@agilo.cz") Or User.IsInRole("novak@agilo.cz") Then
                                @<a class="dropdown-item" href="@Url.Action("FakturaceServisek", "Home")">Fakturace servisek</a>
                            End If
                        End Code
                        @Code
                            If User.IsInRole("hanzl@agilo.cz") Or User.IsInRole("fakturace@agilo.cz") Or User.IsInRole("novak@agilo.cz") Or User.IsInRole("frolikova@agilo.cz") Then
                                @<a class="dropdown-item" href="@Url.Action("Pohyby", "Home")">Bankovní výpisy a pohledávky</a>
                            End If
                        End Code
                        <a class="dropdown-item" href="@Url.Action("MojeOdmeny", "Home")">Moje odměny</a>
                        <a class="dropdown-item" href="@Url.Action("Kalendar", "Home")">Práce na projektech</a>
                        <hr />
                        <a class="dropdown-item" href="#" onclick="uvolnitLicence()">Uvolnit kouslé licence</a>
                        <a class="dropdown-item" href="#" onclick="dostupnostAVIS()">Dostupnost AVIS</a>
                    </div>
                </li>
            </ul>
        </div>
    </nav>

    @RenderBody()

    <style>
        @@media (min-width: 992px) {
            .modal-max {
                max-width: 80%;
            }
        }
    </style>

    <div class="modal fade" id="modal_polozky">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Položky pracáku</h4>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">
                    <select data-auto-bind="false" data-role="listbox" data-text-field="rr_TypPolozkyPracakuHodnota" data-value-field="IDVykazPracePol" data-bind="source: polozky, events: { change: polozkySelect }" data-template="tmppolozky" class="form-control" style="height: 250px"></select>
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
                  { 'field': 'Srovnano', 'title': 'Rezervace', 'template': '#=stornoCell(Srovnano)#', width: 115 },
         { 'field': 'Produkt', 'title': 'Produkt' },
         { 'field': 'Popis', 'title': 'Popis' },
         { 'field': 'SkladovaZasoba', 'title': 'SZ', width: 75 },
         { 'field': 'OperativniZasoba', 'title': 'OZ', width: 75 },
         { 'field': 'DatumNaskladnil', 'format': '{0:d}', 'title': 'Naskladněno', width: 140 },
         { 'field': 'NaskladnenoEMJ', 'title': 'Naskladněno', width: 140 },
         { 'field': 'rr_DodavatelHodnota', 'title': 'Dodavatel' },
         { 'field': 'VSPrijmovehoDokladu', 'title': 'Č. dokladu', width: 123 },
         { 'field': 'CenaNakup', 'format': '{0:n2}', 'title': 'Nákupní', width: 111 },
         { 'field': 'ProdejniCenaPodleNakupu', 'format': '{0:n2}', 'title': 'Prodejní', width: 111 },
         { 'field': 'Rezervoval', 'title': 'Rezervoval' },
         { 'template': '#=btnVyskladnitDoPracaku(OperativniZasoba, RezervovanoProNekohoJineho)#' },
         { 'field': 'RezervovatProFirmu', 'title': 'Pro koho' }]"
                         data-bind="source: AGsp_GetProduktRezervovaneProduktyFirmy" style="height: 250px;"></div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-success" data-bind="events: { click: polozkyNew }"><span class="k-icon k-i-plus"></span> Nová položka</button>
                    <button type="button" class="btn btn-default" data-dismiss="modal"><span class="k-icon k-i-cancel-outline"></span> Zavřít</button>
                </div>
            </div>
        </div>
    </div>

    <script>
        function stornoCell(Srovnano) {
            if (Srovnano == "Má rezervováno") {
                return `<div class="text-center bg-warning">` + Srovnano + `</div>`;
            } else {
                return `<div class="text-center">` + Srovnano + `</div>`;
            }
        }
    </script>

    <div class="modal fade" id="modal_rezervace">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Rezervované zboží</h4>
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
          { 'field': 'Srovnano', 'title': 'Rezervace', 'template': '#=stornoCell(Srovnano)#', width: 115 },
         { 'field': 'Produkt', 'title': 'Produkt' },
         { 'field': 'Popis', 'title': 'Popis' },
         { 'field': 'SkladovaZasoba', 'title': 'SZ', width: 75 },
         { 'field': 'OperativniZasoba', 'title': 'OZ', width: 75 },
         { 'field': 'DatumNaskladnil', 'format': '{0:d}', 'title': 'Naskladněno', width: 140 },
         { 'field': 'NaskladnenoEMJ', 'title': 'Naskladněno', width: 140 },
         { 'field': 'rr_DodavatelHodnota', 'title': 'Dodavatel' },
         { 'field': 'VSPrijmovehoDokladu', 'title': 'Č. dokladu', width: 123 },
         { 'field': 'CenaNakup', 'format': '{0:n2}', 'title': 'Nákupní', width: 111 },
         { 'field': 'ProdejniCenaPodleNakupu', 'format': '{0:n2}', 'title': 'Prodejní', width: 111 },
         { 'field': 'Rezervoval', 'title': 'Rezervoval' },
         { 'template': '#=btnRezervovat(Srovnano, OperativniZasoba)#' },
         { 'field': 'RezervovatProFirmu', 'title': 'Pro koho' }]"
                         data-bind="source: AGsp_GetProduktRezervovaneProduktyFirmy" style="height: 500px;"></div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal"><span class="k-icon k-i-cancel-outline"></span> Zavřít</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="modal_mnozstvi_rezervovat">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Množství</h4>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">
                    <div class="form-row">
                        <div class="form-group col-md-12">
                            <label class="text-muted">Množství</label>
                            <input required data-role="numerictextbox" data-format="n0" type="number" data-min="1" min="1" data-bind="value: rezervovat_selected.OperativniZasoba" class="form-control">
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-success" data-bind="events: { click: modal_mnozstvi_rezervovat_save }"><span class="k-icon k-i-plus"></span> OK</button>
                    <button type="button" class="btn btn-danger" data-dismiss="modal"><span class="k-icon k-i-cancel-outline"></span> Zavřít</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="modal_mnozstvi">
        <div class="modal-dialog modal-sm">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Množství</h4>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">
                    <div class="form-row">
                        <div class="form-group col-md-12">
                            <label class="text-muted">Množství</label>
                            <input required data-role="numerictextbox" data-format="n0" type="number" data-min="1" min="1" data-bind="value: vyskladnit_do_pracaku_selected.OperativniZasoba" class="form-control">
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-success" data-bind="events: { click: modal_mnozstvi_save }"><span class="k-icon k-i-plus"></span> OK</button>
                    <button type="button" class="btn btn-danger" data-dismiss="modal"><span class="k-icon k-i-cancel-outline"></span> Zavřít</button>
                </div>
            </div>
        </div>
    </div>

    <script>
        function btnVyskladnitDoPracaku(OperativniZasoba, RezervovanoProNekohoJineho) {
            if (RezervovanoProNekohoJineho == 0) {
                if (OperativniZasoba > 0) {
                    return `<div class="text-center"><a href="\#" class="text-primary" data-bind="events: { click: btn_vyskladnit_do_pracaku }">Vyskladnit do pracáku</a></div>`
                } else {
                    return `<div class="text-center">Op. zásoba = 0</div>`
                }
            } else {
                return `<div class="text-center">Rez. pro jiného</div>`
            }
        }

        function btnRezervovat(Srovnano, OperativniZasoba) {
            if (Srovnano == 'Volně k dispozici' && OperativniZasoba > 0) {
                return `<button class="btn btn-primary btn-sm" data-bind="events: { click: btn_rezervovat }">Rezervovat</button>`;
            } else {
                return ``;
            }
        }
    </script>

    <div class="modal fade" id="modal_polozka">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Položka</h4>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">
                    <div class="form-row">
                        <div class="form-group col-md-12">
                            <label>Typ položky</label>
                            <input data-role="dropdownlist" data-value-primitive="true" data-value-field="value" data-text-field="text" data-bind="source: typy, value: polozka.rr_TypPolozkyPracaku, events: { change: typChange }" class="form-control">
                        </div>
                    </div>
                    <div class="form-row typy" id="typ3" data-bind="visible: typyVisible">
                        <div class="col-md-6">
                            <table>
                                <tr>
                                    <td class="text-center pr-3"><label for="rr_HodinoveUctovani11">Hodinově vzdáleně</label></td>
                                    <td class="text-center pr-3"><label for="rr_HodinoveUctovani22">Hodinově Výjezd</label></td>
                                    <td class="text-center pr-3"><label for="rr_HodinoveUctovani33">Konzultace telefonát</label></td>
                                    <td class="text-center pr-3"><label for="rr_HodinoveUctovani44">Malý zásah Vzdálenka</label></td>
                                    <td class="text-center pr-3"><label for="rr_HodinoveUctovani55">Velký zásah Vzdálenka</label></td>
                                </tr>
                                <tr>
                                    <td class="text-center pr-3">
                                        <input type="radio" name="rr_HodinoveUctovani1" id="rr_HodinoveUctovani11" value="1" data-bind="checked: polozka.rr_HodinoveUctovani, events: { change: rr_HodinoveUctovani_change }" class="k-radio">
                                        <label class="k-radio-label" for="rr_HodinoveUctovani11"></label>
                                    </td>
                                    <td class="text-center pr-3">
                                        <input type="radio" name="rr_HodinoveUctovani1" id="rr_HodinoveUctovani22" value="2" data-bind="checked: polozka.rr_HodinoveUctovani, events: { change: rr_HodinoveUctovani_change }" class="k-radio">
                                        <label class="k-radio-label" for="rr_HodinoveUctovani22"></label>
                                    </td>
                                    <td class="text-center pr-3">
                                        <input type="radio" name="rr_HodinoveUctovani1" id="rr_HodinoveUctovani33" value="3" data-bind="checked: polozka.rr_HodinoveUctovani, events: { change: rr_HodinoveUctovani_change }" class="k-radio">
                                        <label class="k-radio-label" for="rr_HodinoveUctovani33"></label>
                                    </td>
                                    <td class="text-center pr-3">
                                        <input type="radio" name="rr_HodinoveUctovani1" id="rr_HodinoveUctovani44" value="4" data-bind="checked: polozka.rr_HodinoveUctovani, events: { change: rr_HodinoveUctovani_change }" class="k-radio">
                                        <label class="k-radio-label" for="rr_HodinoveUctovani44"></label>
                                    </td>
                                    <td class="text-center pr-3">
                                        <input type="radio" name="rr_HodinoveUctovani1" id="rr_HodinoveUctovani55" value="5" data-bind="checked: polozka.rr_HodinoveUctovani, events: { change: rr_HodinoveUctovani_change }" class="k-radio">
                                        <label class="k-radio-label" for="rr_HodinoveUctovani55"></label>
                                    </td>
                                </tr>
                            </table>
                            <div class="form-group">
                                <label>Od</label>
                                <input data-role="timepicker" data-interval="15" type="time" required data-bind="value: polozka.CasOd" class="form-control" />
                            </div>
                            <div class="form-group">
                                <label>Do</label>
                                <input data-role="timepicker" data-interval="15" type="time" required data-bind="value: polozka.CasDo" class="form-control" />
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div data-role="grid"
                                 data-filterable="false"
                                 data-column-menu="false"
                                 data-pageable="false"
                                 data-editable="false"
                                 data-sortable="false"
                                 data-selectable="false"
                                 data-scrollable="true"
                                 data-resizable="true"
                                 data-reorderable="false"
                                 data-persist-selection="false"
                                 data-navigatable="false"
                                 data-auto-bind="false"
                                 data-toolbar="[{ template: 'Odpracovnáno v tomto měsíci <b class=zbyvaHod style=float:right></b>' }]"
                                 data-bind="source: seznamhodin"
                                 data-columns='[
                                             { field: "DatVzniku", format: "{0:d}", title: "Datum", width: 100 },
                                             { field: "UserLastName", title: "Technik", width: 150 },
                                             { field: "Hodin", title: "Hodin", width: 60 },
                                             { field: "CerpanoVolnychHodin", title: "Čerpáno volných hodin" },
                                             { field: "IDVykazPrace", template: "#=cellIDVykazPrace(IDVykazPrace)#", title: "Pracák" },
                                             { field: "rr_HodinoveUctovaniText", title: "Typ" },
                                             { field: "TextNaFakturu", attributes: { title: "#=TextNaFakturu#" }, title: "Text na fakturu" }
                                          ]'
                                 style="height:215px;"></div>
                        </div>
                        @*<div class="col">
                    <div class="form-group">
                        <label>Vzdálenka</label><br />
                        <input data-role="switch"
                               data-messages="{
        checked: '✔',
                    unchecked: '✖',
        }" data-bind="checked: polozka.Vzdalenka" />
                    </div>
                </div>
                <div class="col">
                    <div class="form-group">
                        <label>Od</label>
                        <input data-role="timepicker" type="time" required data-bind="value: polozka.CasOd" class="form-control" />
                    </div>
                </div>
                <div class="col">
                    <div class="form-group">
                        <label>Do</label>
                        <input data-role="timepicker" type="time" required data-bind="value: polozka.CasDo" class="form-control" />
                    </div>
                </div>
                <div class="col">
                    <div class="form-group" data-bind="visible: mvvisible">
                        <label>Malý/Velký zásah</label><br />
                        <input data-role="switch"
                               data-messages="{
        checked: 'V',
        unchecked: 'M',
        }"
                               data-bind="checked: polozka.VelkyZasah, events: { change: zaschange }" />
                    </div>
                </div>*@
                        <div class="col-md-12">
                            <div class="form-group">
                                <label>Technik</label>
                                <select data-value-field="value" required data-text-field="text" data-bind="source: technici, value: polozka.IDTechnika" class="form-control"></select>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Text na fakturu</label>
                                <button class="btn btn-sm btn-primary" data-bind="events: { click: text_nf_ticket }">Použít text z ticketu</button>
                                <textarea rows="10" maxlength="250" type="text" data-bind="value: polozka.TextNaFakturu, events: { keyup: tnfkeyup }" class="form-control text-na-fakturu"></textarea>
                                <small class="text-muted">max 250 znaků</small>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Text do emailu</label>
                                <textarea rows="10" type="text" data-bind="value: polozka.TextInterniDoMailu" class="form-control text-do-emailu"></textarea>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Zdarma</label><br />
                                <input data-role="switch"
                                       data-messages="{
                checked: '✔',
                            unchecked: '✖',
                }"
                                       data-bind="checked: polozka.Zdarma" />
                                @*<input type="checkbox" data-role="kendo.mobile.ui.Switch" data-bind="checked: polozka.Zdarma" class="form-control" />*@
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Navzdory servisce účtovat</label><br />
                                <input data-role="switch"
                                       data-messages="{
                checked: '✔',
                            unchecked: '✖',
                }" data-bind="checked: polozka.NavzdoryServisceUctovat" />
                            </div>
                        </div>
                    </div>
                    <div class="form-row typy" id="typ1" data-bind="visible: typyVisible">
                        <div class="col-md-12">
                            <div class="form-group">
                                <a class="btn btn-outline-primary" style="width:100%;" data-bind="events: { click: dejProdukt }">
                                    Produkt
                                    <input type="text" readonly required data-bind="value: polozka.Produkt" class="form-control" />
                                </a>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Text na fakturu</label>
                                <button class="btn btn-sm btn-primary" data-bind="events: { click: text_nf_ticket }">Použít text z ticketu</button>
                                <textarea id="tnf1" rows="10" maxlength="250" data-bind="value: polozka.TextNaFakturu, events: { keyup: tnfkeyup }" class="form-control text-na-fakturu"></textarea>
                                <small class="text-muted">max 250 znaků</small>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Text do emailu</label>
                                <textarea rows="10" data-bind="value: polozka.TextInterniDoMailu" class="form-control text-do-emailu"></textarea>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Počet EMJ</label><br />
                                <input data-format="0 \Ks" data-role="numerictextbox" type="number" required data-min="1" data-bind="value: polozka.PocetEMJ" class="form-control" />
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Cena prodejní bez DPH</label>
                                <input data-format="c" data-role="numerictextbox" type="number" required data-bind="value: polozka.CenaEMJ" class="form-control" />
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Technik</label>
                                <select data-value-field="value" required data-text-field="text" data-bind="source: technici, value: polozka.IDTechnika" class="form-control"></select>
                            </div>
                        </div>
                    </div>
                    <div class="form-row typy" id="typ2" data-bind="visible: typyVisible">
                        <div class="col-md-12">
                            <div class="form-group">
                                <a class="btn btn-outline-primary" style="width:100%;" data-bind="events: { click: dejSluzbu }">
                                    Služba
                                    <input type="text" readonly required data-bind="value: polozka.Produkt" class="form-control" />
                                </a>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Text na fakturu</label>
                                <button class="btn btn-sm btn-primary btn-primary" data-bind="events: { click: text_nf_ticket }">Použít text z ticketu</button>
                                <textarea id="tnf1" rows="10" maxlength="250" type="text" data-bind="value: polozka.TextNaFakturu, events: { keyup: tnfkeyup }" class="form-control text-na-fakturu"></textarea>
                                <small class="text-muted">max 250 znaků</small>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Text do emailu</label>
                                <textarea type="text" rows="10" data-bind="value: polozka.TextInterniDoMailu" class="form-control text-do-emailu"></textarea>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Počet EMJ</label><br />
                                <input data-format="0 \Ks" data-role="numerictextbox" type="number" required data-min="1" data-bind="value: polozka.PocetEMJ" class="form-control" />
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Cena prodejní bez DPH</label>
                                <input data-format="c" data-role="numerictextbox" type="number" required data-bind="value: polozka.CenaEMJ" class="form-control" />
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Technik</label>
                                <select data-value-field="value" required data-text-field="text" data-bind="source: technici, value: polozka.IDTechnika" class="form-control"></select>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Navzdory servisce účtovat</label><br />
                                <input data-role="switch" data-messages="{ checked: '✔', unchecked: '✖' }" data-bind="checked: polozka.NavzdoryServisceUctovat" />
                            </div>
                        </div>
                    </div>
                    <div class="form-row typy" id="typ4" data-bind="visible: typyVisible">
                        <div class="col-md-6">
                            <table>
                                <tr>
                                    <td class="text-center pr-3"><label for="rr_HodinoveUctovani1">Hodinově vzdáleně</label></td>
                                    <td class="text-center pr-3"><label for="rr_HodinoveUctovani2">Hodinově Výjezd</label></td>
                                    <td class="text-center pr-3"><label for="rr_HodinoveUctovani3">Konzultace telefonát</label></td>
                                    <td class="text-center pr-3"><label for="rr_HodinoveUctovani4">Malý zásah Vzdálenka</label></td>
                                    <td class="text-center pr-3"><label for="rr_HodinoveUctovani5">Velký zásah Vzdálenka</label></td>
                                </tr>
                                <tr>
                                    <td class="text-center pr-3">
                                        <input type="radio" name="rr_HodinoveUctovani" id="rr_HodinoveUctovani1" value="1" data-bind="checked: polozka.rr_HodinoveUctovani, events: { change: rr_HodinoveUctovani_change }" class="k-radio">
                                        <label class="k-radio-label" for="rr_HodinoveUctovani1"></label>
                                    </td>
                                    <td class="text-center pr-3">
                                        <input type="radio" name="rr_HodinoveUctovani" id="rr_HodinoveUctovani2" value="2" data-bind="checked: polozka.rr_HodinoveUctovani, events: { change: rr_HodinoveUctovani_change }" class="k-radio">
                                        <label class="k-radio-label" for="rr_HodinoveUctovani2"></label>
                                    </td>
                                    <td class="text-center pr-3">
                                        <input type="radio" name="rr_HodinoveUctovani" id="rr_HodinoveUctovani3" value="3" data-bind="checked: polozka.rr_HodinoveUctovani, events: { change: rr_HodinoveUctovani_change }" class="k-radio">
                                        <label class="k-radio-label" for="rr_HodinoveUctovani3"></label>
                                    </td>
                                    <td class="text-center pr-3">
                                        <input type="radio" name="rr_HodinoveUctovani" id="rr_HodinoveUctovani4" value="4" data-bind="checked: polozka.rr_HodinoveUctovani, events: { change: rr_HodinoveUctovani_change }" class="k-radio">
                                        <label class="k-radio-label" for="rr_HodinoveUctovani4"></label>
                                    </td>
                                    <td class="text-center pr-3">
                                        <input type="radio" name="rr_HodinoveUctovani" id="rr_HodinoveUctovani5" value="5" data-bind="checked: polozka.rr_HodinoveUctovani, events: { change: rr_HodinoveUctovani_change }" class="k-radio">
                                        <label class="k-radio-label" for="rr_HodinoveUctovani4"></label>
                                    </td>
                                </tr>
                            </table>
                            <div class="form-group">
                                <label>Hodin</label>
                                <input data-format="0.0 \Hod." data-step="0.5" required data-role="numerictextbox" type="number" data-bind="value: polozka.Hodin" class="form-control" />
                            </div>
                            <div class="form-group">
                                <label>Technik</label>
                                <select data-value-field="value" required data-text-field="text" data-bind="source: technici, value: polozka.IDTechnika" class="form-control"></select>
                            </div>
                        </div>
                        @*<div class="col-md-6">
                    <div class="form-group">
                        <label>Vzdálenka</label><br />
                        <input data-role="switch" data-messages="{ checked: '✔', unchecked: '✖' }" data-bind="checked: polozka.Vzdalenka" />
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label>Hodin</label>
                        <input data-format="0.0 \Hod." data-step="0.5" required data-role="numerictextbox" type="number" data-bind="value: polozka.Hodin" class="form-control" />
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="form-group">
                        <label>Malý/Velký zásah</label><br />
                        <input data-role="switch"
                               data-messages="{
        checked: 'V',
        unchecked: 'M',
        }"
                               data-bind="checked: polozka.VelkyZasah, events: { change: zaschange }" />
                    </div>
                </div>*@
                        <div class="col-md-6">
                            <div data-role="grid"
                                 data-filterable="false"
                                 data-column-menu="false"
                                 data-pageable="false"
                                 data-editable="false"
                                 data-sortable="false"
                                 data-selectable="false"
                                 data-scrollable="true"
                                 data-resizable="true"
                                 data-reorderable="false"
                                 data-persist-selection="false"
                                 data-navigatable="false"
                                 data-auto-bind="false"
                                 data-toolbar="[{ template: 'Odpracovnáno v tomto měsíci <b class=zbyvaHod style=float:right></b>' }]"
                                 data-bind="source: seznamhodin"
                                 data-columns='[
                                             { field: "DatVzniku", format: "{0:d}", title: "Datum", width: 120 },
                                             { field: "UserLastName", title: "Technik" },
                                             { field: "Hodin", title: "Hodin" },
                                             { field: "CerpanoVolnychHodin", title: "Čerpáno volných hodin" },
                                             { field: "IDVykazPrace", template: "#=cellIDVykazPrace(IDVykazPrace)#", title: "Pracák" },
                                             { field: "rr_HodinoveUctovaniText", title: "Typ" },
                                             { field: "TextNaFakturu", attributes: { title: "#=TextNaFakturu#" }, title: "Text na fakturu" }
                                          ]'
                                 style="height:215px;"></div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Text na fakturu</label>
                                <button class="btn btn-sm btn-primary" data-bind="events: { click: text_nf_ticket }">Použít text z ticketu</button>
                                <textarea id="tnf4" rows="10" maxlength="250" type="text" data-bind="value: polozka.TextNaFakturu, events: { keyup: tnfkeyup }" class="form-control text-na-fakturu"></textarea>
                                <small class="text-muted">max 250 znaků</small>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Text do emailu</label>
                                <textarea type="text" rows="10" data-bind="value: polozka.TextInterniDoMailu" class="form-control text-do-emailu"></textarea>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Zdarma</label><br />
                                <input data-role="switch"
                                       data-messages="{
                checked: '✔',
                            unchecked: '✖',
                }"
                                       data-bind="checked: polozka.Zdarma" />
                                @*<input type="checkbox" data-role="kendo.mobile.ui.Switch" data-bind="checked: polozka.Zdarma" class="form-control" />*@
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Navzdory servisce účtovat</label><br />
                                <input data-role="switch" data-messages="{ checked: '✔', unchecked: '✖' }" data-bind="checked: polozka.NavzdoryServisceUctovat" />
                            </div>
                        </div>
                    </div>
                    <div class="form-row typy" id="typ5" data-bind="visible: typyVisible">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Text na fakturu</label>
                                <button class="btn btn-sm btn-primary" data-bind="events: { click: text_nf_ticket }">Použít text z ticketu</button>
                                <textarea id="tnf4" rows="10" maxlength="250" type="text" data-bind="value: polozka.TextNaFakturu, events: { keyup: tnfkeyup }" class="form-control text-na-fakturu"></textarea>
                                <small class="text-muted">max 250 znaků</small>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <table class="w-100">
                                <tr>
                                    <td>
                                        <div class="form-group">
                                            <label>Účtovaných KM</label><br />
                                            <input data-format="0 \Km" data-role="numerictextbox" type="number" required data-min="0" data-bind="value: polozka.PocetEMJ" class="form-control" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="form-group">
                                            <label>Najeto KM</label><br />
                                            <input data-format="0 \Km" data-role="numerictextbox" type="number" required data-min="0" data-bind="value: polozka.NajetoKM" class="form-control" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="form-group">
                                <label>Technik</label>
                                <select data-value-field="value" required data-text-field="text" data-bind="source: technici, value: polozka.IDTechnika" class="form-control"></select>
                            </div>
                            <table class="w-100">
                                <tr>
                                    <td width="150">
                                        <div class="form-group">
                                            <label>Navzdory servisce účtovat</label><br />
                                            <input data-role="switch" data-messages="{ checked: '✔', unchecked: '✖' }" data-bind="checked: polozka.NavzdoryServisceUctovat" />
                                        </div>
                                    </td>
                                    <td>
                                        <div data-role="grid"
                                             data-filterable="false"
                                             data-column-menu="false"
                                             data-pageable="false"
                                             data-editable="false"
                                             data-sortable="false"
                                             data-selectable="false"
                                             data-scrollable="true"
                                             data-resizable="true"
                                             data-reorderable="false"
                                             data-persist-selection="false"
                                             data-navigatable="false"
                                             data-auto-bind="false"
                                             data-toolbar="[{ template: 'Dosavadní jízdy v tomto měsíci' }]"
                                             data-bind="source: seznamjizd"
                                             data-columns='[
                { field: "DatVzniku", format: "{0:d}", title: "Čas", width: 120 },
                { field: "UserLastName", title: "Kdo" },
                { field: "NajetoKM", title: "Najeto" },
                                             { field: "PocetEMJ", title: "Účtováno KM" },
                                             { field: "PrvniJizdaVMesici", template: "#=cellCheckbox(PrvniJizdaVMesici)#", title: "První jízda zdarma" }
             ]'
                                             style="height:200px;"></div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="form-row typy" id="typ6" data-bind="visible: typyVisible">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Text na fakturu</label>
                                <button class="btn btn-sm btn-primary" data-bind="events: { click: text_nf_ticket }">Použít text z ticketu</button>
                                <textarea id="tnf4" rows="10" maxlength="250" type="text" data-bind="value: polozka.TextNaFakturu, events: { keyup: tnfkeyup }" class="form-control text-na-fakturu"></textarea>
                                <small class="text-muted">max 250 znaků</small>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <table class="w-100">
                                <tr>
                                    <td>
                                        <div class="form-group">
                                            <label>Účtovaných KM</label><br />
                                            <input data-format="0 \Km" data-role="numerictextbox" type="number" required data-min="0" data-bind="value: polozka.PocetEMJ" class="form-control" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="form-group">
                                            <label>Najeto KM</label><br />
                                            <input data-format="0 \Km" data-role="numerictextbox" type="number" required data-min="0" data-bind="value: polozka.NajetoKM" class="form-control" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="form-group">
                                <label>Technik</label>
                                <select data-value-field="value" required data-text-field="text" data-bind="source: technici, value: polozka.IDTechnika" class="form-control"></select>
                            </div>
                            <table class="w-100">
                                <tr>
                                    <td width="150">
                                        <div class="form-group">
                                            <label>Navzdory servisce účtovat</label><br />
                                            <input data-role="switch" data-messages="{ checked: '✔', unchecked: '✖' }" data-bind="checked: polozka.NavzdoryServisceUctovat" />
                                        </div>
                                    </td>
                                    <td>
                                        <div data-role="grid"
                                             data-filterable="false"
                                             data-column-menu="false"
                                             data-pageable="false"
                                             data-editable="false"
                                             data-sortable="false"
                                             data-selectable="false"
                                             data-scrollable="true"
                                             data-resizable="true"
                                             data-reorderable="false"
                                             data-persist-selection="false"
                                             data-navigatable="false"
                                             data-auto-bind="false"
                                             data-toolbar="[{ template: 'Dosavadní jízdy v tomto měsíci' }]"
                                             data-bind="source: seznamjizd"
                                             data-columns='[
                { field: "DatVzniku", format: "{0:d}", title: "Čas", width: 120 },
                { field: "UserLastName", title: "Kdo" },
                { field: "NajetoKM", title: "Najeto" },
                                             { field: "PocetEMJ", title: "Účtováno KM" },
                                             { field: "PrvniJizdaVMesici", template: "#=cellCheckbox(PrvniJizdaVMesici)#", title: "První jízda zdarma" }
             ]'
                                             style="height:200px;"></div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <div class="form-row typy" id="typ7" data-bind="visible: typyVisible">
                        <div class="col-md-6">
                            <div class="form-group">
                                <label>Text na fakturu</label>
                                <button class="btn btn-sm btn-primary" data-bind="events: { click: text_nf_ticket }">Použít text z ticketu</button>
                                <textarea id="tnf4" rows="10" maxlength="250" type="text" data-bind="value: polozka.TextNaFakturu, events: { keyup: tnfkeyup }" class="form-control text-na-fakturu"></textarea>
                                <small class="text-muted">max 250 znaků</small>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <table class="w-100">
                                <tr>
                                    <td>
                                        <div class="form-group">
                                            <label>Účtovaných KM</label><br />
                                            <input data-format="0 \Km" data-role="numerictextbox" type="number" required data-min="0" data-bind="value: polozka.PocetEMJ" class="form-control" />
                                        </div>
                                    </td>
                                    <td>
                                        <div class="form-group">
                                            <label>Najeto KM</label><br />
                                            <input data-format="0 \Km" data-role="numerictextbox" type="number" required data-min="0" data-bind="value: polozka.NajetoKM" class="form-control" />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <div class="form-group">
                                <label>Technik</label>
                                <select data-value-field="value" required data-text-field="text" data-bind="source: technici, value: polozka.IDTechnika" class="form-control"></select>
                            </div>
                            <table class="w-100">
                                <tr>
                                    <td width="150">
                                        <div class="form-group">
                                            <label>Navzdory servisce účtovat</label><br />
                                            <input data-role="switch" data-messages="{ checked: '✔', unchecked: '✖' }" data-bind="checked: polozka.NavzdoryServisceUctovat" />
                                        </div>
                                    </td>
                                    <td>
                                        <div data-role="grid"
                                             data-filterable="false"
                                             data-column-menu="false"
                                             data-pageable="false"
                                             data-editable="false"
                                             data-sortable="false"
                                             data-selectable="false"
                                             data-scrollable="true"
                                             data-resizable="true"
                                             data-reorderable="false"
                                             data-persist-selection="false"
                                             data-navigatable="false"
                                             data-auto-bind="false"
                                             data-toolbar="[{ template: 'Dosavadní jízdy v tomto měsíci' }]"
                                             data-bind="source: seznamjizd"
                                             data-columns='[
                { field: "DatVzniku", format: "{0:d}", title: "Čas", width: 120 },
                { field: "UserLastName", title: "Kdo" },
                { field: "NajetoKM", title: "Najeto" },
                                             { field: "PocetEMJ", title: "Účtováno KM" },
                                             { field: "PrvniJizdaVMesici", template: "#=cellCheckbox(PrvniJizdaVMesici)#", title: "První jízda zdarma" }
             ]'
                                             style="height:200px;"></div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-success" data-bind="events: { click: polozkaSave }"><span class="k-icon k-i-save"></span> Uložit</button>
                    <button type="button" class="btn btn-default" data-dismiss="modal"><span class="k-icon k-i-cancel-outline"></span> Zavřít</button>
                </div>
            </div>
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
                    <span class="k-textbox k-space-right" style="width: 100%;">
                        <input type="text" data-bind="value: produktyHledej, events: { change: hledatprodukt  }" />
                        <a href="#" data-bind="events: { click: hledatprodukt  }" class="k-icon k-i-search">&nbsp;</a>
                    </span>
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
                         data-bind="source: produkty, events: { change: produktySelect }" data-columns="[
                         { field: 'Produkt', title: 'Produkt' },
                         { field: 'Popis', title: 'Popis' },
                         { field: 'Dodavatel', title: 'Dodavatel' },
                         { field: 'SkladovaZasoba', title: 'Skladová zásoba' },
                         { field: 'OperativniZasoba', title: 'Operativní zásoba' },
                         { field: 'PrumernaNakup', title: 'Průměrná nákup' },
                         { field: 'NaposledyNaskladneno', format: '{0:d}', title: 'Naposledy naskladněno' }
                         ]"></div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal"><span class="k-icon k-i-cancel-outline"></span> Zavřít</button>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="modal_sluzby">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title">Služby</h4>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">
                    <span class="k-textbox k-space-right" style="width: 100%;">
                        <input id="hledatsluzbu" type="text" data-bind="value: sluzbyHledej, events: { change: hledatsluzbu  }" />
                        <a href="#" data-bind="events: { click: hledatsluzbu  }" class="k-icon k-i-search">&nbsp;</a>
                    </span>
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
                         data-bind="source: sluzby, events: { change: produktySelect }" data-columns="[
                         { field: 'Produkt', title: 'Produkt' },
                         { field: 'Popis', title: 'Popis' },
                         { field: 'Jednotky', title: 'Jednotky' },
                         { field: 'Cena', title: 'Cena' },
                         { field: 'Cena_nakupni', title: 'Cena nákupní' }
                         ]"></div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal"><span class="k-icon k-i-cancel-outline"></span> Zavřít</button>
                </div>
            </div>
        </div>
    </div>

    <style>
        [data-role="switch"] {
            font-family: Verdana, sans-serif;
        }
        
        .k-colorpalette .k-item {
            width: 32px;
            height: 32px;
            color: white;
            overflow: hidden;
            align-content: center;
            -ms-high-contrast-adjust: none;
        }

        .k-colorpalette tr:nth-child(1) .k-item:nth-child(1)::after {
            padding-left: 12px;
            content: "1";
        }

        .k-colorpalette tr:nth-child(1) .k-item:nth-child(2)::after {
            padding-left: 12px;
            content: "2";
        }

        .k-colorpalette tr:nth-child(2) .k-item:nth-child(1)::after {
            padding-left: 12px;
            content: "3";
        }

        .k-colorpalette tr:nth-child(2) .k-item:nth-child(2)::after {
            padding-left: 12px;
            content: "4";
        }

        .k-colorpalette tr:nth-child(3) .k-item:nth-child(1)::after {
            padding-left: 12px;
            content: "5";
        }

        .k-colorpalette tr:nth-child(3) .k-item:nth-child(2)::after {
            padding-left: 12px;
            content: "6";
        }

        .k-colorpalette tr:nth-child(4) .k-item:nth-child(1)::after {
            padding-left: 12px;
            content: "7";
        }

        .k-colorpalette tr:nth-child(4) .k-item:nth-child(2)::after {
            padding-left: 12px;
            content: "8";
        }

        .k-colorpalette tr:nth-child(5) .k-item:nth-child(1)::after {
            padding-left: 12px;
            content: "9";
        }

        .k-colorpalette tr:nth-child(5) .k-item:nth-child(2)::after {
            padding-left: 8px;
            content: "10";
        }

        .k-colorpalette tr:nth-child(6) .k-item:nth-child(1)::after {
            padding-left: 8px;
            content: "11";
        }
    </style>

    <div class="modal fade" id="modal_email">
        <div class="modal-dialog modal-lg">
            <form class="modal-content" method="post" data-bind="events: { submit: email_post }">
                <div class="modal-header">
                    <h4 class="modal-title">Poslat zprávu zákazníkovi</h4>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">
                    <div class="form-row">
                        <div class="form-group col-md-12">
                            <div class="input-group">
                                <div class="input-group-prepend">
                                    <span class="input-group-text">Komu</span>
                                </div>
                                <input type="email" class="form-control" data-bind="value: ticket_email.emailTo" readonly>
                            </div>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-12">
                            <div class="input-group">
                                <div class="input-group-prepend">
                                    <span class="input-group-text">Předmět</span>
                                </div>
                                <input type="text" class="form-control" data-bind="value: ticket_email.emailSubject" readonly>
                            </div>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-12">
                            <textarea data-role="editor"
                                      data-tools="[]"
                                      rows="5"
                                      class="form-control"
                                      data-bind="value: ticket_email.emailBody"
                                      style="height:600px;"
                                      required></textarea>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="submit" class="btn btn-success"><span class="k-icon k-i-email"></span> Odeslat</button>
                    <button type="button" class="btn btn-danger" data-dismiss="modal" data-bind="events: { click: close_email }"><span class="k-icon k-i-cancel-outline"></span> Neodesílat</button>
                </div>
            </form>
        </div>
    </div>

    <form class="slide-window" id="modal_ticket" method="post" data-bind="events: { submit: ticket_save }">
        <div class="slide-window-header">
            <div style="width:30px;height:30px">
                <a href="#" class="btn-close" data-dismiss="modal">
                    <img src="~/Images/gback.jpg" />
                </a>
            </div>
            <h4 class="modal-title">#<span data-bind="text: ticket.IDTicket"></span> Ticket</h4>
        </div>
        <div class="slide-window-body">
            <div class="form-row">
                <div class="form-group col-md-12">
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text"><span data-bind="visible: ispopis">★</span></span>
                        </div>
                        <select data-role="dropdownlist" class="form-control border" data-value-primitive="true" data-value-field="value" data-text-field="text" data-bind="source: typyzasahu, value: ticket.rr_TypZasahu, events: { change: typyzasahu_change }" required></select>
                        <select data-role="dropdownlist" class="form-control border" data-value-primitive="true" data-auto-bind="false" data-value-field="value" data-text-field="text" data-bind="source: pobocky, value: ticket.Pobocka, events: { dataBound: pobocky_bound }" required></select>
                        <input type="text" class="form-control w-50" data-bind="value: ticket.Predmet" placeholder="Předmět" required />
                    </div>
                </div>
            </div>
            <div class="form-row">
                <div class="form-group col-md-12">
                    <div class="checkbox">
                        <label><input type="checkbox" data-bind="checked: ticket.UdalostVGoogleCalend"> Zobrazit událost v google kalendáři</label>
                    </div>
                </div>
            </div>
            <div class="form-row">
                <div class="form-group col-md-6">
                    <div class="input-group">
                        <input data-role="datetimepicker" class="form-control" data-interval="15" data-bind="value: ticket.DomluvenyTerminCas, events: { change: ticket_date_change }" required />
                        <div class="input-group-prepend text-center">
                            <span class="input-group-text">až</span>
                        </div>
                        <input data-role="datetimepicker" class="form-control" data-interval="15" data-bind="value: ticket.DomluvenyTerminCasDo, minDateTime: ticket.DomluvenyTerminCas" required />
                    </div>
                </div>
            </div>
            <div class="form-row">
                <div class="form-group col-md-2">
                    <input type="text" class="form-control" data-bind="value: ticket.Firma" readonly>
                </div>
                <div class="form-group col-md-2">
                    <select data-role="dropdownlist" class="form-control" data-value-primitive="true" data-value-field="value" data-text-field="text" data-bind="source: resitele, value: ticket.IDUserResitel"></select>
                </div>
                <div class="form-group col-md-2">
                    <div class="input-group">
                        <select data-role="dropdownlist" class="form-control" data-value-primitive="true" data-value-field="value" data-text-field="text" data-bind="enabled: ticket.UdalostVGoogleCalend, source: lokality, value: ticket.rr_LokalitaBarva, events: { change: lokality_change }"></select>
                        <input data-role="colorpicker" type="color" style="font-size:16px;" class="form-control border" data-bind="enabled: ticket.UdalostVGoogleCalend, value: ticket.Barva" data-columns="2" data-palette='["#7986cb",
            "#33b679",
            "#8e24aa",
            "#e67c73",
            "#f6c026",
            "#f5511d",
            "#039be5",
            "#616161",
            "#3f51b5",
            "#0b8043",
            "#d60000"]' />
                    </div>
                </div>
            </div>
            <div class="form-row">
                <div class="form-group col-md-12">
                    <textarea rows="3" maxlength="250" type="text" class="form-control" data-bind="value: ticket.Telo" placeholder="Popis"></textarea>
                </div>
            </div>
            <div class="form-row">
                <div class="form-group col-md-12">
                    <textarea rows="3" maxlength="250" type="text" class="form-control" data-bind="value: ticket.InterniPoznamka" placeholder="Interní poznámka"></textarea>
                </div>
            </div>
            <div class="form-row">
                <div class="form-group col-md-6">
                    <select data-role="dropdownlist" class="form-control" data-value-primitive="true" data-value-field="value" data-text-field="text" data-bind="source: rr_FakturovatNaFirmu, value: ticket.rr_FakturovatNaFirmu"></select>
                </div>
            </div>
        </div>
        <div class="slide-window-footer">
            <div class="col text-left" data-bind="visible: ticket_leftbtn_visible">
                <button type="submit" class="btn btn-primary" name="action" value="1"><span class="k-icon k-i-check-circle"></span> Uložit a ukončit s pracákem</button>
                <button type="submit" class="btn btn-primary" name="action" value="2"><span class="k-icon k-i-check-circle"></span> Uložit a ukončit bez pracáku</button>
                <button type="submit" class="btn btn-danger" name="action" value="4"><span class="k-icon k-i-delete"></span> Stornovat</button>
            </div>
            <div class="col text-right">
                <button type="submit" class="btn btn-success" name="action" value="3"><span class="k-icon k-i-save"></span> Uložit</button>
                <button type="button" class="btn btn-danger btn-close" data-dismiss="modal"><span class="k-icon k-i-undo"></span> Zavřít</button>
            </div>
        </div>
    </form>

    <div class="modal fade" id="modal_google_event">
        <div class="modal-dialog modal-md">
            <div class="modal-content">
                <div class="modal-header">
                    <h4 class="modal-title" data-bind="text: google_event.title"></h4>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                </div>
                <div class="modal-body">
                    <div class="form-row">
                        <div class="form-group col-md-12">
                            <div class="input-group">
                                <input data-role="datetimepicker" class="form-control" data-bind="value: google_event.start" readonly />
                                <div class="input-group-prepend text-center">
                                    <span class="input-group-text">až</span>
                                </div>
                                <input data-role="datetimepicker" class="form-control" data-bind="value: google_event.end" readonly />
                            </div>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-12">
                            <span data-bind="text: google_event.location"></span>
                        </div>
                    </div>
                    <div class="form-row">
                        <div class="form-group col-md-12">
                            <textarea rows="5" type="text" class="form-control" data-bind="value: google_event.description" readonly></textarea>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal"><span class="k-icon k-i-cancel-outline"></span> Zavřít</button>
                </div>
            </div>
        </div>
    </div>

    <script id="tmppolozky" type="text/html">
        <div style="position:relative;width:100%;">
            <div>#:rr_TypPolozkyPracakuHodnota#</div>
            <small class="text-muted">#:TextNaFakturu#</small>
            <button style="position:absolute;right:0;top:0;bottom:0;" class="btn btn-danger" data-bind="events: { click: polozkyDelete }">Smazat</button>
        </div>
    </script>

    <div id="chat-dock"></div>

    <script src="~/Scripts/kendo.helper.js"></script>
    <script src="~/Scripts/jquery.signalR-2.2.3.min.js"></script>
    <script src="~/signalr/hubs"></script>

    <style>
        .k-message-group > img, .k-message-group .k-author {
            display: none;
        }

        .k-avatars .k-message-group:not(.k-alt):not(.k-no-avatar) {
            padding-left: 0;
        }

        .k-avatars .k-message-group.k-alt:not(.k-no-avatar) {
            padding-right: 0;
        }

        @@media (min-width: 992px) {
            .modal-lg {
                max-width: 96%;
            }

            .modal-md {
                max-width: 50%;
            }
        }

        .hr-2 {
            height: 2px;
            background: #6c757d;
        }

        wrap, .k-multiselect-wrap, .k-numeric-wrap, .k-picker-wrap {
            height: 100%;
        }

        .k-colorpicker .k-selected-color {
            height: 100%;
        }

        .k-switch-label-on, .k-switch-label-off {
            display: inline;
            font-family: Verdana, sans-serif;
            text-align: center;
        }

        .k-grid-norecords h3 {
            width: 100%;
        }
    </style>

    <style>
        .slide-window {
            display: flex;
            flex-direction: column;
            background: white;
            position: fixed;
            top: 0;
            right: -100%;
            bottom: 0;
            left: 100%;
            z-index: 1050;
            overflow: hidden;
            outline: 0;
        }

        .slide-window-header {
            display: -ms-flexbox;
            display: flex;
            -ms-flex-align: start;
            align-items: flex-start;
            -ms-flex-pack: justify;
            justify-content: space-between;
            padding: 1rem;
            border-bottom: 1px solid #e9ecef;
        }

        .slide-window-body {
            flex: auto;
            -ms-flex: auto;
            overflow-y: auto;
            padding: 1rem;
        }

        .slide-window-footer {
            display: -ms-flexbox;
            display: flex;
            -ms-flex-align: center;
            align-items: center;
            -ms-flex-pack: end;
            justify-content: flex-end;
            padding: 1rem;
            border-top: 1px solid #e9ecef;
        }
    </style>

    <script>
        var _notifi = null;
        $(document).ready(function () {
            _notifi = $('<span></span>').appendTo('body').kendoNotification({
                autoHideAfter: 0,
                show: function (e) {
                },
                position: {
                    pinned: false,
                    bottom: 50,
                    right: 50,
                },
                stacking: "up"
            }).data("kendoNotification");
        });

        function kendoNotification(message, type, delay) {
            if (type === undefined) { type = "info" };
            if (delay === undefined) { delay = 5000 };
            var sec = delay / 1000;
            var id = 'badge_' + kendo.guid();
            message = '<div style="min-width:150px;"><span class="k-badge k-badge-solid k-badge-md k-badge-' + type + ' k-badge-pill k-badge-edge k-top-end" id="' + id + '">' + sec + '</span> ' + message + '</div>';
            _notifi.setOptions({ autoHideAfter: delay });
            _notifi.show(message, type);
            var int = setInterval(function (e) {
                var span = $("#" + id);
                sec = sec - 1;
                span.text(sec);
                if (sec < 1) {
                    clearInterval(int);
                }
            }, 1000);
        }

        (function ($) {
            $.fn.slideWindow = function (method) {
                if (method == 'show') {
                    this.animate({
                        "left": "0",
                        "right": "0"
                    }, 250)
                }
                if (method == 'hide') {
                    this.animate({
                        "left": "100%",
                        "right": "-100%"
                    }, 250)
                }
            };
            $(document).on("click", ".slide-window .btn-close", function () {
                var win = $(this).closest(".slide-window");
                win.slideWindow("hide");
            });
        }(jQuery));

        //function showNotification() {
        //    if (window.Notification) {
        //        Notification.requestPermission(function (status) {
        //            console.log('Status: ', status); // show notification permission if permission granted then show otherwise message will not show
        //            var options = {
        //                body: 'Test notification message.', // body part of the notification
        //                dir: 'ltr', // use for direction of message
        //                image: '/Images/crm.png' // use for show image

        //            }

        //            new Notification('Title', options);
        //        });
        //    }
        //    else {
        //        alert('Your browser doesn\'t support notifications.');
        //    }
        //}

        function colorLuminance(hex, lum) {
            // validate hex string
            hex = String(hex).replace(/[^0-9a-f]/gi, '');
            if (hex.length < 6) {
                hex = hex[0] + hex[0] + hex[1] + hex[1] + hex[2] + hex[2];
            }
            lum = lum || 0;
            // convert to decimal and change luminosity
            var rgb = "#", c, i;
            for (i = 0; i < 3; i++) {
                c = parseInt(hex.substr(i * 2, 2), 16);
                c = Math.round(Math.min(Math.max(0, c + (c * lum)), 255)).toString(16);
                rgb += ("00" + c).substr(c.length);
            }
            return rgb;
        }

        function textColor(hexcolor) {
            var r = parseInt(hexcolor.substr(0, 2), 16);
            var g = parseInt(hexcolor.substr(2, 2), 16);
            var b = parseInt(hexcolor.substr(4, 2), 16);
            var yiq = ((r * 299) + (g * 587) + (b * 114)) / 1000;
            return (yiq >= 128) ? 'black' : 'white';
        }

        function toggleWrap(e) {
            if (e.style.whiteSpace == 'nowrap') {
                e.style.whiteSpace = 'normal';
            } else {
                e.style.whiteSpace = 'nowrap';
            }
        }

        function uvolnitLicence() {
            $.get("@Url.Action("uvolnit_licence", "Api/AVISService")", null, function (result) {
                if (result.error) { alert(result.error) } else {
                    alert("Licence uvolněny")
                }
            });
        }

            function dostupnostAVIS() {
                $.get("@Url.Action("dostupnost_sluzby", "Api/AVISService")", null, function (messages) {
                    var lines = messages.join("\n")
                    alert(lines)
                });
            }

        $(function () {
            @*var source = "@(Url.Action("chat.mp3", "Sounds"))";
            var audio = new Audio();
            audio.addEventListener("load", function () {
                //audio.play();
            }, true);
            audio.volume = 0.2;
            audio.src = source;
            //audio.autoplay = true;

            var hub = $.connection.AgiloHub;
            var chatModel = kendo.observable({
                source: new kendo.data.DataSource({
                    data: [],
                    schema: {
                        model: { id: "idu" }
                    }
                }),
                showChat: function (e) {
                    var d = e.data;
                    var toID = d.idu;
                    var fromID = dataUser.IDUser
                    var div = $('div[data-idu="' + toID + '"]')
                    if (div.length > 0) {
                        div.show();
                    } else {
                        var tmp = $('<div class="chat-win" data-idu="' + toID + '">' +
                            '<nav class= "navbar bg-primary" >' +
                            '<div class="d-flex flex-row">' +
                            '<a class="navbar-brand" href="\#">' +
                            '<img src="' + d.iconUrl + '" alt="Client box chat" width="32" height="32" /><span class="ml-1 text-white">' + d.name + '</span>' +
                            '</a>' +
                            '</div>' +
                            '<button type="button" class="close" data-dismiss="modal" onclick="$(this).closest(\'.chat-win\').hide();" aria-label="Close">' +
                            '<span aria-hidden="true">&times;</span>' +
                            '</button>' +
                            '</nav >' +
                            '</div >');

                        tmp.click(function () {
                            hub.server.send_showing(toID, fromID);
                        });

                        tmp.appendTo("#chat-dock");

                        var chat = tmp.kendoChat({
                            user: {
                                idu: toID,
                                name: d.name,
                                iconUrl: d.iconUrl
                            },
                            messages: {
                                placeholder: 'Napište zprávu...'
                            },
                            typingStart: function (e) {
                                hub.server.send_typing(toID, fromID);
                            },
                            post: function (e) {
                                e.text = e.text.replace(/:\)/g, "🙂");
                                e.text = e.text.replace(/:-\)/g, "🙂");
                                e.text = e.text.replace(/:D/g, "😃");
                                e.text = e.text.replace(/:-D/g, "😃");
                                e.text = e.text.replace(/\;\)/g, "😉");
                                e.text = e.text.replace(/\;-\)/g, "😉");
                                e.text = e.text.replace(/:\(/g, "🙁");
                                e.text = e.text.replace(/:-\(/g, "🙁");
                                e.text = e.text.replace(/:-o/g, "😮");
                                e.text = e.text.replace(/:o/g, "😮");
                                hub.server.send_message(toID, fromID, e.text);
                            }
                        }).data("kendoChat");
                    }
                }
            });

            kendo.bind($("#chatUsers"), chatModel);

            hub.client.tick = function () {
                hub.server.set_activity(dataUser.IDUser, 0, 0, "browser");
            }
            hub.client.get_activity = function (data) {
                var dataSource = chatModel.get("source");
                dataSource.data(data);
            }
            hub.client.typing = function (toID, sender) {
                if (toID === dataUser.IDUser) {
                    var div = $('div[data-idu="' + sender.idu + '"]');
                    if (div.length > 0) {
                        var chat = div.data("kendoChat");
                        chat.renderMessage({ type: 'typing' }, sender);
                    }
                }
            }
            hub.client.showing = function (toID, sender) {
                if (toID === dataUser.IDUser) {
                    var div = $('div[data-idu="' + sender.idu + '"]');
                    if (div.length > 0) {
                        var msg = div.find(".k-message-group.k-alt:last-child").find(".k-message:last-child");
                        if (msg.length > 0) {
                            msg.find('.text-muted').remove();
                            msg.append('<small class="text-muted">zobrazeno v ' + kendo.toString(new Date(), 'HH:mm') + '</span>')
                        }
                    }
                }
            }
            hub.client.message = function (toID, sender, text) {
                if (toID === dataUser.IDUser) {
                    var div = $('div[data-idu="' + sender.idu + '"]');
                    if (div.length > 0) {
                        div.show();
                        var chat = div.data("kendoChat");
                        chat.renderMessage({ type: "text", text: text }, sender);
                        audio.autoplay = true;
                        audio.play();
                    } else {
                        var tmp = $('<div class="chat-win" data-idu="' + sender.idu + '">' +
                            '<nav class= "navbar bg-primary" >' +
                            '<div class="d-flex flex-row">' +
                            '<a class="navbar-brand" href="\#">' +
                            '<img src="' + sender.iconUrl + '" alt="Client box chat" width="32" height="32" /><span class="ml-1 text-white">' + sender.name + '</span>' +
                            '</a>' +
                            '</div>' +
                            '<button type="button" class="close" data-dismiss="modal" onclick="$(this).closest(\'.chat-win\').hide();" aria-label="Close">' +
                            '<span aria-hidden="true">&times;</span>' +
                            '</button>' +
                            '</nav >' +
                            '</div >');

                        tmp.click(function () {
                            hub.server.send_showing(sender.idu, dataUser.IDUser);
                        });

                        tmp.appendTo("#chat-dock");

                        var chat = tmp.kendoChat({
                            user: {
                                idu: sender.idu,
                                name: sender.name,
                                iconUrl: sender.iconUrl
                            },
                            messages: {
                                placeholder: 'Napište zprávu...'
                            },
                            typingStart: function (e) {
                                hub.server.send_typing(sender.idu, dataUser.IDUser);
                            },
                            post: function (e) {
                                e.text = e.text.replace(/:\)/g, "🙂");
                                e.text = e.text.replace(/:-\)/g, "🙂");
                                e.text = e.text.replace(/:D/g, "😃");
                                e.text = e.text.replace(/:-D/g, "😃");
                                e.text = e.text.replace(/\;\)/g, "😉");
                                e.text = e.text.replace(/\;-\)/g, "😉");
                                e.text = e.text.replace(/:\(/g, "🙁");
                                e.text = e.text.replace(/:-\(/g, "🙁");
                                e.text = e.text.replace(/:-o/g, "😮");
                                e.text = e.text.replace(/:o/g, "😮");
                                hub.server.send_message(sender.idu, dataUser.IDUser, e.text);
                            }
                        }).data("kendoChat");

                        chat.renderMessage({ type: "text", text: text }, sender);

                        audio.autoplay = true;
                        audio.play();
                    }
                }
            }
            $.connection.hub.start({ jsonp: true }).done(function () {
                hub.server.set_activity(dataUser.IDUser, 0, 0, "browser");
            })*@
        })
    </script>
</body>

</html>
