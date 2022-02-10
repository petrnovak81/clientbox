Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc
Imports System.Web.Routing

Public Module RouteConfig
    Public Sub RegisterRoutes(ByVal routes As RouteCollection)
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}")

        routes.MapRoute("Default", "{controller}/{action}/{id}", New With {
    .controller = "Home",
    .action = "Klienti",
    .id = UrlParameter.Optional
}, New String() {"clientbox.Controllers"})

        'routes.MapRoute(
        '    name:="Default",
        '    url:="{controller}/{action}/{id}",
        '    defaults:=New With {.controller = "Home", .action = "Index", .id = UrlParameter.Optional}
        ')
    End Sub
End Module