@Code
    ViewData("Title") = "Tikety"
    Layout = "~/Views/Shared/_Layout.vbhtml"

    Dim db As New Data4995Entities
    Dim _user = db.AG_tblUsers.FirstOrDefault(Function(e) e.UserLogin = User.Identity.Name)
End Code

<div id="main" style="height: 100%;">
    <div data-role="splitter"
         data-panes="[
        { collapsible: true, scrollable: false, size: '400px' },
        { collapsible: true, size: '350px' },
        { collapsible: false }
        ]"
         data-orientation="horizontal"
         style="height:100%;width:100%;">
        <div>
            <div data-role="grid"
                 data-template="ticket-template"
                 data-scrollable="true"
                 data-selectable="true"
                 data-pageable="true"
                 data-toolbar="[{ template: kendo.template($('#toolbar-template').html()) }]"
                 data-row-template="ticket-template"
                 data-bind="source: tickets, toolbar: toolbar, events: { change: ticket_change }"
                 data-columns="[{ field: 'IDTicket', headerAttributes: { style: 'display: none' }}]"
                 style="height:100%;"></div>
        </div>
        <div class="panel-menu">
            <h4 class="p-3 border-top border-bottom">Kalendáře</h4>
            <div class="p-3" data-bind="source: calendars" data-template="clendar-item-template">

            </div>
            <h4 class="p-3 border-top border-bottom">Historie kontaktů</h4>
            <div data-role="grid"
                 data-selectable="true"
                 data-scrollable="true"
                 data-row-template="contact-template"
                 data-bind="source: contacts, events: { change: contacts_change, dataBound: contacts_bound }"
                 data-columns="[{ field: 'IDCall', headerAttributes: { style: 'display: none' }}]" style="height:calc(100% - 290px);"></div>
        </div>
        <div style="min-width:400px;">
            <div data-role="scheduler"
                 data-editable="false"
                 data-selectable="false"
                 data-footer="false"
                 data-views="[{ type: 'day', eventTemplate: $('#event-template').html() }, { type: 'workWeek', selected: true, eventTemplate: $('#event-template').html() }]"
                 data-bind="source: calendar, events: { dataBound: calendar_bound, navigate: navigate }"
                 style="height: 100%"></div>
        </div>
    </div>
</div>

<script type="text/x-kendo-tmpl" id="toolbar-template">
    <div class="row">
        <div class="form-row col-md-12">
            <div class="input-group bg-secondary border">
                <select data-role="dropdownlist" class="form-control" data-value-primitive="true" data-value-field="value" data-text-field="text" data-option-label="{ value: 0, text: 'Všichni uživatelé' }" data-bind="source: resitele, value: iduser, events: { change: user_change }"></select>
                <select data-role="dropdownlist" class="form-control" data-value-primitive="true" data-value-field="value" data-text-field="text" data-bind="source: stavy, value: stav, events: { change: stav_change }"></select>
            </div>
        </div>
        <div class="form-row col-md-12 mt-1">
            <div class="input-group">
                <input data-role="autocomplete"
                       data-placeholder="Hledat ..."
                       data-value-field="firma"
                       data-text-field="firma"
                       data-suggest="true"
                       data-bind="value: ticked_selected.Firma, source: pobocky, events: { select: autocomplete_select }" class="form-control" />
                <div class="input-group-append">
                    <button class="btn btn-default border" aria-label="" title="Odfiltrovat podle firmy" data-bind="events: { click: filter }"><span class="k-icon k-i-filter"></span></button>
                </div>
                <div class="input-group-append">
                    <button class="btn btn-default border" aria-label="" title="Smazat filtr" data-bind="events: { click: filter_clear }"><span class="k-icon k-i-filter-clear"></span></button>
                </div>
                <div class="input-group-append">
                    <button class="btn btn-default border" aria-label="" title="Vytvořit ticket na vybranou firmu" data-bind="events: { click: add_ticket }"><span class="k-icon k-i-plus"></span></button>
                </div>
            </div>
        </div>
    </div>
</script>

<script id="clendar-item-template" type="text/x-kendo-template">
    <div class="w-100">
        <input type="checkbox" name="calendar" id="calendar${id}" value="${calendarId}" data-bind="checked: cid, events: { change: calendar_change }" />
        <label for="calendar${id}"><span class="k-icon k-i-round-corners" style="color:${calendarColor}"></span> ${name}</label>
    </div>
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

<script type="text/x-kendo-tmpl" id="contact-template">
    <tr data-uid="${uid}" role="row" data-callid="${IDCall}">
        <td role="gridcell">
            <div class="ticket p-3">
                # if(Nazev_Pobocky) { #
                <h5 title="${Nazev_Pobocky}" class="text-success">${Nazev_Pobocky}</h5>
                # } #
                # if(JmenoKontaktu) { #
                <div><span class="k-icon k-i-user"></span> ${JmenoKontaktu}</div>
                # } #
                # if(PhoneNumber) { #
                <div>☎ ${PhoneNumber} <span style="float:right;"><span class="k-icon k-i-clock"></span> ${CallTime}</span></div>
                # } #
            </div>
        </td>
    </tr>
</script>

<script type="text/x-kendo-tmpl" id="ticket-template">
    <tr data-uid="${uid}" role="row" data-id="${IDTicket}">
        <td role="gridcell">
            <div class="ticket p-3">
                <div>
                    <span class="badge badge-info">\#${IDTicket}</span>
                    # if(rr_TicketStav == 1) { #
                    <span class="badge badge-primary">Aktivní</span>
                    # } #
                    # if(rr_TicketStav == 4) { #
                    <span class="badge badge-success">Ukončen</span>
                    # } #
                    # if(rr_TicketStav == 5) { #
                    <span class="badge badge-danger">Stornován</span>
                    # } #
                    <span style="float:right;"><span class="badge badge-light">${UserResitel}</span> <span class="badge badge-light">#=kendo.toString(new Date(DomluvenyTerminCas), "dd.MM.yyyy HH:mm")#</span></span>
                </div>
                <h5 title="${Nazev_pobocka}"><a class="k-link" href="\#" data-bind="events: { click: ticket_detail }">${Nazev_pobocka}</a></h5>
                <span>${Predmet}</span>
            </div>
        </td>
    </tr>
</script>

<style>
    .k-scheduler-table tr th {
        text-align: center;
    }

    .ticket {
        -webkit-border-radius: 4px;
        -moz-border-radius: 4px;
        border-radius: 4px;
        margin: 6px;
        color: #1c1e21;
        background: #fff;
        box-shadow: 0 1px 2px rgba(0, 0, 0, 0.20);
    }

        .ticket h5 {
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
        }

    .k-grid a:hover {
        text-decoration: underline;
    }

    [data-role="grid"] {
        background: #f0f2f5
    }

    .panel-menu {
        background: #f0f2f5
    }

    .k-editor .k-editor-toolbar, .k-gantt-toolbar, .k-grid-toolbar, .k-toolbar {
        background: #f0f2f5;
    }

    [data-role="grid"].k-widget {
        border: 0px;
    }

    .k-grid-header {
        border-bottom-width: 0px;
    }

    .k-grid-toolbar {
        border-width: 0 0 0px;
    }

    .k-grid td.k-state-selected, .k-grid th.k-state-selected, .k-grid tr.k-state-selected {
        background-color: #28a745;
    }

    .k-grid tbody tr.k-state-selected:hover {
        background-color: #28a745;
    }

    .k-grid tr.newcall .ticket {
        background: #ffd5ad;
    }
</style>

<script>

    $(function () {

        var elementTop = 0;
        var containerTop = 0;

        kendo.ui.Scheduler.fn.options.workDayStart = new Date("2020/11/18 08:00 AM");
        kendo.ui.Scheduler.fn.options.workDayEnd = new Date("2020/11/18 4:30 PM");

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

        var new_IDCall = null;
        var viewModel = kendo.observable({
            weekdate: new Date(),
            iduser: @_user.IDUser,
            stav: 1,
            cid: ["podpora@agilo.cz", "nezbeda@doctorum.cz"],
            original: null,
            ticket: null,
            ticket_detail: function (e) {
                this.set("original", e.data.toJSON());
                this.set("ticket", e.data.toJSON());
                this.pobocky.read();
                this.lokality.read({ iDUser: e.data.IDUserResitel });
                $('#modal_ticket').slideWindow("show");
            },
            calendars: [
                { id: 1, name: "Pavel Pecher", calendarColor: "#7986CB", calendarId: "pecher@agilo.cz" },
                { id: 2, name: "Tomáš Nezbeda", calendarColor: "#EF6C00", calendarId: "nezbeda@doctorum.cz" },
                { id: 3, name: "Petr Novák", calendarColor: "#8e24aa", calendarId: "700nr6qvjl5a88nee07sl7oen8@group.calendar.google.com" },
                { id: 4, name: "Marek Hanzl", calendarColor: "#039be5", calendarId: "0pq584pdtsp0i8ge18q6g9jb1s@group.calendar.google.com" }
            ],
            contacts: new kendo.data.DataSource({
                schema: {
                    data: "data",
                    total: "total",
                    errors: "error"
                },
                transport: {
                    read: {
                        url: "@Url.Action("AGsp_Get_PhoneCallListUser", "Api/Service")",
                        type: "GET"
                    }
                }
            }),
            contacts_change: function (e) {
                $("#slide-ticket").slideWindow("show");
                var grid = e.sender;
                var row = grid.select();
                var di = grid.dataItem(row);
                if (di.Firma) {
                    var ticket = {
                        IDTicket: 0,
                        Firma: di.Firma,
                        Nazev_firmy: di.Nazev_firmy,
                        Pobocka: null,
                        Nazev_pobocka: di.Nazev_Pobocky,
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
                    };
                    this.toolbar.model.set("ticked_selected", ticket);
                }
            },
            contacts_bound: function (e) {
                if (new_IDCall) {
                    var row = $('[data-callid="' + new_IDCall + '"]');
                    e.sender.select(row);
                    new_IDCall = null;
                }
            },
            ispopis: function () {
                var ticket = this.get("ticket");
                if (ticket) {
                    return (ticket.get("Telo") ? true : false);
                }
                return false;
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
                var ticket = this.get("ticket");
                var items = e.sender.items();
                if (items.length > 0) {
                    if (ticket) {
                        var value = ticket.get("Pobocka");
                        if (!value) {
                            var di = e.sender.dataItem(0);
                            this.ticket.set("Pobocka", di.value);
                        } else {
                            e.sender.value(value);
                        }
                    }
                }
            },
            typyzasahu_change: function (e) {
                this.ticket.set("rr_TypZasahuText", e.sender.text());
            },
            pobocky: new kendo.data.DataSource({
                schema: {
                    data: "data",
                    total: "total",
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
            ticket_leftbtn_visible: function () {
                var ticket = this.get("ticket");
                if (ticket) {
                    return (ticket.get("IDTicket") ? true : false);
                }
                return false;
            },
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
                var savetype = $(e.originalEvent.submitter).attr("value");
                var data = this.get("ticket").toJSON(),
                    orig = this.get("original").toJSON(),
                    that = this,
                    hasChanged = (JSON.stringify(data) !== JSON.stringify(orig));

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
                        that.tickets.read();
                        that.calendar.read();
                        that.set("ticket", null);
                        that.set("original", null);
                        $('#modal_ticket').slideWindow("hide");
                        if (result.action == 1) {
                            if (!result.data) {
                                kendoNotification("Klient nemá vyplněný email", "info", 5000);
                            } else {
                                if (data.rr_TypZasahu != 3) {
                                    that.set("ticket_email", result.data);
                                    $('#modal_email').modal("show");
                                }
                            }
                        }
                        if (result.action == 2) {
                            if (!result.data) {
                                kendoNotification("Klient nemá vyplněný email", "info", 5000);
                            } else {
                                if (data.rr_TypZasahu != 3) {
                                    that.set("ticket_email", result.data);
                                    $('#modal_email').modal("show");
                                }
                            }
                        }
                        if (result.action == 3) {
                            if (!result.data) {
                                kendoNotification("Klient nemá vyplněný email", "info", 5000);
                            } else {
                                if (data.rr_TypZasahu != 3) {
                                    if (hasChanged) {
                                        that.set("ticket_email", result.data);
                                        $('#modal_email').modal("show");
                                    } else {
                                        kendoNotification("Žádná změna", "info", 5000);
                                    }
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
                        if (email.action == 1) {
                             window.open(
                                 '@Url.Action("Fakturace", "Home")' + "?i=" + email.id,
                                '_blank'
                            );
                        }
                    }
                });
            },
            close_email: function (e) {
                var email = this.get("ticket_email");
                if (email.action == 1) {
                    window.open(
                        '@Url.Action("Fakturace", "Home")' + "?i=" + email.id,
                        '_blank'
                    );
                };
            },
            toolbar: {
                template: kendo.template($('#toolbar-template').html()),
                model: kendo.observable({
                    stav: 1,
                    iduser: @_user.IDUser,
                    ticked_selected: {
                        Pobocka: null
                    },
                    resitele: new kendo.data.DataSource({
                        schema: { data: "data", total: "total" },
                        transport: { read: "@Url.Action("Uzivatele", "Api/Service")" }
                    }),
                    stavy: new kendo.data.DataSource({
                        schema: { data: "data", total: "total" },
                        transport: { read: "@Url.Action("RegRest", "Api/Service")?Register=rr_TicketStav" }
                    }),
                    user_change: function (e) {
                        var iduser = this.get("iduser");
                        viewModel.set("iduser", iduser);
                        viewModel.tickets.read();
                    },
                    stav_change: function (e) {
                        var stav = this.get("stav");
                        viewModel.set("stav", stav);
                        viewModel.tickets.read();
                    },
                    filter: function (e) {
                        var selected = this.get("ticked_selected");
                        if (selected) {
                            viewModel.tickets.filter({ field: "Firma", operator: "eq", value: selected.Firma });
                        }
                    },
                    filter_clear: function (e) {
                        viewModel.tickets.filter({});
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
                    autocomplete_select: function (e) {
                        var di = e.sender.dataItem(e.item.index());
                        this.set("ticked_selected", {
                            IDTicket: 0,
                            Firma: di.firma,
                            Nazev_firmy: di.firma,
                            Pobocka: di.pobocka,
                            Nazev_pobocka: di.pobocka,
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
                        });
                    },
                    add_ticket: function (e) {
                        var ticket = this.get("ticked_selected");
                        if (ticket) {
                            var selected = ticket.toJSON();
                            if (selected.Firma) {
                                var ticket = {
                                    IDTicket: 0,
                                    Firma: selected.Firma,
                                    Nazev_firmy: selected.Nazev_firmy,
                                    Pobocka: null,
                                    Nazev_pobocka: selected.Nazev_Pobocky,
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
                                };
                                viewModel.set("ticket", ticket);
                                viewModel.set("original", ticket);
                                viewModel.pobocky.read();
                                viewModel.lokality.read({ iDUser: selected.IDUserResitel });
                                $('#modal_ticket').slideWindow("show");
                            }
                        }
                    }
                })
            },
            calendar_change: function (e) {
                this.calendar.read();
            },
            tickets: new kendo.data.DataSource({
                pageSize: 20,
                schema: {
                    data: "data",
                    total: "total",
                    errors: "error",
                    model: {
                        id: "IDTicket",
                        fields: {

                        }
                    }
                },
                transport: {
                    read: {
                        url: "@Url.Action("AGsp_Get_TicketSeznam", "Api/Service")"
                    },
                    parameterMap: function (options, operation) {
                        var iduser = viewModel.get("iduser"),
                            stav = viewModel.get("stav");
                        return { iDUser: iduser, rr_TicketStav: stav };
                    }
                }
            }),
            calendar: new kendo.data.SchedulerDataSource({
                transport: {
                    read: {
                        url: "@Url.Action("getGoogleEvents", "Api/Service")",
                        dataType: "json",
                        type: "POST"
                    },
                    parameterMap: function (options, operation) {
                        var ids = viewModel.get("cid").toJSON(),
                            wdt = viewModel.get("weekdate");
                        return { "": ids, weekdate: kendo.toString(wdt, "yyyy-MM-dd HH:mm:ss") };
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
                this.calendar.read();
            },
            ticket_change: function (e) {
                var grid = e.sender;
                var di = grid.dataItem(grid.select());
                this.toolbar.model.set("ticked_selected", di.toJSON());
            },
            calendar_bound: function (e) {
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

                var scheduler = e.sender;
                var contentDiv = scheduler.element.find("div.k-scheduler-content");
                var hodrow = contentDiv.find(".k-scheduler-table tr:nth-child(17)");
                elementTop = hodrow.offset().top;
                containerTop = contentDiv.offset().top;
                var top = (elementTop - containerTop);
                if (top > 0) {
                    contentDiv.scrollTop(top);
                }
            },
            google_event: null,
            event_click: function (e) {
                var event = e.data.toJSON();
                this.set("google_event", event);
                if (event.iDT > 0) {
                    $.get("@Url.Action("AGsp_Get_TicketDetail", "Api/Service")", { iDTicket: event.iDT }, function (result) {
                        if (result.error) {
                            alert(result.error);
                        } else {
                            viewModel.set("ticket", result.data);
                            viewModel.set("original", result.data);
                            viewModel.pobocky.read();
                            viewModel.lokality.read({ iDUser: result.IDUserResitel });
                            $('#modal_ticket').slideWindow("show");
                        }
                    });
                } else {
                    $("#modal_google_event").modal("show");
                }
            }
        });
        try {
            kendo.bind(document.body, viewModel);
        } catch (ex) {
            console.log(ex)
        }
        
        var source = "@(Url.Action("chat.mp3", "Sounds"))";
            var audio = new Audio();
            audio.volume = 0.2;
            audio.src = source;
        var hub = $.connection.AgiloHub;
        hub.client.phoneRinging = function (id, login, clientName, clientNumber) {
            if ('@User.Identity.Name.ToLower' == login.toLowerCase()) {
                audio.play();
                new_IDCall = id;
                viewModel.contacts.read();
            }
        };
        $.connection.hub.start({ jsonp: true }).done(function () {

        })
    });
</script>
