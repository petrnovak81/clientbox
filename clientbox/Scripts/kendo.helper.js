function input_email(value) {
    return '<a style="color:\#007bff;" class="k-link" href="mailto:' + value + '">' + value + '</a>';
};
function input_email_edit(container, options) {
    $('<input data-text-field="' + options.field + '" ' +
            'class="k-input k-textbox" ' +
            'required ' +
            'type="email" ' +
            'data-bind="value:' + options.field + '"/>')
            .appendTo(container)
};

function input_phone(value) {
    if (value) {
        return '<a style="color:\#007bff;" class="k-link" href="tel:' + value + '"><span style="color: gray;">+420 </span>' + formatPhoneNumber(value) + '</a>';
    } else {
        return null;
    }
};
function input_phone_edit(container, options) {
    $('<input data-text-field="' + options.field + '" ' +
            'class="k-input k-textbox" ' +
            'type="text" ' +
            'pattern="[0-9]{3}[0-9]{3}[0-9]{3}" ' +
            'maxlength="9" ' +
            'data-bind="value:' + options.field + '"/>')
            .appendTo(container)
};

function input_password(value) {
    return '&#8226;&#8226;&#8226;&#8226;&#8226;&#8226;&#8226;&#8226;&#8226;&#8226;&#8226;&#8226;&#8226;&#8226;&#8226;&#8226;';
};
function input_password_edit(container, options) {
    $('<input data-text-field="' + options.field + '" ' +
            'class="k-input k-textbox" ' +
            'required ' +
            'type="password" ' +
            'pattern=".{6,}" ' +
            'data-bind="value:' + options.field + '"/>')
            .appendTo(container)
};

function cellCheckbox(value) {
    return '<div class="text-center"><input type="checkbox" disabled ' + (value ? 'checked' : '') + '/></div>';
};

function input_switch(value) {
    return '<input data-role="switch" type="checkbox" style="width: 100%;" data-on-label="Ano" data-off-label="Ne" disabled ' + (value ? 'checked' : '') + '/>';
};
function input_switch_edit(container, options) {
    $('<input data-role="switch" type="checkbox" data-on-label="Ano" data-off-label="Ne" name="' + options.field + '" data-bind="checked: ' + options.field + '" />').appendTo(container).kendoMobileSwitch({
        onLabel: "Ano",
        offLabel: "Ne"
    });
};

function input_foreign_key(source, value, text) {
    return '<input disabled data-value-primitive data-text-field="' + text + '" data-value-field="' + value + '" data-role="dropdownlist" data-bind=" source: ' + source + ', value: ' + value + '"/>';
};

function formatPhoneNumber(phoneNumber) {
    var piece1 = phoneNumber.substring(0, 3); //123
    var piece2 = phoneNumber.substring(3, 6); //456
    var piece3 = phoneNumber.substring(6); //789

    return kendo.format("{0} {1} {2}", piece1, piece2, piece3);
}

function tmpEmail(container, options) {
    if (container) {
        //editable
    } else {
        //readonly
        return '<a style="color:\#007bff;" class="k-link" href="mailto:' + options.value + '">' + options.value + '</a>';
    }
}
function tmpTel(container, options) {

}
function tmpEnum(container, options) { }
function tmpSwitch(container, options) { }