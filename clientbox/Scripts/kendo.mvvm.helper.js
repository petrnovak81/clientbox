(function ($) {
    $.grid = {
        cell: {
            template: function (options) {
                var settings = $.extend({
                    type: "string", //string, number, date, email, link, tel, pasword
                    value: "",
                    format: null
                }, options);
                var value = null
                switch (settings.type) {
                    case "email":
                        return '<a title="' + settings.value + '" href="mailto:' + settings.value + '">' + settings.value + '</a>';
                    case "link":
                        return '<a title="' + settings.value + '" href="' + settings.value + '">' + settings.value + '</a>';
                    case "tel":
                        return '<a title="' + settings.value + '" href="tel:' + settings.value + '">' + settings.value + '</a>';
                    case "password":
                        return '<label title="&#8226;&#8226;&#8226;&#8226;&#8226;&#8226;">&#8226;&#8226;&#8226;&#8226;&#8226;&#8226;</label>';
                    default:
                        value = (settings.format ? kendo.toString(settings.value, settings.format) : kendo.toString(settings.value, "n"));
                        return '<label title="' + value + '">' + value + '</label>';
                }
            }
        }
    }
}(jQuery));

//var g = $.grid.cell.template({ value: new Date(), format: "yyyy M d" })

//console.log(g)