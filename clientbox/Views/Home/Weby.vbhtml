@Code
    ViewData("Title") = "Weby"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<style>
    .k-grid td .k-dropdown-wrap, .k-grid td .btn, .k-grid td .k-button {
        width: 50%!important;
    }

    .k-grid-delete {
        border-color: #dc3545;
        background: #dc3545;
        color: white!important;
    }
        .k-grid .k-grid-edit-row td:not(.k-hierarchy-cell), .k-grid .k-command-cell, .k-grid .k-edit-cell {
            padding: 0;
        }

    .k-grid td:nth-child(1), .k-grid td:nth-child(2) {
        padding-left: .75rem;
    }

    .k-grid .k-link {
        color: #007bff;
        text-decoration: underline;
    }
</style>

<div id="main" style="height: 100%;">
    <div data-role="grid"
         data-scrollable="true"
         data-pageable="true"
         data-editable="inline"
         data-toolbar="['create']"
         data-bind="source: source"
         data-columns="[{ field: 'NazevWebu', title: 'Název', width: 200 }, { field: 'AdresaWebu', template: '#=btnUrl(AdresaWebu)#', title: 'Adresa'}, { command: ['edit', 'destroy'], title: '&nbsp;', width: '250px' }]" style="height:100%;"></div>
</div>

<script>
    function btnUrl(AdresaWebu) {
        return `<a class="k-link" href="` + AdresaWebu + `" target="_blank">` + AdresaWebu + `</a>`;
    }

    $(function () {
        var viewModel = kendo.observable({
            source: new kendo.data.DataSource({
                transport: {
                    read: {
                        url: "@Url.Action("AGsp_Get_NaseWebySeznam", "Api/Service")",
                        type: "GET"
                    },
                    update: {
                        url: "@Url.Action("AGsp_Do_IUNaseWeby", "Api/Service")",
                        type: "POST"
                    },
                    create: {
                        url: "@Url.Action("AGsp_Do_IUNaseWeby", "Api/Service")",
                        type: "POST"
                    },
                    destroy: {
                        url: "@Url.Action("AGsp_Do_DelNaseWeby", "Api/Service")",
                        type: "POST"
                    },
                    parameterMap: function (options, operation) {
                        if (operation !== "read") {
                            return options;
                        }
                    }
                },
                error: function (e) {
                    if (e.status == "customerror") {
                        alert(e.errors)
                    }
                },
                batch: false,
                pageSize: 100,
                schema: {
                    data: "data",
                    total: "total",
                    errors: "error",
                    model: {
                        id: "IDWebu",
                        fields: {
                            IDWebu: { editable: false, nullable: true },
                            NazevWebu: { validation: { required: true } },
                            AdresaWebu: { validation: { required: true } }
                        }
                    }
                }
            })
        });
        kendo.bind(document.body, viewModel);
    })
</script>