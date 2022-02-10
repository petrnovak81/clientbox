@Code
    ViewData("Title") = "Číselníky"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div id="main" style="height: 100%;">
    <div data-role="grid"
         data-column-menu="true"
         data-resizable="true"
         data-scrollable="true"
         data-editable="false"
         data-filterable="true"
         data-sortable="true"
         data-selectable="false"
         data-no-records="{ template: '<h3 style=\'text-align:center;\'>Žádné záznamy</h3>' }"
         data-navigatable="true"
         data-columns="[
        { 'field': 'Produkt', 'title': 'Produkt' },
        { 'field': 'Typ_produktu', 'title': 'Typ produktu', 'groupHeaderTemplate': groupHeader, 'hidden': true  },
        { 'field': 'Popis', 'title': 'Popis' },
        { 'field': 'Carovy_kod', 'title': 'Čárový kód' },
        { 'field': 'Jednotky', 'title': 'Jednotky' },
        { 'field': 'Cena', 'title': 'Cena' },
        { 'field': 'Poznamka', 'title': 'Poznámka' },
        { 'title': ' ', 'template': btnedit, width: 70 },,
        ]"
        data-bind="source: AGsp_GetSluzbyPreddefinovane, events: { dataBound: AGsp_GetSluzbyPreddefinovane_onDataBound }" style="height: 100%;"></div>
</div>

<div class="modal fade" id="modal_add">
    <form class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">Nový produkt: <span data-bind="text: addData.Typ_produktu"></span></h4>
                <button type="button" class="close" data-dismiss="modal">&times;</button>
            </div>
            <div class="modal-body">
                <div class="form-group">
                    <label>Produkt</label>
                    <input type="text" required data-bind="value: addData.Produkt" class="form-control" />
                </div>
                <div class="form-group">
                    <label>Popis</label>
                    <input type="text" data-bind="value: addData.Popis" class="form-control" />
                </div>
                <div class="form-group">
                    <label>Čárový kód</label>
                    <input type="text" data-bind="value: addData.Carovy_kod" class="form-control" />
                </div>
                <div class="form-group">
                    <label>Jednotky</label>
                    <input type="text" data-bind="value: addData.Jednotky" class="form-control" />
                </div>
                <div class="form-group">
                    <label>Cena</label>
                    <input data-format="n0" data-role="numerictextbox" type="number" data-min="0" data-bind="value: addData.Cena" class="form-control" />
                </div>
                <div class="form-group">
                    <label>Poznámka</label>
                    <textarea type="text" data-bind="value: addData.Poznamka" class="form-control"></textarea>
                </div>
            </div>
            <div class="modal-footer">
                <button type="submit" class="btn btn-success" data-bind="events: { click: addProdukt }">Uložit</button>
                <button type="button" class="btn btn-warning" data-dismiss="modal">Zavřít</button>
            </div>
        </div>
    </form>
</div>

<div class="modal fade" id="modal_edit">
    <form class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <h4 class="modal-title">Upravit produkt: <span data-bind="text: editData.Typ_produktu"></span></h4>
                <button type="button" class="close" data-dismiss="modal">&times;</button>
            </div>
            <div class="modal-body">
                <div class="form-group">
                    <label>Produkt</label>
                    <input type="text" required data-bind="value: editData.Produkt" class="form-control" />
                </div>
                <div class="form-group">
                    <label>Popis</label>
                    <input type="text" data-bind="value: editData.Popis" class="form-control" />
                </div>
                <div class="form-group">
                    <label>Čárový kód</label>
                    <input type="text" data-bind="value: editData.Carovy_kod" class="form-control" />
                </div>
                <div class="form-group">
                    <label>Jednotky</label>
                    <input type="text" data-bind="value: editData.Jednotky" class="form-control" />
                </div>
                <div class="form-group">
                    <label>Cena</label>
                    <input data-format="n0" data-role="numerictextbox" type="number" data-min="0" data-bind="value: editData.Cena" class="form-control" />
                </div>
                <div class="form-group">
                    <label>Poznámka</label>
                    <textarea type="text" data-bind="value: editData.Poznamka" class="form-control"></textarea>
                </div>
            </div>
            <div class="modal-footer">
                <button type="submit" class="btn btn-success" data-bind="events: { click: updateProdukt }">Uložit</button>
                <button type="button" class="btn btn-warning" data-dismiss="modal">Zavřít</button>
            </div>
        </div>
    </form>
</div>

<script>
    function groupHeader(e) {
        return '<button style="width:auto;" data-typ="' + e.value + '" type="button" class="btn btn-success btn-sm btn-add"><span class="k-icon k-i-plus"></span>Nový produkt: ' + e.value + '</button>';
    }

    function btnedit(e) {
        return '<button type="button" class="btn btn-primary btn-sm" data-bind="events: { click: btnedit }"><span class="k-icon k-i-edit"></span> Upravit</button>';
    };

    $(function () {
        var viewModel = kendo.observable({
            AGsp_GetSluzbyPreddefinovane: new kendo.data.DataSource({
                transport: {
                    read: function (options) {
                        $.get('@Url.Action("AGsp_GetSluzbyPreddefinovane", "Api/Service")', {}, function (result) {
                            options.success(result.data);
                        });
                    }
                },
                group: { field: "Typ_produktu" }
            }),
            AGsp_GetSluzbyPreddefinovane_onDataBound: function (e) {
                var that = this;
                $(".k-reset").delegate(".btn-add", "click", function (e) {
                    var typ = $(this).data("typ");
                    e.preventDefault();
                    that.set("addData", {
                        Produkt: "",
                        Typ_produktu: typ,
                        Popis: "",
                        Carovy_kod: "",
                        Jednotky: "Ks",
                        Cena: 0,
                        Poznamka: ""
                    });
                    $('#modal_add').modal("show");
                });
            },
            editData: {},
            addData: {},
            btnedit: function (e) {
                this.set("editData", e.data);
                $('#modal_edit').modal("show");
            },
            addProdukt: function (e) {
                e.preventDefault();
                var form = $(e.currentTarget).closest("form");
                var valid = $(form).valid();
                if (!valid) {
                    return false;
                };
                var d = this.get("addData");
                var that = this;
                $.post('@Url.Action("AGsp_AddSluzbaPreddefinovana", "Api/Service")', d.toJSON(), function (result) {
                    if (result.error) {
                        alert(result.error);
                    } else {
                        that.AGsp_GetSluzbyPreddefinovane.read();
                        $('#modal_add').modal("hide");
                    };
                });
            },
            updateProdukt: function (e) {
                e.preventDefault();
                var form = $(e.currentTarget).closest("form");
                var valid = $(form).valid();
                if (!valid) {
                    return false;
                };
                var d = this.get("editData");
                $.post('@Url.Action("AGsp_EditSluzbaPreddefinovana", "Api/Service")', d.toJSON(), function (result) {
                    if (result.error) {
                        alert(result.error);
                    } else {
                        $('#modal_edit').modal("hide");
                    };
                });
            }
        });
        kendo.bind(document.body, viewModel);
    });
</script>