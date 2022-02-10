@Code
    ViewData("Title") = "Práce na projektech"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div id="main" style="height: 100%;">
    <div data-role="scheduler"
         data-editable="false"
         data-views="['day', { type: 'workWeek', selected: true }]"
         data-bind="source: source, events: { navigate: navigate, dataBound: dataBound }"
         style="height: 100%;"></div>
</div>

<script>
    $(function () {
        var curr = new Date;
        var monthback = new Date(curr.setMonth(curr.getMonth() - 1, 1));

        var viewModel = kendo.observable({
            iDUser: 6,
            obdobiOd: monthback,
            obdobiDo: new Date,
            source: new kendo.data.SchedulerDataSource({
                transport: {
                    read: {
                        url: "@Url.Action("AGsp_Get_PraceNaProjektechGraficky", "Api/Service")"
                    },
                    parameterMap: function (options, operation) {
                        var id = viewModel.get("iDUser"),
                            d1 = kendo.toString(viewModel.get("obdobiOd"), "yyyy-MM-dd HH:mm:ss"),
                            d2 = kendo.toString(viewModel.get("obdobiDo"), "yyyy-MM-dd HH:mm:ss"),
                            m = { iDUser: id, obdobiOd: d1, obdobiDo: d2 };
                        console.log(m)
                        return m;
                    }
                },
                schema: {
                    data: "data",
                    total: "total",
                    errors: "error",
                    model: {
                        id: "iDPraceNaProjektu",
                        fields: {
                            iDPraceNaProjektu: { from: "IDPraceNaProjektu", type: "number" },
                            iDProjektu: { from: "IDProjektu", type: "number" },
                            title: { from: "Title" },
                            start: { type: "date", from: "Start" },
                            end: { type: "date", from: "End" },
                            startTimezone: { from: "StartTimezone" },
                            endTimezone: { from: "EndTimezone" },
                            description: { from: "Description" },
                            recurrenceId: { from: "RecurrenceID" },
                            recurrenceRule: { from: "RecurrenceRule" },
                            recurrenceException: { from: "RecurrenceException" },
                            isAllDay: { type: "boolean", from: "IsAllDay" },
                            color: { from: "Color" }
                        }
                    }
                }
            }),
            navigate: function (e) {
                //monthback = new Date(e.date.setMonth(e.date.getMonth() - 1, 1));
                //this.set("obdobiOd", monthback);
                //this.source.read();
            },
            dataBound: function (e) {
                var view = e.sender.view();
                var events = e.sender.dataItems();
                var eventElement;
                var event;

                for (var idx = 0, length = events.length; idx < length; idx++) {
                    event = events[idx];

                    eventElement = view.element.find("[data-uid=" + event.uid + "]");

                    var color = "#000000";
                    var bg = "#f5f5f5";
                    if (event.color) {
                        bg = event.color;
                        color = textColor(bg);
                    }
 
                    var border = colorLuminance(bg, -0.3);
                    eventElement.css("color", color);
                    eventElement.css("border-color", border);
                    eventElement.css("background-color", bg);
                }
            }
        })
        kendo.bind(document.body, viewModel);
    });
</script>