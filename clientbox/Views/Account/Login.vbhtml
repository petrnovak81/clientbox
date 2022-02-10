
@Code
    Layout = Nothing
End Code

<!DOCTYPE html>

<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>Přihlášení</title>

    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.1.0/css/bootstrap.min.css">
    <link rel="stylesheet" href="https://kendo.cdn.telerik.com/2018.2.516/styles/kendo.bootstrap-v4.min.css" />

    <link href="//fonts.googleapis.com/css?family=Roboto+Condensed:300,300i,400,400i,700,700i&subset=cyrillic,cyrillic-ext,greek,greek-ext,latin-ext,vietnamese" rel="stylesheet">
    <link href="//fonts.googleapis.com/css?family=Open+Sans" rel="stylesheet">
    <link href="//fonts.googleapis.com/css?family=Philosopher" rel="stylesheet">

    <style>
        html, body {
            height: 100%;
        }

        .text-center {
            text-align: center !important;
        }

        body {
            font-family: 'Roboto Condensed', sans-serif;
            display: -ms-flexbox;
            display: -webkit-box;
            display: flex;
            -ms-flex-align: center;
            -ms-flex-pack: center;
            -webkit-box-align: center;
            align-items: center;
            -webkit-box-pack: center;
            justify-content: center;
            padding-top: 40px;
            padding-bottom: 40px;
            background-color: #f5f5f5;
        }

        .form-signin {
            width: 100%;
            max-width: 330px;
            padding: 15px;
            margin: 0 auto;
        }

            .form-signin input[type="email"] {
                margin-bottom: -1px;
                border-bottom-right-radius: 0;
                border-bottom-left-radius: 0;
            }

        input[type="password"] {
            margin-bottom: 10px;
            border-top-left-radius: 0;
            border-top-right-radius: 0;
        }
    </style>

    <script src="https://kendo.cdn.telerik.com/2018.2.516/js/jquery.min.js"></script>
    @*<script>

        var doklad = {
            Kniha: "Vydané doklady",
            TypZaznamu: "FV",
            Poznamka: "poznamka test",
            Mena: "CZK",
            Firma: "KVALIDENT s.r.o.",
            Datum: new Date(),
            DatumZdanitelnehoPlneni: new Date(),
            DorucovaciAdresa: {
                Ulice: "Široká 45",
                Okres: "Králíkárna",
                PSC: "132 99",
                Obec: "Králík"
            },
            Polozky: []
        };

        doklad.Polozky.push({
            MnozstviJednotek: 1,
            NazevSazbyDPH: "Základní",
            Produkt: "eKompas",
            SazbaDPH: 0,
            CelkemDPH: 0,
            CenaCelkemBezDPH: 125,
            CenaCelkemVcetneDPH: 125,
        });

        $.ajax({
            type: "POST",
            data: JSON.stringify(doklad),
            url: "@Url.Action("novy_doklad", "Api/AVISService")",
            contentType: "application/json",
            success: function (result) {
                if (result.error) {
                    if (result.error.detail) {
                        alert(result.error.detail.Description)
                    }
                } else {
                    var cd = result.data.CisloDokladu;
                    $.get("@Url.Action("AGsp_Run_Pracak20to30", "Api/Service")", { iDVykazPrace: 0, cisloFaktury: cd }, function (result) {
                        if (result.error) {
                            alert(result.error)
                        } else {

                        }
                    });
                }
            },
            error: function (xhr, status, message) {
                console.log(xhr);
            }
        });
    </script>*@
</head>
<body class="text-center">
    <form method="post" action="@Url.Action("Login", "Account")" class="form-signin">
        <img class="mb-4" src="~/Images/crm_512.png" alt="" width="72" height="72">
        <h1 class="h3 mb-3 font-weight-normal">Clientbox - přihlášení</h1>
        <label for="login" class="sr-only">Uživatelské jméno</label>
        <input type="email" name="email" id="login" class="form-control" placeholder="E-mailová adresa" required autofocus>
        <label for="password" class="sr-only">Heslo</label>
        <input type="password" name="password" id="password" class="form-control" placeholder="Heslo" required>
        @Code
            If Html.ValidationMessage("error") IsNot Nothing Then
                @<p style="color:#ff4242;font-style:italic;background:#ffbcbc">@Html.Raw(Html.ValidationMessage("error").ToHtmlString())</p>
            End If
        End Code
        <button class="btn btn-lg btn-primary btn-block" type="submit">Přihlásit</button>
        <a href="@Url.Action("GetUser", "Account")" class="btn btn-link">Resetovat heslo</a>
        <p class="mt-5 mb-3 text-muted">© @Now.Year</p>
        <p><a href="@Url.Action("Download", "Home")?file=callsignal.apk">Call Signal</a></p>
        <p><a href="@Url.Action("Download", "Home")?file=app-debug.apk">verze pro android</a></p>
    </form>
</body>
</html>
