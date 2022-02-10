@Code
    Layout = Nothing
End Code

<!DOCTYPE html>

<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>Inventurni stav</title>
    <script src="https://kendo.cdn.telerik.com/2018.2.516/js/jquery.min.js"></script>
    <script src="https://kendo.cdn.telerik.com/2018.2.516/js/kendo.all.min.js"></script>
    <script src="https://kendo.cdn.telerik.com/2018.2.516/js/cultures/kendo.culture.cs-CZ.min.js"></script>
    <script>
        kendo.culture("cs-CZ");
    </script>
    <style>
        body {
            background: rgb(204,204,204);
            font-family: Arial;
        }

        page[size="A4"] {
            background: white;
            width: 21cm;
            min-height: 29.7cm;
            display: block;
            margin: 0 auto;
            margin-bottom: 0.5cm;
            box-shadow: 0 0 0.5cm rgba(0,0,0,0.5);
            border: 1cm solid white;
        }

        @@media print {
            body, page[size="A4"] {
                margin: 0;
                box-shadow: none;
                border: 0;
                margin-bottom: 0;
            }
        }

        .grid-container {
            display: grid;
            padding-bottom: 6px;
            margin-top: 6px;
            grid-template-columns: 1fr 1fr 1fr 50px 50px 100px;
            grid-template-rows: auto auto;
            grid-template-areas: "Produkt NaposledyNaskl PrumNakup SZ OZ Skutek" "Popis Popis Popis Popis Popis Popis";
            border-bottom: 1px solid rgb(204,204,204)
        }

        .Produkt {
            grid-area: Produkt;
        }

        .NaposledyNaskl {
            grid-area: NaposledyNaskl;
        }

        .Popis {
            grid-area: Popis;
            padding-top: 6px;
        }

        .PrumNakup {
            grid-area: PrumNakup;
            text-align: center;
        }

        .SZ {
            grid-area: SZ;
            text-align: center;
        }

        .OZ {
            grid-area: OZ;
            text-align: center;
        }

        .Skutek {
            grid-area: Skutek;
            text-align: center;
        }
    </style>
</head>
<body>
    <page size="A4">
        <div class="grid-container">
            <div class="Produkt"><div><b>Produkt</b></div></div>
            <div class="NaposledyNaskl"><div><b>Naposledy nakladněno</b></div></div>
            <div class="Popis"></div>
            <div class="PrumNakup"><div><b>Průměrná nákupní</b></div></div>
            <div class="SZ"><div><b>SZ</b></div></div>
            <div class="OZ"><div><b>OZ</b></div></div>
            <div class="Skutek" style="border: 0;"><div><b>Skutečnost</b></div></div>
        </div>
        <div data-bind="source: source" data-template="sestava-tmp-row">

        </div>
    </page>

    <script id="sestava-tmp-row" type="text/html">
        <div class="grid-container">
            <div class="Produkt"><span data-bind="text: Produkt"></span></div>
            <div class="NaposledyNaskl"><span>#=datum(NaposledyNaskladneno)#</span></div>
            <div class="Popis"><small><i><span data-bind="text: Popis"></span></i></small></div>
            <div class="PrumNakup"><span data-bind="text: PrumernaNakup"></span></div>
            <div class="SZ"><span data-bind="text: SkladovaZasoba"></span></div>
            <div class="OZ"><span data-bind="text: OperativniZasoba"></span></div>
            <div class="Skutek"><div contenteditable="true"></div></div>
        </div>
    </script>

    <script>
        //Public Property Produkt As String
        //Public Property Popis As String
        //Public Property SkladovaZasoba As Decimal
        //Public Property OperativniZasoba As Decimal
        //Public Property PrumernaNakup As Decimal
        //Public Property NaposledyNaskladneno As Nullable(Of Date)
        
        function datum(value) {
            var d = kendo.toString(new Date(value), "d");
            return d;
        }

        $(function () {
            var viewModel = kendo.observable({
                source: new kendo.data.DataSource({
                    transport: {
                        read: function (options) {
                            $.get("@Url.Action("AGsp_GetInventurniStav", "Api/Service")", null, function (result) {
                                var data = result.data || [];
                                console.log(data)
                                options.success(data);
                            });
                        }
                    }
                })
            });
            kendo.bind(document.body, viewModel);
        });
    </script>
</body>
</html>
