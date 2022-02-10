@Code
    Layout = Nothing
End Code

<!DOCTYPE html>

<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>Uživatel</title>
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

        button {
        margin-top:6px;
        }
    </style>
</head>
<body class="text-center">
    <form method="post" action="@Url.Action("GetUser", "Account")" class="form-signin">
        <img class="mb-4" src="~/Images/crm_512.png" alt="" width="72" height="72">
        <h1 class="h3 mb-3 font-weight-normal">Clientbox - uživatel</h1>
        <label for="login" class="sr-only">Uživatelské jméno</label>
        <input type="email" name="email" id="login" class="form-control" placeholder="E-mailová adresa" required autofocus>
        @Code
            If Html.ValidationMessage("error") IsNot Nothing Then
                @<p style="color:#ff4242;font-style:italic;background:#ffbcbc">@Html.Raw(Html.ValidationMessage("error").ToHtmlString())</p>
            End If
        End Code
        <button class="btn btn-lg btn-primary btn-block" type="submit">Pokračovat</button>
    </form>
</body>
</html>
