Imports System.Globalization
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Web.Http
Imports Microsoft.Owin.Security.OAuth
Imports Newtonsoft.Json.Converters
Imports Newtonsoft.Json.Serialization

Public Module WebApiConfig
    Public Sub Register(config As HttpConfiguration)
        config.Routes.MapHttpRoute(
                                             name:="Service",
                                             routeTemplate:="api/{controller}/{action}/{id}",
                                             defaults:=New With {.id = UrlParameter.Optional}
                                         )

        'Dim Converter As IsoDateTimeConverter = New IsoDateTimeConverter With {
        '    .DateTimeStyles = DateTimeStyles.RoundtripKind,
        '    .Culture = CultureInfo.InstalledUICulture,
        '    .DateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss.FFFFFFFK"}

        'config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(Converter)
        'var json = JsonConvert.SerializeObject(someObject, Formatting.Indented, New JsonSerializerSettings() {DateFormatString = "yyyy-MM-ddThh:mm:ssZ"});

        config.Formatters.JsonFormatter.SupportedMediaTypes.Add(New MediaTypeHeaderValue("text/html"))
        config.Formatters.JsonFormatter.SupportedMediaTypes.Add(New MediaTypeHeaderValue("application/json"))

    End Sub
End Module
