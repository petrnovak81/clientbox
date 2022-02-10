(function (global) {

 'use strict';

var agilo = global.agilo = global.agilo || {};

//<StartSchema>
    //<Start[AGsp_AddNewPracakyPolozkaProduktZablokovat]>
    agilo.AGsp_AddNewPracakyPolozkaProduktZablokovat_Columns = [
        { field: 'ErrorNumber', format: '{0:n1}' },
        { field: 'ErrorMessage' }
    ];

    agilo.AGsp_AddNewPracakyPolozkaProduktZablokovat = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "ErrorNumber",
                fields: {
                    ErrorNumber: { type: 'number' },
                    ErrorMessage: { type: 'string' }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_AddNewPracakyPolozkaProduktZablokovat?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_AddNewPracakyPolozkaProduktZablokovat?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_AddNewPracakyPolozkaProduktZablokovat?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_AddNewPracakyPolozkaProduktZablokovat?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_AddNewPracakyPolozkaProduktZablokovat]>

    //<Start[AGsp_AddOrEditVykazPracePol]>
    agilo.AGsp_AddOrEditVykazPracePol_Columns = [
        { field: 'ErrorNumber', format: '{0:n1}' },
        { field: 'ErrorMessage' }
    ];

    agilo.AGsp_AddOrEditVykazPracePol = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "ErrorNumber",
                fields: {
                    ErrorNumber: { type: 'number' },
                    ErrorMessage: { type: 'string' }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_AddOrEditVykazPracePol?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_AddOrEditVykazPracePol?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_AddOrEditVykazPracePol?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_AddOrEditVykazPracePol?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_AddOrEditVykazPracePol]>

    //<Start[AGsp_FA_PracakyDetailHL]>
    agilo.AGsp_FA_PracakyDetailHL_Columns = [
        { field: 'IDVykazPrace', format: '{0:n1}' },
        { field: 'DatVzniku', format: '{0:d}' },
        { field: 'Firma' },
        { field: 'FirmaAdresa' },
        { field: 'Poznamka' },
        { field: 'rr_StavPracaku', format: '{0:n1}' },
        { field: 'rr_StavPracakuHodnota' },
        { field: 'HtmlZnacka' },
        { field: 'rr_TypServisniSmlouvy', format: '{0:n1}' },
        { field: 'rr_TypServisniSmlouvyHodnota' },
        { field: 'CisloFaktury' },
        { field: 'IDUserZalozil', format: '{0:n1}' },
        { field: 'UserZalozil' },
        { field: 'IDUserUpravil', format: '{0:n1}' },
        { field: 'UserUpravil' },
        { field: 'rr_FakturovatNaFirmu', format: '{0:n1}' },
        { field: 'rr_FakturovatNaFirmuHodnota' }
    ];

    agilo.AGsp_FA_PracakyDetailHL = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "IDVykazPrace",
                fields: {
                    IDVykazPrace: { type: 'number', validation: { required: true } },
                    DatVzniku: { type: 'date', validation: { required: true } },
                    Firma: { type: 'string', validation: { required: true } },
                    FirmaAdresa: { type: 'string' },
                    Poznamka: { type: 'string' },
                    rr_StavPracaku: { type: 'number', validation: { required: true } },
                    rr_StavPracakuHodnota: { type: 'string', validation: { required: true } },
                    HtmlZnacka: { type: 'string' },
                    rr_TypServisniSmlouvy: { type: 'number', validation: { required: true } },
                    rr_TypServisniSmlouvyHodnota: { type: 'string' },
                    CisloFaktury: { type: 'string' },
                    IDUserZalozil: { type: 'number' },
                    UserZalozil: { type: 'string' },
                    IDUserUpravil: { type: 'number' },
                    UserUpravil: { type: 'string' },
                    rr_FakturovatNaFirmu: { type: 'number', validation: { required: true } },
                    rr_FakturovatNaFirmuHodnota: { type: 'string', validation: { required: true } }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_FA_PracakyDetailHL?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_FA_PracakyDetailHL?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_FA_PracakyDetailHL?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_FA_PracakyDetailHL?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_FA_PracakyDetailHL]>

    //<Start[AGsp_FA_PracakyDetailPol]>
    agilo.AGsp_FA_PracakyDetailPol_Columns = [
        { field: 'IDVykazPracePol', format: '{0:n1}' },
        { field: 'IDVykazPrace', format: '{0:n1}' },
        { field: 'rr_TypPolozkyPracaku', format: '{0:n1}' },
        { field: 'rr_TypPolozkyPracakuHodnota' },
        { field: 'Produkt' },
        { field: 'TextNaFakturu' },
        { field: 'TextInterniDoMailu' },
        { field: 'PocetEMJ', format: '{0:n1}' },
        { field: 'CenaEMJ', format: '{0:n1}' },
        { field: 'CelkemBezDPH', format: '{0:n1}' },
        { field: 'Vzdal' },
        { field: 'Zdrm' },
        { field: 'IDTechnika', format: '{0:n1}' },
        { field: 'IDUserUpravil', format: '{0:n1}' },
        { field: 'Technik' },
        { field: 'Upravil' }
    ];

    agilo.AGsp_FA_PracakyDetailPol = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "IDVykazPracePol",
                fields: {
                    IDVykazPracePol: { type: 'number', validation: { required: true } },
                    IDVykazPrace: { type: 'number', validation: { required: true } },
                    rr_TypPolozkyPracaku: { type: 'number' },
                    rr_TypPolozkyPracakuHodnota: { type: 'string', validation: { required: true } },
                    Produkt: { type: 'string' },
                    TextNaFakturu: { type: 'string' },
                    TextInterniDoMailu: { type: 'string' },
                    PocetEMJ: { type: 'number', validation: { required: true } },
                    CenaEMJ: { type: 'number', validation: { required: true } },
                    CelkemBezDPH: { type: 'number' },
                    Vzdal: { type: 'string', validation: { required: true } },
                    Zdrm: { type: 'string', validation: { required: true } },
                    IDTechnika: { type: 'number', validation: { required: true } },
                    IDUserUpravil: { type: 'number' },
                    Technik: { type: 'string' },
                    Upravil: { type: 'string' }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_FA_PracakyDetailPol?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_FA_PracakyDetailPol?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_FA_PracakyDetailPol?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_FA_PracakyDetailPol?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_FA_PracakyDetailPol]>

    //<Start[AGsp_GetDialogObjednavkaTlacitka]>
    agilo.AGsp_GetDialogObjednavkaTlacitka_Columns = [
        { field: 'rr_StavObjednavky', format: '{0:n1}' },
        { field: 'rr_StavObjednavkyHodnota' }
    ];

    agilo.AGsp_GetDialogObjednavkaTlacitka = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "rr_StavObjednavky",
                fields: {
                    rr_StavObjednavky: { type: 'number', validation: { required: true } },
                    rr_StavObjednavkyHodnota: { type: 'string' }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetDialogObjednavkaTlacitka?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetDialogObjednavkaTlacitka?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetDialogObjednavkaTlacitka?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetDialogObjednavkaTlacitka?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetDialogObjednavkaTlacitka]>

    //<Start[AGsp_GetFirmaAll]>
    agilo.AGsp_GetFirmaAll_Columns = [
        { field: 'Typ_zaznamu' },
        { field: 'Firma' },
        { field: 'Nazev_firmy' },
        { field: 'Ulice' },
        { field: 'PSC' },
        { field: 'Mesto' },
        { field: 'Telefon_1' },
        { field: 'Telefon_2' },
        { field: 'E_mail' },
        { field: 'ICO' },
        { field: 'DIC' },
        { field: 'Vzdalenost', format: '{0:n1}' },
        { field: 'Kategorie' },
        { field: 'Obor_cinnosti' },
        { field: 'Poznamky' },
        { field: 'Datum_aktualizace', format: '{0:d}' },
        { field: 'Titul' },
        { field: 'Krestni' },
        { field: 'Prijmeni' },
        { field: 'Funkce' },
        { field: 'Zasilat' }
    ];

    agilo.AGsp_GetFirmaAll = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "Typ_zaznamu",
                fields: {
                    Typ_zaznamu: { type: 'string' },
                    Firma: { type: 'string', validation: { required: true } },
                    Nazev_firmy: { type: 'string' },
                    Ulice: { type: 'string' },
                    PSC: { type: 'string' },
                    Mesto: { type: 'string' },
                    Telefon_1: { type: 'string' },
                    Telefon_2: { type: 'string' },
                    E_mail: { type: 'string' },
                    ICO: { type: 'string' },
                    DIC: { type: 'string' },
                    Vzdalenost: { type: 'number' },
                    Kategorie: { type: 'string' },
                    Obor_cinnosti: { type: 'string' },
                    Poznamky: { type: 'string' },
                    Datum_aktualizace: { type: 'date' },
                    Titul: { type: 'string' },
                    Krestni: { type: 'string' },
                    Prijmeni: { type: 'string' },
                    Funkce: { type: 'string' },
                    Zasilat: { type: 'string' }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetFirmaAll?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaAll?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaAll?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaAll?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetFirmaAll]>

    //<Start[AGsp_GetFirmaDetail]>
    agilo.AGsp_GetFirmaDetail_Columns = [
        { field: 'Firma' },
        { field: 'Nazev_firmy' },
        { field: 'Adresa' },
        { field: 'ICO' },
        { field: 'DIC' },
        { field: 'Datum_aktualizace', format: '{0:d}' },
        { field: 'Titul' },
        { field: 'Krestni' },
        { field: 'Prijmeni' },
        { field: 'Funkce' },
        { field: 'Obor_cinnosti' },
        { field: 'Poznamky' },
        { field: 'Kategorie' },
        { field: 'Zasilat' }
    ];

    agilo.AGsp_GetFirmaDetail = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "Firma",
                fields: {
                    Firma: { type: 'string', validation: { required: true } },
                    Nazev_firmy: { type: 'string' },
                    Adresa: { type: 'string' },
                    ICO: { type: 'string' },
                    DIC: { type: 'string' },
                    Datum_aktualizace: { type: 'date' },
                    Titul: { type: 'string' },
                    Krestni: { type: 'string' },
                    Prijmeni: { type: 'string' },
                    Funkce: { type: 'string' },
                    Obor_cinnosti: { type: 'string' },
                    Poznamky: { type: 'string' },
                    Kategorie: { type: 'string' },
                    Zasilat: { type: 'string' }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetFirmaDetail?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaDetail?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaDetail?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaDetail?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetFirmaDetail]>

    //<Start[AGsp_GetFirmaInventarDetail]>
    agilo.AGsp_GetFirmaInventarDetail_Columns = [
        { field: 'IDInventare', format: '{0:n1}' },
        { field: 'InventarPopis' },
        { field: 'InventarProdukt' },
        { field: 'DatumNaposledyZakoupeno', format: '{0:d}' },
        { field: 'rr_TypInventare', format: '{0:n1}' },
        { field: 'DatumExpirace', format: '{0:d}' }
    ];

    agilo.AGsp_GetFirmaInventarDetail = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "IDInventare",
                fields: {
                    IDInventare: { type: 'number', validation: { required: true } },
                    InventarPopis: { type: 'string' },
                    InventarProdukt: { type: 'string' },
                    DatumNaposledyZakoupeno: { type: 'date' },
                    rr_TypInventare: { type: 'number', validation: { required: true } },
                    DatumExpirace: { type: 'date' }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetFirmaInventarDetail?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaInventarDetail?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaInventarDetail?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaInventarDetail?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetFirmaInventarDetail]>

    //<Start[AGsp_GetFirmaInventarFoto]>
    agilo.AGsp_GetFirmaInventarFoto_Columns = [
        { field: 'IDFoto', format: '{0:n1}' },
        { field: 'FilePath' }
    ];

    agilo.AGsp_GetFirmaInventarFoto = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "IDFoto",
                fields: {
                    IDFoto: { type: 'number', validation: { required: true } },
                    FilePath: { type: 'string' }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetFirmaInventarFoto?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaInventarFoto?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaInventarFoto?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaInventarFoto?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetFirmaInventarFoto]>

    //<Start[AGsp_GetFirmaInventarSeznam]>
    agilo.AGsp_GetFirmaInventarSeznam_Columns = [
        { field: 'IDInventare', format: '{0:n1}' },
        { field: 'Firma' },
        { field: 'InventarPopis' },
        { field: 'InventarProdukt' },
        { field: 'DatumNaposledyZakoupeno', format: '{0:d}' },
        { field: 'Pracoviste' },
        { field: 'rr_TypInventare', format: '{0:n1}' },
        { field: 'DatumExpirace', format: '{0:d}' }
    ];

    agilo.AGsp_GetFirmaInventarSeznam = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "IDInventare",
                fields: {
                    IDInventare: { type: 'number', validation: { required: true } },
                    Firma: { type: 'string', validation: { required: true } },
                    InventarPopis: { type: 'string' },
                    InventarProdukt: { type: 'string' },
                    DatumNaposledyZakoupeno: { type: 'date' },
                    Pracoviste: { type: 'string' },
                    rr_TypInventare: { type: 'number', validation: { required: true } },
                    DatumExpirace: { type: 'date' }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetFirmaInventarSeznam?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaInventarSeznam?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaInventarSeznam?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaInventarSeznam?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetFirmaInventarSeznam]>

    //<Start[AGsp_GetFirmaKontaktyDetail]>
    agilo.AGsp_GetFirmaKontaktyDetail_Columns = [
        { field: 'Kontakt' },
        { field: 'Kontaktni_udaj', format: '{0:n1}' },
        { field: 'Typ_KU' },
        { field: 'Nazev_KU' },
        { field: 'Hodnota_KU' }
    ];

    agilo.AGsp_GetFirmaKontaktyDetail = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "Kontakt",
                fields: {
                    Kontakt: { type: 'string', validation: { required: true } },
                    Kontaktni_udaj: { type: 'number', validation: { required: true } },
                    Typ_KU: { type: 'string' },
                    Nazev_KU: { type: 'string' },
                    Hodnota_KU: { type: 'string' }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetFirmaKontaktyDetail?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaKontaktyDetail?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaKontaktyDetail?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaKontaktyDetail?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetFirmaKontaktyDetail]>

    //<Start[AGsp_GetFirmaKontaktySeznam]>
    agilo.AGsp_GetFirmaKontaktySeznam_Columns = [
        { field: 'Kontakt' },
        { field: 'Kontaktni_udaj', format: '{0:n1}' },
        { field: 'Typ_KU' },
        { field: 'Nazev_KU' },
        { field: 'Hodnota_KU' }
    ];

    agilo.AGsp_GetFirmaKontaktySeznam = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "Kontakt",
                fields: {
                    Kontakt: { type: 'string', validation: { required: true } },
                    Kontaktni_udaj: { type: 'number', validation: { required: true } },
                    Typ_KU: { type: 'string' },
                    Nazev_KU: { type: 'string' },
                    Hodnota_KU: { type: 'string' }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetFirmaKontaktySeznam?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaKontaktySeznam?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaKontaktySeznam?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaKontaktySeznam?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetFirmaKontaktySeznam]>

    //<Start[AGsp_GetFirmaObjednavkySeznam]>
    agilo.AGsp_GetFirmaObjednavkySeznam_Columns = [
        { field: 'IDObjednavkyPol', format: '{0:n1}' },
        { field: 'FirmaObjednal' },
        { field: 'Produkt' },
        { field: 'Poznamka' },
        { field: 'rr_DeadLine', format: '{0:n1}' },
        { field: 'rr_DeadLineHodnota' },
        { field: 'DeadLineDatum', format: '{0:d}' },
        { field: 'Barva', format: '{0:n1}' }
    ];

    agilo.AGsp_GetFirmaObjednavkySeznam = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "IDObjednavkyPol",
                fields: {
                    IDObjednavkyPol: { type: 'number', validation: { required: true } },
                    FirmaObjednal: { type: 'string' },
                    Produkt: { type: 'string' },
                    Poznamka: { type: 'string' },
                    rr_DeadLine: { type: 'number', validation: { required: true } },
                    rr_DeadLineHodnota: { type: 'string', validation: { required: true } },
                    DeadLineDatum: { type: 'date', validation: { required: true } },
                    Barva: { type: 'number', validation: { required: true } }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetFirmaObjednavkySeznam?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaObjednavkySeznam?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaObjednavkySeznam?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaObjednavkySeznam?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetFirmaObjednavkySeznam]>

    //<Start[AGsp_GetFirmaObjednavkySeznamDlePracoviste]>
    agilo.AGsp_GetFirmaObjednavkySeznamDlePracoviste_Columns = [
        { field: 'IDObjednavkyPol', format: '{0:n1}' },
        { field: 'FirmaObjednal' },
        { field: 'Produkt' },
        { field: 'Poznamka' },
        { field: 'rr_DeadLine', format: '{0:n1}' },
        { field: 'rr_DeadLineHodnota' },
        { field: 'DeadLineDatum', format: '{0:d}' },
        { field: 'Barva', format: '{0:n1}' }
    ];

    agilo.AGsp_GetFirmaObjednavkySeznamDlePracoviste = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "IDObjednavkyPol",
                fields: {
                    IDObjednavkyPol: { type: 'number', validation: { required: true } },
                    FirmaObjednal: { type: 'string' },
                    Produkt: { type: 'string' },
                    Poznamka: { type: 'string' },
                    rr_DeadLine: { type: 'number', validation: { required: true } },
                    rr_DeadLineHodnota: { type: 'string', validation: { required: true } },
                    DeadLineDatum: { type: 'date', validation: { required: true } },
                    Barva: { type: 'number', validation: { required: true } }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetFirmaObjednavkySeznamDlePracoviste?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaObjednavkySeznamDlePracoviste?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaObjednavkySeznamDlePracoviste?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaObjednavkySeznamDlePracoviste?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetFirmaObjednavkySeznamDlePracoviste]>

    //<Start[AGsp_GetFirmaParametry]>
    agilo.AGsp_GetFirmaParametry_Columns = [
        { field: 'Firma' },
        { field: 'PlatnostOd', format: '{0:d}' },
        { field: 'HodinovaSazba', format: '{0:n1}' },
        { field: 'SazbaKm', format: '{0:n1}' },
        { field: 'rr_TypServisniSmlouvy', format: '{0:n1}' },
        { field: 'ServiskaCenaMesicne', format: '{0:n1}' },
        { field: 'ServiskaNaposledyVyuctovana', format: '{0:d}' },
        { field: 'ServiskaIntervalObnoveni', format: '{0:n1}' },
        { field: 'ServiskaVolneHodiny', format: '{0:n1}' },
        { field: 'UserLastName' },
        { field: 'SazbaMalyZasah', format: '{0:n1}' },
        { field: 'SazbaVelkyZasah', format: '{0:n1}' },
        { field: 'IDFirmaParametr', format: '{0:n1}' }
    ];

    agilo.AGsp_GetFirmaParametry = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "Firma",
                fields: {
                    Firma: { type: 'string', validation: { required: true } },
                    PlatnostOd: { type: 'date', validation: { required: true } },
                    HodinovaSazba: { type: 'number' },
                    SazbaKm: { type: 'number' },
                    rr_TypServisniSmlouvy: { type: 'number' },
                    ServiskaCenaMesicne: { type: 'number' },
                    ServiskaNaposledyVyuctovana: { type: 'date' },
                    ServiskaIntervalObnoveni: { type: 'number' },
                    ServiskaVolneHodiny: { type: 'number' },
                    UserLastName: { type: 'string' },
                    SazbaMalyZasah: { type: 'number' },
                    SazbaVelkyZasah: { type: 'number' },
                    IDFirmaParametr: { type: 'number', validation: { required: true } }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetFirmaParametry?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaParametry?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaParametry?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaParametry?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetFirmaParametry]>

    //<Start[AGsp_GetFirmaPracovisteDetail]>
    agilo.AGsp_GetFirmaPracovisteDetail_Columns = [
        { field: 'Firma' },
        { field: 'rr_TypAdresy' },
        { field: 'Nazev_firmy' },
        { field: 'Adresa' },
        { field: 'ICO' },
        { field: 'DIC' },
        { field: 'Vzdalenost', format: '{0:n1}' },
        { field: 'Obor_cinnosti' },
        { field: 'Poznamky' },
        { field: 'Datum_aktualizace', format: '{0:d}' },
        { field: 'Titul' },
        { field: 'Krestni' },
        { field: 'Prijmeni' },
        { field: 'Funkce' },
        { field: 'Detail1' },
        { field: 'Detail2' },
        { field: 'Detail3' },
        { field: 'Detail4' },
        { field: 'GPSLat' },
        { field: 'GPSLng' },
        { field: 'GPSValid' }
    ];

    agilo.AGsp_GetFirmaPracovisteDetail = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "Firma",
                fields: {
                    Firma: { type: 'string', validation: { required: true } },
                    rr_TypAdresy: { type: 'string' },
                    Nazev_firmy: { type: 'string' },
                    Adresa: { type: 'string' },
                    ICO: { type: 'string' },
                    DIC: { type: 'string' },
                    Vzdalenost: { type: 'number' },
                    Obor_cinnosti: { type: 'string' },
                    Poznamky: { type: 'string' },
                    Datum_aktualizace: { type: 'date' },
                    Titul: { type: 'string' },
                    Krestni: { type: 'string' },
                    Prijmeni: { type: 'string' },
                    Funkce: { type: 'string' },
                    Detail1: { type: 'string' },
                    Detail2: { type: 'string' },
                    Detail3: { type: 'string' },
                    Detail4: { type: 'string' },
                    GPSLat: { type: 'string' },
                    GPSLng: { type: 'string' },
                    GPSValid: { type: 'boolean' }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetFirmaPracovisteDetail?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaPracovisteDetail?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaPracovisteDetail?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaPracovisteDetail?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetFirmaPracovisteDetail]>

    //<Start[AGsp_GetFirmaPracovisteDetailKontakty]>
    agilo.AGsp_GetFirmaPracovisteDetailKontakty_Columns = [
        { field: 'Kontaktni_udaj', format: '{0:n1}' },
        { field: 'Typ_KU' },
        { field: 'Nazev_KU' },
        { field: 'Hodnota_KU' }
    ];

    agilo.AGsp_GetFirmaPracovisteDetailKontakty = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "Kontaktni_udaj",
                fields: {
                    Kontaktni_udaj: { type: 'number', validation: { required: true } },
                    Typ_KU: { type: 'string' },
                    Nazev_KU: { type: 'string' },
                    Hodnota_KU: { type: 'string' }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetFirmaPracovisteDetailKontakty?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaPracovisteDetailKontakty?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaPracovisteDetailKontakty?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaPracovisteDetailKontakty?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetFirmaPracovisteDetailKontakty]>

    //<Start[AGsp_GetFirmaPracovisteSeznam]>
    agilo.AGsp_GetFirmaPracovisteSeznam_Columns = [
        { field: 'Firma' },
        { field: 'rr_TypAdresy' },
        { field: 'Nazev_firmy' },
        { field: 'AdresaPracoviste' },
        { field: 'Prijmeni' }
    ];

    agilo.AGsp_GetFirmaPracovisteSeznam = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "Firma",
                fields: {
                    Firma: { type: 'string', validation: { required: true } },
                    rr_TypAdresy: { type: 'string' },
                    Nazev_firmy: { type: 'string' },
                    AdresaPracoviste: { type: 'string' },
                    Prijmeni: { type: 'string' }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetFirmaPracovisteSeznam?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaPracovisteSeznam?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaPracovisteSeznam?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaPracovisteSeznam?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetFirmaPracovisteSeznam]>

    //<Start[AGsp_GetFirmaPracovisteSeznamHledej]>
    agilo.AGsp_GetFirmaPracovisteSeznamHledej_Columns = [
        { field: 'Pobocka' },
        { field: 'NazevPobocky' },
        { field: 'Firma' },
        { field: 'Nazev_firmy' },
        { field: 'rr_TypAdresy' },
        { field: 'AdresaPracoviste' },
        { field: 'GPSValid' },
        { field: 'ParametryValidni', format: '{0:n1}' }
    ];

    agilo.AGsp_GetFirmaPracovisteSeznamHledej = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "Pobocka",
                fields: {
                    Pobocka: { type: 'string', validation: { required: true } },
                    NazevPobocky: { type: 'string' },
                    Firma: { type: 'string', validation: { required: true } },
                    Nazev_firmy: { type: 'string' },
                    rr_TypAdresy: { type: 'string' },
                    AdresaPracoviste: { type: 'string' },
                    GPSValid: { type: 'boolean' },
                    ParametryValidni: { type: 'number' }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetFirmaPracovisteSeznamHledej?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaPracovisteSeznamHledej?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaPracovisteSeznamHledej?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmaPracovisteSeznamHledej?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetFirmaPracovisteSeznamHledej]>

    //<Start[AGsp_GetFirmyObjednavkaDetail]>
    agilo.AGsp_GetFirmyObjednavkaDetail_Columns = [
        { field: 'IDObjednavkyPol', format: '{0:n1}' },
        { field: 'FirmaObjednal' },
        { field: 'Produkt' },
        { field: 'Poznamka' },
        { field: 'ObjednanoEMJ', format: '{0:n1}' },
        { field: 'DomluvenaProdejniCena', format: '{0:n1}' },
        { field: 'rr_DeadLine', format: '{0:n1}' },
        { field: 'DeadLineDatum', format: '{0:d}' },
        { field: 'IDUserObjednal', format: '{0:n1}' },
        { field: 'DatumObjednano', format: '{0:d}' },
        { field: 'rr_StavObjednavky', format: '{0:n1}' },
        { field: 'Pracoviste' },
        { field: 'UserObjednal' }
    ];

    agilo.AGsp_GetFirmyObjednavkaDetail = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "IDObjednavkyPol",
                fields: {
                    IDObjednavkyPol: { type: 'number', validation: { required: true } },
                    FirmaObjednal: { type: 'string' },
                    Produkt: { type: 'string' },
                    Poznamka: { type: 'string' },
                    ObjednanoEMJ: { type: 'number', validation: { required: true } },
                    DomluvenaProdejniCena: { type: 'number' },
                    rr_DeadLine: { type: 'number', validation: { required: true } },
                    DeadLineDatum: { type: 'date', validation: { required: true } },
                    IDUserObjednal: { type: 'number' },
                    DatumObjednano: { type: 'date' },
                    rr_StavObjednavky: { type: 'number' },
                    Pracoviste: { type: 'string' },
                    UserObjednal: { type: 'string' }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetFirmyObjednavkaDetail?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmyObjednavkaDetail?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmyObjednavkaDetail?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmyObjednavkaDetail?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetFirmyObjednavkaDetail]>

    //<Start[AGsp_GetHledejFirmaFulltext]>
    agilo.AGsp_GetHledejFirmaFulltext_Columns = [
        { field: 'Firma' },
        { field: 'Nazev_firmy' },
        { field: 'Ulice' },
        { field: 'Mesto' },
        { field: 'Telefon_1' },
        { field: 'Telefon_2' },
        { field: 'E_mail' },
        { field: 'ICO' },
        { field: 'Kategorie' },
        { field: 'Krestni' },
        { field: 'Prijmeni' }
    ];

    agilo.AGsp_GetHledejFirmaFulltext = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "Firma",
                fields: {
                    Firma: { type: 'string', validation: { required: true } },
                    Nazev_firmy: { type: 'string' },
                    Ulice: { type: 'string' },
                    Mesto: { type: 'string' },
                    Telefon_1: { type: 'string' },
                    Telefon_2: { type: 'string' },
                    E_mail: { type: 'string' },
                    ICO: { type: 'string' },
                    Kategorie: { type: 'string' },
                    Krestni: { type: 'string' },
                    Prijmeni: { type: 'string' }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetHledejFirmaFulltext?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetHledejFirmaFulltext?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetHledejFirmaFulltext?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetHledejFirmaFulltext?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetHledejFirmaFulltext]>

    //<Start[AGsp_GetHledejGlobalFullText]>
    agilo.AGsp_GetHledejGlobalFullText_Columns = [
        { field: 'Firma' },
        { field: 'Nazev_firmy' },
        { field: 'Mesto' },
        { field: 'Prijmeni' }
    ];

    agilo.AGsp_GetHledejGlobalFullText = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "Firma",
                fields: {
                    Firma: { type: 'string', validation: { required: true } },
                    Nazev_firmy: { type: 'string' },
                    Mesto: { type: 'string' },
                    Prijmeni: { type: 'string' }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetHledejGlobalFullText?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetHledejGlobalFullText?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetHledejGlobalFullText?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetHledejGlobalFullText?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetHledejGlobalFullText]>

    //<Start[AGsp_GetMojePracaky]>
    agilo.AGsp_GetMojePracaky_Columns = [
        { field: 'IDVykazPrace', format: '{0:n1}' },
        { field: 'DatVzniku', format: '{0:d}' },
        { field: 'Firma' },
        { field: 'Poznamka' },
        { field: 'rr_StavPracaku', format: '{0:n1}' },
        { field: 'rr_StavPracakuHodnota' }
    ];

    agilo.AGsp_GetMojePracaky = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "IDVykazPrace",
                fields: {
                    IDVykazPrace: { type: 'number', validation: { required: true } },
                    DatVzniku: { type: 'date', validation: { required: true } },
                    Firma: { type: 'string', validation: { required: true } },
                    Poznamka: { type: 'string' },
                    rr_StavPracaku: { type: 'number', validation: { required: true } },
                    rr_StavPracakuHodnota: { type: 'string', validation: { required: true } }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetMojePracaky?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetMojePracaky?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetMojePracaky?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetMojePracaky?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetMojePracaky]>

    //<Start[AGsp_GetObjednavkySeznam]>
    agilo.AGsp_GetObjednavkySeznam_Columns = [
        { field: 'IDObjednavkyPol', format: '{0:n1}' },
        { field: 'Barva' },
        { field: 'FirmaObjednal' },
        { field: 'Pracoviste' },
        { field: 'rr_DeadLine', format: '{0:n1}' },
        { field: 'rr_DeadLineHodnota' },
        { field: 'DeadLineDatum', format: '{0:d}' },
        { field: 'Produkt' },
        { field: 'Carovy_kod' },
        { field: 'Popis' },
        { field: 'Poznamka' },
        { field: 'ObjednanoEMJ', format: '{0:n1}' },
        { field: 'DomluvenaProdejniCena', format: '{0:n1}' },
        { field: 'Cena_prodejni', format: '{0:n1}' },
        { field: 'Cena_nakupni', format: '{0:n1}' },
        { field: 'Nazev_firmy' },
        { field: 'NazevPracoviste' },
        { field: 'Dodavatel' },
        { field: 'rr_StavObjednavky', format: '{0:n1}' },
        { field: 'rr_StavObjednavkyHodnota' },
        { field: 'DatumObjednano', format: '{0:d}' },
        { field: 'IDUserObjednal', format: '{0:n1}' },
        { field: 'UserLastName' }
    ];

    agilo.AGsp_GetObjednavkySeznam = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "IDObjednavkyPol",
                fields: {
                    IDObjednavkyPol: { type: 'number', validation: { required: true } },
                    Barva: { type: 'string', validation: { required: true } },
                    FirmaObjednal: { type: 'string' },
                    Pracoviste: { type: 'string' },
                    rr_DeadLine: { type: 'number', validation: { required: true } },
                    rr_DeadLineHodnota: { type: 'string', validation: { required: true } },
                    DeadLineDatum: { type: 'date', validation: { required: true } },
                    Produkt: { type: 'string' },
                    Carovy_kod: { type: 'string' },
                    Popis: { type: 'string' },
                    Poznamka: { type: 'string' },
                    ObjednanoEMJ: { type: 'number', validation: { required: true } },
                    DomluvenaProdejniCena: { type: 'number' },
                    Cena_prodejni: { type: 'number' },
                    Cena_nakupni: { type: 'number' },
                    Nazev_firmy: { type: 'string' },
                    NazevPracoviste: { type: 'string' },
                    Dodavatel: { type: 'string' },
                    rr_StavObjednavky: { type: 'number' },
                    rr_StavObjednavkyHodnota: { type: 'string', validation: { required: true } },
                    DatumObjednano: { type: 'date' },
                    IDUserObjednal: { type: 'number' },
                    UserLastName: { type: 'string' }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetObjednavkySeznam?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetObjednavkySeznam?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetObjednavkySeznam?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetObjednavkySeznam?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetObjednavkySeznam]>

    //<Start[AGsp_GetObjednavkyVse]>
    agilo.AGsp_GetObjednavkyVse_Columns = [
        { field: 'IDObjednavkyPol', format: '{0:n1}' },
        { field: 'FirmaObjednal' },
        { field: 'Produkt' },
        { field: 'Poznamka' },
        { field: 'rr_DeadLine', format: '{0:n1}' },
        { field: 'rr_DeadLineHodnota' },
        { field: 'DeadLineDatum', format: '{0:d}' },
        { field: 'Barva', format: '{0:n1}' }
    ];

    agilo.AGsp_GetObjednavkyVse = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "IDObjednavkyPol",
                fields: {
                    IDObjednavkyPol: { type: 'number', validation: { required: true } },
                    FirmaObjednal: { type: 'string' },
                    Produkt: { type: 'string' },
                    Poznamka: { type: 'string' },
                    rr_DeadLine: { type: 'number', validation: { required: true } },
                    rr_DeadLineHodnota: { type: 'string', validation: { required: true } },
                    DeadLineDatum: { type: 'date', validation: { required: true } },
                    Barva: { type: 'number', validation: { required: true } }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetObjednavkyVse?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetObjednavkyVse?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetObjednavkyVse?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetObjednavkyVse?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetObjednavkyVse]>

    //<Start[AGsp_GetProduktDetail]>
    agilo.AGsp_GetProduktDetail_Columns = [
        { field: 'Produkt' },
        { field: 'Popis' },
        { field: 'Carovy_kod' },
        { field: 'Jednotky' },
        { field: 'Cena', format: '{0:n1}' },
        { field: 'Dodavatel' },
        { field: 'Mnozstvi_minimalni', format: '{0:n1}' },
        { field: 'Internet' },
        { field: 'Poznamka' },
        { field: 'Datum_aktualizace', format: '{0:d}' }
    ];

    agilo.AGsp_GetProduktDetail = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "Produkt",
                fields: {
                    Produkt: { type: 'string', validation: { required: true } },
                    Popis: { type: 'string' },
                    Carovy_kod: { type: 'string' },
                    Jednotky: { type: 'string' },
                    Cena: { type: 'number', validation: { required: true } },
                    Dodavatel: { type: 'string' },
                    Mnozstvi_minimalni: { type: 'number', validation: { required: true } },
                    Internet: { type: 'string' },
                    Poznamka: { type: 'string' },
                    Datum_aktualizace: { type: 'date' }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetProduktDetail?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetProduktDetail?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetProduktDetail?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetProduktDetail?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetProduktDetail]>

    //<Start[AGsp_GetProduktProcentoProvizeSeznam]>
    agilo.AGsp_GetProduktProcentoProvizeSeznam_Columns = [
        { field: 'Produkt' },
        { field: 'Popis' },
        { field: 'IDProduktProvize', format: '{0:n1}' },
        { field: 'ProcentoProvize', format: '{0:n1}' }
    ];

    agilo.AGsp_GetProduktProcentoProvizeSeznam = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "Produkt",
                fields: {
                    Produkt: { type: 'string', validation: { required: true } },
                    Popis: { type: 'string' },
                    IDProduktProvize: { type: 'number' },
                    ProcentoProvize: { type: 'number' }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetProduktProcentoProvizeSeznam?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetProduktProcentoProvizeSeznam?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetProduktProcentoProvizeSeznam?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetProduktProcentoProvizeSeznam?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetProduktProcentoProvizeSeznam]>

    //<Start[AGsp_GetProduktSeznamHledaci]>
    agilo.AGsp_GetProduktSeznamHledaci_Columns = [
        { field: 'Produkt' },
        { field: 'Kniha' },
        { field: 'Popis' },
        { field: 'Carovy_kod' },
        { field: 'Dodavatel' },
        { field: 'SkladovaZasoba', format: '{0:n1}' },
        { field: 'OperativniZasoba', format: '{0:n1}' },
        { field: 'PrumernaNakup', format: '{0:n1}' },
        { field: 'PrumernaProdej', format: '{0:n1}' },
        { field: 'NaposledyNaskladneno', format: '{0:d}' }
    ];

    agilo.AGsp_GetProduktSeznamHledaci = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "Produkt",
                fields: {
                    Produkt: { type: 'string', validation: { required: true } },
                    Kniha: { type: 'string' },
                    Popis: { type: 'string' },
                    Carovy_kod: { type: 'string' },
                    Dodavatel: { type: 'string' },
                    SkladovaZasoba: { type: 'number', validation: { required: true } },
                    OperativniZasoba: { type: 'number', validation: { required: true } },
                    PrumernaNakup: { type: 'number', validation: { required: true } },
                    PrumernaProdej: { type: 'number', validation: { required: true } },
                    NaposledyNaskladneno: { type: 'date' }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetProduktSeznamHledaci?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetProduktSeznamHledaci?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetProduktSeznamHledaci?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetProduktSeznamHledaci?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetProduktSeznamHledaci]>

    //<Start[AGsp_GetTaskDetail]>
    agilo.AGsp_GetTaskDetail_Columns = [
        { field: 'IDTask', format: '{0:n1}' },
        { field: 'DatumZadani', format: '{0:d}' },
        { field: 'IDUserZadal', format: '{0:n1}' },
        { field: 'ZadalPrijmeni' },
        { field: 'DatumResitelPrevzal', format: '{0:d}' },
        { field: 'rr_DeadLine', format: '{0:n1}' },
        { field: 'DatumDeadLine', format: '{0:d}' },
        { field: 'Predmet' },
        { field: 'Telo' },
        { field: 'rr_TaskStav', format: '{0:n1}' }
    ];

    agilo.AGsp_GetTaskDetail = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "IDTask",
                fields: {
                    IDTask: { type: 'number', validation: { required: true } },
                    DatumZadani: { type: 'date', validation: { required: true } },
                    IDUserZadal: { type: 'number' },
                    ZadalPrijmeni: { type: 'string' },
                    DatumResitelPrevzal: { type: 'date' },
                    rr_DeadLine: { type: 'number' },
                    DatumDeadLine: { type: 'date' },
                    Predmet: { type: 'string' },
                    Telo: { type: 'string' },
                    rr_TaskStav: { type: 'number', validation: { required: true } }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetTaskDetail?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetTaskDetail?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetTaskDetail?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetTaskDetail?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetTaskDetail]>

    //<Start[AGsp_GetTaskSeznam]>
    agilo.AGsp_GetTaskSeznam_Columns = [
        { field: 'IDTask', format: '{0:n1}' },
        { field: 'DatumResitelPrevzal', format: '{0:d}' },
        { field: 'rr_DeadLine', format: '{0:n1}' },
        { field: 'DatumDeadLine', format: '{0:d}' },
        { field: 'Predmet' },
        { field: 'rr_TaskStav', format: '{0:n1}' },
        { field: 'TaskStav' }
    ];

    agilo.AGsp_GetTaskSeznam = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "IDTask",
                fields: {
                    IDTask: { type: 'number', validation: { required: true } },
                    DatumResitelPrevzal: { type: 'date' },
                    rr_DeadLine: { type: 'number' },
                    DatumDeadLine: { type: 'date' },
                    Predmet: { type: 'string' },
                    rr_TaskStav: { type: 'number', validation: { required: true } },
                    TaskStav: { type: 'string', validation: { required: true } }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetTaskSeznam?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetTaskSeznam?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetTaskSeznam?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetTaskSeznam?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetTaskSeznam]>

    //<Start[AGsp_GetVykazPraceDetail]>
    agilo.AGsp_GetVykazPraceDetail_Columns = [
        { field: 'IDVykazPrace', format: '{0:n1}' },
        { field: 'DatVzniku', format: '{0:d}' },
        { field: 'IDPobocky' },
        { field: 'NazevPobocky' },
        { field: 'Poznamka' },
        { field: 'rr_StavPracaku', format: '{0:n1}' },
        { field: 'rr_TypServisniSmlouvy', format: '{0:n1}' },
        { field: 'CisloFaktury' },
        { field: 'rr_StavPracakuHodnota' },
        { field: 'rr_TypServisniSmlouvyHodnota' },
        { field: 'rr_FakturovatNaFirmu', format: '{0:n1}' },
        { field: 'rr_FakturovatNaFirmuHodnota' },
        { field: 'Zalozil' },
        { field: 'Upravil' },
        { field: 'Vzdalenost', format: '{0:n1}' },
        { field: 'Detail1' },
        { field: 'Detail2' },
        { field: 'Nazev_firmy' },
        { field: 'ICO' }
    ];

    agilo.AGsp_GetVykazPraceDetail = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "IDVykazPrace",
                fields: {
                    IDVykazPrace: { type: 'number', validation: { required: true } },
                    DatVzniku: { type: 'date', validation: { required: true } },
                    IDPobocky: { type: 'string', validation: { required: true } },
                    NazevPobocky: { type: 'string' },
                    Poznamka: { type: 'string' },
                    rr_StavPracaku: { type: 'number', validation: { required: true } },
                    rr_TypServisniSmlouvy: { type: 'number', validation: { required: true } },
                    CisloFaktury: { type: 'string' },
                    rr_StavPracakuHodnota: { type: 'string', validation: { required: true } },
                    rr_TypServisniSmlouvyHodnota: { type: 'string' },
                    rr_FakturovatNaFirmu: { type: 'number', validation: { required: true } },
                    rr_FakturovatNaFirmuHodnota: { type: 'string' },
                    Zalozil: { type: 'string' },
                    Upravil: { type: 'string' },
                    Vzdalenost: { type: 'number' },
                    Detail1: { type: 'string' },
                    Detail2: { type: 'string' },
                    Nazev_firmy: { type: 'string' },
                    ICO: { type: 'string' }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetVykazPraceDetail?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetVykazPraceDetail?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetVykazPraceDetail?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetVykazPraceDetail?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetVykazPraceDetail]>

    //<Start[AGsp_GetVykazPraceFirmaSeznam]>
    agilo.AGsp_GetVykazPraceFirmaSeznam_Columns = [
        { field: 'IDVykazPrace', format: '{0:n1}' },
        { field: 'DatVzniku', format: '{0:d}' },
        { field: 'Firma' },
        { field: 'rr_StavPracaku', format: '{0:n1}' },
        { field: 'rr_StavPracakuHodnota' },
        { field: 'IDUserZalozil', format: '{0:n1}' },
        { field: 'IDUserUpravil', format: '{0:n1}' },
        { field: 'ZalozilLastName' },
        { field: 'UpravilLastName' },
        { field: 'Nazev_firmy' }
    ];

    agilo.AGsp_GetVykazPraceFirmaSeznam = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "IDVykazPrace",
                fields: {
                    IDVykazPrace: { type: 'number', validation: { required: true } },
                    DatVzniku: { type: 'date', validation: { required: true } },
                    Firma: { type: 'string', validation: { required: true } },
                    rr_StavPracaku: { type: 'number', validation: { required: true } },
                    rr_StavPracakuHodnota: { type: 'string', validation: { required: true } },
                    IDUserZalozil: { type: 'number' },
                    IDUserUpravil: { type: 'number' },
                    ZalozilLastName: { type: 'string' },
                    UpravilLastName: { type: 'string' },
                    Nazev_firmy: { type: 'string' }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetVykazPraceFirmaSeznam?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetVykazPraceFirmaSeznam?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetVykazPraceFirmaSeznam?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetVykazPraceFirmaSeznam?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetVykazPraceFirmaSeznam]>

    //<Start[AGsp_GetVykazPraceNedokoncene]>
    agilo.AGsp_GetVykazPraceNedokoncene_Columns = [
        { field: 'IDVykazPrace', format: '{0:n1}' },
        { field: 'DatVzniku', format: '{0:d}' },
        { field: 'Firma' },
        { field: 'rr_StavPracaku', format: '{0:n1}' },
        { field: 'rr_StavPracakuHodnota' }
    ];

    agilo.AGsp_GetVykazPraceNedokoncene = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "IDVykazPrace",
                fields: {
                    IDVykazPrace: { type: 'number', validation: { required: true } },
                    DatVzniku: { type: 'date', validation: { required: true } },
                    Firma: { type: 'string', validation: { required: true } },
                    rr_StavPracaku: { type: 'number', validation: { required: true } },
                    rr_StavPracakuHodnota: { type: 'string', validation: { required: true } }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetVykazPraceNedokoncene?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetVykazPraceNedokoncene?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetVykazPraceNedokoncene?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetVykazPraceNedokoncene?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetVykazPraceNedokoncene]>

    //<Start[AGsp_GetVykazPracePolDetail]>
    agilo.AGsp_GetVykazPracePolDetail_Columns = [
        { field: 'IDVykazPracePol', format: '{0:n1}' },
        { field: 'IDVykazPrace', format: '{0:n1}' },
        { field: 'rr_TypPolozkyPracaku', format: '{0:n1}' },
        { field: 'CasOd' },
        { field: 'CasDo' },
        { field: 'Hodin', format: '{0:n1}' },
        { field: 'IDTechnika', format: '{0:n1}' },
        { field: 'Produkt' },
        { field: 'TextNaFakturu' },
        { field: 'TextInterniDoMailu' },
        { field: 'PocetEMJ', format: '{0:n1}' },
        { field: 'CenaEMJ', format: '{0:n1}' },
        { field: 'Vzdalenka' },
        { field: 'Zdarma' },
        { field: 'VelkyZasah', format: '{0:n1}' }
    ];

    agilo.AGsp_GetVykazPracePolDetail = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "IDVykazPracePol",
                fields: {
                    IDVykazPracePol: { type: 'number', validation: { required: true } },
                    IDVykazPrace: { type: 'number', validation: { required: true } },
                    rr_TypPolozkyPracaku: { type: 'number' },
                    CasOd: { type: 'Time' },
                    CasDo: { type: 'Time' },
                    Hodin: { type: 'number', validation: { required: true } },
                    IDTechnika: { type: 'number', validation: { required: true } },
                    Produkt: { type: 'string' },
                    TextNaFakturu: { type: 'string' },
                    TextInterniDoMailu: { type: 'string' },
                    PocetEMJ: { type: 'number', validation: { required: true } },
                    CenaEMJ: { type: 'number', validation: { required: true } },
                    Vzdalenka: { type: 'boolean', validation: { required: true } },
                    Zdarma: { type: 'boolean', validation: { required: true } },
                    VelkyZasah: { type: 'number', validation: { required: true } }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetVykazPracePolDetail?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetVykazPracePolDetail?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetVykazPracePolDetail?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetVykazPracePolDetail?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetVykazPracePolDetail]>

    //<Start[AGsp_GetVykazPracePolSeznam]>
    agilo.AGsp_GetVykazPracePolSeznam_Columns = [
        { field: 'IDVykazPracePol', format: '{0:n1}' },
        { field: 'IDVykazPrace', format: '{0:n1}' },
        { field: 'rr_TypPolozkyPracaku', format: '{0:n1}' },
        { field: 'rr_TypPolozkyPracakuHodnota' },
        { field: 'TextNaFakturu' },
        { field: 'PocetEMJ', format: '{0:n1}' }
    ];

    agilo.AGsp_GetVykazPracePolSeznam = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "IDVykazPracePol",
                fields: {
                    IDVykazPracePol: { type: 'number', validation: { required: true } },
                    IDVykazPrace: { type: 'number', validation: { required: true } },
                    rr_TypPolozkyPracaku: { type: 'number' },
                    rr_TypPolozkyPracakuHodnota: { type: 'string', validation: { required: true } },
                    TextNaFakturu: { type: 'string' },
                    PocetEMJ: { type: 'number', validation: { required: true } }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetVykazPracePolSeznam?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetVykazPracePolSeznam?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetVykazPracePolSeznam?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetVykazPracePolSeznam?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetVykazPracePolSeznam]>

    //<Start[AGsp_GetVykazPraceSeznam]>
    agilo.AGsp_GetVykazPraceSeznam_Columns = [
        { field: 'IDVykazPrace', format: '{0:n1}' },
        { field: 'DatVzniku', format: '{0:d}' },
        { field: 'Firma' },
        { field: 'rr_StavPracaku', format: '{0:n1}' },
        { field: 'rr_StavPracakuHodnota' }
    ];

    agilo.AGsp_GetVykazPraceSeznam = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "IDVykazPrace",
                fields: {
                    IDVykazPrace: { type: 'number', validation: { required: true } },
                    DatVzniku: { type: 'date', validation: { required: true } },
                    Firma: { type: 'string', validation: { required: true } },
                    rr_StavPracaku: { type: 'number', validation: { required: true } },
                    rr_StavPracakuHodnota: { type: 'string', validation: { required: true } }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetVykazPraceSeznam?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetVykazPraceSeznam?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetVykazPraceSeznam?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetVykazPraceSeznam?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetVykazPraceSeznam]>

    //<Start[AGsp_HledejProduktFullText]>
    agilo.AGsp_HledejProduktFullText_Columns = [
        { field: 'Produkt' },
        { field: 'Carovy_kod' },
        { field: 'Jednotky' },
        { field: 'Cena', format: '{0:n1}' },
        { field: 'Cena_nakupni', format: '{0:n1}' },
        { field: 'Popis' }
    ];

    agilo.AGsp_HledejProduktFullText = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "Produkt",
                fields: {
                    Produkt: { type: 'string', validation: { required: true } },
                    Carovy_kod: { type: 'string' },
                    Jednotky: { type: 'string' },
                    Cena: { type: 'number', validation: { required: true } },
                    Cena_nakupni: { type: 'number', validation: { required: true } },
                    Popis: { type: 'string' }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_HledejProduktFullText?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_HledejProduktFullText?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_HledejProduktFullText?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_HledejProduktFullText?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_HledejProduktFullText]>

    //<Start[AGsp_Run_Pracak00to10]>
    agilo.AGsp_Run_Pracak00to10_Columns = [
        { field: 'ErrorNumber', format: '{0:n1}' },
        { field: 'ErrorSeverity', format: '{0:n1}' },
        { field: 'ErrorState', format: '{0:n1}' },
        { field: 'ErrorProcedure' },
        { field: 'ErrorLine', format: '{0:n1}' },
        { field: 'ErrorMessage' }
    ];

    agilo.AGsp_Run_Pracak00to10 = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "ErrorNumber",
                fields: {
                    ErrorNumber: { type: 'number' },
                    ErrorSeverity: { type: 'number' },
                    ErrorState: { type: 'number' },
                    ErrorProcedure: { type: 'string' },
                    ErrorLine: { type: 'number' },
                    ErrorMessage: { type: 'string' }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_Run_Pracak00to10?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_Run_Pracak00to10?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_Run_Pracak00to10?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_Run_Pracak00to10?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_Run_Pracak00to10]>

    //<Start[AGsp_AddOrEditVykazPracePol1]>
    agilo.AGsp_AddOrEditVykazPracePol_Columns1 = [
        { field: 'ErrorNumber', format: '{0:n1}' },
        { field: 'ErrorMessage' }
    ];

    agilo.AGsp_AddOrEditVykazPracePol1 = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "ErrorNumber",
                fields: {
                    ErrorNumber: { type: 'number' },
                    ErrorMessage: { type: 'string' }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_AddOrEditVykazPracePol1?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_AddOrEditVykazPracePol1?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_AddOrEditVykazPracePol1?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_AddOrEditVykazPracePol1?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_AddOrEditVykazPracePol1]>

    //<Start[AGsp_Run_Pracak00to101]>
    agilo.AGsp_Run_Pracak00to10_Columns1 = [
        { field: 'ErrorNumber', format: '{0:n1}' },
        { field: 'ErrorSeverity', format: '{0:n1}' },
        { field: 'ErrorState', format: '{0:n1}' },
        { field: 'ErrorProcedure' },
        { field: 'ErrorLine', format: '{0:n1}' },
        { field: 'ErrorMessage' }
    ];

    agilo.AGsp_Run_Pracak00to101 = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "ErrorNumber",
                fields: {
                    ErrorNumber: { type: 'number' },
                    ErrorSeverity: { type: 'number' },
                    ErrorState: { type: 'number' },
                    ErrorProcedure: { type: 'string' },
                    ErrorLine: { type: 'number' },
                    ErrorMessage: { type: 'string' }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_Run_Pracak00to101?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_Run_Pracak00to101?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_Run_Pracak00to101?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_Run_Pracak00to101?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_Run_Pracak00to101]>

    //<Start[AGsp_GetFA_Hlavicka]>
    agilo.AGsp_GetFA_Hlavicka_Columns = [
        { field: 'Firma' },
        { field: 'ICO' },
        { field: 'Kniha' },
        { field: 'Text' },
        { field: 'TypZaznamu' },
        { field: 'Mena' },
        { field: 'Datum', format: '{0:d}' },
        { field: 'DatumZdanitelnehoPlneni', format: '{0:d}' },
        { field: 'Email' },
        { field: 'Telefon' }
    ];

    agilo.AGsp_GetFA_Hlavicka = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "Firma",
                fields: {
                    Firma: { type: 'string', validation: { required: true } },
                    ICO: { type: 'string', validation: { required: true } },
                    Kniha: { type: 'string', validation: { required: true } },
                    Text: { type: 'string', validation: { required: true } },
                    TypZaznamu: { type: 'string', validation: { required: true } },
                    Mena: { type: 'string', validation: { required: true } },
                    Datum: { type: 'date', validation: { required: true } },
                    DatumZdanitelnehoPlneni: { type: 'date', validation: { required: true } },
                    Email: { type: 'string', validation: { required: true } },
                    Telefon: { type: 'string', validation: { required: true } }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetFA_Hlavicka?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFA_Hlavicka?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFA_Hlavicka?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFA_Hlavicka?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetFA_Hlavicka]>

    //<Start[AGsp_GetFA_Polozky]>
    agilo.AGsp_GetFA_Polozky_Columns = [
        { field: 'Produkt' },
        { field: 'TextNaFakturu' },
        { field: 'PocetEMJ', format: '{0:n1}' },
        { field: 'SazbaDPH', format: '{0:n1}' },
        { field: 'NazevSazbyDPH' },
        { field: 'CenaCelkemBezDPH', format: '{0:n1}' },
        { field: 'Zakazka', format: '{0:n1}' },
        { field: 'CenaCelkemVcetneDPH', format: '{0:n1}' },
        { field: 'Poradi', format: '{0:n1}' },
        { field: 'Jednotky' }
    ];

    agilo.AGsp_GetFA_Polozky = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "Produkt",
                fields: {
                    Produkt: { type: 'string' },
                    TextNaFakturu: { type: 'string' },
                    PocetEMJ: { type: 'number', validation: { required: true } },
                    SazbaDPH: { type: 'number', validation: { required: true } },
                    NazevSazbyDPH: { type: 'string', validation: { required: true } },
                    CenaCelkemBezDPH: { type: 'number' },
                    Zakazka: { type: 'number' },
                    CenaCelkemVcetneDPH: { type: 'number' },
                    Poradi: { type: 'number' },
                    Jednotky: { type: 'string' }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetFA_Polozky?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFA_Polozky?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFA_Polozky?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFA_Polozky?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetFA_Polozky]>

    //<Start[AGsp_GetFA_Hlavicka1]>
    agilo.AGsp_GetFA_Hlavicka_Columns1 = [
        { field: 'Firma' },
        { field: 'ICO' },
        { field: 'Kniha' },
        { field: 'Text' },
        { field: 'TypZaznamu' },
        { field: 'Mena' },
        { field: 'Datum', format: '{0:d}' },
        { field: 'DatumZdanitelnehoPlneni', format: '{0:d}' }
    ];

    agilo.AGsp_GetFA_Hlavicka1 = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "Firma",
                fields: {
                    Firma: { type: 'string', validation: { required: true } },
                    ICO: { type: 'string', validation: { required: true } },
                    Kniha: { type: 'string', validation: { required: true } },
                    Text: { type: 'string', validation: { required: true } },
                    TypZaznamu: { type: 'string', validation: { required: true } },
                    Mena: { type: 'string', validation: { required: true } },
                    Datum: { type: 'date', validation: { required: true } },
                    DatumZdanitelnehoPlneni: { type: 'date', validation: { required: true } }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetFA_Hlavicka1?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFA_Hlavicka1?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFA_Hlavicka1?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFA_Hlavicka1?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetFA_Hlavicka1]>

    //<Start[AGsp_HledejSluzbaFullText]>
    agilo.AGsp_HledejSluzbaFullText_Columns = [
        { field: 'Produkt' },
        { field: 'Carovy_kod' },
        { field: 'Jednotky' },
        { field: 'Cena', format: '{0:n1}' },
        { field: 'Cena_nakupni', format: '{0:n1}' },
        { field: 'Popis' }
    ];

    agilo.AGsp_HledejSluzbaFullText = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "Produkt",
                fields: {
                    Produkt: { type: 'string', validation: { required: true } },
                    Carovy_kod: { type: 'string' },
                    Jednotky: { type: 'string' },
                    Cena: { type: 'number', validation: { required: true } },
                    Cena_nakupni: { type: 'number', validation: { required: true } },
                    Popis: { type: 'string' }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_HledejSluzbaFullText?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_HledejSluzbaFullText?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_HledejSluzbaFullText?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_HledejSluzbaFullText?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_HledejSluzbaFullText]>

    //<Start[AGsp_GetSluzbyPreddefinovane]>
    agilo.AGsp_GetSluzbyPreddefinovane_Columns = [
        { field: 'Produkt' },
        { field: 'Typ_produktu' },
        { field: 'Popis' },
        { field: 'Carovy_kod' },
        { field: 'Jednotky' },
        { field: 'Cena', format: '{0:n1}' },
        { field: 'Poznamka' }
    ];

    agilo.AGsp_GetSluzbyPreddefinovane = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "Produkt",
                fields: {
                    Produkt: { type: 'string', validation: { required: true } },
                    Typ_produktu: { type: 'string' },
                    Popis: { type: 'string' },
                    Carovy_kod: { type: 'string' },
                    Jednotky: { type: 'string' },
                    Cena: { type: 'number', validation: { required: true } },
                    Poznamka: { type: 'string' }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetSluzbyPreddefinovane?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetSluzbyPreddefinovane?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetSluzbyPreddefinovane?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetSluzbyPreddefinovane?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetSluzbyPreddefinovane]>

    //<Start[AGsp_GetMegaPolSeznam]>
    agilo.AGsp_GetMegaPolSeznam_Columns = [
        { field: 'IDMega', format: '{0:n1}' },
        { field: 'MegaSti_IDProduktu', format: '{0:n1}' },
        { field: 'rr_MegaStav', format: '{0:n1}' },
        { field: 'CasVzniku' },
        { field: 'Objednavka' },
        { field: 'MegaCodeObjednaci' },
        { field: 'MegaName' },
        { field: 'MegaCelkovaCena', format: '{0:n1}' },
        { field: 'MegaQtyPocetEMJ', format: '{0:n1}' },
        { field: 'DohodnutaProdejniCenaEMJ', format: '{0:n1}' },
        { field: 'Poznamka' },
        { field: 'rr_MegaStavHodnota' },
        { field: 'HtmlZnacka' }
    ];

    agilo.AGsp_GetMegaPolSeznam = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "IDMega",
                fields: {
                    IDMega: { type: 'number', validation: { required: true } },
                    MegaSti_IDProduktu: { type: 'number' },
                    rr_MegaStav: { type: 'number', validation: { required: true } },
                    CasVzniku: { type: 'string' },
                    Objednavka: { type: 'string' },
                    MegaCodeObjednaci: { type: 'string' },
                    MegaName: { type: 'string' },
                    MegaCelkovaCena: { type: 'number' },
                    MegaQtyPocetEMJ: { type: 'number' },
                    DohodnutaProdejniCenaEMJ: { type: 'number' },
                    Poznamka: { type: 'string' },
                    rr_MegaStavHodnota: { type: 'string' },
                    HtmlZnacka: { type: 'string' }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetMegaPolSeznam?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetMegaPolSeznam?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetMegaPolSeznam?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetMegaPolSeznam?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetMegaPolSeznam]>

    //<Start[AGsp_AddNewPracakyPolozkaProduktZablokovat1]>
    agilo.AGsp_AddNewPracakyPolozkaProduktZablokovat_Columns1 = [
        { field: 'ErrorNumber', format: '{0:n1}' },
        { field: 'ErrorMessage' }
    ];

    agilo.AGsp_AddNewPracakyPolozkaProduktZablokovat1 = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "ErrorNumber",
                fields: {
                    ErrorNumber: { type: 'number' },
                    ErrorMessage: { type: 'string' }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_AddNewPracakyPolozkaProduktZablokovat1?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_AddNewPracakyPolozkaProduktZablokovat1?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_AddNewPracakyPolozkaProduktZablokovat1?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_AddNewPracakyPolozkaProduktZablokovat1?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_AddNewPracakyPolozkaProduktZablokovat1]>

    //<Start[AGsp_GetFA_Hlavicka2]>
    agilo.AGsp_GetFA_Hlavicka_Columns2 = [
        { field: 'Firma' },
        { field: 'ICO' },
        { field: 'Kniha' },
        { field: 'Text' },
        { field: 'TypZaznamu' },
        { field: 'Mena' },
        { field: 'Datum', format: '{0:d}' },
        { field: 'DatumZdanitelnehoPlneni', format: '{0:d}' }
    ];

    agilo.AGsp_GetFA_Hlavicka2 = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "Firma",
                fields: {
                    Firma: { type: 'string', validation: { required: true } },
                    ICO: { type: 'string', validation: { required: true } },
                    Kniha: { type: 'string', validation: { required: true } },
                    Text: { type: 'string', validation: { required: true } },
                    TypZaznamu: { type: 'string', validation: { required: true } },
                    Mena: { type: 'string', validation: { required: true } },
                    Datum: { type: 'date', validation: { required: true } },
                    DatumZdanitelnehoPlneni: { type: 'date', validation: { required: true } }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetFA_Hlavicka2?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFA_Hlavicka2?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFA_Hlavicka2?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFA_Hlavicka2?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetFA_Hlavicka2]>

    //<Start[AGsp_GetFA_Hlavicka3]>
    agilo.AGsp_GetFA_Hlavicka_Columns3 = [
        { field: 'Firma' },
        { field: 'ICO' },
        { field: 'Kniha' },
        { field: 'Text' },
        { field: 'TypZaznamu' },
        { field: 'Mena' },
        { field: 'Datum', format: '{0:d}' },
        { field: 'DatumZdanitelnehoPlneni', format: '{0:d}' }
    ];

    agilo.AGsp_GetFA_Hlavicka3 = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "Firma",
                fields: {
                    Firma: { type: 'string', validation: { required: true } },
                    ICO: { type: 'string', validation: { required: true } },
                    Kniha: { type: 'string', validation: { required: true } },
                    Text: { type: 'string', validation: { required: true } },
                    TypZaznamu: { type: 'string', validation: { required: true } },
                    Mena: { type: 'string', validation: { required: true } },
                    Datum: { type: 'date', validation: { required: true } },
                    DatumZdanitelnehoPlneni: { type: 'date', validation: { required: true } }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetFA_Hlavicka3?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFA_Hlavicka3?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFA_Hlavicka3?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFA_Hlavicka3?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetFA_Hlavicka3]>

    //<Start[AGsp_GetFA_Hlavicka4]>
    agilo.AGsp_GetFA_Hlavicka_Columns4 = [
        { field: 'Firma' },
        { field: 'ICO' },
        { field: 'Kniha' },
        { field: 'Text' },
        { field: 'TypZaznamu' },
        { field: 'Mena' },
        { field: 'Datum', format: '{0:d}' },
        { field: 'DatumZdanitelnehoPlneni', format: '{0:d}' }
    ];

    agilo.AGsp_GetFA_Hlavicka4 = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "Firma",
                fields: {
                    Firma: { type: 'string', validation: { required: true } },
                    ICO: { type: 'string', validation: { required: true } },
                    Kniha: { type: 'string', validation: { required: true } },
                    Text: { type: 'string', validation: { required: true } },
                    TypZaznamu: { type: 'string', validation: { required: true } },
                    Mena: { type: 'string', validation: { required: true } },
                    Datum: { type: 'date', validation: { required: true } },
                    DatumZdanitelnehoPlneni: { type: 'date', validation: { required: true } }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetFA_Hlavicka4?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFA_Hlavicka4?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFA_Hlavicka4?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFA_Hlavicka4?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetFA_Hlavicka4]>

    //<Start[AGsp_GetFA_Hlavicka5]>
    agilo.AGsp_GetFA_Hlavicka_Columns5 = [
        { field: 'Firma' },
        { field: 'ICO' },
        { field: 'Kniha' },
        { field: 'Text' },
        { field: 'TypZaznamu' },
        { field: 'Mena' },
        { field: 'Datum', format: '{0:d}' },
        { field: 'DatumZdanitelnehoPlneni', format: '{0:d}' },
        { field: 'Email' },
        { field: 'Telefon' }
    ];

    agilo.AGsp_GetFA_Hlavicka5 = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "Firma",
                fields: {
                    Firma: { type: 'string', validation: { required: true } },
                    ICO: { type: 'string', validation: { required: true } },
                    Kniha: { type: 'string', validation: { required: true } },
                    Text: { type: 'string', validation: { required: true } },
                    TypZaznamu: { type: 'string', validation: { required: true } },
                    Mena: { type: 'string', validation: { required: true } },
                    Datum: { type: 'date', validation: { required: true } },
                    DatumZdanitelnehoPlneni: { type: 'date', validation: { required: true } },
                    Email: { type: 'string', validation: { required: true } },
                    Telefon: { type: 'string', validation: { required: true } }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetFA_Hlavicka5?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFA_Hlavicka5?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFA_Hlavicka5?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFA_Hlavicka5?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetFA_Hlavicka5]>

    //<Start[AGsp_GetPracakPolozkyNahled]>
    agilo.AGsp_GetPracakPolozkyNahled_Columns = [
        { field: 'IDVykazPracePol', format: '{0:n1}' },
        { field: 'TextNaFakturu' },
        { field: 'PocetEMJ', format: '{0:n1}' },
        { field: 'CenaEMJBezDPHNaFakturu', format: '{0:n1}' },
        { field: 'Celkem', format: '{0:n1}' },
        { field: 'rr_VypocetKomentHodnota' }
    ];

    agilo.AGsp_GetPracakPolozkyNahled = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "IDVykazPracePol",
                fields: {
                    IDVykazPracePol: { type: 'number', validation: { required: true } },
                    TextNaFakturu: { type: 'string', validation: { required: true } },
                    PocetEMJ: { type: 'number' },
                    CenaEMJBezDPHNaFakturu: { type: 'number' },
                    Celkem: { type: 'number' },
                    rr_VypocetKomentHodnota: { type: 'string', validation: { required: true } }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetPracakPolozkyNahled?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetPracakPolozkyNahled?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetPracakPolozkyNahled?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetPracakPolozkyNahled?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetPracakPolozkyNahled]>

    //<Start[AGsp_GetPuvodPolozkyNaPracaku]>
    agilo.AGsp_GetPuvodPolozkyNaPracaku_Columns = [
        { field: 'BlokaceEMJ', format: '{0:n1}' },
        { field: 'Produkt' },
        { field: 'NaskladnenoEMJ', format: '{0:n1}' },
        { field: 'CenaNakup', format: '{0:n1}' },
        { field: 'VSPrijmovehoDokladu' },
        { field: 'SkladovaZasoba', format: '{0:n1}' },
        { field: 'OperativniZasoba', format: '{0:n1}' },
        { field: 'Dodavatel' },
        { field: 'DatumObjednal', format: '{0:d}' },
        { field: 'KdoObjednal' },
        { field: 'DatumNaskladnil', format: '{0:d}' },
        { field: 'KdoNaskladnil' }
    ];

    agilo.AGsp_GetPuvodPolozkyNaPracaku = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "BlokaceEMJ",
                fields: {
                    BlokaceEMJ: { type: 'number' },
                    Produkt: { type: 'string', validation: { required: true } },
                    NaskladnenoEMJ: { type: 'number' },
                    CenaNakup: { type: 'number' },
                    VSPrijmovehoDokladu: { type: 'string' },
                    SkladovaZasoba: { type: 'number' },
                    OperativniZasoba: { type: 'number' },
                    Dodavatel: { type: 'string' },
                    DatumObjednal: { type: 'date' },
                    KdoObjednal: { type: 'string' },
                    DatumNaskladnil: { type: 'date' },
                    KdoNaskladnil: { type: 'string' }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetPuvodPolozkyNaPracaku?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetPuvodPolozkyNaPracaku?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetPuvodPolozkyNaPracaku?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetPuvodPolozkyNaPracaku?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetPuvodPolozkyNaPracaku]>

    //<Start[AGsp_GetInventurniStav]>
    agilo.AGsp_GetInventurniStav_Columns = [
        { field: 'Produkt' },
        { field: 'Popis' },
        { field: 'SkladovaZasoba', format: '{0:n1}' },
        { field: 'OperativniZasoba', format: '{0:n1}' },
        { field: 'PrumernaNakup', format: '{0:n1}' },
        { field: 'NaposledyNaskladneno', format: '{0:d}' }
    ];

    agilo.AGsp_GetInventurniStav = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "Produkt",
                fields: {
                    Produkt: { type: 'string', validation: { required: true } },
                    Popis: { type: 'string' },
                    SkladovaZasoba: { type: 'number', validation: { required: true } },
                    OperativniZasoba: { type: 'number', validation: { required: true } },
                    PrumernaNakup: { type: 'number', validation: { required: true } },
                    NaposledyNaskladneno: { type: 'date' }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetInventurniStav?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetInventurniStav?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetInventurniStav?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetInventurniStav?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetInventurniStav]>

    //<Start[AGsp_GetFirmyRecordHistorySeznam]>
    agilo.AGsp_GetFirmyRecordHistorySeznam_Columns = [
        { field: 'IDFirmyRecordHistory', format: '{0:n1}' },
        { field: 'IDFirmy' },
        { field: 'RecordDate', format: '{0:d}' },
        { field: 'RecordCommentType' },
        { field: 'RecordCommentTxt' },
        { field: 'IDUser', format: '{0:n1}' }
    ];

    agilo.AGsp_GetFirmyRecordHistorySeznam = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "IDFirmyRecordHistory",
                fields: {
                    IDFirmyRecordHistory: { type: 'number', validation: { required: true } },
                    IDFirmy: { type: 'string' },
                    RecordDate: { type: 'date', validation: { required: true } },
                    RecordCommentType: { type: 'string' },
                    RecordCommentTxt: { type: 'string' },
                    IDUser: { type: 'number' }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_GetFirmyRecordHistorySeznam?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmyRecordHistorySeznam?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmyRecordHistorySeznam?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_GetFirmyRecordHistorySeznam?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_GetFirmyRecordHistorySeznam]>

    //<Start[AGsp_FA_MailTextyPolozek]>
    agilo.AGsp_FA_MailTextyPolozek_Columns = [
        { field: 'IDVykazPracePol', format: '{0:n1}' },
        { field: 'IDVykazPrace', format: '{0:n1}' },
        { field: 'FormaZasahu' },
        { field: 'IDTechnika', format: '{0:n1}' },
        { field: 'TechnikOdpovednaOsoba' },
        { field: 'PocetEMJ', format: '{0:n1}' },
        { field: 'Jednotky' },
        { field: 'TextDoMailu' },
        { field: 'Zdarma' }
    ];

    agilo.AGsp_FA_MailTextyPolozek = new kendo.data.DataSource({
        serverSorting: true,
        serverFiltering: true,
        serverPaging: true,
        //pageSize: 100,
        //filter: (idobjektu ? { field: "IDOrder", operator: "eq", value: IDOrder } : {}),
        //sort: { field: "IDOrder", dir: "desc" },
        schema: {
            data: "data",
            total: "total",
            errors: "error",
            model: {
                id: "IDVykazPracePol",
                fields: {
                    IDVykazPracePol: { type: 'number', validation: { required: true } },
                    IDVykazPrace: { type: 'number', validation: { required: true } },
                    FormaZasahu: { type: 'string', validation: { required: true } },
                    IDTechnika: { type: 'number', validation: { required: true } },
                    TechnikOdpovednaOsoba: { type: 'string' },
                    PocetEMJ: { type: 'number', validation: { required: true } },
                    Jednotky: { type: 'string', validation: { required: true } },
                    TextDoMailu: { type: 'string' },
                    Zdarma: { type: 'string', validation: { required: true } }
                }
            }
        },
        transport: {
            read: {
                url: function (options) {
                    return '/Api/Service/AGsp_FA_MailTextyPolozek?type=read';
                },
                type: "GET"
            },
            update: {
                url: function (data) {
                    return '/Api/Service/AGsp_FA_MailTextyPolozek?type=update';
                },
                type: "POST"
            },
            create: {
                url: function (data) {
                    return '/Api/Service/AGsp_FA_MailTextyPolozek?type=create';
                },
                type: "POST"
            },
            destroy: {
                url: function (data) {
                    return '/Api/Service/AGsp_FA_MailTextyPolozek?type=remove';
                },
                type: "POST"
            },
            parameterMap: function (data, type) {
                if (type === "read") {
                    var pm = kendo.data.transports.odata.parameterMap(data);
                    return pm;
                } else {
                    return data;
                }
            }
        },
        error: function (e) {
            e.sender.cancelChanges();
            if (e.errors) {
                agiloAlert("Hlášení", e.errors);
            } else {
                agiloAlert("Error " + e.xhr.status, "Systémová chyba aplikace. Kontaktujte vývojáře.");
            }
        }
    });
    //<End[AGsp_FA_MailTextyPolozek]>

//<EndSchema>
})(window);