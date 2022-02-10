@ModelType Nullable(Of Integer)

@Code
    Layout = Nothing
End Code

<!DOCTYPE html>

<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>Graf Dostupnosti</title>
    <script src="https://code.highcharts.com/stock/highstock.js"></script>
    <script src="https://code.highcharts.com/stock/modules/data.js"></script>
    <script src="https://code.highcharts.com/stock/modules/exporting.js"></script>
    <script src="https://code.highcharts.com/stock/modules/export-data.js"></script>
    <style>
        html, body, #container {
            height: 100%;
            width: 100%;
            margin: 0;
            padding: 0;
        }
    </style>
</head>
<body>
    <div id="container"></div>

    <script>
        Highcharts.setOptions({
            lang: {
                loading: 'Nahrávám...',
                months: ['Leden', 'Únor', 'Březen', 'Duben', 'Květen', 'Červen', 'Červenec', 'Srpen', 'Září', 'Říjen', 'Listopad', 'Prosinec'],
                weekdays: ['Neděle', 'Pondělí', 'Úterý', 'Středa', 'Čtvrtek', 'Pátek', 'Sobota'],
                shortMonths: ['Led', 'Uno', 'Bře', 'Dub', 'Kvě', 'Čer', 'Črv', 'Srp', 'Zář', 'Říj', 'Lis', 'Pro'],
                exportButtonTitle: "Exportovat",
                printButtonTitle: "Tisk",
                rangeSelectorFrom: "Z",
                rangeSelectorTo: "Do",
                rangeSelectorZoom: "Přiblížení",
                downloadPNG: 'Stáhnout jako PNG',
                downloadJPEG: 'Stáhnout jako JPEG',
                downloadPDF: 'Stáhnout jako PDF',
                downloadSVG: 'Stáhnout jako SVG',
                noData: 'Data nejsou k dispozici',
                printChart: 'Tisk grafu'
            },
            global: {
                useUTC: false
            }
        });

        Highcharts.getJSON('@Url.Action("AGsp_GetSBGrafDostupnosti", "Api/Service")?IDBackupProfile=@(Model)', function (data) {
            var dt = [];
            try {
                for (i in data) {
                    var datetime = Date.parse(data[i].Cas);
                    var value = (data[i].H ? 1 : 0);
                    var ID = data[i].ID
                    dt.push({
                        id: ID,
                        x: datetime,
                        y: value
                    });
                }
            }
            catch (ex) { };

            Highcharts.stockChart('container', {
                rangeSelector: {
                    selected: 1
                },
                title: {
                    text: 'Graf dostupnosti'
                },
                xAxis: {
                    type: 'datetime',
                    ordinal: false,
                    dateTimeLabelFormats: {
                        day: '%A %d.%m.'
                    }
                },
                tooltip: {
                    headerFormat: '',
                    xDateFormat: '%A %d.%m. %H:%M',
                    pointFormatter: function () {
                        if (this.y == 1) {
                            return "Dostupnost: ONLINE<br>" + this.key
                        } else {
                            return "Dostupnost: OFFLINE<br>" + this.key
                        }
                        return ""
                    },
                    valueSuffix: '',
                    shared: true,
                    crosshairs: true
                },
                series: [{
                    name: 'Dostupnost',
                    data: dt,
                    step: true,
                    tooltip: {
                        valueDecimals: 0
                    }
                }]
            });
        });
    </script>
</body>
</html>
