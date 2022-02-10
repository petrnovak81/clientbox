<style>
    html, body {
        margin: 0;
        padding: 0;
        height: 100%;
        background: #f9f7fa;
        overflow: hidden;
    }

    body {
        background-image: url('../../Images/background.jpg');
        background-position: center;
        background-repeat: no-repeat;
        background-size: cover;
    }

        body:after {
            filter: grayscale(100%);
        }

    #navbar-content {
        width: 100%;
        border-bottom: 1px solid gray;
        box-shadow: 0 4px 2px -2px gray;
        background-color: #fff;
    }

    #page, #navbar-container {
        width: 1255px;
        margin: auto;
        padding: 0;
        position: relative;
    }

    .scroll-body {
        width: 100%;
        height: calc(100% - 65px);
        overflow: auto;
    }

    .content {
        padding-bottom: 0;
        position: relative;
        z-index: 1;
    }

    .products, .products .product {
        display: -webkit-box;
        display: -ms-flexbox;
        display: flex;
    }

    .products {
        -ms-flex-wrap: wrap;
        flex-wrap: wrap;
    }

        .products .product {
            -ms-flex-direction: column;
            -webkit-box-orient: vertical;
            -webkit-box-direction: normal;
            flex-direction: column;
            -ms-flex-pack: justify;
            -webkit-box-pack: justify;
            /*justify-content: space-between;*/
        }

            .products .product table {
                margin-top: auto !important;
                margin-bottom: 1rem !important;
            }

    .product {
        /*height: 250px;*/
        width: 25%;
        clear: both;
        border-right: 1px solid #e8e8e8;
        border-bottom: 1px solid #e8e8e8;
    }

    .selected {
        background: rgba(1, 129, 0, 0.10);
    }

    .header-img {
        text-align: center;
        height: 50%;
    }

        .header-img img {
            width: 100%;
            height: 100%;
            object-fit: cover;
        }

    .product-price {
        color: #018100;
        font-weight: bold;
    }

    .product h5, .product p {
        margin: 0;
    }

    .logo-img {
        height: 40px;
    }

    .wnd-font-size-110 {
        font-size: 110%;
    }

    .col-l {
        float: left;
        width: 75%;
    }

    .col-r {
        float: right;
        width: 25%;
        position: -webkit-sticky;
        position: sticky;
        top: 0;
    }

    .logo-col-r {
        width: 200px;
    }

    .disabled {
        filter: grayscale(100%);
        opacity: 0.8;
    }

    .today {
        background: #dff1ff !important;
    }
    .blur {
        filter: blur(3px);
        -webkit-filter: blur(3px);
    }
</style>

<div id="navbar-content">
    <div id="navbar-container">
        <nav class="navbar navbar-expand-lg navbar-light bg-white">
            <a class="navbar-brand" href="#">
                <img class="logo-img" src="~/Images/kantynalogo.png" />
                <font class="wnd-font-size-110"><strong>KANTÝNA ČSAD</strong> KLADNO</font>
            </a>
            <ul class="navbar-nav mr-auto"></ul>
            <ul class="navbar-nav ml-auto">
                <li class="nav-item">
                    <a class="nav-link" href="tel:773034033">Mobil: +420 773 034 033</a>
                </li>
            </ul>
        </nav>
    </div>
</div>

<div class="scroll-body">
    <div id="page">
        <h3 class="p-3 mt-3 bg-white mb-0">
            <span class="text-danger">ROZVOZ PO KLADNĚ A OKOLÍ</span><br />
            Aktuální nabídka (<strong data-bind="text: oddo"></strong>)
        </h3>
        <div class="col-l">
            <h3 class="p-3 mt-3 bg-white mb-0" data-bind="css: { today: po.today, disabled: po.disabled }"><b data-bind="text: po.title"></b></h3>
            <div class="content bg-white" data-bind="css: { today: po.today, disabled: po.disabled }">
                <div class="products" data-bind="source: po.data" data-template="produkt-template"></div>
            </div>
            <h3 class="p-3 mt-3 bg-white mb-0" data-bind="css: { today: ut.today, disabled: ut.disabled }"><b data-bind="text: ut.title"></b></h3>
            <div class="content bg-white" data-bind="css: { today: ut.today, disabled: ut.disabled }">
                <div class="products" data-bind="source: ut.data" data-template="produkt-template"></div>
            </div>
            <h3 class="p-3 mt-3 bg-white mb-0" data-bind="css: { today: st.today, disabled: st.disabled }"><b data-bind="text: st.title"></b></h3>
            <div class="content bg-white" data-bind="css: { today: st.today, disabled: st.disabled }">
                <div class="products" data-bind="source: st.data" data-template="produkt-template"></div>
            </div>
            <h3 class="p-3 mt-3 bg-white mb-0" data-bind="css: { today: ct.today, disabled: ct.disabled }"><b data-bind="text: ct.title"></b></h3>
            <div class="content bg-white" data-bind="css: { today: ct.today, disabled: ct.disabled }">
                <div class="products" data-bind="source: ct.data" data-template="produkt-template"></div>
            </div>
            <h3 class="p-3 mt-3 bg-white mb-0" data-bind="css: { today: pa.today, disabled: pa.disabled }"><b data-bind="text: pa.title"></b></h3>
            <div class="content bg-white" data-bind="css: { today: pa.today, disabled: pa.disabled }">
                <div class="products" data-bind="source: pa.data" data-template="produkt-template"></div>
            </div>
            <div class="text-center mt-3 mb-3">
                <button class="btn btn-success text-uppercase" data-bind="events: { click: objednat }">Objednat rozvoz</button>
            </div>
        </div>
        <div class="col-r">
            <div class="mt-3 ml-3 bg-white p-3">
                <div class="text-center text-success">
                    Hlavní jídlo 90,-<br />
                    Polévka 20,-<br />
                    <strong>(v ceně obal i doprava)</strong><br />
                    <i class="fa fa-car fa-5x"></i>
                </div>
                <div class="text-center text-danger mt-3">
                    Objednávky pro rozvoz přijímáme nejpozději do <strong>10:00 hodin</strong>, ideálně však den předem
                    rozvážíme po <strong>Kladně a blízkém okolí</strong>.
                </div>
                <div class="text-center mt-3">
                    <h5 id="pricetotal">
                        Cena celkem: <span data-bind="text: totalprice"></span> Kč
                    </h5>
                    <button class="btn btn-success text-uppercase" data-bind="events: { click: objednat }">Objednat rozvoz</button>
                </div>
            </div>
            <div class="mt-3 ml-3 bg-white p-3">
                <div class="text-center">
                    <img class="logo-col-r" src="~/Images/kantynalogo.png" />
                    <p>
                        <strong>Železničářů 885, 272 01 Kladno</strong>
                    </p>
                </div>
                <p class="mt-5">
                    <strong>Provozovatel:</strong> <br>
                    Kateřina
                    Šámalová<br>
                    28. Října 738, Kladno 27309<br>
                    IČO: 02287838
                </p>
                <p class="mt-5">
                    <strong>Zodpovědná osoba:</strong> <br>
                    Kateřina
                    Šámalová<br>
                    <i class="fa fa-phone-square"></i> <a href="tel:773034033">+420 773 034 033</a><br>
                    <i class="fa fa-envelope"></i> <a href="mailto:kantynacsadkladno@email.cz" target="_blank">kantynacsadkladno@email.cz</a>
                </p>
                <iframe class="mt-5" src="https://web-1071.webnode.com/widgets/googlemaps/?z=15&amp;a=%C5%BDelezni%C4%8D%C3%A1%C5%99%C5%AF+885%2C+Kladno&amp;s=LcwxCsAgDEbhu2QuQlfP0U0cHH6LkGpJ0kHEu9dC5-_xwqCMZI_g6DfIk0mqWsxxqaCNwLhQ7ccT7YJJd7kwL1XrDFHyYZB-l2Sl1RXuNOOMLw.." style="width:100%;height:250px;border:0;"></iframe>
            </div>
        </div>
    </div>
</div>

<script id="produkt-template" type="text/html">
    <div class="product" data-bind="css: { selected: selected }">
        <div class="header-img mr-3 ml-3 border rounded bg-white">
            <img src="${img}" onError="this.onerror=null;this.src='/Images/placeholder.png';" class="${(blur ? 'blur' : '')}" />
        </div>
        <div class="ml-3 mr-3">
            <small><i class="text-muted">Ilustrační foto</i></small>
            <h5><b>#=title#</b></h5>
            <p>#=subtitle#</p>
        </div>
        <table class="m-3">
            <tr>
                <td class="product-price w-50">Cena: #=price# Kč</td>
                <td>
                    <div class="input-group input-group-sm">
                        <div class="input-group-prepend">
                            <button class="btn btn-outline-warning" type="button" data-bind="events: { click: minus }" #=disabled#><i class="fa fa-minus"></i></button>
                        </div>
                        <input type="text" readonly class="form-control text-center" data-bind="value: number" #=disabled#>
                        <div class="input-group-append">
                            <button class="btn btn-outline-success" type="button" data-bind="events: { click: plus }" #=disabled#><i class="fa fa-plus"></i></button>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</script>

<div id="form-objednavka"
     data-role="window"
     data-width="800"
     data-height="auto"
     data-max-height="90%"
     data-modal="true"
     data-visible="false"
     data-title="Objednávka"
     data-actions="[]">
    <div data-role="grid"
         data-selectable="false"
         data-resizable="false"
         data-scrollable="false"
         data-toolbar="[{ template: '<h4>Položky objednávky</h4>' }]"
         data-columns="[
         { 'field': 'day', 'title': 'Kdy', width: 120 },
         { 'field': 'subtitle', 'title': ' ' },
         { 'field': 'number', format: '{0:n0}x', attributes: { class: 'text-right' }, 'title': 'Počet', width: 100, footerTemplate: 'Celkem:' },
         { 'field': 'price', format: '{0:n2} Kč', attributes: { class: 'text-right' }, 'title': 'Cena', width: 100, footerTemplate: '#=kendo.toString(data.price ? data.price.sum : 0, \'n2\')# Kč' }]"
         data-bind="source: source"></div>
    <h4 class="mt-3">Kam a komu to máme dovézt</h4>
    <form method="post" data-bind="events: { submit: dovezt }">
        <div class="row mt-2">
            <div class="col-md-4">
                <label for="name">Jméno a příjmení <span class="text-danger">*</span></label>
            </div>
            <div class="col-md-8">
                <input class="form-control mb-1" id="name" name="name" required type="text" data-bind="value: form.name" maxlength="150" placeholder="Zadejte Váše jméno a příjmení">
            </div>
            <div class="col-md-4">
                <label for="mail">Email <span class="text-danger">*</span></label>
            </div>
            <div class="col-md-8">
                <input class="form-control mb-1" id="mail" name="mail" required type="email" data-bind="value: form.email" maxlength="150" placeholder="Zadejte Váš e-mail">
            </div>
            <div class="col-md-4">
                <label for="tel">Telefon <span class="text-danger">*</span></label><span class="float-right">+420</span>
            </div>
            <div class="col-md-8">
                <input class="form-control mb-1" id="tel" name="tel" required type="tel" maxlength="9" data-bind="value: form.tel" placeholder="Zadejte Váše telefonní číslo (606111222)">
            </div>
            <div class="col-md-4">
                <label for="addr">Adresa <span class="text-danger">*</span></label>
            </div>
            <div class="col-md-8">
                <input class="form-control mb-1" list="adresy" id="addr" name="addr" required type="text" maxlength="150" data-bind="value: form.addr" placeholder="Zadejte adresu vyzvednutí">
                <datalist id="adresy" data-bind="source: adresy" data-template="adresa-template"></datalist>
            </div>
            <div class="col-md-4">
                <label for="msg">Doplňující informace k objednávce</label>
            </div>
            <div class="col-md-8">
                <textarea class="form-control" rows="5" id="msg" name="msg" data-bind="value: form.msg"></textarea>
            </div>
        </div>
        <div class="window-footer text-right mt-4">
            <button type="submit" role="button" class="btn btn-success ml-3">Objednat</button>
            <button type="button" role="button" class="btn btn-light" data-bind="events: { click: close }">Zavřít</button>
        </div>
    </form>
</div>

<script id="adresa-template" type="text/x-kendo-template">
    <option value="${Adresa}">
</script>

<script>
    function formObjednavka(price, data) {
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
        var window = "",
            viewModel = new kendo.observable({
                adresy: new kendo.data.DataSource({
                    transport: {
                        read: {
                            url: "@Url.Action("Adresy", "Api/Service")"
                        }
                    }
                }),
                source: new kendo.data.DataSource({
                    type: "json",
                    data: data || [],
                    aggregate: [
                        { field: "price", aggregate: "sum" }
                    ]
                }),
                form: form,
                dataSource: new kendo.data.DataSource({
                    data: data || []
                }),
                close: function (e) {
                    window.close();
                },
                dovezt: function (e) {
                    e.preventDefault();
                    window.close();
                    var postdata = data;
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
                                width: 600,
                                title: "Děkujeme",
                                content: "<div class='text-center'>Omlouváme se ale něco se nepovedlo... Zkuste opakovat objednavku nebo nám zavolejte na tel: <a href='tel:773034033'></a>773 034 033</div>"
                            }).data("kendoAlert").open();
                        } else {
                            initMenu();
                            $("<div></div>").kendoAlert({
                                width: 600,
                                title: "Děkujeme",
                                content: "<div class='text-center'>Vaši objednávku jsme přijali. Podrobnosti o objenávce jsme Vám poslali na Vámi zvolený email.</div>"
                            }).data("kendoAlert").open();
                        }
                    });
                }
            });
        kendo.bind($("#form-objednavka"), viewModel);
        window = $("#form-objednavka").data("kendoWindow");
        window.open().center();
    }

    (function (global) {
        "use strict";

        var app = global.app = global.app || {};

        app.viewModel = kendo.observable({
            oddo: "",
            po: null,
            ut: null,
            st: null,
            ct: null,
            pa: null,
            totalprice: 0,
            plus: function (e) {
                var val = e.data.get("number");
                val += 1
                e.data.set("selected", true);
                e.data.set("number", val);
                var price = this.get("totalprice");
                price += e.data.get("price");
                this.set("totalprice", price);
                $("#pricetotal").animate({ zoom: '120%' }, 100, function () {
                    $("#pricetotal").animate({ zoom: '100%' })
                });
            },
            minus: function (e) {
                var val = e.data.get("number");
                val -= 1
                if (val > -1) {
                    e.data.set("number", val);
                    var price = this.get("totalprice");
                    price -= e.data.get("price");
                    this.set("totalprice", price);
                }
                if (val < 1) {
                    e.data.set("selected", false);
                }
                $("#pricetotal").animate({ zoom: '120%' }, 250, function () {
                    $("#pricetotal").animate({ zoom: '100%' })
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
                    formObjednavka(this.get("totalprice"), data);
                } else {
                    $("<div></div>").kendoAlert({
                        width: 600,
                        title: "Upozornění",
                        content: "<div class='text-center'>Nejdříve vyberte jídlo pomocí tlačítek <i class='fa fa-minus text-warning'></i>/<i class='fa fa-plus text-success'></i> u jednotlivých položek</div>"
                    }).data("kendoAlert").open();
                }
            }
        });

        kendo.bind($(document.body), app.viewModel);
    })(window);

    function initMenu() {
        kendo.ui.progress($("body"), true);
            $.get("@Url.Action("GetMenu", "Api/Service")", {}, function (result) {
                app.viewModel.set("oddo", result.data.oddo);
                app.viewModel.set("po", result.data.po);
                app.viewModel.set("ut", result.data.ut);
                app.viewModel.set("st", result.data.st);
                app.viewModel.set("ct", result.data.ct);

                result.data.pa.disabled = "disabled";
                $.each(result.data.pa.data, function (a, b) {
                    b.disabled = "disabled";
                })

                app.viewModel.set("pa", result.data.pa);
                app.viewModel.set("totalprice", 0);
                kendo.ui.progress($("body"), false);
            });
        }
    initMenu();

    //var midnight = "10:00:00";
    //setInterval(function () {
    //    var now = kendo.toString(new Date(), "HH:mm:ss");
    //    if (now == midnight) {
    //        initMenu();
    //    }
    //}, 1000);
</script>
