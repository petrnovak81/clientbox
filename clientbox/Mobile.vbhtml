<style>
    .km-nova .k-toolbar, .km-nova .km-navbar, .km-nova .km-tabstrip {
        background-image: linear-gradient(to bottom,#e4322b 0,#e4322b 100%);
    }
    .km-nova .km-collapsible-header, .km-nova .km-collapsible-header h1, .km-nova .km-collapsible-header h2, .km-nova .km-collapsible-header h3, .km-nova .km-collapsible-header h4, .km-nova .km-collapsible-header h5, .km-nova .km-collapsible-header h6, .km-nova .km-legend-button, .km-nova .km-list > li > .km-listview-link {
        color: #e4322b;
    }
    .text-danger {
        color: #e4322b !important;
    }
    img {
        object-fit: cover;
    }
    .selected {
        background: rgba(1, 129, 0, 0.10)!important;
    }
    .disabled {
        filter: grayscale(100%);
        opacity: 0.8;
    }
    .today {
        background: #dff1ff !important;
    }
    .k-window-titlebar {
        width: auto;
    }
    .km-widget .km-listview-label, .km-widget .km-listview-link {
        line-height: 1;
    }
    .km-nova .km-list > li {
        padding-left: .89em;
        padding: 1.09em .84em 1.23em .84em;
    }
    .k-dialog-buttongroup {
        padding: 0;
    }
    .km-nova .km-list .km-label-above .k-dropdown, .km-nova .km-list .km-label-above input[type=color], .km-nova .km-list .km-label-above input[type=date], .km-nova .km-list .km-label-above input[type=datetime-local], .km-nova .km-list .km-label-above input[type=datetime], .km-nova .km-list .km-label-above input[type=email], .km-nova .km-list .km-label-above input[type=file], .km-nova .km-list .km-label-above input[type=month], .km-nova .km-list .km-label-above input[type=number], .km-nova .km-list .km-label-above input[type=password], .km-nova .km-list .km-label-above input[type=search], .km-nova .km-list .km-label-above input[type=tel], .km-nova .km-list .km-label-above input[type=text]:not(.k-input), .km-nova .km-list .km-label-above input[type=time], .km-nova .km-list .km-label-above input[type=url], .km-nova .km-list .km-label-above input[type=week], .km-nova .km-list .km-label-above select:not([multiple]), .km-nova .km-list .km-label-above textarea {
        width: 90%;
    }
    .footer-submit {
        border: 0;
        background: transparent !important;
        font-size: 1.2em;
        line-height: 2.78em;
        color: white;
    }
    .blur {
        filter: blur(1px);
        -webkit-filter: blur(1px);
    }
</style>

<div data-role="view" data-title="" id="produkty" data-use-native-scrolling="true" data-model="app.produkty" data-reload="false">
    <header data-role="header">
        <div data-role="navbar">
            <span data-align="left" class="pl-3"><a href="tel:773034033" class="text-white">Mobil: +420 773 034 033</a></span>
            <span data-role="view-title"></span>
            <span data-align="right" class="pr-3"><a href="#" class="text-white">Cena celkem <span data-bind="text: totalprice"></span> Kč</a></span>
        </div>
    </header>
    <div class="mt-2 text-center">
        <img src="~/Images/kantynalogo.png" />
    </div>
    <h3 class="p-3 bg-white mb-0 text-center">
        <span class="text-danger">
            ROZVOZ PO KLADNĚ A OKOLÍ<br />
            Objednávky pro rozvoz přijímáme nejpozději do <strong>10:00 hodin</strong>, ideálně však den předem
        </span><br />
        Aktuální nabídka<br /><strong data-bind="text: oddo"></strong>
    </h3>

    <div data-bind="css: { today: po.today, disabled: po.disabled }" class="border-top">
        <div class="km-group-title">
            <div class="km-text font-weight-bold" data-bind="text: po.title"></div>
        </div>
        <ul data-role="listview" data-style="inset" data-bind="source: po.data" data-template="produkt-template"></ul>
    </div>
    <div data-bind="css: { today: ut.today, disabled: ut.disabled }">
        <div class="km-group-title">
            <div class="km-text font-weight-bold" data-bind="text: ut.title"></div>
        </div>
        <ul data-role="listview" data-style="inset" data-bind="source: ut.data" data-template="produkt-template"></ul>
    </div>
    <div data-bind="css: { today: st.today, disabled: st.disabled }">
        <div class="km-group-title">
            <div class="km-text font-weight-bold" data-bind="text: st.title"></div>
        </div>
        <ul data-role="listview" data-style="inset" data-bind="source: st.data" data-template="produkt-template"></ul>
    </div>
    <div data-bind="css: { today: ct.today, disabled: ct.disabled }">
        <div class="km-group-title">
            <div class="km-text font-weight-bold" data-bind="text: ct.title"></div>
        </div>
        <ul data-role="listview" data-style="inset" data-bind="source: ct.data" data-template="produkt-template"></ul>
    </div>
    <div data-bind="css: { today: pa.today, disabled: pa.disabled }">
        <div class="km-group-title">
            <div class="km-text font-weight-bold" data-bind="text: pa.title"></div>
        </div>
        <ul data-role="listview" data-style="inset" data-bind="source: pa.data" data-template="produkt-template"></ul>
    </div>
    <footer data-role="footer">
        <div data-role="navbar">
            <button type="button" data-icon="cart" class="footer-submit"  data-bind="events: { click: objednat }">Objednat rozvoz</button>
        </div>
    </footer>
</div>

<div data-role="view" data-use-native-scrolling="true" data-title="Objednávka" id="objednavka" data-model="app.objednavka" data-show="app.objednavka.show">
    <header data-role="header">
        <div data-role="navbar">
            <a data-align="left" href="#/" data-role="backbutton"></a>
            <span data-role="view-title"></span>
        </div>
    </header>
    <form data-bind="events: { submit: objednat }">
        <div class="km-group-title">
            <div class="km-text">Položky objednávky</div>
        </div>
        <ul data-role="listview" data-style="inset" data-bind="source: data" data-template="souhrn-template"></ul>
        <div class="km-group-title">
            <div class="km-text">Kam a komu to máme dovézt</div>
        </div>
        <ul data-role="listview" data-style="inset">
            <li>
                <label class="km-required km-label-above pl-2">
                    Jméno a příjmení
                    <input type="text" data-bind="value: form.name" maxlength="150" required class="pl-2" placeholder="Zadejte Váše jméno a příjmení" />
                </label>
            </li>
            <li>
                <label class="km-required km-label-above pl-2">
                    Email
                    <input type="email" data-bind="value: form.email" maxlength="150" required class="pl-2" placeholder="Zadejte Váš e-mail" />
                </label>
            </li>
            <li>
                <label class="km-required km-label-above pl-2">
                    Telefon
                    <input type="tel" data-bind="value: form.tel" maxlength="9" required class="pl-2" placeholder="Zadejte Váše telefonní číslo (606111222)" />
                </label>
            </li>
            <li>
                <label class="km-required km-label-above pl-2">
                    Adresa
                    <input type="text" data-bind="value: form.addr" maxlength="150" required class="pl-2" placeholder="Zadejte adresu doručení" />
                </label>
            </li>
            <li>
                <a data-role="button" data-bind="events: { click: call }" data-icon="phone">Zavolat a zeptat se na adresu</a>
            </li>
            <li>
                <label class="km-label-above pl-2">
                    Doplňující informace k objednávce
                    <textarea rows="5" data-bind="value: form.msg"></textarea>
                </label>
            </li>
        </ul>
        <div data-role="footer">
            <div data-role="navbar">
                <button type="submit" class="footer-submit">Objednat</button>
            </div>
        </div>
    </form>
</div>

<script id="adresa-template" type="text/x-kendo-template">
    <option value="${Adresa}">
</script>

<script id="souhrn-template" type="text/x-kendo-template">
    <span>${day}, ${subtitle}</span>
    <br />
    <small>Počet porcí: ${number}, Cena: ${price} Kč</small>
</script>

<script id="produkt-template" type="text/x-kendo-template">
    # if(!disabled) { #
    <a class="km-listview-link" data-role="listview-link" data-bind="events: { click: mnozstvi }">
        <table class="w-100 text-body">
            <tr>
                <td width="60">
                    <img width="48" height="48" src="${img}" onError="this.onerror=null;this.src='/Images/placeholder.png';" class="rounded-circle border ${(blur ? 'blur' : '')}" />
                </td>
                <td>${title}<br /><small>${subtitle}</small></td>
                <td width="120" class="text-right pr-4"><small>Cena: <span>${price}</span> Kč</small><br /><small>Počet porcí: <span data-bind="text: number"></span></small></td>
            </tr>
        </table>
    </a>
    # } else { #
    <div class="km-listview-link">
    <table class="w-100">
        <tr>
            <td width="60">
                <img width="48" height="48" src="${img}" onError="this.onerror=null;this.src='/Images/placeholder.png';" class="rounded-circle border ${(blur ? 'blur' : '')}" />
            </td>
            <td>${title}<br /><small>${subtitle}</small></td>
            <td width="120" class="text-right text-success pr-4"><small>Cena: <span>${price}</span> Kč</small><br /><small>Počet porcí: <span data-bind="text: number"></span></small></td>
        </tr>
    </table>
    </div>
    # } #
</script>

<div id="form-porce"
     data-role="window"
     data-width="90%"
     data-height="auto"
     data-modal="true"
     data-visible="false"
     data-title="Počet porcí"
     data-actions="[]" style="display:none;">
    <div class="text-center mb-3">
        <h3 data-bind="text: item.title"></h3>
        <span data-bind="text: item.subtitle"></span>
    </div>
    <form method="post" data-bind="events: { submit: ok }">
        <div class="input-group input-group-lg">
            <div class="input-group-prepend">
                <button class="btn btn-outline-warning" type="button" data-bind="events: { click: minus }"><i class="fa fa-minus"></i></button>
            </div>
            <input type="text" readonly class="form-control text-center" data-bind="value: item.number">
            <div class="input-group-append">
                <button class="btn btn-outline-success" type="button" data-bind="events: { click: plus }"><i class="fa fa-plus"></i></button>
            </div>
        </div>
        <button type="submit" role="button" class="btn btn-success w-100 mt-3">Ok</button>
    </form>
</div>

<script>
    function porce(item, totalprice, callback) {
        var window = "",
            viewModel = new kendo.observable({
                item: item,
                totalprice: totalprice,
                plus: function (e) {
                    var val = this.item.get("number");
                    val += 1;
                    this.item.set("number", val);
                    var price = this.get("totalprice");
                    price += this.item.get("price");
                    this.set("totalprice", price);
                },
                minus: function (e) {
                    var val = this.item.get("number");
                    if (val > 0) {
                        val -= 1;
                        this.item.set("number", val);
                        var price = this.get("totalprice");
                        price -= this.item.get("price");
                        this.set("totalprice", price);
                    }
                },
                ok: function (e) {
                    e.preventDefault();
                    window.close();
                    return callback(this.get("totalprice"));
                }
            });
        kendo.bind($("#form-porce"), viewModel);
        window = $("#form-porce").data("kendoWindow");
        window.open().center();
    }

    (function (global) {
        "use strict";

        var app = global.app = global.app || {};

        app.kMobile = new kendo.mobile.Application(document.body, {
            skin: "nova",
            hideAddressBar: true,
            transition: "slide",
        });

        app.produkty = kendo.observable({
            oddo: "",
            po: null,
            ut: null,
            st: null,
            ct: null,
            pa: null,
            totalprice: 0,
            mnozstvi: function (e) {
                var that = this;
                var totalprice = this.get("totalprice");
                porce(e.data, totalprice, function (totalprice) {
                    that.set("totalprice", totalprice);
                    if (e.data.number > 0) {
                        $(e.currentTarget).closest("li").addClass("selected");
                    } else {
                        $(e.currentTarget).closest("li").removeClass("selected");
                    }
                });
            },
            selected: function () {
                var data = [];
                var po = this.po.get("data").toJSON();
                $.each(po, function (a, b) {
                    if (b.number > 0) {
                        b.price = b.number * b.price;
                        data.push(b)
                    }
                });
                var ut = this.ut.get("data").toJSON();
                $.each(ut, function (a, b) {
                    if (b.number > 0) {
                        b.price = b.number * b.price;
                        data.push(b)
                    }
                });
                var st = this.st.get("data").toJSON();
                $.each(st, function (a, b) {
                    if (b.number > 0) {
                        b.price = b.number * b.price;
                        data.push(b)
                    }
                });
                var ct = this.ct.get("data").toJSON();
                $.each(ct, function (a, b) {
                    if (b.number > 0) {
                        b.price = b.number * b.price;
                        data.push(b)
                    }
                });
                var pa = this.pa.get("data").toJSON();
                $.each(pa, function (a, b) {
                    if (b.number > 0) {
                        b.price = b.number * b.price;
                        data.push(b)
                    }
                });
                return data;
            },
            objednat: function (e) {
                var data = this.selected();
                if (data.length > 0) {
                    localStorage["data"] = JSON.stringify(data);
                    app.kMobile.navigate("#objednavka");
                } else {
                    $("<div></div>").kendoAlert({
                        width: "90%",
                        title: "Upozornění",
                        content: "<div class='text-center'>Nejdříve vyberte jídlo. Klikněte na položku v nabídce a zadejte počet porcí 😉.</div>"
                    }).data("kendoAlert").open();
                }
            }
        });

        app.objednavka = kendo.observable({
            data: [],
            form: {
                name: "",
                email: "",
                tel: "",
                addr: "",
                msg: ""
            },
            call: function (e) {
                window.location = 'tel:773034033';
            },
            show: function (e) {
                var form = localStorage["form"];
                if (form) {
                    form = JSON.parse(form);
                } else {
                    form = {
                        name: "",
                        email: "",
                        tel: "",
                        addr: "",
                        msg: ""
                    };
                }
                app.objednavka.set("form", form);
                var data = localStorage["data"];
                if (data) {
                    app.objednavka.set("data", JSON.parse(data));
                } else {
                    app.kMobile.navigate("#produkty");
                }
            },
            objednat: function (e) {
                e.preventDefault();
                window.close();
                app.kMobile.navigate("#:back");
                var postdata = this.get("data").toJSON();
                var form = this.get("form").toJSON();
                form = {
                    name: form.name,
                    email: form.email,
                    tel: form.tel,
                    addr: form.addr
                };
                localStorage["form"] = JSON.stringify(form);
                $.post("@Url.Action("Objednat", "Api/Service")", { '': postdata, form: form }, function (result) {
                    if (result.error) {
                        $("<div></div>").kendoAlert({
                            width: "90%",
                            title: "Děkujeme",
                            content: "<div class='text-center'>Omlouváme se ale něco se nepovedlo... Zkuste opakovat objednavku nebo nám zavolejte na tel: <a href='tel:773034033'></a>773 034 033</div>"
                        }).data("kendoAlert").open();
                    } else {
                        initMenu();
                        $("<div></div>").kendoAlert({
                            width: "90%",
                            title: "Děkujeme",
                            content: "<div class='text-center'>Vaši objednávku jsme přijali. Podrobnosti o objenávce jsme Vám poslali na Vámi zvolený email.</div>"
                        }).data("kendoAlert").open();
                    }
                });
            }
        });

    })(window);

    function initMenu() {
        kendo.ui.progress($("body"), true);
        $.get("@Url.Action("GetMenu", "Api/Service")", {}, function (result) {
            app.produkty.set("oddo", result.data.oddo);
            app.produkty.set("po", result.data.po);
            app.produkty.set("ut", result.data.ut);
            app.produkty.set("st", result.data.st);
            app.produkty.set("ct", result.data.ct);

            result.data.pa.disabled = "disabled";
            $.each(result.data.pa.data, function (a, b) {
                b.disabled = "disabled";
            })

            app.produkty.set("pa", result.data.pa);
            app.produkty.set("totalprice", 0);
            kendo.ui.progress($("body"), false);
        });
    };
    initMenu();
</script>
