@ModelType clientbox.AG_tblUsers

@Code
    Layout = Nothing
End Code

<!DOCTYPE html>

<html>
<head>
    <meta name="viewport" content="width=device-width" />
    <title>Změna hesla</title>
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

        #password1 {
            margin-bottom: -1px;
            border-bottom-right-radius: 0;
            border-bottom-left-radius: 0;
        }

        #password2 {
            margin-bottom: 10px;
            border-top-left-radius: 0;
            border-top-right-radius: 0;
        }
    </style>
</head>
<body class="text-center">
    <form method="post" action="@Url.Action("ResetPassword", "Account")" class="form-signin">
        <img class="mb-4" src="~/Images/crm_512.png" alt="" width="72" height="72">
        <h1 class="h3 mb-3 font-weight-normal">Clientbox - změna hesla</h1>
        <h4>@Model.UserLogin</h4>

        <label for="password1" class="sr-only">Nové heslo</label>
        <input type="password" name="password1" id="password1" class="form-control" placeholder="Nové heslo" required autofocus>
        <label for="password2" class="sr-only">Nové heslo</label>
        <input type="password" name="password2" id="password2" class="form-control" placeholder="Opakuj heslo" required>

        <input type="hidden" name="id" value="@Model.IDUser" />
        @Code
            If Html.ValidationMessage("error") IsNot Nothing Then
                @<p style="color:#ff4242;font-style:italic;background:#ffbcbc">@Html.Raw(Html.ValidationMessage("error").ToHtmlString())</p>
            End If
        End Code
        <button class="btn btn-lg btn-primary btn-block" type="submit">Pokračovat</button>
    </form>
</body>
</html>
