<!DOCTYPE html>

<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>Detail</title>
    <link rel="icon" type="image/png" href="~/Images/crm.png" />

    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.1.0/css/bootstrap.min.css">
    <link rel="stylesheet" href="https://kendo.cdn.telerik.com/2018.2.516/styles/kendo.bootstrap-v4.min.css" />
    <link href="~/Content/app.css" rel="stylesheet" />

    <link href="//fonts.googleapis.com/css?family=Roboto+Condensed:300,300i,400,400i,700,700i&subset=cyrillic,cyrillic-ext,greek,greek-ext,latin-ext,vietnamese" rel="stylesheet">
    <link href="//fonts.googleapis.com/css?family=Open+Sans" rel="stylesheet">
    <link href="//fonts.googleapis.com/css?family=Philosopher" rel="stylesheet">

    <script src="https://kendo.cdn.telerik.com/2018.2.516/js/jquery.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.1.0/js/bootstrap.min.js"></script>
    <script src="https://kendo.cdn.telerik.com/2018.2.516/js/kendo.all.min.js"></script>
    <script src="https://kendo.cdn.telerik.com/2018.2.516/js/cultures/kendo.culture.cs-CZ.min.js"></script>
    <script src="https://kendo.cdn.telerik.com/2018.2.516/js/messages/kendo.messages.cs-CZ.min.js"></script>
    <script>
        kendo.culture("cs-CZ");
    </script>
</head>
<body style="font-family: 'Roboto Condensed', sans-serif;">
    <nav class="navbar navbar-expand-lg fixed-top bg-success navbar-dark">
        <a class="navbar-brand" href="#">
            <img src="~/Images/crm.png" alt="Client box" /><span style="margin-left:10px;">&Copf;&Bopf;&oopf;&xopf;</span>
        </a>
    </nav>
    <div id="main" class="container">
        <h2>Detail volání</h2>
        <h3>@ViewData("typ") hovor: @ViewData("telefon")</h3>
    </div>
</body>
</html>
